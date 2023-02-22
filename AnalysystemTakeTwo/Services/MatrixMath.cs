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

        static double BFx = -312;
        static double BFy = 0;
        static double BFz = 22;
        static double CFx = BFx-410;
        static double CFy = 0;
        static double CFz = BFz;
        static double AFx = ((BFx+CFx)/2);
        static double AFy = 0;
        static double AFz = 350 + BFz;
        static double DFx = ((BFx + CFx) / 2);
        static double DFy = 0;
        static double DFz = 248 + BFz;

        static double CSx = 0;
        static double CSy = -210;
        static double CSz = 18;
        static double BSx = 0;
        static double BSy = CSy-408;
        static double BSz = CSz;
        static double ASx = 0;
        static double ASy = (BSy+CSy)/2;
        static double ASz = 357+CSz;
        static double DSx = 0;
        static double DSy = (BSy+CSy)/2;
        static double DSz = 260 + CSz;

        List<double> frontPx = new List<double> { };
        List<double> frontPy = new List<double> { };
        List<double> sidePx = new List<double> { };
        List<double> sidePy = new List<double> { };

        List<double> frontSideX = new List<double> { };
        List<double> frontSideY = new List<double> { };

        List<double> globalFSX = new List<double> { };
        List<double> globalFSY = new List<double> { };

        double[,] NC1;
        double[,] NC2;

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

            globalFSX.Add(AFx);
            globalFSX.Add(BFx);
            globalFSX.Add(CFx);
            globalFSX.Add(DFx);
            globalFSX.Add(ASx);
            globalFSX.Add(BSx);
            globalFSX.Add(CSx);
            globalFSX.Add(DSx);

            globalFSY.Add(AFy);
            globalFSY.Add(BFy);
            globalFSY.Add(CFy);
            globalFSY.Add(DFy);
            globalFSY.Add(ASy);
            globalFSY.Add(BSy);
            globalFSY.Add(CSy);
            globalFSY.Add(DSy);
        }

        public void ImitateMATLAB()
        {
            double scaledpX;
            double scaledpY;
            double centeredpX;
            double centeredpY;

            double scaledX;
            double scaledY;
            double centeredX;
            double centeredY;

            InitLists();
            scaledpX = ScalePoints(frontSideX);
            scaledpY = ScalePoints(frontSideY);

            centeredpX = CenterPoints(frontSideX);
            centeredpY = CenterPoints(frontSideY);

            CreateNC1Matrix(scaledpX, scaledpY, centeredpX, centeredpY);

            Console.WriteLine("\nMatrix One Results - pixel positions");
            MatrixMultiplication3x3(NC1, aFx, aFy);
            MatrixMultiplication3x3(NC1, bFx, bFy);
            MatrixMultiplication3x3(NC1, cFx, cFy);
            MatrixMultiplication3x3(NC1, dFx, dFy);

            scaledX = ScalePoints(globalFSX);
            scaledY = ScalePoints(globalFSY);

            centeredX = CenterPoints(globalFSX);
            centeredY = CenterPoints(globalFSY);

            CreateNC2Matrix(scaledX, scaledY, centeredX, centeredY);

            Console.WriteLine("\nMatrix Two Results - global positions");
            MatrixMultiplication4x4(NC2, AFx, AFy, AFz);
            MatrixMultiplication4x4(NC2, BFx, BFy, BFz);
            MatrixMultiplication4x4(NC2, CFx, CFy, CFz);
            MatrixMultiplication4x4(NC2, DFx, DFy, DFz);
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

        public void CreateNC2Matrix(double scalex, double scaley, double centerx, double centery)
        {
            NC2 = new double[4, 4]
{
                { scalex, 0, 0, -(centerx * scalex) },
                { 0, scaley, 0, -(centery * scaley) },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
};
        }

        public void MatrixMultiplication3x3(double[,] nc1, double pointX, double pointY)
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

        public void MatrixMultiplication4x4(double[,] nc2, double pointX, double pointY, double pointZ)
        {
            double[,] someF = new double[4, 1]{
                {0},
                {0},
                {0},
                {0}
            };

            for (int i = 0; i < 4; i++)
            {
                someF[i, 0] = ((nc2[i, 0] * pointX) + (nc2[i, 1] * pointY) + (nc2[i, 2] * pointZ) + (nc2[i,3]*1));
            }

            for (int j = 0; j < 4; j++)
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

            //maxPoint = GetMax(points);
            maxPoint = points.Max();
            minPoint = points.Min();

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
