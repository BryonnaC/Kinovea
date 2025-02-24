#region License
/*
Copyright � Joan Charmant 2008.
jcharmant@gmail.com 
 
This file is part of Kinovea.

Kinovea is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License version 2 
as published by the Free Software Foundation.

Kinovea is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Kinovea. If not, see http://www.gnu.org/licenses/.

*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Linq;
using Kinovea.Services;
using Kinovea.Video;

namespace Kinovea.ScreenManager
{
    /// <summary>
    /// Main class representing the analysis document.
    /// </summary>
    public class Metadata
    {
        #region Events and commands
        public EventHandler KVAImported;
        public EventHandler<KeyframeEventArgs> KeyframeAdded;
        public EventHandler KeyframeDeleted;
        public EventHandler<DrawingEventArgs> DrawingAdded;
        public EventHandler<DrawingEventArgs> DrawingModified; 
        public EventHandler DrawingDeleted;
        public EventHandler<MultiDrawingItemEventArgs> MultiDrawingItemAdded;
        public EventHandler MultiDrawingItemDeleted;
        public EventHandler CameraCalibrationAsked;
            
        public RelayCommand<ITrackable> AddTrackableDrawingCommand { get; set; }
        public RelayCommand<ITrackable> DeleteTrackableDrawingCommand { get; set; }
        #endregion
        
        #region Properties
        /// <summary>
        /// Helper function to generate timecodes.
        /// </summary>
        public TimeCodeBuilder TimeCodeBuilder
        {
            get { return timecodeBuilder; }
        }
        public HistoryStack HistoryStack
        {
            get { return historyStack; }
        }
        public bool IsDirty
        {
            get 
            {
                int currentHash = GetContentHash();
                bool dirty = currentHash != referenceHash;
                log.DebugFormat("Dirty:{0}, reference hash:{1}, current:{2}.", dirty.ToString(), referenceHash, currentHash);
                return dirty;
            }
        }
        public bool KVAImporting
        {
            get { return kvaImporting; }
        }

        /// <summary>
        /// The path to the KVA file this metadata was imported from or last saved.
        /// Returns an empty string if the file is a default KVA, to avoid overwriting them.
        /// </summary>
        public string LastKVAPath
        {
            get 
            {
                if (string.IsNullOrEmpty(lastKVAPath) ||
                    lastKVAPath == PreferencesManager.PlayerPreferences.PlaybackKVA ||
                    lastKVAPath == PreferencesManager.CapturePreferences.CaptureKVA ||
                    lastKVAPath.StartsWith(Software.TempDirectory))
                    return "";
                    
                return lastKVAPath;
            } 
            set
            {
                lastKVAPath = value;
            }
        }
        public string GlobalTitle
        {
            get { return globalTitle; }
            set { globalTitle = value; }
        }
        public Size ImageSize
        {
            get { return imageSize; }
            set
            {
                imageSize = value;
                trackabilityManager.Initialize(imageSize);
                calibrationHelper.Initialize(imageSize, GetCalibrationOrigin, GetCalibrationQuad, HasTrackingData);
            }
        }
        public ImageTransform ImageTransform
        {
            get { return imageTransform; }
        }

        public ImageAspectRatio ImageAspect { get; set; }
        public ImageRotation ImageRotation { get; set; }
        public bool Mirrored { get; set; }
        public Demosaicing Demosaicing { get; set; }
        public bool Deinterlacing { get; set; }

        /// <summary>
        /// Path to the video file this metadata was created on.
        /// </summary>
        public string VideoPath
        {
            get { return videoPath; }
            set { videoPath = value;}
        }

        public Keyframe this[int index]
        {
            // Indexor
            get 
            { 
                if(index < 0 || index >= keyframes.Count)
                    return null;
                else
                    return keyframes[index]; 
            }
            set { keyframes[index] = value; }
        }
        public List<Keyframe> Keyframes
        {
            get { return keyframes; }
        }
        public int Count
        {
            get { return keyframes.Count; }
        }
        public bool HasData
        {
            get 
            {
                // This is used to know if there is anything to draw on the images when saving.
                // All kind of objects should be taken into account here, even those
                // that we currently don't save to the .kva but only draw on the image.
                return keyframes.Count > 0 ||
                        spotlightManager.Count > 0 ||
                        autoNumberManager.Count > 0 ||
                        chronoManager.Drawings.Count > 0 ||
                        trackManager.Drawings.Count > 0 ||
                        extraDrawings.Count > totalStaticExtraDrawings ||
                        magnifier.Mode != MagnifierMode.None;
            }
        }
        public bool Tracking 
        {
            get { return TrackManager.Tracking || TrackabilityManager.Tracking; }
        }
        public bool HasTrack 
        {
            get { return trackManager.Drawings.Count > 0; }
        }
        public bool TextEditingInProgress
        {
            get { return Labels().Any(l => l.Editing); }
        }
        public List<AbstractDrawing> ExtraDrawings
        {
            get { return extraDrawings;}
        }
        public AbstractDrawing HitDrawing
        {
            get { return hitDrawing;}
        }
        public Keyframe HitKeyframe
        {
            get { return hitKeyframe; }
        }
        
        public Magnifier Magnifier
        {
            get { return magnifier;}
            set { magnifier = value;}
        }
        public SpotlightManager SpotlightManager
        {
            get { return spotlightManager;}
        }
        public AutoNumberManager AutoNumberManager
        {
            get { return autoNumberManager;}
        }
        public DrawingCoordinateSystem DrawingCoordinateSystem
        {
            get { return drawingCoordinateSystem; }
        }
        public DrawingTestGrid DrawingTestGrid
        {
            get { return drawingTestGrid; }
        }
        public ChronoManager ChronoManager
        {
            get { return chronoManager; }
        }
        public TrackManager TrackManager
        {
            get { return trackManager; }
        }
        
        public bool DrawingInitializing
        {
            get
            {
                IInitializable initializable = hitDrawing as IInitializable;
                if (initializable == null)
                    return false;

                return initializable.Initializing;
            }
        }
        
        // General infos
        public long AverageTimeStampsPerFrame
        {
            get { return averageTimeStampsPerFrame; }
            set { averageTimeStampsPerFrame = value;}
        }
        public double AverageTimeStampsPerSecond
        {
            get { return averageTimeStampsPerSecond; }
            set { averageTimeStampsPerSecond = value; }
        }
        public long FirstTimeStamp
        {
            get { return firstTimeStamp; }
            set { firstTimeStamp = value; }
        }
        public long SelectionStart
        {
            get { return selectionStart; }
            set { selectionStart = value; }
        }
        public long SelectionEnd
        {
            get { return selectionEnd; }
            set { selectionEnd = value; }
        }

        /// <summary>
        /// User-defined origin of the time coordinate system, in absolute timestamps. 
        /// </summary>
        public long TimeOrigin
        {
            get { return timeOrigin; }
            set { timeOrigin = value; }
        }
        public bool TestGridVisible
        {
            get { return drawingTestGrid.Visible; }
            set { drawingTestGrid.Visible = value; }        
        }

        /// <summary>
        /// The ratio between the capture framerate and the video framerate.
        /// The slowdown factor of the video relatively to real time.
        /// </summary>
        public double HighSpeedFactor
        {
            get { return highSpeedFactor; }
            set { highSpeedFactor = value == 0 ? 1.0 : value; }
        }

        /// <summary>
        /// The frame interval used for playback timer, as specified by the user.
        /// </summary>
        public double UserInterval
        {
            get { return userInterval; }
            set { userInterval = value; }
        }

        public VideoFilterType ActiveVideoFilterType
        {
            get { return activeVideoFilterType; }
        }

        public IVideoFilter ActiveVideoFilter
        {
            get 
            {
                if (activeVideoFilterType == VideoFilterType.None)
                    return null;
                else
                    return videoFilters[activeVideoFilterType];
            }
        }

        public CalibrationHelper CalibrationHelper 
        {
            get { return calibrationHelper; }
        }
        public TrackabilityManager TrackabilityManager
        {
            get { return trackabilityManager;}
        }
        #endregion

        #region Members
        private Guid id = Guid.NewGuid();
        private bool initialized;
        private TimeCodeBuilder timecodeBuilder;
        private HistoryStack historyStack;
        private int referenceHash;
        private bool kvaImporting;
        private bool captureKVA;

        // Folders
        private string videoPath;
        private string tempFolder;
        private AutoSaver autoSaver;
        private string lastKVAPath;

        // Keyframes & attached drawings.
        private List<Keyframe> keyframes = new List<Keyframe>();
        private Keyframe hitKeyframe;
        private AbstractDrawing hitDrawing;
        
        // Drawings not attached to any key image.
        private List<AbstractDrawing> extraDrawings = new List<AbstractDrawing>();
        private int totalStaticExtraDrawings;           // TODO: might be removed when even Chronos and tracks are represented by a single manager object.
        private Magnifier magnifier = new Magnifier();
        private SpotlightManager spotlightManager;
        private AutoNumberManager autoNumberManager;
        private DrawingCoordinateSystem drawingCoordinateSystem;
        private Guid memoCoordinateSystemId;
        private DrawingTestGrid drawingTestGrid;
        private ChronoManager chronoManager = new ChronoManager();
        private TrackManager trackManager = new TrackManager();
        private TrackerParameters lastUsedTrackerParameters;
        private TrackExtraData trackExtraData;
        private TrackabilityManager trackabilityManager = new TrackabilityManager();

        // Other info not related to drawings.
        private string globalTitle;
        private Size imageSize = new Size(0,0);
        private CalibrationHelper calibrationHelper = new CalibrationHelper();
        private Temporizer calibrationChangedTemporizer;
        private ImageTransform imageTransform = new ImageTransform();

        // Timing information
        private long averageTimeStampsPerFrame = 1;
        private double averageTimeStampsPerSecond = 25;
        private long firstTimeStamp;
        private long timeOrigin;
        private double highSpeedFactor = 1.0;
        private double userInterval = 40;
        private long selectionStart;
        private long selectionEnd;

        // Video filters
        private Dictionary<VideoFilterType, IVideoFilter> videoFilters = new Dictionary<VideoFilterType, IVideoFilter>();
        private VideoFilterType activeVideoFilterType = VideoFilterType.None;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor

        /// <summary>
        /// Main constructor used by the screens when they initialize themselves.
        /// This is the primary metadata that all other loads are going to merge into.
        /// The timecodeBuilder MUST be implemented as any metadata is subject to be exported
        /// and the user time attributes use it.
        /// </summary>
        public Metadata(HistoryStack historyStack, TimeCodeBuilder timecodeBuilder)
        {
            this.historyStack = historyStack;
            this.timecodeBuilder = timecodeBuilder;

            calibrationHelper.CalibrationChanged += CalibrationHelper_CalibrationChanged;
            
            autoSaver = new AutoSaver(this);
            
            CreateStaticExtraDrawings();
            CreateVideoFilters();
            CleanupHash();
            SetupTempDirectory(id);

            calibrationChangedTemporizer = new Temporizer(200, TracksCalibrationChanged);

            log.Debug("Constructing new Metadata object.");
        }
        public Metadata(string kvaString,  VideoInfo info, HistoryStack historyStack, TimeCodeBuilder timecodeBuilder, ClosestFrameDisplayer closestFrameDisplayer)
            : this(historyStack, timecodeBuilder)
        {
            // This should reflect what we do in FrameServerPlayer.SetupMetadata
            imageSize = info.ReferenceSize;
            userInterval = info.FrameIntervalMilliseconds;
            averageTimeStampsPerFrame = info.AverageTimeStampsPerFrame;
            averageTimeStampsPerSecond = info.AverageTimeStampsPerSeconds;
            calibrationHelper.CaptureFramesPerSecond = info.FramesPerSeconds;
            firstTimeStamp = info.FirstTimeStamp;
            
            videoPath = info.FilePath;

            MetadataSerializer serializer = new MetadataSerializer();
            serializer.Load(this, kvaString, false);
        }
        #endregion

        #region Public Interface
        
        #region Keyframes
        public Keyframe GetKeyframe(Guid id)
        {
            return keyframes.FirstOrDefault(kf => kf.Id == id);
        }
        public void AddKeyframe(Keyframe keyframe)
        {
            keyframes.Add(keyframe);
            keyframes.Sort();
            SelectKeyframe(keyframe);
            UpdateTrajectoriesForKeyframes();
            
            if (KeyframeAdded != null)
                KeyframeAdded(this, new KeyframeEventArgs(keyframe.Id));
        }
        public void DeleteKeyframe(Guid id)
        {
            foreach (Keyframe keyframe in keyframes)
            {
                if (keyframe.Id != id)
                    continue;

                foreach (AbstractDrawing drawing in keyframe.Drawings)
                    BeforeDrawingDeletion(drawing);
            }

            keyframes.RemoveAll(k => k.Id == id);
            UpdateTrajectoriesForKeyframes();
            
            if (KeyframeDeleted != null)
                KeyframeDeleted(this, new KeyframeEventArgs(id));
        }
        public void SelectKeyframe(Keyframe keyframe)
        {
            hitKeyframe = keyframe;
        }
        public void EnableDisableKeyframes()
        {
            foreach(Keyframe keyframe in keyframes)
            {
                keyframe.TimeCode = timecodeBuilder(keyframe.Position, TimeType.UserOrigin, PreferencesManager.PlayerPreferences.TimecodeFormat, true);
                keyframe.Disabled = keyframe.Position < selectionStart || keyframe.Position > selectionEnd;
            }
        }
        public int GetKeyframeIndex(long position)
        {
            for (int i = 0; i < keyframes.Count; i++)
                if (keyframes[i].Position == position)
                    return i;

            return -1;
        }
        public Guid GetKeyframeId(int keyframeIndex)
        {
            return keyframes[keyframeIndex].Id;
        }

        /// <summary>
        /// Returns whether this drawing is the kind of drawing that is attached to a keyframe.
        /// </summary>
        public bool IsAttachedDrawing(AbstractDrawing drawing)
        {
            return !(drawing is DrawingChrono) && !(drawing is DrawingTrack);
        }

        /// <summary>
        /// Returns the id of the manager managing this drawing.
        /// </summary>
        public Guid FindManagerId(AbstractDrawing drawing)
        {
            if (drawing is DrawingChrono)
                return chronoManager.Id;
            else if (drawing is DrawingTrack)
                return trackManager.Id;
            else
                return FindAttachmentKeyframeId(drawing);
        }

        /// <summary>
        /// Returns the id of the keyframe the drawing is attached to.
        /// </summary>
        public Guid FindAttachmentKeyframeId(AbstractDrawing drawing)
        {
            Keyframe foundKeyframe = null;
            foreach (Keyframe k in keyframes)
            {
                foreach (AbstractDrawing d in k.Drawings)
                {
                    if (d.Id != drawing.Id)
                        continue;

                    foundKeyframe = k;
                    break;
                }

                if (foundKeyframe != null)
                    break;
            }

            return foundKeyframe == null ? Guid.Empty : foundKeyframe.Id;
        }


        /// <summary>
        /// Returns the id of the keyframe the drawing is attached to.
        /// </summary>
        public AbstractDrawing FindDrawing(Guid drawingId)
        {
            foreach (Keyframe k in keyframes)
            {
                foreach (AbstractDrawing d in k.Drawings)
                {
                    if (d.Id != drawingId)
                        continue;

                    return d;
                }
            }

            return null;
        }


        public int GetKeyframeIndex(Guid id)
        {
            // Temporary function to accomodate clients of the old API where we used indices to reference keyframes and drawings.
            for (int i = 0; i < keyframes.Count; i++)
                if (keyframes[i].Id == id)
                    return i;

            return -1;
        }
        public void MergeInsertKeyframe(Keyframe keyframe)
        {
            bool processed = false;

            // If the keyframe is already known (by ID) we don't import nor merge it.
            Keyframe known = keyframes.FirstOrDefault((kf) => kf.Id == keyframe.Id);
            if (known != null)
                return;
            
            for (int i = 0; i < keyframes.Count; i++)
            {
                Keyframe k = keyframes[i];

                if (keyframe.Position < k.Position)
                {
                    keyframes.Insert(i, keyframe);
                    processed = true;
                    break;
                }
                else if (keyframe.Position == k.Position)
                {
                    foreach (AbstractDrawing ad in keyframe.Drawings)
                        k.Drawings.Add(ad);

                    processed = true;
                    break;
                }
            }

            if (!processed)
                keyframes.Add(keyframe);

            // Post-init for the new drawings.
            foreach (AbstractDrawing ad in keyframe.Drawings)
                AfterDrawingCreation(ad);
        }
        #endregion
        
        #region Filtered iterators
        public IEnumerable<DrawingTrack> Tracks()
        {
            foreach (AbstractDrawing drawing in TrackManager.Drawings)
                if (drawing is DrawingTrack)
                    yield return (DrawingTrack)drawing;
        }
        public IEnumerable<AbstractDrawing> AttachedDrawings()
        {
            foreach (Keyframe kf in keyframes)
                foreach (AbstractDrawing drawing in kf.Drawings)
                    yield return drawing;
        }
        public IEnumerable<DrawingText> Labels()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingText)
                    yield return (DrawingText)drawing;
        }
        public IEnumerable<DrawingLine> Lines()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingLine)
                    yield return (DrawingLine)drawing;
        }
        public IEnumerable<DrawingAngle> Angles()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingAngle)
                    yield return (DrawingAngle)drawing;
        }
        public IEnumerable<DrawingCrossMark> CrossMarks()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingCrossMark)
                    yield return (DrawingCrossMark)drawing;
        }
        public IEnumerable<DrawingGenericPosture> GenericPostures()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingGenericPosture)
                    yield return (DrawingGenericPosture)drawing;
        }
        public IEnumerable<DrawingPlane> Planes()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingPlane)
                    yield return (DrawingPlane)drawing;
        }
        
        public IEnumerable<DrawingSVG> SVGs()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingSVG)
                    yield return (DrawingSVG)drawing;
        }

        /// <summary>
        /// Returns trackable drawings, does not include Tracks themselves.
        /// </summary>
        public IEnumerable<ITrackable> TrackableDrawings()
        {
            foreach (AbstractDrawing drawing in extraDrawings)
            {
                if (drawing is ITrackable)
                    yield return (ITrackable)drawing;
            }

            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is ITrackable)
                    yield return (ITrackable)drawing;
        }
        public IEnumerable<DrawingDistortionGrid> DistortionGrids()
        {
            foreach (AbstractDrawing drawing in AttachedDrawings())
                if (drawing is DrawingDistortionGrid)
                    yield return (DrawingDistortionGrid)drawing;
        }
        #endregion
        
        #region Drawings
        public AbstractDrawingManager GetDrawingManager(Guid managerId)
        {
            if (managerId == chronoManager.Id)
                return chronoManager;
            else if (managerId == trackManager.Id)
                return trackManager;
            else
                return GetKeyframe(managerId);
        }

        public AbstractDrawing GetDrawing(Guid managerId, Guid drawingId)
        {
            AbstractDrawingManager manager = null;

            if (managerId == chronoManager.Id)
                manager = chronoManager;
            else if (managerId == trackManager.Id)
                manager = trackManager;
            else
                manager = GetKeyframe(managerId);

            if (manager == null)
                return null;

            return manager.GetDrawing(drawingId);
        }
        
        /// <summary>
        /// General method to add a drawing on a manager (keyframe-attched, chrono, track).
        /// </summary>
        public void AddDrawing(Guid managerId, AbstractDrawing drawing)
        {
            if (drawing == null)
                return;

            Keyframe keyframe = GetKeyframe(managerId);
            if (keyframe != null)
            {
                bool known = keyframe.Drawings.Any(d => d.Id == drawing.Id);
                if (!known)
                    AddDrawing(keyframe, drawing);
                return;
            }

            if (chronoManager.Id == managerId && drawing is DrawingChrono)
            {
                bool known = chronoManager.Drawings.Any(d => d.Id == drawing.Id);
                if (!known)
                    AddChrono(drawing as DrawingChrono);
                return;
            }

            if (trackManager.Id == managerId && drawing is DrawingTrack)
            {
                bool known = trackManager.Drawings.Any(d => d.Id == drawing.Id);
                if (!known)
                    AddTrack(drawing as DrawingTrack);
                return;
            }
        }
        
        /// <summary>
        /// Adds a drawing to the specified keyframe.
        /// </summary>
        public void AddDrawing(Keyframe keyframe, AbstractDrawing drawing)
        {
            if (keyframe == null || drawing == null || !drawing.IsValid)
                return;

            keyframe.AddDrawing(drawing);
            drawing.InfosFading.ReferenceTimestamp = keyframe.Position;
            drawing.InfosFading.AverageTimeStampsPerFrame = averageTimeStampsPerFrame;
            if (captureKVA)
            {
                drawing.InfosFading.UseDefault = false;
                drawing.InfosFading.AlwaysVisible = true;
            }

            SelectKeyframe(keyframe);
            SelectDrawing(drawing);

            AfterDrawingCreation(drawing);

            if (DrawingAdded != null)
                DrawingAdded(this, new DrawingEventArgs(drawing, keyframe.Id));
        }
        
        /// <summary>
        /// Adds a new item to a multi drawing.
        /// </summary>
        public void AddMultidrawingItem(AbstractMultiDrawing multidrawing, AbstractMultiDrawingItem item)
        {
            multidrawing.Add(item);
            SelectDrawing(multidrawing);

            if (MultiDrawingItemAdded != null)
                MultiDrawingItemAdded(this, new MultiDrawingItemEventArgs(item, multidrawing));
        }
        
        /// <summary>
        /// Adds a new chronometer drawing.
        /// </summary>
        public void AddChrono(DrawingChrono chrono)
        {
            chronoManager.AddDrawing(chrono);
            chrono.ParentMetadata = this;
            
            hitDrawing = chrono;

            AfterDrawingCreation(chrono);

            if (DrawingAdded != null)
                DrawingAdded(this, new DrawingEventArgs(chrono, chronoManager.Id));
        }

        /// <summary>
        /// Adds a new track drawing.
        /// </summary>
        public void AddTrack(DrawingTrack track)
        {
            trackManager.AddDrawing(track);

            track.ParentMetadata = this;
            
            if (lastUsedTrackerParameters != null)
                track.TrackerParameters = lastUsedTrackerParameters;
            
            track.TrackerParametersChanged += Track_TrackerParametersChanged;

            hitDrawing = track;
            
            AfterDrawingCreation(track);

            // The following is necessary for the "undo of deletion" case.
            track.UpdateKinematics();
            track.IntegrateKeyframes();
            
            if (DrawingAdded != null)
                DrawingAdded(this, new DrawingEventArgs(track, trackManager.Id));
        }

        public void ModifiedDrawing(Guid managerId, Guid drawingId)
        {
            AbstractDrawing drawing = GetDrawing(managerId, drawingId);
            DrawingTrack track = drawing as DrawingTrack;
            if (track != null)
            {
                track.UpdateKinematics();
                track.IntegrateKeyframes();
            }

            if (DrawingModified != null)
                DrawingModified(this, new DrawingEventArgs(drawing, managerId));
        }
        
        public void DeleteDrawing(Guid managerId, Guid drawingId)
        {
            // Remove event handlers from the drawing as well as all associated data like tracking data,
            // and finally remove the drawing itself.

            AbstractDrawingManager manager = null;

            if (managerId == chronoManager.Id)
                manager = chronoManager;
            else if (managerId == trackManager.Id)
                manager = trackManager;
            else
                manager = GetKeyframe(managerId);

            if (manager == null)
                return;
                
            AbstractDrawing drawing = manager.GetDrawing(drawingId);
            if (drawing == null)
                return;

            BeforeDrawingDeletion(drawing);
            
            manager.RemoveDrawing(drawingId);
            UnselectAll();
            
            if (DrawingDeleted != null)
                DrawingDeleted(this, EventArgs.Empty);
        }
        
        public void DeleteMultiDrawingItem(AbstractMultiDrawing manager, Guid itemId)
        {
            ITrackable item = manager.GetItem(itemId) as ITrackable;
            if (item != null)
                DeleteTrackableDrawing(item);

            manager.Remove(itemId);
            UnselectAll();

            if (MultiDrawingItemDeleted != null)
                MultiDrawingItemDeleted(this, EventArgs.Empty);
        }
        
        private void DeleteTrackableDrawing(ITrackable drawing)
        {
            trackabilityManager.Remove(drawing);
        }

        private void Track_TrackerParametersChanged(object sender, EventArgs e)
        {
            // Remember these track parameters to bootstrap the next trackable.
            DrawingTrack track = sender as DrawingTrack;
            if (track == null)
                return;

            lastUsedTrackerParameters = track.TrackerParameters;
        }
        
        public void InitializeCommit(VideoFrame videoFrame, PointF point)
        {
            magnifier.InitializeCommit(point);

            IInitializable initializable = hitDrawing as IInitializable;
            if (initializable == null || !initializable.Initializing)
                return;
            
            string key = initializable.InitializeCommit(point);

            if (string.IsNullOrEmpty(key))
                return;

            ITrackable trackable = hitDrawing as ITrackable;
            if (trackable != null)
                trackabilityManager.AddPoint(trackable, videoFrame, key, point);
        }
        public void InitializeEnd(bool cancelCurrentPoint)
        {
            IInitializable initializable = hitDrawing as IInitializable;
            if (initializable == null || !initializable.Initializing)
                return;

            string key = initializable.InitializeEnd(cancelCurrentPoint);

            if (string.IsNullOrEmpty(key) || !cancelCurrentPoint)
                return;
            
            ITrackable trackable = hitDrawing as ITrackable;
            if (trackable != null)
                trackabilityManager.RemovePoint(trackable, key);
        }
        #endregion
        
        /// <summary>
        /// Collect measured data for spreadsheet export.
        /// </summary>
        public MeasuredData CollectMeasuredData()
        {
            MeasuredData md = new MeasuredData();
            md.Producer = Software.ApplicationName + "." + Software.Version;
            md.OriginalFilename = Path.GetFileNameWithoutExtension(videoPath);
            md.FullPath = videoPath;
            md.ImageSize = imageSize;
            md.CaptureFramerate = (float)calibrationHelper.CaptureFramesPerSecond;
            md.UserFramerate = (float)(1000.0 / userInterval);

            MeasuredDataUnits mdu = new MeasuredDataUnits();

            if (PreferencesManager.PlayerPreferences.ExportSpace == ExportSpace.WorldSpace)
            { 
                mdu.LengthUnit = CalibrationHelper.LengthUnit.ToString();
                mdu.LengthSymbol = UnitHelper.LengthAbbreviation(CalibrationHelper.LengthUnit);
            }
            else
            {
                mdu.LengthUnit = LengthUnit.Pixels.ToString();
                mdu.LengthSymbol = UnitHelper.LengthAbbreviation(LengthUnit.Pixels);
            }
            
            mdu.SpeedUnit = PreferencesManager.PlayerPreferences.SpeedUnit.ToString();
            mdu.SpeedSymbol = UnitHelper.SpeedAbbreviation(PreferencesManager.PlayerPreferences.SpeedUnit);
            mdu.AccelerationUnit = PreferencesManager.PlayerPreferences.AccelerationUnit.ToString();
            mdu.AccelerationSymbol = UnitHelper.AccelerationAbbreviation(PreferencesManager.PlayerPreferences.AccelerationUnit);
            mdu.AngleUnit = PreferencesManager.PlayerPreferences.AngleUnit.ToString();
            mdu.AngleSymbol = UnitHelper.AngleAbbreviation(PreferencesManager.PlayerPreferences.AngleUnit);
            mdu.AngularVelocityUnit = PreferencesManager.PlayerPreferences.AngularVelocityUnit.ToString();
            mdu.AngularVelocitySymbol = UnitHelper.AngularVelocityAbbreviation(PreferencesManager.PlayerPreferences.AngularVelocityUnit);
            mdu.AngularAccelerationUnit = PreferencesManager.PlayerPreferences.AngularAccelerationUnit.ToString();
            mdu.AngularAccelerationSymbol = UnitHelper.AngularAccelerationAbbreviation(PreferencesManager.PlayerPreferences.AngularAccelerationUnit);
            mdu.TimeSymbol = UnitHelper.TimeAbbreviation(PreferencesManager.PlayerPreferences.TimecodeFormat);
            md.Units = mdu;

            foreach (Keyframe kf in Keyframes.Where(kf => !kf.Disabled))
            {
                var mdkf = kf.CollectMeasuredData();
                md.Keyframes.Add(mdkf);

                List<MeasuredDataPosition> mdps = new List<MeasuredDataPosition>();
                List<MeasuredDataDistance> mdds = new List<MeasuredDataDistance>();
                List<MeasuredDataAngle> mdas = new List<MeasuredDataAngle>();
                foreach (AbstractDrawing d in kf.Drawings)
                {
                    // Positions from markers.
                    if (d is DrawingCrossMark)
                        mdps.Add(((DrawingCrossMark)d).CollectMeasuredData());

                    // Positions from postures.
                    if (d is DrawingGenericPosture)
                        mdps.AddRange(((DrawingGenericPosture)d).CollectMeasuredDataPositions());

                    // Distances from lines.
                    if (d is DrawingLine)
                        mdds.Add(((DrawingLine)d).CollectMeasuredData());

                    // Distances from postures.
                    if (d is DrawingGenericPosture)
                        mdds.AddRange(((DrawingGenericPosture)d).CollectMeasuredDataDistances());

                    // Angles from angle tools.
                    if (d is DrawingAngle)
                        mdas.Add(((DrawingAngle)d).CollectMeasuredData());

                    // Angles from postures.
                    if (d is DrawingGenericPosture)
                        mdas.AddRange(((DrawingGenericPosture)d).CollectMeasuredDataAngles());
                }

                // Sort drawings on the same keyframe by name.
                mdps.Sort((a, b) => a.Name.CompareTo(b.Name));
                mdds.Sort((a, b) => a.Name.CompareTo(b.Name));
                mdas.Sort((a, b) => a.Name.CompareTo(b.Name));

                // Inject time.
                foreach (MeasuredDataPosition mdp in mdps)
                    mdp.Time = mdkf.Time;
                foreach (MeasuredDataDistance mdd in mdds)
                    mdd.Time = mdkf.Time;
                foreach (MeasuredDataAngle mda in mdas)
                    mda.Time = mdkf.Time;

                // Add to the global list.
                md.Positions.AddRange(mdps);
                md.Distances.AddRange(mdds);
                md.Angles.AddRange(mdas);
            }

            // Times.
            foreach (DrawingChrono chrono in ChronoManager.Drawings)
                md.Times.Add(chrono.CollectMeasuredData());
            md.Times.Sort((a, b) => a.Start.CompareTo(b.Start));

            md.Timeseries = new List<MeasuredDataTimeseries>();
            
            // Tracks.
            foreach (DrawingTrack track in TrackManager.Drawings)
                md.Timeseries.Add(track.CollectMeasuredData());
            
            // Timelines.
            trackabilityManager.CollectMeasuredData(this, md.Timeseries);

            md.Timeseries.Sort((a, b) => a.FirstTimestamp.CompareTo(b.FirstTimestamp));

            return md;
        }

        /// <summary>
        /// Convert from timestamps to a numerical time format.
        /// If the preferred time format is not numeric we return seconds.
        /// TimeType.WorkingZone is not supported.
        /// </summary>
        public float GetNumericalTime(long timestamps, TimeType type)
        {
            TimecodeFormat tcf = PreferencesManager.PlayerPreferences.TimecodeFormat;
            
            // TimecodeFormat.Normalized is not supported at this point.
            if (tcf == TimecodeFormat.Normalized)
                tcf = TimecodeFormat.ClassicTime;

            long actualTimestamps = timestamps;
            if (type == TimeType.UserOrigin)
                actualTimestamps = timestamps - timeOrigin;

            // TODO: use double for info.AverageTimestampsPerFrame.
            //double averageTimestampsPerFrame = AverageTimeStampsPerSeconds / FramesPerSeconds;
            double averageTimestampsPerFrame = this.AverageTimeStampsPerFrame;

            float frames = 0;
            if (AverageTimeStampsPerFrame != 0)
                frames = (float)Math.Round(actualTimestamps / averageTimestampsPerFrame);

            if (type == TimeType.Duration)
                frames++;

            double milliseconds = frames * UserInterval / HighSpeedFactor;
            
            double time;
            switch (tcf)
            {
                case TimecodeFormat.Frames:
                    time = frames;
                    break;
                case TimecodeFormat.Milliseconds:
                    time = milliseconds;
                    break;
                case TimecodeFormat.Microseconds:
                    time = milliseconds * 1000;
                    break;
                case TimecodeFormat.TenThousandthOfHours:
                    // 1 Ten Thousandth of Hour = 360 ms.
                    time = Math.Round(milliseconds) / 360.0;
                    break;
                case TimecodeFormat.HundredthOfMinutes:
                    // 1 Hundredth of minute = 600 ms.
                    time = Math.Round(milliseconds) / 600.0;
                    break;
                case TimecodeFormat.Timestamps:
                    time = timestamps;
                    break;
                default:
                    time = Math.Round(milliseconds) / 1000.0;
                    break;
            }

            return (float)time;
        }

        public void PostSetup(bool init)
        {
            if (init)
            {
                trackabilityManager.Initialize(imageSize);
                calibrationHelper.Initialize(imageSize, GetCalibrationOrigin, GetCalibrationQuad, HasTrackingData);
            }

            if (!initialized)
            {
                for (int i = 0; i < totalStaticExtraDrawings; i++)
                    AfterDrawingCreation(extraDrawings[i]);
            }
            else
            {
                AfterDrawingCreation(drawingCoordinateSystem);
            }

            CleanupHash();
            initialized = true;
        }

        public void PostSetupCapture()
        {
            captureKVA = true;
            trackabilityManager.Initialize(imageSize);
            calibrationHelper.Initialize(imageSize, GetCalibrationOrigin, GetCalibrationQuad, HasTrackingData);

            for (int i = 0; i < totalStaticExtraDrawings; i++)
                AfterDrawingCreation(extraDrawings[i]);
        }

        public void Reset()
        {
            // Complete reset. (used when over loading a new video)
            log.Debug("Metadata Reset.");

            globalTitle = "";
            imageSize = new Size(0, 0);
            videoPath = "";
            lastKVAPath = "";
            averageTimeStampsPerFrame = 1;
            firstTimeStamp = 0;
            
            ResetCoreContent();
            autoSaver.Stop();
            EmptyTempDirectory();
            CleanupHash();
        }
        public void Close()
        {
            foreach (IVideoFilter filter in videoFilters.Values)
                filter.Dispose();

            DeleteTempDirectory();
        }
        public void ShowCoordinateSystem()
        {
            drawingCoordinateSystem.Visible = true;
        }
        public void UpdateTrajectoriesForKeyframes()
        {
            foreach (DrawingTrack t in Tracks())
                t.CalibrationChanged();
        }
        public void FixRelativeTrajectories()
        {
            foreach (DrawingTrack t in Tracks())
                t.FixRelativeTrajectories();
        }
        public void AllDrawingTextToNormalMode()
        {
            foreach (DrawingText label in Labels())
                label.SetEditMode(false, PointF.Empty, null);
        }
        public void PerformTracking(VideoFrame videoframe)
        {
            foreach(DrawingTrack t in Tracks())
                if (t.Status == TrackStatus.Edit)
                    t.TrackCurrentPosition(videoframe);
        }
        public void StopAllTracking()
        {
            foreach(DrawingTrack t in Tracks())
                t.StopTracking();
        }
        public void ClearTracking()
        {
            foreach (DrawingTrack t in Tracks())
                t.Clear();
        }

        public void UpdateTrackPoint(Bitmap bitmap)
        {
            // Happens when mouse up and editing a track.
            DrawingTrack t = hitDrawing as DrawingTrack;
            if(t != null && (t.Status == TrackStatus.Edit || t.Status == TrackStatus.Configuration))
                t.UpdateTrackPoint(bitmap);
        }
        public int GetContentHash()
        {
            int hash = 0;
            hash ^= ImageAspect.GetHashCode();
            hash ^= ImageRotation.GetHashCode();
            hash ^= Mirrored.GetHashCode();
            hash ^= Demosaicing.GetHashCode();
            hash ^= Deinterlacing.GetHashCode();
            hash ^= selectionStart.GetHashCode();
            hash ^= selectionEnd.GetHashCode();
            hash ^= timeOrigin.GetHashCode();
            hash ^= calibrationHelper.ContentHash;
            hash ^= GetKeyframesContentHash();
            hash ^= GetExtraDrawingsContentHash();
            hash ^= trackabilityManager.ContentHash;
            hash ^= GetVideoFiltersContentHash();
            return hash;
        }
        public void CleanupHash()
        {
            referenceHash = GetContentHash();
            autoSaver.Clear();
            log.Debug(String.Format("Metadata content hash reset:{0}.", referenceHash));
        }
        public void ResizeFinished()
        {
            // This function can be used to trigger an update to drawings that do not 
            // render in the same way when the user is resizing the window or not.
            foreach(DrawingSVG svg in SVGs())
                svg.ResizeFinished();
        }
        public void UnselectAll()
        {
            hitKeyframe = null;
            hitDrawing = null;
        }
        public void SelectDrawing(AbstractDrawing drawing)
        {
            hitDrawing = drawing;
        }
        private void AfterDrawingCreation(AbstractDrawing drawing)
        {
            // When passing here, it is possible that the drawing has already been initialized.
            // (for example, for undo of delete, paste or reload from KVA).

            if (string.IsNullOrEmpty(drawing.Name))
                SetDrawingName(drawing);

            if (drawing is IScalable)
                ((IScalable)drawing).Scale(this.ImageSize);

            if (drawing is ITrackable && AddTrackableDrawingCommand != null)
                AddTrackableDrawingCommand.Execute(drawing as ITrackable);

            if (drawing is IMeasurable)
            {
                IMeasurable measurableDrawing = drawing as IMeasurable;
                measurableDrawing.CalibrationHelper = calibrationHelper;

                measurableDrawing.InitializeMeasurableData(trackExtraData);
                measurableDrawing.ShowMeasurableInfoChanged += MeasurableDrawing_ShowMeasurableInfoChanged;
            }

            if (drawing is DrawingDistortionGrid)
            {
                DrawingDistortionGrid d = drawing as DrawingDistortionGrid;
                d.LensCalibrationAsked += LensCalibrationAsked;
            }
        }

        private void BeforeDrawingDeletion(AbstractDrawing drawing)
        {
            ITrackable trackableDrawing = drawing as ITrackable;
            if (trackableDrawing != null)
                DeleteTrackableDrawing(trackableDrawing);

            IMeasurable measurableDrawing = drawing as IMeasurable;
            if (measurableDrawing != null)
                measurableDrawing.ShowMeasurableInfoChanged -= MeasurableDrawing_ShowMeasurableInfoChanged;

            if (drawing is DrawingDistortionGrid)
                ((DrawingDistortionGrid)drawing).LensCalibrationAsked -= LensCalibrationAsked;
        }

        public void BeforeKVAImport()
        {
            kvaImporting = true;
            StopAllTracking();
            UnselectAll();
            
            memoCoordinateSystemId = drawingCoordinateSystem.Id;
        }
        public void AfterKVAImport()
        {
            trackabilityManager.UpdateId(memoCoordinateSystemId, drawingCoordinateSystem.Id);
            
            foreach (ITrackable drawing in TrackableDrawings())
                trackabilityManager.Assign(drawing);

            trackabilityManager.CleanUnassigned();
            
            AfterCalibrationChanged();
            kvaImporting = false;

            if (KVAImported != null)
                KVAImported(this, EventArgs.Empty);
        }
        public void AfterManualExport()
        {
            CleanupHash();
            DeleteAutosaveFile();
        }
        public bool Recover(Guid id)
        {
            DeleteTempDirectory();
            SetupTempDirectory(id);
            string autosaveFile = Path.Combine(tempFolder, "autosave.kva");
            bool recovered = false;
            if (File.Exists(autosaveFile))
            {
                MetadataSerializer s = new MetadataSerializer();
                s.Load(this, autosaveFile, true);
                recovered = true;
            }

            return recovered;
        }

        public List<List<PointF>> GetCameraCalibrationPoints()
        {
            List<List<PointF>> points = new List<List<PointF>>();
            foreach (DrawingDistortionGrid grid in DistortionGrids())
                points.Add(grid.Points);

            return points;
        }

        #region Autosave
        public void StartAutosave()
        {
            autoSaver.FreshStart();
        }
        public void PauseAutosave()
        {
            autoSaver.Stop();
        }
        public void UnpauseAutosave()
        {
            autoSaver.Start();
        }
        public void PerformAutosave()
        {
            MetadataSerializer serializer = new MetadataSerializer();
            serializer.SaveToFile(this, Path.Combine(tempFolder, "autosave.kva"));
        }
        #endregion
        
        #region Objects Hit Tests
        // Note: these hit tests are for right click only.
        // They work slightly differently than the hit test in the PointerTool which is for left click.
        // The main difference is that here we only need to know if the drawing was hit at all,
        // in the pointer tool, we need to differenciate which handle was hit.
        // For example, Tracks can here be handled with all other ExtraDrawings.
        public bool IsOnDrawing(int _iActiveKeyframeIndex, PointF point, long _iTimestamp)
        {
            // Returns whether the mouse is on a drawing attached to a key image.
            bool hit = false;
            
            if (PreferencesManager.PlayerPreferences.DefaultFading.Enabled && keyframes.Count > 0)
            {
                int[] zOrder = GetKeyframesZOrder(_iTimestamp);

                for(int i=0;i<zOrder.Length;i++)
                {
                    hit = DrawingsHitTest(zOrder[i], point, _iTimestamp);
                    if (hit)
                        break;
                }
            }
            else if (_iActiveKeyframeIndex >= 0)
            {
                // If fading is off, only try the current keyframe (if any)
                hit = DrawingsHitTest(_iActiveKeyframeIndex, point, _iTimestamp);
            }

            return hit;
        }
        public AbstractDrawing IsOnExtraDrawing(PointF point, long timestamp)
        {
            // Check if the mouse is on one of the drawings not attached to any key image.
            // Returns the drawing on which we stand (or null if none), and select it on the way.
            // the caller will then check its type and decide which action to perform.
            
            AbstractDrawing result = null;

            foreach (DrawingChrono chrono in chronoManager.Drawings)
            {
                int hit = chrono.HitTest(point, timestamp, calibrationHelper.DistortionHelper, imageTransform, imageTransform.Zooming);
                if (hit < 0)
                    continue;

                result = chrono;
                hitDrawing = chrono;
                break;
            }

            if (result != null)
                return result;

            foreach (DrawingTrack track in trackManager.Drawings)
            {
                int hit = track.HitTest(point, timestamp, calibrationHelper.DistortionHelper, imageTransform, imageTransform.Zooming);
                if (hit < 0)
                    continue;

                result = track;
                hitDrawing = track;
                break;
            }

            if (result != null)
                return result;

            for(int i = extraDrawings.Count - 1; i >= 0; i--)
            {
                AbstractDrawing candidate = extraDrawings[i];
                int hitRes = candidate.HitTest(point, timestamp, calibrationHelper.DistortionHelper, imageTransform, imageTransform.Zooming);
                if (hitRes < 0)
                    continue;
                
                result = candidate;
                hitDrawing = candidate;
                break;
            }
            
            return result;
        }
        public int[] GetKeyframesZOrder(long _iTimestamp)
        {
            // TODO:�turn this into an iterator.
            
            // Get the Z ordering of Keyframes for hit tests & draw.
            int[] zOrder = new int[keyframes.Count];

            if (keyframes.Count > 0)
            {
                if (_iTimestamp <= keyframes[0].Position)
                {
                    // All key frames are after this position
                    for(int i=0;i<keyframes.Count;i++)
                    {
                        zOrder[i] = i;
                    }
                }
                else if (_iTimestamp > keyframes[keyframes.Count - 1].Position)
                {
                    // All keyframes are before this position
                    for (int i = 0; i < keyframes.Count; i++)
                    {
                        zOrder[i] = keyframes.Count - i - 1;
                    }
                }
                else
                {
                    // Some keyframes are after, some before.
                    // Start at the first kf after this position until the end,
                    // then go backwards from the first kf before this position until the begining.

                    int iCurrentFrame = keyframes.Count;
                    int iClosestNext = keyframes.Count - 1;
                    while (iCurrentFrame > 0)
                    {
                        iCurrentFrame--;
                        if (keyframes[iCurrentFrame].Position >= _iTimestamp)
                        {
                            iClosestNext = iCurrentFrame;
                        }
                        else
                        {
                            break;
                        }
                    }

                    for(int i=iClosestNext;i<keyframes.Count;i++)
                    {
                        zOrder[i - iClosestNext] = i;
                    }
                    for (int i = 0; i < iClosestNext; i++)
                    {
                        zOrder[keyframes.Count - i - 1] = i;
                    }
                }
            }

            return zOrder;

        }
        #endregion

        #region Video filters
        public void ActivateVideoFilter(VideoFilterType type)
        {
            if (!videoFilters.ContainsKey(type))
                throw new InvalidProgramException();
            
            activeVideoFilterType = type;
        }

        public void DeactivateVideoFilter()
        {
            activeVideoFilterType = VideoFilterType.None;
        }

        public void WriteVideoFilters(XmlWriter w)
        {
            foreach (var pair in videoFilters)
            {
                string xmlName = VideoFilterFactory.GetName(pair.Key);
                w.WriteStartElement(xmlName);
                pair.Value.WriteData(w);
                w.WriteEndElement();
            }
        }

        public void ReadVideoFilters(XmlReader r)
        {
            bool isEmpty = r.IsEmptyElement;

            if (r.MoveToAttribute("active"))
                activeVideoFilterType = VideoFilterFactory.GetFilterType(r.ReadContentAsString());
            
            r.ReadStartElement();

            if (isEmpty)
                return;

            while (r.NodeType == XmlNodeType.Element)
            {
                VideoFilterType type = VideoFilterFactory.GetFilterType(r.Name);
                if (type == VideoFilterType.None)
                {
                    log.DebugFormat("Unsupported video filter: {0}", r.Name);
                    r.ReadOuterXml();
                    continue;
                }

                videoFilters[type].ReadData(r);
            }

            r.ReadEndElement();
        }
        #endregion

        #endregion

        #region Lower level Helpers
        private void ResetCoreContent()
        {
            // Semi reset: we keep Image size and AverageTimeStampsPerFrame
            trackabilityManager.Clear();
            keyframes.Clear();
            ClearTracking();
            trackManager.Clear();
            chronoManager.Clear();
            extraDrawings.RemoveRange(totalStaticExtraDrawings, extraDrawings.Count - totalStaticExtraDrawings);
            magnifier.ResetData();
            imageTransform.Reset();
            drawingCoordinateSystem.Visible = false;
            drawingTestGrid.Visible = false;
            
            // Do not reset the calibration when loading new files in the same screen.
            // The existing calibration is as good the default one.
            // This supports the scenario of setting up the calibration in a dedicated video and using it in all videos of the same folder,
            // without having to save and load a dedicated KVA file for it.
            // If the new file has its own calibration in the KVA it will still be loaded correctly later.
            
            ResetVideoFilters();
            
            foreach(AbstractDrawing extraDrawing in extraDrawings)
            {
                if(extraDrawing is AbstractMultiDrawing)
                    ((AbstractMultiDrawing)extraDrawing).Clear();
            }

            ImageAspect = ImageAspectRatio.Auto;
            ImageRotation = ImageRotation.Rotate0;
            Mirrored = false;
            Demosaicing = Demosaicing.None;
            Deinterlacing = false;

            UnselectAll();
        }
        private bool DrawingsHitTest(int keyFrameIndex, PointF mouseLocation, long timestamp)
        {
            // Look for a hit in all drawings of a particular Key Frame.
            // Important side effect : the drawing being hit becomes Selected. This is then used for right click menu.

            if (keyframes.Count == 0)
                return false;

            bool isOnDrawing = false;
            Keyframe keyframe = keyframes[keyFrameIndex];

            UnselectAll();
            
            int currentDrawing = 0;
            int hitResult = -1;
            while (hitResult < 0 && currentDrawing < keyframe.Drawings.Count)
            {
                AbstractDrawing drawing = keyframe.Drawings[currentDrawing];
                hitResult = drawing.HitTest(mouseLocation, timestamp, calibrationHelper.DistortionHelper, imageTransform, imageTransform.Zooming);
                
                if (hitResult < 0)
                {
                    currentDrawing++;
                    continue;
                }

                isOnDrawing = true;
                SelectDrawing(drawing);
                SelectKeyframe(keyframe);
            }

            return isOnDrawing;
        }
        private void SetDrawingName(AbstractDrawing drawing)
        {
            // Use a unique name based on drawing type.
            string toolDisplayName = drawing.ToolDisplayName;
            int index = 1;
            bool done = false;
            string name = "";
            while (!done)
            {
                name = string.Format("{0} {1}", toolDisplayName, index);
                if (!IsNameTaken(name))
                    break;

                index++;
            }

            drawing.Name = name;
        }
        private bool IsNameTaken(string name)
        {
            // Lookup all drawing names to find a match.
            return AttachedDrawings().Any(d => d.Name == name) || 
                   chronoManager.Drawings.Any(d => d.Name == name) ||
                   trackManager.Drawings.Any(d => d.Name == name);
        }
        private int GetKeyframesContentHash()
        {
            int hash = 0;
            foreach (Keyframe kf in keyframes)
                hash ^= kf.ContentHash;

            return hash;
        }
        private int GetExtraDrawingsContentHash()
        {
            int hash = 0;
            foreach (DrawingChrono chrono in chronoManager.Drawings)
                hash ^= chrono.ContentHash;

            foreach (DrawingTrack track in trackManager.Drawings)
                hash ^= track.ContentHash;

            foreach (AbstractDrawing ad in extraDrawings)
                hash ^= ad.ContentHash;

            return hash;
        }

        private int GetVideoFiltersContentHash()
        {
            int hash = 0;
            hash ^= activeVideoFilterType.GetHashCode();
            foreach (IVideoFilter filter in videoFilters.Values)
                hash ^= filter.ContentHash;

            return hash;
        }
        private void CreateStaticExtraDrawings()
        {
            // Add the static extra drawings.
            // These drawings are unique and not attached to any particular key image.
            
            spotlightManager = new SpotlightManager();
            autoNumberManager = new AutoNumberManager(ToolManager.GetStylePreset("AutoNumbers"));
            drawingCoordinateSystem = new DrawingCoordinateSystem(Point.Empty, ToolManager.GetStylePreset("CoordinateSystem"));
            drawingTestGrid = new DrawingTestGrid();

            extraDrawings.Add(spotlightManager);
            extraDrawings.Add(autoNumberManager);
            extraDrawings.Add(drawingCoordinateSystem);
            extraDrawings.Add(drawingTestGrid);

            // totalStaticExtraDrawings is used to differenciate between static extra drawings like multidrawing managers
            // and dynamic extra drawings like tracks and chronos.
            totalStaticExtraDrawings = extraDrawings.Count;
            
            spotlightManager.TrackableDrawingAdded += (s, e) =>
            {
                if(AddTrackableDrawingCommand != null) 
                    AddTrackableDrawingCommand.Execute(e.TrackableDrawing); 
            };
            
            spotlightManager.TrackableDrawingDeleted += (s, e) => DeleteTrackableDrawing(e.TrackableDrawing);
        }
        private void CalibrationHelper_CalibrationChanged(object sender, EventArgs e)
        {
            AfterCalibrationChanged();
        }
        private void AfterCalibrationChanged()
        {
            if (drawingCoordinateSystem.CalibrationHelper == null)
                drawingCoordinateSystem.CalibrationHelper = calibrationHelper;

            drawingCoordinateSystem.UpdateOrigin();
            calibrationChangedTemporizer.Call();
        }
        private void TracksCalibrationChanged()
        {
            foreach (DrawingTrack t in Tracks())
                t.CalibrationChanged();
        }
        private void MeasurableDrawing_ShowMeasurableInfoChanged(object sender, EventArgs<TrackExtraData> e)
        {
            trackExtraData = e.Value;
        }

        /// <summary>
        /// Returns the position of the coordinate system origin at the specified time.
        /// </summary>
        private PointF GetCalibrationOrigin(long time)
        {
            if (captureKVA)
                return imageSize.Center();
            
            // When using CalibrationLine and a tracked coordinate system, 
            // this function retrieves the coordinates origin based on the specified time.
            return trackabilityManager.GetLocation(drawingCoordinateSystem, "0", time);
        }

        /// <summary>
        /// Returns the calibration quad at the specified time.
        /// </summary>
        private QuadrilateralF GetCalibrationQuad(long time, CalibratorType calibratorType, Guid calibrationDrawingId)
        {
            if (captureKVA)
                return QuadrilateralF.GetUnitSquare();
            
            if (calibratorType == CalibratorType.None || calibrationDrawingId == Guid.Empty)
                throw new InvalidProgramException();


            QuadrilateralF quadImage = QuadrilateralF.GetUnitSquare();

            // Retrieve the image coordinates of the quad defining the calibration transform at the specified time.
            if (calibratorType == CalibratorType.Plane)
            {
                // Get the corners of the quad.
                PointF a = trackabilityManager.GetLocation(calibrationDrawingId, "0", time);
                PointF b = trackabilityManager.GetLocation(calibrationDrawingId, "1", time);
                PointF c = trackabilityManager.GetLocation(calibrationDrawingId, "2", time);
                PointF d = trackabilityManager.GetLocation(calibrationDrawingId, "3", time);

                quadImage = new QuadrilateralF(a, b, c, d);
            }
            else
            {
                PointF a = trackabilityManager.GetLocation(calibrationDrawingId, "a", time);
                PointF b = trackabilityManager.GetLocation(calibrationDrawingId, "b", time);

                // Create a fake quad just to transport the segment.
                // This will be turned into a real quad based on the calibration axis in the caller.
                quadImage = new QuadrilateralF(a, b, a, b);
            }

            return quadImage;
        }

        private bool HasTrackingData(Guid id)
        {
            return trackabilityManager.HasData(id);
        }

        private void CreateVideoFilters()
        {
            videoFilters.Add(VideoFilterType.Kinogram, VideoFilterFactory.CreateFilter(VideoFilterType.Kinogram, this));
        }

        private void ResetVideoFilters()
        {
            foreach (var filter in videoFilters.Values)
                filter.ResetData();
        }

        private void SetupTempDirectory(Guid id)
        {
            tempFolder = Path.Combine(Software.TempDirectory, id.ToString());
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);
        }
        private void DeleteTempDirectory()
        {
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);
        }
        private void EmptyTempDirectory()
        {
            DeleteAutosaveFile();
        }
        private void DeleteAutosaveFile()
        {
            string autosaveFile = Path.Combine(tempFolder, "autosave.kva");
            if (File.Exists(autosaveFile))
                File.Delete(autosaveFile);
        }
        private void LensCalibrationAsked(object sender, EventArgs e)
        {
            if (CameraCalibrationAsked != null)
                CameraCalibrationAsked(this, EventArgs.Empty);
        }
        #endregion
    }
}
