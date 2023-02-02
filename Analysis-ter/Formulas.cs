using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Analysistem
{
    //struct Marker
    //{
    //    public float x;
    //    public float y;

    //    public Marker(float x, float y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }
    //}

    struct Markers
    {
        public List<Tuple<double, double>> frontMarkers;
        public List<Tuple<double, double>> sideMarkers;

        public Markers(List<Tuple<double, double>> frontMarkers, List<Tuple<double, double>> sideMarkers)
        {
            this.frontMarkers = frontMarkers;
            this.sideMarkers = sideMarkers;
        }

        public Matrix<double> GetNormalizeAndCentralizeMatrix()
        {
            List<double> frontMarkersX = frontMarkers.Select(_ => _.Item1).ToList();
            List<double> frontMarkersY = frontMarkers.Select(_ => _.Item2).ToList();
            List<double> sideMarkersX = frontMarkers.Select(_ => _.Item1).ToList();
            List<double> sideMarkersY = frontMarkers.Select(_ => _.Item2).ToList();
            List<double> markersX = frontMarkersX.AddRange(sideMarkersX);
            List<double> markersY = frontMarkersY.AddRange(sideMarkersY);

            double scalePX = 1 / (Math.Max(markersX) - Math.Min(markersY);
            double centerPX = markersX.Average();
            double scalePY = 1 / (Math.Max(markersY) - Math.Min(markersX);
            double centerPY = markersY.Average();

            return DenseMatrix.OfArray(new double[,]
            {
                { scalePX, 0, -centerPX * scalePX },
                { 0, scalePY, -centerPY * scalePY },
                { 0, 0, 1 }
            });
        }

        private List<Tuple<double, double>> NormalizeAndCentralizeCoords(List<Tuple<double, double>> markers)
        {
            return markers.Select((marker) =>
             {
                 Matrix<double> NormAndCentMarker = NormAndCentMatrix.Multiply(DenseMatrix.OfArray(new double[,]
                 {
                    { marker.Item1 },
                    { marker.Item2 },
                    { 1 }
                 }));

                 return (NormAndCentMarker[0], NormAndCentMarker[1]);
             })
        }

        public void NormalizeAndCentralizeCoords()
        {
            Matrix<double> NormAndCentMatrix = GetNormalizeAndCentralizeMatrix();

            frontMarkers = NormalizeAndCentralizeCoords(frontMarkers);
            sideMarkers = NormalizeAndCentralizeCoords(sideMarkers);
        }
    }
}