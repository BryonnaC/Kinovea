using Emgu.CV.CvEnum;
using Emgu.CV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AnalysystemTakeTwo
{
    public enum Template
    {
        SparkvueStart,
        SparkvueStop,
        HamburgerButton,
        ExportData,
    }

    public struct TemplateData
    {
        public Mat mat;
        public Dictionary<bool, int> thresholds;

        public TemplateData(string base64String, int? onScreenThreshold = null, int? offScreenThreshold = null)
        {
            mat = LoadMat(base64String);
            thresholds = new Dictionary<bool, int>()
            {
                { true, onScreenThreshold is int valueOn ? valueOn : 0 },
                { false, offScreenThreshold is int valueOff ? valueOff : 0 },
            };
        }

        public TemplateData(Mat mat, int? onScreenThreshold = null, int? offScreenThreshold = null)
        {
            this.mat = mat;
            thresholds = new Dictionary<bool, int>()
            {
                { true, onScreenThreshold is int valueOn ? valueOn : 0 },
                { false, offScreenThreshold is int valueOff ? valueOff : 0 },
            };
        }

        public int this[bool onScreen]
        {
            get { return thresholds[onScreen]; }
            set 
            {
                var temp = thresholds[onScreen];
                thresholds[onScreen] = value;
                if (thresholds[true] > thresholds[false])
                {
                    thresholds[onScreen] = temp;
                    // TODO: some form of error handling,
                    // reaching this block means the user screwed up
                    // and claims the target is/isn't on the screen when it isn't/is
                }
            }
        }

        public static Mat LoadMat(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String); // base64 string to u8 bytes

            Bitmap bitmap;
            using (MemoryStream ms = new MemoryStream(bytes)) // stream across the bytes
            {
                bitmap = (Bitmap)Image.FromStream(ms); // convert bytes to Bitmap
            }

            return bitmap.ToMatBgr(); // convert RGBA Bitmap to BGR Mat and return
        }
    }

    public struct TemplateDictionary : IEnumerable<KeyValuePair<Template, TemplateData>>
    {
        public Dictionary<Template, TemplateData> templates;

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)templates).GetEnumerator();
        }

        IEnumerator<KeyValuePair<Template, TemplateData>> IEnumerable<KeyValuePair<Template, TemplateData>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Template, TemplateData>>)templates).GetEnumerator();
        }

        public void Add(Template template, TemplateData templateData)
        {
            if (templates == null) templates = new Dictionary<Template, TemplateData>();
            templates.Add(template, templateData);
        }

        public Mat this[Template key]
        {
            get { return templates[key].mat; }
        }

        public int this[Template templateKey, bool thresholdKey]
        {
            get { return templates[templateKey][thresholdKey]; }
        }
    }

    static class TemplateHandler
    {
        // converts Bitmap (RGBA) to Mat (BGR)
        public static Mat ToMatBgr(this Bitmap bitmap)
        {
            Mat matBgr = new Mat();
            CvInvoke.CvtColor(bitmap.ToMat(), matBgr, ColorConversion.Rgba2Bgr);

            return matBgr;
        }  
    }
}
