using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixInverse;
using Accord.Math;

namespace CodeTranslation
{
    class MatrixMath
    {
        #region Old Test Vals
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
        static double CFx = BFx - 410;
        static double CFy = 0;
        static double CFz = BFz;
        static double AFx = ((BFx + CFx) / 2);
        static double AFy = 0;
        static double AFz = 350 + BFz;
        static double DFx = ((BFx + CFx) / 2);
        static double DFy = 0;
        static double DFz = 248 + BFz;

        static double CSx = 0;
        static double CSy = -210;
        static double CSz = 18;
        static double BSx = 0;
        static double BSy = CSy - 408;
        static double BSz = CSz;
        static double ASx = 0;
        static double ASy = (BSy + CSy) / 2;
        static double ASz = 357 + CSz;
        static double DSx = 0;
        static double DSy = (BSy + CSy) / 2;
        static double DSz = 260 + CSz;
        #endregion

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

        //This is where the most up to date variables start for the most recent version of the code

        //Camera calibration specifications
        double k1 = 0.0136;
        double k2 = 0.0363;
        double fx, fy = 2060;

        //double[][] intrinsicMatrixGoPro = new double[3][];

        public void TakeInValues(List<string> horizVals, List<string> vertVals)
        {
            
            
            List<double> horizData = CsvStringToListDouble(horizVals);
            List<double> vertData = CsvStringToListDouble(vertVals);
            //now we need to decide whether we are calibrating or calculating

        }

        public void Calibrate(List<string> horizCali, List<string> vertCali)
        {
            InitGoProInMat();
        }

        private void InitGoProInMat()
        {
            // GoPro camera intrinsic matrix
            double[][] intrinsicMatrixGoPro = new double[3][];
            intrinsicMatrixGoPro[0] = new double[] { fx, 0, 0 };
            intrinsicMatrixGoPro[1] = new double[] { 0, fy, 0 };
            intrinsicMatrixGoPro[2] = new double[] { 0, 0, 1 };
        }

        public List<double> CsvStringToListDouble(List<string> csvString)
        {
            List<List<string>> listStringData = String1DtoString2D(csvString);

            double[][] doubleCSV = new double[listStringData.Count][];
            doubleCSV = StringToDouble(listStringData);

            if(listStringData.Count == 8)
            {
                //format of calibration object (two sets of 4)
                List<double> listCSVdouble = new List<double> { doubleCSV[1][0], doubleCSV[2][0], doubleCSV[3][0],
                    doubleCSV[4][0], doubleCSV[5][0], doubleCSV[6][0], doubleCSV[7][0], doubleCSV[8][0]};
                return listCSVdouble;
            }
            else if(listStringData.Count == 12)
            {
                //format of leg marker set of 12
                List<double> listCSVdouble = new List<double> { doubleCSV[1][0], doubleCSV[2][0], doubleCSV[3][0],
                    doubleCSV[4][0], doubleCSV[5][0], doubleCSV[6][0], doubleCSV[7][0], doubleCSV[8][0], doubleCSV[9][0],
                    doubleCSV[10][0], doubleCSV[11][0], doubleCSV[12][0]};
                return listCSVdouble;
            }
            else
            {
                return null;
            }

        }

        public void PerformRealization()
        {
            //this is where we realize the 2D pixel space points into 3D world space points
        }

        public void GraphResults()
        {

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

            string path1 = "C:\\Users\\Bryonna\\Documents\\GoPro_Dummy_Horiz.csv";
            string path2 = "C:\\Users\\Bryonna\\Documents\\GoPro_Dummy_Vert.csv";
            AnalysisSystemFinal.CsvFile csvFile1 = new AnalysisSystemFinal.CsvFile(path1);
            AnalysisSystemFinal.CsvFile csvFile2 = new AnalysisSystemFinal.CsvFile(path2);

            double[][] horizCSV = new double[csvFile1.columns.Count][];
            horizCSV = StringToDouble(csvFile1.columns);

            double[][] vertCSV = new double[csvFile2.columns.Count][];
            vertCSV = StringToDouble(csvFile2.columns);

            List<double> dataHorizontal = new List<double> { horizCSV[1][0], horizCSV[2][0], horizCSV[3][0],
                horizCSV[4][0], horizCSV[5][0], horizCSV[6][0], horizCSV[7][0], horizCSV[8][0], horizCSV[9][0],
                horizCSV[10][0], horizCSV[11][0], horizCSV[12][0]};
            List<double> dataVertical = new List<double> { vertCSV[1][0], vertCSV[2][0], vertCSV[3][0],
                vertCSV[4][0], vertCSV[5][0], vertCSV[6][0], vertCSV[7][0], vertCSV[8][0], vertCSV[9][0],
                vertCSV[10][0], vertCSV[11][0], vertCSV[12][0]};

            HandleLegMarkerData(dataHorizontal, dataVertical, matrixH);

            Console.WriteLine("yay no errors");
        }

        //I don't want the count, I want the first column and parse it. It should have 12 tracking values.
        //thats not true, I do want the count but I also need to part each string of the list 

        public List<List<string>> String1DtoString2D(List<string> csvString)
        {
            List<List<string>> headlessCSV = new List<List<string>>();
            //might need to check this for zeros in the time dimension oops
            for(int row = 1; row < csvString.Count; row++)
            {
                headlessCSV[row-1].AddRange(csvString[row].Split(','));
            }

            return headlessCSV;
        }

        public double[][] StringToDouble(List<string> csvString)
        {
            double[][] csvAsDoubleMatrix = new double[csvString.Count][];

            return csvAsDoubleMatrix;
        }

        public double[][] StringToDouble(List<List<string>> loadedCSV)
        {
            double[][] csvAsDoubleMatrix = new double[loadedCSV.Count][];

            for (int row = 0; row < loadedCSV.Count; row++)
            {
                for (int col = 0; col < loadedCSV[row].Count; col++)
                {
                    if (col == 0)
                    {
                        csvAsDoubleMatrix[row] = new double[loadedCSV[row].Count];
                    }

                    csvAsDoubleMatrix[row][col] = double.Parse(loadedCSV[row][col]);
                }
            }

            return csvAsDoubleMatrix;
        }

        public double[,] ListToArray(List<double> list)
        {
            double[,] array = new double[list.Count, 1];

            for (int i = 0; i < list.Count; i++)
            {
                array[i, 0] = list[i];
            }
            return array;
        }

        private void HandleLegMarkerData(List<double> dataHoriz, List<double> dataVert, double[,] matrixH)
        {
            double[,] tempMatrixT = new double[2, 1];

            List<double> tibia1 = new List<double>();
            List<double> tibia2 = new List<double>();
            List<double> tibia3 = new List<double>();
            List<double> tibia4 = new List<double>();
            List<double> tibia5 = new List<double>();
            List<double> tibia6 = new List<double>();

            double[,] tibia_1 = new double[2, 1];
            double[,] tibia_2 = new double[2, 1];
            double[,] tibia_3 = new double[2, 1];
            double[,] tibia_4 = new double[2, 1];
            double[,] tibia_5 = new double[2, 1];
            double[,] tibia_6 = new double[2, 1];

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[0], dataVert[0], matrixH);
            tibia1.Add(tempMatrixT[0, 0]);
            tibia1.Add(0);
            tibia1.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[1], dataVert[1], matrixH);
            tibia2.Add(tempMatrixT[0, 0]);
            tibia2.Add(0);
            tibia2.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[2], dataVert[2], matrixH);
            tibia3.Add(tempMatrixT[0, 0]);
            tibia3.Add(0);
            tibia3.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[3], dataVert[3], matrixH);
            tibia4.Add(tempMatrixT[0, 0]);
            tibia4.Add(0);
            tibia4.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF2(dataHoriz[4], dataVert[4], matrixH);
            tibia5.Add(0);
            tibia5.Add(tempMatrixT[0, 0]);
            tibia5.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF2(dataHoriz[5], dataVert[5], matrixH);
            tibia6.Add(0);
            tibia6.Add(tempMatrixT[0, 0]);
            tibia6.Add(tempMatrixT[1, 0]);

            tibia_1 = ListToArray(tibia1);
            tibia_2 = ListToArray(tibia2);
            tibia_3 = ListToArray(tibia3);
            tibia_4 = ListToArray(tibia4);
            tibia_5 = ListToArray(tibia5);
            tibia_6 = ListToArray(tibia6);

            double length1_2 = Norm.Norm2(MatrixSubtraction(tibia_1, tibia_2));
            double length2_3 = Norm.Norm2(MatrixSubtraction(tibia_2, tibia_3));
            double length3_4 = Norm.Norm2(MatrixSubtraction(tibia_3, tibia_4));
            double length1_4 = Norm.Norm2(MatrixSubtraction(tibia_1, tibia_4));
            double length4_5 = Norm.Norm2(MatrixSubtraction(tibia_4, tibia_5));
            double length5_6 = Norm.Norm2(MatrixSubtraction(tibia_5, tibia_6));
            double length3_6 = Norm.Norm2(MatrixSubtraction(tibia_3, tibia_6));

            List<double> femur1 = new List<double>();
            List<double> femur2 = new List<double>();
            List<double> femur3 = new List<double>();
            List<double> femur4 = new List<double>();
            List<double> femur5 = new List<double>();
            List<double> femur6 = new List<double>();

            /*            double[,] femur_1 = new double[2, 1];
                        double[,] femur_2 = new double[2, 1];
                        double[,] femur_3 = new double[2, 1];
                        double[,] femur_4 = new double[2, 1];
                        double[,] femur_5 = new double[2, 1];
                        double[,] femur_6 = new double[2, 1];*/

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[6], dataVert[6], matrixH);
            femur1.Add(tempMatrixT[0, 0]);
            femur1.Add(0);
            femur1.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[7], dataVert[7], matrixH);
            femur2.Add(tempMatrixT[0, 0]);
            femur2.Add(0);
            femur2.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[8], dataVert[8], matrixH);
            femur3.Add(tempMatrixT[0, 0]);
            femur3.Add(0);
            femur3.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF1(dataHoriz[9], dataVert[9], matrixH);
            femur4.Add(tempMatrixT[0, 0]);
            femur4.Add(0);
            femur4.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF2(dataHoriz[10], dataVert[10], matrixH);
            femur5.Add(0);
            femur5.Add(tempMatrixT[0, 0]);
            femur5.Add(tempMatrixT[1, 0]);

            tempMatrixT = FindGlobalCoordsF2(dataHoriz[11], dataVert[11], matrixH);
            femur6.Add(0);
            femur6.Add(tempMatrixT[0, 0]);
            femur6.Add(tempMatrixT[1, 0]);

            /*            femur_1 = ListToArray(femur1);
                        femur_2 = ListToArray(femur2);
                        femur_3 = ListToArray(femur3);
                        femur_4 = ListToArray(femur4);
                        femur_5 = ListToArray(femur5);
                        femur_6 = ListToArray(femur6);*/ // well it looks like right now she isn't getting length for these guys

            //now we normalize and centralize the global coords

            //scale x, center x - tibia
            List<double> xpoints = new List<double> { tibia1[0], tibia2[0], tibia3[0], tibia4[0], tibia5[0], tibia6[0] };
            double scalex = ScalePoints(xpoints);
            double centerx = CenterPoints(xpoints);

            //scale y, center y - tibia
            List<double> ypoints = new List<double> { tibia1[1], tibia2[1], tibia3[1], tibia4[1], tibia5[1], tibia6[1] };
            double scaley = ScalePoints(ypoints);
            double centery = CenterPoints(ypoints);

            //scale z, center z - tibia
            List<double> zpoints = new List<double> { tibia1[2], tibia2[2], tibia3[2], tibia4[2], tibia5[2], tibia6[2] };
            double scalez = ScalePoints(zpoints);
            double centerz = CenterPoints(zpoints);

            //create new nc2 matrix
            double[,] nc2 = new double[4, 4];
            nc2 = CreateNC2(scalex, scaley, centerx, centery);

            List<double> finalT1 = new List<double>();
            finalT1 = MatrixMultiplicationGlobal(nc2, tibia1[0], tibia1[1], tibia1[2]);

            List<double> finalT2 = new List<double>();
            finalT2 = MatrixMultiplicationGlobal(nc2, tibia2[0], tibia2[1], tibia2[2]);

            List<double> finalT3 = new List<double>();
            finalT3 = MatrixMultiplicationGlobal(nc2, tibia3[0], tibia3[1], tibia3[2]);

            List<double> finalT4 = new List<double>();
            finalT4 = MatrixMultiplicationGlobal(nc2, tibia4[0], tibia4[1], tibia4[2]);

            List<double> finalT5 = new List<double>();
            finalT5 = MatrixMultiplicationGlobal(nc2, tibia5[0], tibia5[1], tibia5[2]);

            List<double> finalT6 = new List<double>();
            finalT6 = MatrixMultiplicationGlobal(nc2, tibia6[0], tibia6[1], tibia6[2]);

            List<double> finalT7 = new List<double>();
            finalT7 = MatrixMultiplicationGlobal(nc2, femur1[0], femur1[1], femur1[2]);

            List<double> finalT8 = new List<double>();
            finalT8 = MatrixMultiplicationGlobal(nc2, femur2[0], femur2[1], femur2[2]);

            List<double> finalT9 = new List<double>();
            finalT9 = MatrixMultiplicationGlobal(nc2, femur3[0], femur3[1], femur3[2]);

            List<double> finalT10 = new List<double>();
            finalT10 = MatrixMultiplicationGlobal(nc2, femur4[0], femur4[1], femur4[2]);

            List<double> finalT11 = new List<double>();
            finalT11 = MatrixMultiplicationGlobal(nc2, femur5[0], femur5[1], femur5[2]);

            List<double> finalT12 = new List<double>();
            finalT12 = MatrixMultiplicationGlobal(nc2, femur6[0], femur6[1], femur6[2]);

            //norm and cent pixel coords
            //scale x, center x - tibia
            List<double> pxpoints = new List<double> { dataHoriz[0], dataHoriz[1], dataHoriz[2], dataHoriz[3], dataHoriz[4], dataHoriz[5] };
            double scalepx = ScalePoints(xpoints);
            double centerpx = CenterPoints(xpoints);

            //scale y, center y - tibia
            List<double> pypoints = new List<double> { dataVert[0], dataVert[1], dataVert[2], dataVert[3], dataVert[4], dataVert[5] };
            double scalepy = ScalePoints(ypoints);
            double centerpy = CenterPoints(ypoints);

            //create new nc1 matrix
            double[,] nc1 = new double[3, 4];
            nc1 = CreateNC1(scalepx, scalepy, centerpx, centerpy);


            List<double> finalt1 = new List<double>();
            finalt1 = MatrixMultiplicationPixel(nc1, dataHoriz[0], dataVert[0]);

            List<double> finalt2 = new List<double>();
            finalt2 = MatrixMultiplicationPixel(nc1, dataHoriz[1], dataVert[1]);

            List<double> finalt3 = new List<double>();
            finalt3 = MatrixMultiplicationPixel(nc1, dataHoriz[2], dataVert[2]);

            List<double> finalt4 = new List<double>();
            finalt4 = MatrixMultiplicationPixel(nc1, dataHoriz[3], dataVert[3]);

            List<double> finalt5 = new List<double>();
            finalt5 = MatrixMultiplicationPixel(nc1, dataHoriz[4], dataVert[4]);

            List<double> finalt6 = new List<double>();
            finalt6 = MatrixMultiplicationPixel(nc1, dataHoriz[5], dataVert[5]);

            //now make that big matrix T again
            List<double> allGlobalMarkers = new List<double>();
            allGlobalMarkers.AddRange(finalT1);
            allGlobalMarkers.AddRange(finalT2);
            allGlobalMarkers.AddRange(finalT3);
            allGlobalMarkers.AddRange(finalT4);
            allGlobalMarkers.AddRange(finalT5);
            allGlobalMarkers.AddRange(finalT6);

            List<double> allPixelMarkers = new List<double>();
            allPixelMarkers.AddRange(finalt1);
            allPixelMarkers.AddRange(finalt2);
            allPixelMarkers.AddRange(finalt3);
            allPixelMarkers.AddRange(finalt4);
            allPixelMarkers.AddRange(finalt5);
            allPixelMarkers.AddRange(finalt6);

            double[,] matrixT_leg;
            matrixT_leg = HomgraphicMatrixLeg(allGlobalMarkers, allPixelMarkers);
        }

        private double[,] MatrixSubtraction(double[,] matrix1, double[,] matrix2)
        {
            double[,] matrixSub = new double[matrix1.GetLength(0), matrix1.GetLength(1)];

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    matrixSub[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            return matrixSub;
        }

        #region Deprecated(lmao)
        //don't need the matrices here anymore, but i'll keep it in case I do one day
        private void WorkWithLegMarkerDataFromCSV(double[][] dataHorizontal, double[][] dataVertical, double[,] matrixH) //this needs to be refactored - but a lot of stuff does tbh 
        {
            // Tibia Markers
            double t1x = dataHorizontal[1][10];
            double t1y = dataVertical[1][10];
            double t2x = dataHorizontal[1][2];
            double t2y = dataVertical[1][2];
            double t3x = dataHorizontal[1][3];
            double t3y = dataVertical[1][3];
            double t4x = dataHorizontal[1][4];
            double t4y = dataVertical[1][4];
            double t5x = dataHorizontal[1][5];
            double t5y = dataVertical[1][5];
            double t6x = dataHorizontal[1][6];
            double t6y = dataVertical[1][6];
            // Femur Markers
            double t7x = dataHorizontal[1][7];
            double t7y = dataVertical[1][7];
            double t8x = dataHorizontal[1][8];
            double t8y = dataVertical[1][8];
            double t9x = dataHorizontal[1][9];
            double t9y = dataVertical[1][9];
            double t10x = dataHorizontal[1][11];
            double t10y = dataVertical[1][11];
            double t11x = dataHorizontal[1][12];
            double t11y = dataVertical[1][12];
            double t12x = dataHorizontal[1][13];
            double t12y = dataVertical[1][13];

            List<double> tibiaList = new List<double>();
            List<double> femurList = new List<double>();

            tibiaList.Add(t1x);
            tibiaList.Add(t1y);
            tibiaList.Add(t2x);
            tibiaList.Add(t2y);
            tibiaList.Add(t3x);
            tibiaList.Add(t3y);
            tibiaList.Add(t4x);
            tibiaList.Add(t4y);
            tibiaList.Add(t5x);
            tibiaList.Add(t5y);
            tibiaList.Add(t6x);
            tibiaList.Add(t6y);

            femurList.Add(t7x);
            femurList.Add(t7y);
            femurList.Add(t8x);
            femurList.Add(t8y);
            femurList.Add(t9x);
            femurList.Add(t9y);
            femurList.Add(t10x);
            femurList.Add(t10y);
            femurList.Add(t11x);
            femurList.Add(t11y);
            femurList.Add(t12x);
            femurList.Add(t12y);

            double[,] tempMatrixT = new double[2, 1];

            List<double> tibia1 = new List<double>();
            List<double> tibia2 = new List<double>();
            List<double> tibia3 = new List<double>();
            List<double> tibia4 = new List<double>();
            List<double> tibia5 = new List<double>();
            List<double> tibia6 = new List<double>();

            tempMatrixT = FindGlobalCoordsF1(t1x, t1y, matrixH);
            tibia1.Add(tempMatrixT[1, 0]);
            tibia1.Add(0);
            tibia1.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF1(t2x, t2y, matrixH);
            tibia2.Add(tempMatrixT[1, 0]);
            tibia2.Add(0);
            tibia2.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF1(t3x, t3y, matrixH);
            tibia3.Add(tempMatrixT[1, 0]);
            tibia3.Add(0);
            tibia3.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF1(t4x, t4y, matrixH);
            tibia4.Add(tempMatrixT[1, 0]);
            tibia4.Add(0);
            tibia4.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF2(t5x, t5y, matrixH);
            tibia5.Add(0);
            tibia5.Add(tempMatrixT[1, 0]);
            tibia5.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF2(t6x, t6y, matrixH);
            tibia6.Add(0);
            tibia6.Add(tempMatrixT[1, 0]);
            tibia6.Add(tempMatrixT[2, 0]);

            List<double> femur1 = new List<double>();
            List<double> femur2 = new List<double>();
            List<double> femur3 = new List<double>();
            List<double> femur4 = new List<double>();
            List<double> femur5 = new List<double>();
            List<double> femur6 = new List<double>();

            tempMatrixT = FindGlobalCoordsF1(t7x, t7y, matrixH);
            femur1.Add(tempMatrixT[1, 0]);
            femur1.Add(0);
            femur1.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF1(t8x, t8y, matrixH);
            femur2.Add(tempMatrixT[1, 0]);
            femur2.Add(0);
            femur2.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF1(t9x, t9y, matrixH);
            femur3.Add(tempMatrixT[1, 0]);
            femur3.Add(0);
            femur3.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF1(t10x, t10y, matrixH);
            femur4.Add(tempMatrixT[1, 0]);
            femur4.Add(0);
            femur4.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF2(t11x, t11y, matrixH);
            femur5.Add(0);
            femur5.Add(tempMatrixT[1, 0]);
            femur5.Add(tempMatrixT[2, 0]);

            tempMatrixT = FindGlobalCoordsF2(t12x, t12y, matrixH);
            femur6.Add(0);
            femur6.Add(tempMatrixT[1, 0]);
            femur6.Add(tempMatrixT[2, 0]);
        }
        #endregion

        // formula 1 - used for first 4 of 6 markers
        private double[,] FindGlobalCoordsF1(double tn_x, double tn_y, double[,] matrixH)
        {
            double[][] matrixT = new double[2][];
            //the formula for each T matrix, given a marker with coordinates tn_x and tn_y (used on markers 1-4)
            /*          matrixT[0] = new double[] { matrixH[0, 0] + matrixH[2, 0] * tn_x, matrixH[0, 2] + matrixH[2, 2] * tn_x };
                        matrixT[1] = new double[] { matrixH[1, 0] - matrixH[2, 0] * tn_y, matrixH[1, 2] - matrixH[2, 2] * tn_y };*/

            //multiplicant matrix
            double[][] matrixMultBy = new double[2][];
            /*          matrixMultBy[0] = new double[] { -matrixH[0, 3] - matrixH[2, 3] * tn_x};
                        matrixMultBy[1] = new double[] { -matrixH[1, 3] + matrixH[2, 3] * tn_y };*/

            //prepare T matrix
            matrixT[0] = new double[] { matrixH[0, 0] + matrixH[2, 0] * tn_x, matrixH[0, 2] + matrixH[2, 2] * tn_x };
            matrixT[1] = new double[] { matrixH[1, 0] - matrixH[2, 0] * tn_y, matrixH[1, 2] - matrixH[2, 2] * tn_y };
            //prepare secondary matrix
            matrixMultBy[0] = new double[] { -matrixH[0, 3] - matrixH[2, 3] * tn_x };
            matrixMultBy[1] = new double[] { -matrixH[1, 3] + matrixH[2, 3] * tn_y };
            double[,] matrixM = new double[2, 1];
            matrixM = ChangeArrayTypeBACK(matrixMultBy);
            //then we need to inverse T and multiply it by another matrix
            double[,] matrixT_inv = new double[2, 2];
            matrixT_inv = McCaffreyMatrixInverse(matrixT);

            double[,] matrixFinal = new double[2, 1];
            matrixFinal = MatrixMultiplication(matrixM, matrixT_inv);

            return matrixFinal;
        }
        //formula 2 - used for last 2 of 6 markers
        private double[,] FindGlobalCoordsF2(double tn_x, double tn_y, double[,] matrixH)
        {
            double[][] matrixT = new double[2][];
            //the formula for the other T matrix (used on markers 5 and 6)
            /*          matrixT[0] = new double[] { matrixH[0, 1] + matrixH[2, 1] * tn_x, matrixH[0, 2] + matrixH[2, 2] * tn_x };
                        matrixT[1] = new double[] { matrixH[1, 1] - matrixH[2, 1] * tn_y, matrixH[1, 2] - matrixH[2, 2] * tn_y };*/

            //multiplicant matrix
            double[][] matrixMultBy = new double[2][];
            /*          matrixMultBy[0] = new double[] { -matrixH[0, 3] - matrixH[2, 3] * tn_x};
                        matrixMultBy[1] = new double[] { -matrixH[1, 3] + matrixH[2, 3] * tn_y };*/

            //prepare T matrix
            matrixT[0] = new double[] { matrixH[0, 1] + matrixH[2, 1] * tn_x, matrixH[0, 2] + matrixH[2, 2] * tn_x };
            matrixT[1] = new double[] { matrixH[1, 1] - matrixH[2, 1] * tn_y, matrixH[1, 2] - matrixH[2, 2] * tn_y };
            //prepare secondary matrix
            matrixMultBy[0] = new double[] { -matrixH[0, 3] - matrixH[2, 3] * tn_x };
            matrixMultBy[1] = new double[] { -matrixH[1, 3] + matrixH[2, 3] * tn_y };
            double[,] matrixM = new double[2, 1];
            matrixM = ChangeArrayTypeBACK(matrixMultBy);
            //then we need to inverse T and multiply it by another matrix
            double[,] matrixT_inv = new double[2, 2];
            matrixT_inv = McCaffreyMatrixInverse(matrixT);

            double[,] matrixFinal = new double[2, 1];
            matrixFinal = MatrixMultiplication(matrixM, matrixT_inv);

            return matrixFinal;
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
            inversedNC1 = McCaffreyMatrixInverse(nc1_std);

            //make new matrix from U values according to specs
            double[,] specialUMatrix = new double[3, 4]
            {
                { matrixU[0,0], matrixU[1,0], matrixU[2,0], matrixU[3,0] },
                { matrixU[4,0], matrixU[5,0], matrixU[6,0], matrixU[7,0] },
                { matrixU[8,0], matrixU[9,0], matrixU[10,0], 1 }
            };

            //now multiply this U matrix by NC2
            double[,] uByNC2 = new double[specialUMatrix.GetLength(0), NC2.GetLength(1)];
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

            theta3 = Math.Atan2(h[1, 0], h[0, 0]);
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
            inversedProduct = McCaffreyMatrixInverse(standardMatrix);

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

        public double[,] CreateNC1(double scalePx, double scalePy, double centerPx, double centerPy)
        {
            double[,] nc1 = new double[3, 3]
            {
                { scalePx, 0, -(centerPx * scalePx) },
                { 0, scalePy, -(centerPy * scalePy) },
                { 0, 0, 1 }
            };
            return nc1;
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

        public double[,] CreateNC2(double scalex, double scaley, double centerx, double centery)
        {
            double[,] nc2 = new double[4, 4]
            {
                { scalex, 0, 0, -(centerx * scalex) },
                { 0, scaley, 0, -(centery * scaley) },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };

            return nc2;
        }

        private List<double> SetNewValues(double[,] newMatrix, List<double> points)
        {
            for (int i = 0; i < points.Count; i++)
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

            for (int i = 0; i < 3; i++)
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
                someF[i, 0] = ((nc2[i, 0] * pointX) + (nc2[i, 1] * pointY) + (nc2[i, 2] * pointZ) + (nc2[i, 3] * 1));
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

            for (int i = 0; i < points.Count; i++)
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

            for (int row = 0; row < 16; row++)
            {
                if (row > 1 && (row % 2 == 0))
                {
                    globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                }
                for (int column = 0; column < 11; column++)
                {
                    if (row % 2 == 0)
                    {
                        if (column < 3)
                        {
                            homGraphT[row, column] = globalPts[globalPtsTracker];
                            globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                        }
                        else if (column == 3)
                        {
                            homGraphT[row, column] = 1;
                        }
                        else if (3 < column && column <= 7)
                        {
                            homGraphT[row, column] = 0;
                        }
                        else if (column > 7)
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
                            if (column != 10)
                            {
                                globalPtsTracker = GlobalPtIdxHelper(row, globalPtsTracker);
                            }
                        }
                    }
                }
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

        private double[,] HomgraphicMatrixLeg(List<double> globalMarkers, List<double> pixelMarkers)
        {
            double[,] matrixT = new double[12, 11];

            int globalPtsTracker = 0;

            for (int row = 0; row < 12; row++)
            {
                if (row > 1 && (row % 2 == 0))
                {
                    globalPtsTracker = LegGlobalIdxHelper(row, globalPtsTracker);
                }
                for (int column = 0; column < 11; column++)
                {
                    if (row % 2 == 0)
                    {
                        if (column < 3)
                        {
                            matrixT[row, column] = globalMarkers[globalPtsTracker];
                            globalPtsTracker = LegGlobalIdxHelper(row, globalPtsTracker);
                        }
                        else if (column == 3)
                        {
                            matrixT[row, column] = 1;
                        }
                        else if (3 < column && column <= 7)
                        {
                            matrixT[row, column] = 0;
                        }
                        else if (column > 7)
                        {
                            matrixT[row, column] = globalMarkers[globalPtsTracker] * pixelMarkers[row];
                            globalPtsTracker = LegGlobalIdxHelper(row, globalPtsTracker);
                        }
                        //Console.WriteLine(globalPtsTracker);
                    }
                    else if (row % 2 == 1)
                    {
                        if (column < 4)
                        {
                            matrixT[row, column] = 0;
                        }
                        else if (4 <= column && column < 7)
                        {
                            matrixT[row, column] = globalMarkers[globalPtsTracker];
                            globalPtsTracker = LegGlobalIdxHelper(row, globalPtsTracker);
                        }
                        else if (column == 7)
                        {
                            matrixT[row, column] = 1;
                        }
                        else if (column > 7)
                        {
                            matrixT[row, column] = -(globalMarkers[globalPtsTracker]) * pixelMarkers[row];
                            if (column != 10)
                            {
                                globalPtsTracker = LegGlobalIdxHelper(row, globalPtsTracker);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine("\n");
                for (int j = 0; j < 11; j++)
                {
                    Console.Write(matrixT[i, j] + " ");
                }
            }
            Console.WriteLine("\nEND HOMGRAPH\n");

            return matrixT;
        }

        private int LegGlobalIdxHelper(int row, int currentIdx)
        {
            //AF - 0,1,2
            //BF - 3,4,5
            //etc
            int baseIdx = 0;

            if (((currentIdx + 1) % 3) == 0)
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
                }
                return baseIdx;
            }
            else
            {
                return (currentIdx + 1);
            }
        }

        private int GlobalPtIdxHelper(int row, int currentIdx)
        {
            //AF - 0,1,2
            //BF - 3,4,5
            //etc
            int baseIdx = 0;

            if (((currentIdx + 1) % 3) == 0)
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
                return (currentIdx + 1);
            }
        }

        private double[,] TransposeMatrix(double[,] originalMatrix, int origRows, int origColumns)
        {
            double[,] transposeMatrix = new double[origColumns, origRows];

            for (int i = 0; i < origColumns; i++)
            {
                for (int j = 0; j < origRows; j++)
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
        }

        private double[,] MatrixMultiplication(double[,] originalMatrix, double[,] transposeMatrix)
        {
            int productRows = transposeMatrix.GetLength(0);
            int productColumns = originalMatrix.GetLength(1);
            double[,] productMatrix = new double[productRows, productColumns];

            //Console.WriteLine("\n A^T*A");
            for (int i = 0; i < productRows; i++)
            {
                //Console.Write("\n");
                for (int j = 0; j < productColumns; j++)
                {
                    for (int k = 0; k < originalMatrix.GetLength(0); k++)
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

            for (int i = 0; i < rows; i++)
            {
                stdMatrix[i] = new double[] { matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3], matrix[i, 4], matrix[i, 5], matrix[i, 6], matrix[i, 7], matrix[i, 8], matrix[i, 9], matrix[i, 10] };
            }

            return stdMatrix;
        }

        private double[,] ChangeArrayTypeBACK(double[][] matrix)
        {
            int rows = matrix.Length;
            int columns = matrix[0].Length;
            double[,] returnMatrix = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    returnMatrix[i, j] = matrix[i][j];
                }
            }

            return returnMatrix;
        }

        private double[,] McCaffreyMatrixInverse(double[][] matrixToInvert)
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


        //Correct Radial Distortion
        //% k1, k2 are the distortion coefficents
        //% fx, fy are the scaling factors along x and y direction
        //% x_measured, y_measured are the measured pixels reading along x, y direction
        public double[] CorrectRadialDistortion(double k1, double k2, double fx, double fy, double x_measured, double y_measured)
        {
            double[] x = new double[10];
            double[] y = new double[10];
            x[0] = x_measured;
            y[0] = y_measured;

            for (int i = 0; i < 8; i++)
            {
                x[i + 1] = x_measured - (x[i] * k1 * ((Math.Pow(x[i], 2) / Math.Pow(fx, 2)) + (Math.Pow(y[i], 2) / Math.Pow(fy, 2))) - (x[i] * k2 * (Math.Pow(x[i], 2) / Math.Pow(fx, 2)) + (Math.Pow(y[i], 2) / Math.Pow(fy, 2))));
                y[i + 1] = y_measured - (y[i] * k1 * ((Math.Pow(x[i], 2) / Math.Pow(fx, 2)) + (Math.Pow(y[i], 2) / Math.Pow(fy, 2))) - (y[i] * k2 * (Math.Pow(x[i], 2) / Math.Pow(fx, 2)) + (Math.Pow(y[i], 2) / Math.Pow(fy, 2))));
            }

            //x_cor = x(9)
            //y_cor = y(9)
            double[] cor_double = new double[2];
            cor_double[0] = x[9];
            cor_double[1] = y[9];

            return cor_double;
        }

        //Tune Side Coordinates

        public double[] TuneSideCoordinates(double[] x, double[] T3, double[] T4, double L45, double L36, double L56)
        {
            double[] f = new double[3];

            f[0] = (Math.Pow(T4[0] - x[0], 2) + Math.Pow(T4[1] - x[1], 2) + Math.Pow(Math.Pow(T4[2] - x[2], 2) - Math.Pow(L45, 2), 2));
            f[1] = (Math.Pow(T3[0] - x[3], 2) + Math.Pow(T3[1] - x[4], 2) + Math.Pow(Math.Pow(T3[2] - x[5], 2) - Math.Pow(L36, 2), 2));
            f[2] = (Math.Pow(x[0] - x[3], 2) + Math.Pow(x[1] - x[4], 2) + Math.Pow(Math.Pow(x[2] - x[5], 2) - Math.Pow(L56, 2), 2));

            return f;
        }
    }
}
