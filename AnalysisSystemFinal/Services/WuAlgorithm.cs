using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixInverse;
using Accord.Math;

namespace CodeTranslation
{
    class WuAlgorithm
    {
        //Camera calibration specifications
        double k1 = 0.0136;
        double k2 = 0.0363;
        double fx, fy = 2060;

        //Have we calibrated?
        private bool calibrationComplete = false;

        //double[][] intrinsicMatrixGoPro = new double[3][];

        public void TakeInValues(List<string> horizVals, List<string> vertVals)
        {

            List<double> horizData = CsvStringToListDouble(horizVals);
            List<double> vertData = CsvStringToListDouble(vertVals);
            //now we need to decide whether we are calibrating or calculating
            if (horizData.Count == 8)
            {
                InitGoProInMat();
                CalibrateObject(horizData, vertData);
                calibrationComplete = true;
            }
            else if(horizData.Count == 12 && calibrationComplete)
            {

            }
            else
            {
                Console.WriteLine("Data does not represent an accepted number of markers.");
            }
        }

        public void CalibrateObject(List<double> horizCali, List<double> vertCali)
        {
            //pull world space coordinates from save data? -- look into how to have save data with .net form
            double[][] worldPts = LoadPlaceholderCalibrationDimensions();

            double[][] correctPixelPts = new double[horizCali.Count][];

            for (int i=0; i<horizCali.Count; i++)
            {
                correctPixelPts[i] = CorrectRadialDistortion(horizCali[i], vertCali[i]);
            }

            double[][] NC1 = GetMatrixNC(correctPixelPts);

            double[][] calibratedPixelPts = ncMultiplication(NC1, correctPixelPts);

            //Now for the world dimensions of the calibration object 
            double[][] NC2 = GetMatrixNC(worldPts);

            double[][] calibratedWorldPts = ncMultiplication(NC2, worldPts);

            double[][] matrixT = HomographicMatrixT(calibratedWorldPts, calibratedPixelPts);

            //double[][] matrixU
            double[] matrixU1D = GetUFromSVD(matrixT);
            //double[][] matrixH
            //double[][] matrixH = Math
            //method 2 using row vectors
            //double[][] matrixR

            //return matrixH
        }

        public void CalibrateLegPts(List<double> horizPos, List<double> vertPos, double[][] matrixH)
        {
            double[][] correctedPixelPts = new double[horizPos.Count][];

            for(int i=0; i<horizPos.Count; i++)
            {
                correctedPixelPts[i] = CorrectRadialDistortion(horizPos[i], vertPos[i]);
            }

            //Find the global coordinates of the tibia and femur markers
            double[][] globalPts = FindGlobalLegCoords(correctedPixelPts, matrixH);

            double[][][] separatedPts = SeparateTibiaFemur(globalPts);
            double[][] tibiaGlobalPts = separatedPts[0];
            double[][] femurGlobalPts = separatedPts[1];

            double[][] tibiaNC2 = GetMatrixNC(tibiaGlobalPts);
            double[][] femurNC2 = GetMatrixNC(femurGlobalPts);

            //NEED CLARIFICATION - identity matrix of NC2's - not sure if this step is only for testing purposes in matlab?

            double[][] calibratedTibiaGlobal = ncMultiplication(tibiaNC2, tibiaGlobalPts);
            double[][] calibratedFemurGlobal = ncMultiplication(femurNC2, femurGlobalPts);

            double[][][] separatedPixelPts = SeparateTibiaFemur(correctedPixelPts);
            double[][] tibiaPixelPts = separatedPixelPts[0];
            double[][] femurPixelPts = separatedPixelPts[1];

            double[][] tibiaNC1 = GetMatrixNC(tibiaPixelPts);
            double[][] femurNC1 = GetMatrixNC(femurPixelPts);

            //need same clarification as above
            double[][] calibratedTibiaPixel = ncMultiplication(tibiaNC1, tibiaPixelPts);
            double[][] calibratedFemurPixel = ncMultiplication(femurNC1, femurPixelPts);

            double[][] tibiaMatrixT = HomographicMatrixT(calibratedTibiaGlobal, calibratedTibiaPixel);
            double[][] femurMatrixT = HomographicMatrixT(calibratedFemurGlobal, calibratedFemurPixel);

            double[] tibiaMatrixU = MatrixInverseProgram.MatVecProd(MatrixInverseProgram.MatInverse(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatTranspose(tibiaMatrixT), tibiaMatrixT)), TransposeMultPixelPts(tibiaMatrixT, tibiaPixelPts));
            double[] femurMatrixU = MatrixInverseProgram.MatVecProd(MatrixInverseProgram.MatInverse(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatTranspose(femurMatrixT), femurMatrixT)), TransposeMultPixelPts(femurMatrixT, femurPixelPts));

            //matrixH
            double[][] tibiaMatrixH = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(tibiaNC1), PopulateHMultplierMat(tibiaMatrixU)), tibiaNC2);
            double[][] femurMatrixH = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(femurNC1), PopulateHMultplierMat(femurMatrixU)), femurNC2);
            //matrixR

        }

        private double[] GetUFromSVD(double[][] matrixT)
        {
            Accord.Math.Decompositions.SingularValueDecomposition svd = new Accord.Math.Decompositions.SingularValueDecomposition(ChangeArrayType(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(matrixT), matrixT)));
            double[] matrixU = ChangeArrayType1D(svd.LeftSingularVectors);

            return matrixU;
        }

        private double[,] ChangeArrayType(double[][] matrix)
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

        private double[] ChangeArrayType1D(double[,] matrix)
        {
            double[] stdMatrix = new double[matrix.GetLength(0)];

            for (int i = 0; i < stdMatrix.Length; i++)
            {
                stdMatrix[i] = matrix[i, 0];
            }

            return stdMatrix;
        }

        private void CalculateAngularVelocity()
        {

        }

        private double[][] PopulateHMultplierMat(double[] matrixU)
        {
            double[][] multiplierMat = new double[3][];
            multiplierMat[0] = new double[] { matrixU[0], matrixU[1], matrixU[2], matrixU[3] };
            multiplierMat[1] = new double[] { matrixU[4], matrixU[5], matrixU[6], matrixU[7] };
            multiplierMat[2] = new double[] { matrixU[8], matrixU[9], matrixU[10], 1 };

            return multiplierMat;
        }

        private double[] TransposeMultPixelPts(double[][] matrixT, double[][] pixelPts)
        {
            double[] pixelPtVec = new double[pixelPts.Length * 2];
            double[][] inverseT = MatrixInverseProgram.MatInverse(matrixT);

            for(int i=0; i<pixelPtVec.Length; i++)
            {
                if (i % 2 == 0)
                {
                    pixelPtVec[i] = -pixelPts[i][0];
                }
                else
                {
                    pixelPtVec[i] = pixelPts[i][1];
                }
            }

            return MatrixInverseProgram.MatVecProd(inverseT,pixelPtVec);
        }

        private double[][][] SeparateTibiaFemur(double[][] allPoints)
        {
            double[][] tibiaPts = new double[allPoints.Length / 2][];
            double[][] femurPts = new double[allPoints.Length / 2][];

            for (int i = 0; i < allPoints.Length; i++)
            {
                if (i < allPoints.Length / 2)
                {
                    for (int j = 0; j < allPoints[i].Length; j++)
                    {
                        tibiaPts[i][j] = allPoints[i][j];
                    }
                }
                else
                {
                    for (int j = 0; j < allPoints[i].Length; j++)
                    {
                        femurPts[i][j] = allPoints[i][j];
                    }
                }
            }

            double[][][] arrayOfarrays = new double[2][][];
            arrayOfarrays[0] = tibiaPts;
            arrayOfarrays[1] = femurPts;

            return arrayOfarrays;
        }

        private double[][] FindGlobalLegCoords(double[][] legPts, double[][] matrixH)
        {
            double[][] globalPts = new double[legPts.Length][];

            double[][] initialMatrix = new double[2][];
            double[][] inverseMatrix = new double[2][];
            double[] tempMatrix = new double[2];
            double[] tempVec = new double[2];

            for(int i=0; i<legPts.Length; i++)
            {
                if(i == 4 || i == 5 || i == 10 || i == 11)
                {
                    initialMatrix[0] = new double[] { matrixH[0][1] + matrixH[2][1] * legPts[i][0], matrixH[0][2] + matrixH[2][2] * legPts[i][0] };
                    initialMatrix[1] = new double[] { matrixH[1][1] - matrixH[2][1] * legPts[i][1], matrixH[1][2] - matrixH[2][2] * legPts[i][1] };

                    inverseMatrix = MatrixInverseProgram.MatInverse(initialMatrix);
                    tempVec[0] = (-matrixH[0][3] - matrixH[2][3] * legPts[i][0]);
                    tempVec[1] = (-matrixH[1][3] + matrixH[2][3] * legPts[i][1]);

                    tempMatrix = MatrixInverseProgram.MatVecProd(inverseMatrix, tempVec);

                    globalPts[i][0] = 0;
                    globalPts[i][1] = tempMatrix[0];
                    globalPts[i][2] = tempMatrix[1];
                }
                else
                {
                    initialMatrix[0] = new double[] { matrixH[0][0] + matrixH[2][0] * legPts[i][0], matrixH[0][2] + matrixH[2][2] * legPts[i][0] };
                    initialMatrix[1] = new double[] { matrixH[1][0] - matrixH[2][0] * legPts[i][1], matrixH[1][2] - matrixH[2][2] * legPts[i][1] };
                
                    inverseMatrix = MatrixInverseProgram.MatInverse(initialMatrix);
                    tempVec[0] = (-matrixH[0][3] - matrixH[2][3] * legPts[i][0]);
                    tempVec[1] = (-matrixH[1][3] + matrixH[2][3] * legPts[i][1]);

                    tempMatrix = MatrixInverseProgram.MatVecProd(inverseMatrix, tempVec);

                    globalPts[i][0] = tempMatrix[0];
                    globalPts[i][1] = 0;
                    globalPts[i][2] = tempMatrix[1];
                }
            }

            return globalPts;
        }

        private double[][] GetMatrixNC(double[][] points)
        {
            double[][] matrixNC = new double[points[0].Length+1][];

            double[] scaledXY = ScalePoints(points);
            double[] centeredXY = CenterPoints(points);

            if(points[0].Length == 2)
            {
                matrixNC = CreateNC1_2(scaledXY[0], scaledXY[1], centeredXY[0], centeredXY[1]);
            }
            else if(points[0].Length == 3)
            {
                matrixNC = CreateNC2_2(scaledXY[0], scaledXY[1], centeredXY[0], centeredXY[1]);
            }

            return matrixNC;
        }

        public double[][] ncMultiplication(double[][] matrixNC, double[][] pts)
        {
            double[][] someF = new double[matrixNC.Length][];
            double[][] calibratedPts = new double[pts.Length][];

            //NC1 matrix
            if(someF.Length == 3)
            {
                for(int i = 0; i<pts.Length; i++)
                {
                    for(int j = 0; j<someF.Length; j++)
                    {
                        someF[j] = new double[] { (matrixNC[j][0] * pts[i][0]) + (matrixNC[j][1] * pts[i][1]) + (matrixNC[j][2]*1)};
                    }
                    //add values to return double[][]
                    calibratedPts[i] = new double[] { someF[0][0], someF[1][0]};
                    //hope someF gets reset 
                }
            }
            //NC2 matrix
            else if(someF.Length == 4)
            {
                for (int i = 0; i < pts.Length; i++)
                {
                    for (int j = 0; j < someF.Length; j++)
                    {
                        someF[j] = new double[] { (matrixNC[j][0] * pts[i][0]) + (matrixNC[j][1] * pts[i][1]) + (matrixNC[j][2]*pts[i][2]) + (matrixNC[j][3] * 1) };
                    }
                    //add values to return double[][]
                    calibratedPts[i] = new double[] { someF[0][0], someF[1][0], someF[2][0] };
                    //hope someF gets reset 
                }
            }

            return calibratedPts;
        }

        public double[][] LoadPlaceholderCalibrationDimensions()
        {
            //these are the measured dimensions of the calibration object
            //we should be able to set these from the GUI though 
            double BFx = -312;
            double BFy = 0;
            double BFz = 22;
            double CFx = BFx - 410;
            double CFy = 0;
            double CFz = BFz;
            double AFx = ((BFx + CFx) / 2);
            double AFy = 0;
            double AFz = 350 + BFz;
            double DFx = ((BFx + CFx) / 2);
            double DFy = 0;
            double DFz = 248 + BFz;

            double CSx = 0;
            double CSy = -210;
            double CSz = 18;
            double BSx = 0;
            double BSy = CSy - 408;
            double BSz = CSz;
            double ASx = 0;
            double ASy = (BSy + CSy) / 2;
            double ASz = 357 + CSz;
            double DSx = 0;
            double DSy = (BSy + CSy) / 2;
            double DSz = 260 + CSz;

            double[][] caliDimensions = new double[8][];
            caliDimensions[0] = new double[] { AFx, AFy, AFz};
            caliDimensions[1] = new double[] { BFx, BFy, BFz };
            caliDimensions[2] = new double[] { CFx, CFy, CFz };
            caliDimensions[3] = new double[] { DFx, DFy, DFz };
            caliDimensions[4] = new double[] { ASx, ASy, ASz };
            caliDimensions[5] = new double[] { BSx, BSy, BSz };
            caliDimensions[6] = new double[] { CSx, CSy, CSz };
            caliDimensions[7] = new double[] { DSx, DSy, DSz };

            return caliDimensions;
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

        public double[][] CreateNC1_2(double scalePx, double scalePy, double centerPx, double centerPy)
        {
            double[][] NC1 = new double[3][];
            NC1[0] = new double[] { scalePx, 0, -(centerPx * scalePx) };
            NC1[1] = new double[] { 0, scalePy, -(centerPy * scalePy) };
            NC1[2] = new double[] { 0, 0, 1 };

            return NC1;
        }

        private double[][] CreateNC2_2(double scalex, double scaley, double centerx, double centery)
        {
            double[][] NC2 = new double[3][];
            NC2[0] = new double[] { scalex, 0, 0, -(centerx * scalex) };
            NC2[1] = new double[] { 0, scaley, 0, -(centery * scaley) };
            NC2[2] = new double[] { 0, 0, 1, 0 };
            NC2[3] = new double[] { 0, 0, 0, 1 };

            return NC2;
        }

        //rewritten to support a simpler format
        public double[] ScalePoints(double[][] points)
        {
            double[] scaledValues = new double[2];

            double[] xPts = new double[8];
            double[] yPts = new double[8];

            for(int i=0; i < points.Length; i++)
            {
                xPts[i] = points[i][0];
                yPts[i] = points[0][i];
            }

            scaledValues[0] = 1 / (xPts.Max() - xPts.Min());    //scaled x
            scaledValues[1] = 1 / (yPts.Min() - yPts.Min());    //scaled y

            return scaledValues;
        }

        //rewritten to support a simpler format
        public double[] CenterPoints(double[][] points)
        {
            double[] centeredValues = new double[2];

            double[] xPts = new double[8];
            double[] yPts = new double[8];
            double sumX = 0;
            double sumY = 0;

            for (int i = 0; i < points.Length; i++)
            {
                xPts[i] = points[i][0];
                yPts[i] = points[0][i];

                sumX += xPts[i];
                sumY += yPts[i];
            }

            centeredValues[0] = (sumX / xPts.Length);
            centeredValues[1] = (sumY / yPts.Length);

            return centeredValues;
        }

        private double[][] HomographicMatrixT(double[][] globalPts, double[][]pixelPts)
        {
            double[][] homGraphT = new double[(globalPts.Length + pixelPts.Length)][];

            for(int row = 0; row<homGraphT.Length; row++)
            {
                //we are iterating through the rows to populate the entire matrix
                for(int col = 0; col<12; col++)
                {
                    //the column defines the behavior - ie which type of value is populated - ex: x, y, z
                    if(row % 2 == 0)
                    {
                        if (col < 3)
                        {
                            homGraphT[row][col] = globalPts[GetAdjustedRow(row)][col];
                        }
                        else if (col == 3)
                        {
                            homGraphT[row][col] = 1;
                        }
                        else if (3 < col && col <= 7)
                        {
                            homGraphT[row][col] = 0;
                        }
                        else if (col > 7 && col < 11)
                        {
                            homGraphT[row][col] = globalPts[GetAdjustedRow(row)][col - 8]*pixelPts[GetAdjustedRow(row)][0];
                        }
                        else if(col == 11)
                        {
                            homGraphT[row][col] = pixelPts[GetAdjustedRow(row)][0];
                        }
                    }
                    else
                    {
                        if (col < 4)
                        {
                            homGraphT[row][col] = 0;
                        }
                        else if (4 <= col && col < 7)
                        {
                            homGraphT[row][col] = globalPts[GetAdjustedRow(row)][col-4];
                        }
                        else if (col == 7)
                        {
                            homGraphT[row][col] = 1;
                        }
                        else if (col > 7 && col < 11)
                        {
                            homGraphT[row][col] = -(globalPts[GetAdjustedRow(row)][col - 8] * pixelPts[GetAdjustedRow(row)][1]);
                        }
                        else if (col == 11)
                        {
                            homGraphT[row][col] = -(pixelPts[GetAdjustedRow(row)][1]);
                        }
                    }
                }
            }

            return homGraphT;
        }

        private int GetAdjustedRow(int row)
        {
            int adjustedRow = 0;

            switch (row)
            {
                case 0:
                case 1:
                    adjustedRow = 0;
                    break;
                case 2:
                case 3:
                    adjustedRow = 1;
                    break;
                case 4:
                case 5:
                    adjustedRow = 2;
                    break;
                case 6:
                case 7:
                    adjustedRow = 3;
                    break;
                case 8:
                case 9:
                    adjustedRow = 4;
                    break;
                case 10:
                case 11:
                    adjustedRow = 5;
                    break;
                case 12:
                case 13:
                    adjustedRow = 6;
                    break;
                case 14:
                case 15:
                    adjustedRow = 7;
                    break;
            }

            return adjustedRow;
        }

        //Correct Radial Distortion
        //% k1, k2 are the distortion coefficents
        //% fx, fy are the scaling factors along x and y direction
        //% x_measured, y_measured are the measured pixels reading along x, y direction
        public double[] CorrectRadialDistortion(double x_measured, double y_measured)
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
