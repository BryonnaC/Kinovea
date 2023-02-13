using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using static System.Math;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Analysistem
{
    struct Marker
    {
        public double x;
        public double y;

        public Marker(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    struct Markers
    {
        public List<Marker> frontMarkers;
        public List<Marker> sideMarkers;

        public Markers(List<Marker> frontMarkers, List<Marker> sideMarkers)
        {
            this.frontMarkers = frontMarkers;
            this.sideMarkers = sideMarkers;
        }

        public Matrix<double> GetNormalizeAndCentralizeMatrix()
        {
            List<double> markersX = frontMarkers.Select(_ => _.x).ToList();
            markersX.AddRange(sideMarkers.Select(_ => _.x).ToList());
            List<double> markersY = frontMarkers.Select(_ => _.y).ToList();
            markersY.AddRange(sideMarkers.Select(_ => _.y).ToList());

            double scalePX = 1 / (markersX.Max() - markersY.Min());
            double centerPX = markersX.Average();
            double scalePY = 1 / (markersY.Max() - markersX.Min());
            double centerPY = markersY.Average();

            return DenseMatrix.OfArray(new double[,]
            {
                { scalePX, 0, -centerPX * scalePX },
                { 0, scalePY, -centerPY * scalePY },
                { 0, 0, 1 }
            });
        }

        private List<Marker> NormalizeAndCentralizeCoords(Matrix<double> normAndCentMatrix, List<Marker> markers)
        {
            return markers.Select((marker) =>
            {
                Matrix<double> NormAndCentMarker = normAndCentMatrix.Multiply(DenseMatrix.OfArray(new double[,]
                {
                    { marker.x },
                    { marker.y },
                    { 1 }
                }));

                return new Marker(NormAndCentMarker.At(0, 0), NormAndCentMarker.At(1, 0));
            }).ToList();
        }

        public void NormalizeAndCentralizeCoords()
        {
            Matrix<double> NormAndCentMatrix = GetNormalizeAndCentralizeMatrix();

            frontMarkers = NormalizeAndCentralizeCoords(NormAndCentMatrix, frontMarkers);
            sideMarkers = NormalizeAndCentralizeCoords(NormAndCentMatrix, sideMarkers);
        }
    }
}