using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixInverse;

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
        double[,] matrixT_Transposed;

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

            double[,] matrixU = new double[11, 1];
            matrixU = CalculateMatrix_U(matrixT);
            Console.WriteLine("\n");

            double[,] matrixH = new double[3, 4];
            matrixH = CalculateMatrix_H(matrixU);

            double[,] matrixR = new double[3, 3];
            matrixR = globalToCameraTheta(matrixH);
        }

        private double[,] CalculateMatrix_H(double[,] matrixU)
        {
            double[,] matrixH;
            double[][] nc1_std = new double[3][];
            nc1_std[0] = new double[] { NC1[0, 0], NC1[0, 1], NC1[0, 2] };
            nc1_std[1] = new double[] { NC1[1, 0], NC1[1, 1], NC1[1, 2] };
            nc1_std[2] = new double[] { NC1[2, 0], NC1[2, 1], NC1[2, 2] };

            //invert the NC1
            double[,] inversedNC1 = new double[nc1_std.Length, 1];
            inversedNC1 = UseMcCaffreyMatrixInverse(nc1_std);

            //make new matrix from U values according to specs
            double[,] specialUMatrix = new double[3, 4]
            {
                { matrixU[0,0], matrixU[1,0], matrixU[2,0], matrixU[3,0] },
                { matrixU[4,0], matrixU[5,0], matrixU[6,0], matrixU[7,0] },
                { matrixU[8,0], matrixU[9,0], matrixU[10,0], 1 }
            };

            //now multiply this U matrix by NC2
            double[,] uByNC2 = new double[specialUMatrix.GetLength(0),NC2.GetLength(1)];
            uByNC2 = MatrixMultiplication(NC2, specialUMatrix);

            //and finally multiply that product by the inversed nc1
            matrixH = MatrixMultiplication(uByNC2, inversedNC1);

            return matrixH;
        }

        private double[,] globalToCameraTheta(double[,] h)
        {
            double theta1;
            double theta2;
            double theta3;

            theta3 = Math.Atan2(h[1,0], h[0,0]);
            theta1 = Math.Atan2(h[2, 1], h[2, 2]);
            theta2 = Math.Atan2(-(h[2, 0]), (h[2, 2] / Math.Cos(theta1)));

            double[,] matrixR = new double[3, 3]
            {
                { Math.Cos(theta2)*Math.Cos(theta3), -Math.Sin(theta3)*Math.Cos(theta1)+Math.Sin(theta1)*Math.Sin(theta2)*Math.Cos(theta3), Math.Sin(theta1)*Math.Sin(theta3)+Math.Cos(theta1)*Math.Sin(theta2)*Math.Cos(theta3) },
                { Math.Cos(theta2)*Math.Sin(theta3), Math.Cos(theta1)*Math.Cos(theta3)+Math.Sin(theta1)*Math.Sin(theta2)*Math.Sin(theta3), -Math.Sin(theta1)*Math.Cos(theta3)+Math.Cos(theta1)*Math.Sin(theta2)*Math.Sin(theta3) },
                { -Math.Sin(theta2), Math.Sin(theta1)*Math.Cos(theta2), Math.Cos(theta1)*Math.Cos(theta2) }
            };
            return matrixR;
        }

/*        R=[cosd(theta2)*cosd(theta3) -sind(theta3)*cosd(theta1)+sind(theta1)*sind(theta2)*cosd(theta3) sind(theta1)*sind(theta3)+cosd(theta1)*sind(theta2)*cosd(theta3);
             cosd(theta2)*sind(theta3) cosd(theta1)*cosd(theta3)+sind(theta1)*sind(theta2)*sind(theta3) -sind(theta1)*cosd(theta3)+cosd(theta1)*sind(theta2)*sind(theta3);
             -sind(theta2) sind(theta1)*cosd(theta2) cosd(theta1)*cosd(theta2)
        ];*/

        private double[,] CalculateMatrix_U(double[,] matrixT)
        {
            //find the transpose of matrix T
            matrixT_Transposed = TransposeMatrix(matrixT, 16, 11);

            //multiply the transpose and T together
            double[,] tCrossTtranspose = MatrixMultiplication(matrixT, matrixT_Transposed);

            //take the inverse of that product
            //but first change type to match matrix inverse found in Visual Studio Magazine
            double[][] standardMatrix = new double[tCrossTtranspose.GetLength(0)][];
            standardMatrix = ChangeArrayType(tCrossTtranspose);

            //then change that matrix back to my preferred format - unneccsary step but the problem is that I'd have to change my initial code and that's not right now's problem
            double[,] inversedProduct = new double[standardMatrix.Length, standardMatrix[0].Length];
            inversedProduct = UseMcCaffreyMatrixInverse(standardMatrix);

            //don't forget to multiple T' by the pixel position 16x1 matrix
            double[,] transposedXpixelpos = new double[inversedProduct.GetLength(0), 1];
            List<double> allPixelsNormed = new List<double>();
            allPixelsNormed.AddRange(frontPixelPosNormed);
            allPixelsNormed.AddRange(sidePixelPosNormed);
            transposedXpixelpos = TransposeCrossPixels(matrixT_Transposed, allPixelsNormed);

            //now multiply that by the inversed product
            return MatrixMultiplication(transposedXpixelpos, inversedProduct);
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
                Console.WriteLine("\n");
                for (int j = 0; j < 11; j++)
                {
                    Console.Write(homGraphT[i, j] + " ");
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

        private double[,] TransposeMatrix(double[,] originalMatrix, int origRows, int origColumns)
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
                Console.WriteLine("\n");
                for (int j = 0; j < origRows; j++)
                {
                    Console.Write(transposeMatrix[i, j] + " ");
                }
            }

            return transposeMatrix;
            //CheckLength(originalMatrix);
            //CheckLength(transposeMatrix);
            //MatrixMultiplication(originalMatrix, transposeMatrix);
        }

        private void CheckLength(double[,] originalMatrix)
        {
            int rows = originalMatrix.GetLength(0);
            int columns = originalMatrix.GetLength(1);

            Console.WriteLine(rows + " " + columns);
        }

        private double[,] MatrixMultiplication(double[,] originalMatrix, double[,] transposeMatrix)
        {
            int productRows = transposeMatrix.GetLength(0);
            int productColumns = originalMatrix.GetLength(1);
            double[,] productMatrix = new double[productRows, productColumns];
            
            //Console.WriteLine("\n A^T*A");
            for(int i=0; i < productRows; i++)
            {
                //Console.Write("\n");
                for(int j=0; j < productColumns; j++)
                {
                    for(int k=0; k<originalMatrix.GetLength(0); k++)
                    {
                        productMatrix[i, j] += (transposeMatrix[i, k] * originalMatrix[k, j]);
                    }
                    //Console.Write(productMatrix[i, j] + " ");
                }
            }
            return productMatrix;
        }
        // should this just be overloaded operators? maybe
        private double[,] TransposeCrossPixels(double[,] transposeMatrix, List<double> pixelPositions)
        {
            int productRows = transposeMatrix.GetLength(0);
            int productColumns = pixelPositions.Count;
            double[,] productMatrix = new double[productRows, productColumns];

            try
            {
                for (int i = 0; i < productRows; i++)
                {
                    Console.Write("\n");
                    for (int j = 0; j < productColumns; j++)
                    {
                        if (j % 2 == 0)
                        {
                            productMatrix[i, 0] += (transposeMatrix[i, j] * -(pixelPositions[j]));
                        }
                        else if (j % 2 == 1)
                        {
                            productMatrix[i, 0] += (transposeMatrix[i, j] * pixelPositions[j]);
                        }

                    }
                    Console.Write(productMatrix[i, 0] + " ");
                }
                return productMatrix;
            }
            catch
            {
                Console.WriteLine("Are product row and product column equal?");
            }
            return null;
        }

        private double[][] ChangeArrayType(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            double[][] stdMatrix = new double[rows][];

            for(int i=0; i<rows; i++)
            {
                stdMatrix[i] = new double[] { matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3], matrix[i, 4], matrix[i, 5], matrix[i, 6], matrix[i, 7], matrix[i, 8], matrix[i, 9], matrix[i, 10] };
            }

            return stdMatrix;
        }

        private double[,] ChangeArrayTypeBACK(double[][] matrix)
        {
            int rows = matrix.Length;
            int columns = matrix[0].Length;
            double[,] returnMatrix = new double[rows,columns];

            for(int i=0; i<rows; i++)
            {
                for(int j=0; j<columns; j++)
                {
                    returnMatrix[i, j] = matrix[i][j];
                }
            }

            return returnMatrix;
        }

        private double[,] UseMcCaffreyMatrixInverse(double[][] matrixToInvert)
        {
            double d = MatrixInverseProgram.MatDeterminant(matrixToInvert);
            if (Math.Abs(d) < 1.0e-5)
                Console.WriteLine("\nMatrix has no inverse");
            else
                Console.WriteLine("\nDet(m) = " + d.ToString("F4"));

            double[][] inv = MatrixInverseProgram.MatInverse(matrixToInvert);
            Console.WriteLine("\nInverse matrix inv is ");
            MatrixInverseProgram.MatShow(inv, 4, 8);

            //verify that it makes an identity matrix when crossed with original
            double[][] prod = MatrixInverseProgram.MatProduct(matrixToInvert, inv);
            Console.WriteLine("\nThe product of matrixToInvert * inv is ");
            MatrixInverseProgram.MatShow(prod, 1, 6);

            return ChangeArrayTypeBACK(inv);
        }

        #region Ignore_Me
        /*private int MatrixDecomposition(double[,] matrix, double[,] lum, int[] perm)
        {
            int toggle = +1;
            int n = matrix.GetLength(0); //n is row /and/ column bc it's a square matrix

            //make a copy of matrix into lum
            lum = new double[n, n];
            for (int i = 0; i<n; ++i)
            {
                for(int j=0; j<n; ++j)
                {
                    lum[i, j] = matrix[i, j];
                }
            }

            //make perm
            perm = new int[n];
            for(int i=0; i<n; ++i)
            {
                perm[i] = i;
            }

            for(int j=0; j<n-1; ++j)
            {
                double max = Math.Abs(lum[j, j]);
                int piv = j;

                for (int i = j + 1; i < n; ++i) // find pivot index
                {
                    double xij = Math.Abs(lum[i, j]);
                    if (xij > max)
                    {
                        max = xij;
                        piv = i;
                    }
                } // i

                if (piv != j)
                {
                    double[] tmp = lum[piv]; // swap rows j, piv
                    lum[piv] = lum[j];
                    lum[j] = tmp;

                    int t = perm[piv]; // swap perm elements
                    perm[piv] = perm[j];
                    perm[j] = t;

                    toggle = -toggle;
                }

                double xjj = lum[j,j];
                if (xjj != 0.0)
                {
                    for (int i = j + 1; i < n; ++i)
                    {
                        double xij = lum[i,j] / xjj;
                        lum[i,j] = xij;
                        for (int k = j + 1; k < n; ++k)
                            lum[i,k] -= xij * lum[j,k];
                    }
                }
            }

            return toggle;
        }*/
        // Function to get cofactor of A[p,q] in [,]temp. n is current
        // dimension of [,]A
        static void getCofactor(int[,] A, int[,] temp, int p, int q, int n)
        {
            int i = 0, j = 0;

            // Looping for each element of the matrix
            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    // Copying into temporary matrix only those element
                    // which are not in given row and column
                    if (row != p && col != q)
                    {
                        temp[i, j++] = A[row, col];

                        // Row is filled, so increase row index and
                        // reset col index
                        if (j == n - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
        }

        private void GetCofactor(double[,] matrix, int p, int q)
        {
            int n = matrix.GetLength(0);
            int i = 0;
            int j = 0;

            for (int row=0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    if(row != p && col != q)
                    {
                        
                    }
                }
            }

        }

        private void FindDeterminant(double[,] matrix, int n)
        {
            double[,] lum;
            int perm;
            double result = 0;
        }
        #endregion 

    }
}
