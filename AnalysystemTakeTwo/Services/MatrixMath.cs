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

        List<double> pixelPositionsNormalized;
        List<double> globalPositionsNormalized;

        List<double> sidePixelPosNormed;
        List<double> frontPixelPosNormed;
        List<double> sideGlobalPosNormed;
        List<double> frontGlobalPosNormed;

        double[,] matrixT;

        #region test method
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
#endregion

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

            sidePixelPosNormed = new List<double>();
            frontPixelPosNormed = new List<double>();
            sideGlobalPosNormed = new List<double>();
            frontGlobalPosNormed = new List<double>();

            InitLists();
            scaledpX = ScalePoints(frontSideX);
            scaledpY = ScalePoints(frontSideY);

            centeredpX = CenterPoints(frontSideX);
            centeredpY = CenterPoints(frontSideY);

            CreateNC1Matrix(scaledpX, scaledpY, centeredpX, centeredpY);

            //Console.WriteLine("\nMatrix One Results - pixel positions");
            frontPixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, aFx, aFy));
            frontPixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, bFx, bFy));
            frontPixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, cFx, cFy));
            frontPixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, dFx, dFy));
            sidePixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, aSx, aSy));
            sidePixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, bSx, bSy));
            sidePixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, cSx, cSy));
            sidePixelPosNormed.AddRange(MatrixMultiplicationPixel(NC1, dSx, dSy));

            scaledX = ScalePoints(globalFSX);
            scaledY = ScalePoints(globalFSY);

            centeredX = CenterPoints(globalFSX);
            centeredY = CenterPoints(globalFSY);

            CreateNC2Matrix(scaledX, scaledY, centeredX, centeredY);

            //Console.WriteLine("\nMatrix Two Results - global positions");
            frontGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, AFx, AFy, AFz));
            frontGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, BFx, BFy, BFz));
            frontGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, CFx, CFy, CFz));
            frontGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, DFx, DFy, DFz));
            sideGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, ASx, ASy, ASz));
            sideGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, BSx, BSy, BSz));
            sideGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, CSx, CSy, CSz));
            sideGlobalPosNormed.AddRange(MatrixMultiplicationGlobal(NC2, DSx, DSy, DSz));

            matrixT = HomgraphicMatrix(frontGlobalPosNormed, frontPixelPosNormed, sideGlobalPosNormed, sidePixelPosNormed);
            TransposeMatrix(matrixT, 16, 11);
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

        private List<double> SetNewValues(double[,] newMatrix, List<double> points)
        {
            for(int i=0; i<points.Count; i++)
            {
                points[i] = newMatrix[i, 0];
                //Console.WriteLine(points[i]);
            }

            return points;
        }

        public List<double> MatrixMultiplicationPixel(double[,] nc1, double pointX, double pointY)
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

/*            for(int j=0; j<3; j++)
            {
                Console.WriteLine(someF[j, 0]);
            }*/

            List<double> points = new List<double> { pointX, pointY };

            return SetNewValues(someF, points);
        }

        public List<double> MatrixMultiplicationGlobal(double[,] nc2, double pointX, double pointY, double pointZ)
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

/*            for (int j = 0; j < 4; j++)
            {
                Console.WriteLine(someF[j, 0]);
            }*/

            List<double> points = new List<double> { pointX, pointY, pointZ };

            return SetNewValues(someF, points);
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

        //Holy shit this thing is HUGE
        /*
         * T=[
         AFx AFy AFz 1 0 0 0 0 AFx*aFx AFy*aFx AFz*aFx;
         0 0 0 0 AFx AFy AFz 1 -AFx*aFy -AFy*aFy -AFz*aFy;
         BFx BFy BFz 1 0 0 0 0 BFx*bFx BFy*bFx BFz*bFx;
         0 0 0 0 BFx BFy BFz 1 -BFx*bFy -BFy*bFy -BFz*bFy;
         CFx CFy CFz 1 0 0 0 0 CFx*cFx CFy*cFx CFz*cFx;
         0 0 0 0 CFx CFy CFz 1 -CFx*cFy -CFy*cFy -CFz*cFy;
         DFx DFy DFz 1 0 0 0 0 DFx*dFx DFy*dFx DFz*dFx;
         0 0 0 0 DFx DFy DFz 1 -DFx*dFy -DFy*dFy -DFz*dFy;
         ASx ASy ASz 1 0 0 0 0 ASx*aSx ASy*aSx ASz*aSx;
         0 0 0 0 ASx ASy ASz 1 -ASx*aSy -ASy*aSy -ASz*aSy;
         BSx BSy BSz 1 0 0 0 0 BSx*bSx BSy*bSx BSz*bSx;
         0 0 0 0 BSx BSy BSz 1 -BSx*bSy -BSy*bSy -BSz*bSy;
         CSx CSy CSz 1 0 0 0 0 CSx*cSx CSy*cSx CSz*cSx;
         0 0 0 0 CSx CSy CSz 1 -CSx*cSy -CSy*cSy -CSz*cSy;
         DSx DSy DSz 1 0 0 0 0 DSx*dSx DSy*dSx DSz*dSx;
         0 0 0 0 DSx DSy DSz 1 -DSx*dSy -DSy*dSy -DSz*dSy; 
         ];*/

        private double[,] HomgraphicMatrix(List<double> globalsFront, List<double> pixelsFront, List<double> globalsSide, List<double> pixelsSide)
        {
            List<double> globalPts = new List<double>();
            globalPts.AddRange(globalsFront);
            globalPts.AddRange(globalsSide);

            List<double> pixelPts = new List<double>();
            pixelPts.AddRange(pixelsFront);
            pixelPts.AddRange(pixelsSide);

            double[,] homGraphT = new double[16, 11];

            int globalPtsTracker = 0;

            //so it looks like, if the row is even it starts with points
            //if row is odd, starts with 0 0 0 0 and ends with 1
            //also if row is even, globals are multiplied by pixel x's
            //whereas if row is odd, globals are multiplied by pixel y's

            for(int row = 0; row < 16; row++)
            {
                if (row > 1 && (row%2==0))
                {
                    globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                }
                for(int column = 0; column < 11; column++)
                {
                    if (row % 2 == 0)
                    {
                        if(column < 3)
                        {
                            homGraphT[row, column] = globalPts[globalPtsTracker];
                            globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                        }
                        else if(column == 3)
                        {
                            homGraphT[row, column] = 1;
                        }
                        else if(3 < column && column <= 7)
                        {
                            homGraphT[row, column] = 0;
                        }
                        else if(column > 7)
                        {
                            homGraphT[row, column] = globalPts[globalPtsTracker] * pixelPts[row];
                            globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                        }
                        //Console.WriteLine(globalPtsTracker);
                    }
                    else if (row % 2 == 1)
                    {
                        if (column < 4)
                        {
                            homGraphT[row, column] = 0;
                        }
                        else if (4 <= column && column < 7)
                        {
                            homGraphT[row, column] = globalPts[globalPtsTracker];
                            globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                        }
                        else if (column == 7)
                        {
                            homGraphT[row, column] = 1;
                        }
                        else if (column > 7)
                        {
                            homGraphT[row, column] = -(globalPts[globalPtsTracker]) * pixelPts[row];
                            if(column != 10)
                            {
                                globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                            }
                        }
                    }

                    //globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                }
                //globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
            }

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    Console.WriteLine(homGraphT[i, j]);
                }
            }
            Console.WriteLine("\nEND HOMGRAPH\n");

            return homGraphT;
        }

        private int GlobalPtIdxHelper(int row, int currentIdx)
        {
            //AF - 0,1,2
            //BF - 3,4,5
            //etc
            int baseIdx = 0;

            if(((currentIdx + 1) % 3) == 0)
            {
                switch (row)
                {
                    case 0:
                    case 1:
                        baseIdx = 0;
                        break;
                    case 2:
                    case 3:
                        baseIdx = 3;
                        break;
                    case 4:
                    case 5:
                        baseIdx = 6;
                        break;
                    case 6:
                    case 7:
                        baseIdx = 9;
                        break;
                    case 8:
                    case 9:
                        baseIdx = 12;
                        break;
                    case 10:
                    case 11:
                        baseIdx = 15;
                        break;
                    case 12:
                    case 13:
                        baseIdx = 18;
                        break;
                    case 14:
                    case 15:
                        baseIdx = 21;
                        break;
                }
                return baseIdx;
            }
            else
            {
                return (currentIdx+1);
            }
        }

        private void TransposeMatrix(double[,] originalMatrix, int origRows, int origColumns)
        {
            double[,] transposeMatrix = new double[origColumns, origRows];

            for(int i=0; i < origColumns; i++)
            {
                for(int j=0; j < origRows; j++)
                {
                    transposeMatrix[i, j] = originalMatrix[j, i];
                }
            }

            for (int i = 0; i < origColumns; i++)
            {
                for (int j = 0; j < origRows; j++)
                {
                    Console.WriteLine(transposeMatrix[i, j]);
                }
            }
        }
    }
}
