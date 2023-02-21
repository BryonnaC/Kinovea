using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTranslation
{
    class MatrixMath
    {
        double aFx = 256.98;
        double aFy = -201.43;
        
        double bFx = 98.80;
        double bFy = -631.48;
        
        double cFx = 422.57;
        double cFy = -574.63;
        
        double dFx = 254.40;
        double dFy = -315.12;

        double aSx = -516.62;
        double aSy = -201.43;

        double bSx = -588.29;
        double bSy = -572.16;

        double cSx = -373.27;
        double cSy = -648.78;

        double dSx = -499.32;
        double dSy = -305.24;

        List<double> frontPx = new List<double> { };
        List<double> frontPy = new List<double> { };
        List<double> sidePx = new List<double> { };
        List<double> sidePy = new List<double> { };

        List<double> frontSideX = new List<double> { };
        List<double> frontSideY = new List<double> { };

        double[,] NC1;

        //This is just a testing method, should be able to get positions from Kinovea tracking
        public void InitLists()
        {
            frontSideX.Add(aFx);
            frontSideX.Add(bFx);
            frontSideX.Add(cFx);
            frontSideX.Add(dFx);
            frontSideX.Add(aSx);
            frontSideX.Add(bSx);
            frontSideX.Add(cSx);
            frontSideX.Add(dSx);

            frontSideY.Add(aFy);
            frontSideY.Add(bFy);
            frontSideY.Add(cFy);
            frontSideY.Add(dFy);
            frontSideY.Add(aSy);
            frontSideY.Add(bSy);
            frontSideY.Add(cSy);
            frontSideY.Add(dSy);

        }

        public void ImitateMATLAB()
        {
            double scaledX;
            double scaledY;
            double centeredX;
            double centeredY;

            InitLists();
            scaledX = ScalePoints(frontSideX);
            scaledY = ScalePoints(frontSideY);

            centeredX = CenterPoints(frontSideX);
            centeredY = CenterPoints(frontSideY);

            CreateNC1Matrix(scaledX, scaledY, centeredX, centeredY);

            MatrixMultiplication(NC1, aFx, aFy);
            MatrixMultiplication(NC1, bFx, bFy);
            MatrixMultiplication(NC1, cFx, cFy);
            MatrixMultiplication(NC1, dFx, dFy);

        }

        public void CreateNC1Matrix(double scalePx, double scalePy, double centerPx, double centerPy)
        {
            #region Just for me to visualize
            double[] row1 = new double[] { scalePx, 0, -(centerPx * scalePx) };
            double[] row2 = new double[] { 0, scalePy, -(centerPy * scalePy) };
            double[] row3 = new double[] { 0, 0, 1 };
            #endregion

            NC1 = new double[3, 3]
            {
                { scalePx, 0, -(centerPx * scalePx) },
                { 0, scalePy, -(centerPy * scalePy) },
                { 0, 0, 1 }
            };

/*            Console.WriteLine("NC1 Matrix: \n");
            for(int i = 0; i < 3; i++)
            {
                for(int j=0; j<3; j++)
                {
                    Console.WriteLine(NC1[i, j]);
                }
            }*/
        }

        public void MatrixMultiplication(double[,] nc1, double pointX, double pointY)
        {
            double[,] someF = new double[3, 1]{
                {0},
                {0},
                {0}
            };

            for(int i=0; i<3; i++)
            {
                someF[i, 0] = ((nc1[i, 0] * pointX) + (nc1[i, 1] * pointY) + (nc1[i, 2] * 1));
            }

            for(int j=0; j<3; j++)
            {
                Console.WriteLine(someF[j, 0]);
            }
        }

        public double CenterPoints(List<double> points)
        {
            double centeredValue;
            double aggregateValue = 0;

            for(int i = 0; i < points.Count; i++)
            {
                aggregateValue += points[i];
            }

            centeredValue = (aggregateValue / points.Count);
            //Console.WriteLine("centered: " centeredValue + "\n");

            return centeredValue;
        }

        //pass in all front X's and side X's OR all front Y's and side Y's
        public double ScalePoints(List<double> points)
        {
            double maxPoint;
            double minPoint;
            double scaledValue;

            maxPoint = GetMax(points);
            minPoint = GetMin(points);

            scaledValue = (1 / (maxPoint - minPoint));
            //Console.WriteLine("scaled: " + scaledValue + "\n");

            return scaledValue;
        }

        public double GetMax(List<double> points)
        {
            return points.Max();
        }

        public double GetMin(List<double> points)
        {
            return points.Min();
        }

       
    }
}
