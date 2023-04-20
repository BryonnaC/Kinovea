﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixInverse;
using Accord.Math;
using NWaves;
using OxyPlot;

namespace AnalysisSystemFinal
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

        //placehold these here for now so I can graph
        double[] xtG = new double[240];
        double[] ytG = new double[240];
        double[] ztG = new double[240];

        double[] xfG = new double[240];
        double[] yfG = new double[240];
        double[] zfG = new double[240];

        double[] GtibiaTheta1 = new double[240];
        double[] GtibiaTheta2 = new double[240];
        double[] GtibiaTheta3 = new double[240];

        double[] GfemurTheta1 = new double[240];
        double[] GfemurTheta2 = new double[240];
        double[] GfemurTheta3 = new double[240];


        public void TakeInPositionValues(List<string> horizVals, List<string> vertVals)
        {
            List<double> horizData = GetFirstFrameVals(horizVals);
            List<double> vertData = GetFirstFrameVals(vertVals);
            //now we need to decide whether we are calibrating or calculating
            if (horizData.Count == 8)
            {
                //InitGoProInMat();
                CalibrateObject(horizData, vertData);
                calibrationComplete = true;
            }
            else if(horizData.Count == 12 && calibrationComplete)
            {
                //CalibrateLegPts(horizData, vertData);
                //IterateThroughFrames(horizVals, vertVals);
            }
            else
            {
                Console.WriteLine("Data does not represent an accepted number of markers.");
            }
        }

        public void TestCSVFiles()
        {
/*            string caliPath = "C:\\Users\\Bryonna\\Documents\\calibrationTestData021023 - Sheet1";*/
            string path1 = "C:\\Users\\Bryonna\\Documents\\Gopro_trial3_021023_horz_pos";
            string path2 = "C:\\Users\\Bryonna\\Documents\\Gopro_trial3_021023_vert_pos";

            AnalysisSystemFinal.CsvFile csvFile1 = new AnalysisSystemFinal.CsvFile(path1);
            AnalysisSystemFinal.CsvFile csvFile2 = new AnalysisSystemFinal.CsvFile(path2);

            /*            AnalysisSystemFinal.CsvFile csvFile3 = new AnalysisSystemFinal.CsvFile(caliPath);
                        double[][] caliPix = new double[csvFile3.columns.Count][];
                        //caliPix = StringToDouble(csvFile3.columns);
                        //caliPix = MatrixInverseProgram.MatTranspose(caliPix);
                        double[][] caliX = new double[1][];
            *//*            Console.WriteLine(csvFile3.col)

                        for(int i = 0; i<8; i++)
                        {
                            caliX[0] = csvFile3.columns[0];
                        }*/

/*            List<double> horizData = GetFirstFrameVals(horizVals);
            List<double> vertData = GetFirstFrameVals(vertVals);*/

            /*            double[][] horizCSV = new double[csvFile1.columns.Count][];
                        horizCSV = StringToDouble(csvFile1.columns);

                        double[][] vertCSV = new double[csvFile2.columns.Count][];
                        vertCSV = StringToDouble(csvFile2.columns);*/

            /*            double[][] horizC = new double[1][];
                        horizC[0] = new double[] { 329.49, 184.5, 472, 325.89, -381.09, -470.64, -244.4, -374.02 };

                        double[][] vertC = new double[1][];
                        vertC[0] = new double[] { -160.48, -557.33, -510.2, -269.83, -147.29, -488.99, -557.33, -246.26 };*/

            List<double> horizCal = new List<double> { 329.49, 184.5, 472, 325.89, -381.09, -470.64, -244.4, -374.02 };
            List<double> vertCal = new List<double> { -160.48, -557.33, -510.2, -269.83, -147.29, -488.99, -557.33, -246.26 };

            double[][] matrixH = CalibrateObject(horizCal, vertCal);
            calibrationComplete = true;

            //CalibrateLegPts(horizCSV, vertCSV, matrixH);
        }

        private void HandleForceData(double[][] forceData, double[][] tibiaOmegas, double[][] femurOmegas)
        {
            int samplingForce = 500; //sample frequency of PASCO force plate
            int goproRate = 240;
            double weight = 60; //kg - need to get this from somewhere
            double height = 1.6; //meters - need to get this from the form too

            double g = 9.81; //gravity coeff

            double thigh_CG_knee = 0.567;
            double thigh_mass = 0.1 * weight;
            double thigh_length = (0.53-.285) * height;
            double thigh_J_knee = thigh_mass * Math.Pow((thigh_length*thigh_CG_knee),2);
            double thigh_J_center = thigh_J_knee-(thigh_mass*Math.Pow((thigh_length*thigh_CG_knee),2));
            double jx_f = thigh_J_center;
            double jy_f = thigh_J_center;
            double jz_f = 0;

            double leg_CG_ankle = 0.567;
            double leg_mass = 0.0465 * weight;
            double leg_length = (0.285 - 0.039) * height;
            double leg_J_ankle = leg_mass * Math.Pow((0.643 * leg_length),2);
            double leg_J_center = leg_J_ankle - (leg_mass * Math.Pow((leg_length*leg_CG_ankle), 2));
            double jx_t = leg_J_center;
            double jy_t = leg_J_center;
            double jz_t = 0;

            float[] downForce = new float[forceData.Length];
            for(int i=0; i<forceData.Length; i++)
            {
                downForce[i] = Convert.ToSingle(forceData[i][6]);
            }

            NWaves.Signals.DiscreteSignal ds = new NWaves.Signals.DiscreteSignal(samplingForce, downForce, false);
            NWaves.Operations.Resampler rs = new NWaves.Operations.Resampler();

            NWaves.Signals.DiscreteSignal downSampledForce = rs.Resample(ds, goproRate);

            //Mx.Length = frames-2
            double[] Mx = new double[goproRate - 2];
            double[] My = new double[goproRate - 2];
            double[] Mz = new double[goproRate - 2];

            int distance = 10; //placeholder?
            for(int i=3; i < goproRate - 2; i++)
            {
                Mx[i] = downSampledForce[i] * distance + jx_t * tibiaOmegas[0][i] - (jy_t - jz_t) * tibiaOmegas[1][i] * tibiaOmegas[2][i];
                My[i] = downSampledForce[i] * distance + jy_t * tibiaOmegas[0][i] - (jz_t - jx_t) * tibiaOmegas[1][i] * tibiaOmegas[2][i];
                Mz[i] = downSampledForce[i] * distance + jz_t * tibiaOmegas[0][i] - (jx_t - jy_t) * tibiaOmegas[1][i] * tibiaOmegas[2][i];
            }
        }

        public double[][] CalibrateObject(List<double> horizCali, List<double> vertCali)
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

            double[][] matrixU = GetUFromSVD(matrixT);

            double[][] matrixH = GetHMatrix(matrixU, NC1, NC2);
            //method 2 using row vectors
            //double[][] matrixR
            double[][] matrixR = CrossVectors(matrixH, false);
            double[][] matrixR_init = globalToCameraTheta(matrixR);
            //return matrixH? return matrixR_init?
            return matrixH;
        }

        public void GraphAdjusted(List<string> horizCSV, List<string> vertCSV)
        {
            //format entire data sheet
            List<List<string>> listStringDataH = String1DtoString2D(horizCSV);
            List<List<string>> listStringDataV = String1DtoString2D(vertCSV);
            //format again
            double[][] horizPos = StringToDouble(listStringDataH);
            double[][] vertPos = StringToDouble(listStringDataV);

            OxyPlot.PlotModel pm = new PlotModel();
            pm.Title = "testme";

        }

        public void GraphAdjusted()
        {
            string path1 = "C:\\Users\\Bryonna\\Documents\\GoPro_Dummy_Horiz.csv";
            string path2 = "C:\\Users\\Bryonna\\Documents\\GoPro_Dummy_Vert.csv";
            AnalysisSystemFinal.CsvFile csvFile1 = new AnalysisSystemFinal.CsvFile(path1);
            AnalysisSystemFinal.CsvFile csvFile2 = new AnalysisSystemFinal.CsvFile(path2);

            double[][] horizCSV = new double[csvFile1.columns.Count][];
            horizCSV = StringToDouble(csvFile1.columns);

            double[][] vertCSV = new double[csvFile2.columns.Count][];
            vertCSV = StringToDouble(csvFile2.columns);

            OutputGraph og = new OutputGraph();
            og.ShowDialog();
            og.Dispose();
        }

        private double[][][] IterateThroughFrames(List<string> horizCSV, List<string> vertCSV, double[][][] calibratedTibFemGlob)
        {
            //format entire data sheet
            List<List<string>> listStringDataH = String1DtoString2D(horizCSV);
            List<List<string>> listStringDataV = String1DtoString2D(vertCSV);
            //format again
            double[][] horizPos = StringToDouble(listStringDataH);
            double[][] vertPos = StringToDouble(listStringDataV);

            double[][] tibiaNC2 = GetMatrixNC(calibratedTibFemGlob[0]);
            double[][] femurNC2 = GetMatrixNC(calibratedTibFemGlob[1]);

            int frames = horizPos.Length; //length here is the number of rows - rows are number of frames

            double[] xt = new double[frames];
            double[] yt = new double[frames];
            double[] zt = new double[frames];

            double[] xf = new double[frames];
            double[] yf = new double[frames];
            double[] zf = new double[frames];

            double[][] tibMatrixR_init = new double[3][];
            double[][] femMatrixR_init = new double[3][];

            double[] tibiaTheta1 = new double[frames];
            double[] tibiaTheta2 = new double[frames];
            double[] tibiaTheta3 = new double[frames];

            double[] femurTheta1 = new double[frames];
            double[] femurTheta2 = new double[frames];
            double[] femurTheta3 = new double[frames];

            for (int i=0; i<frames; i++)
            {
                double[][] legPixelPts = new double[horizPos[0].Length][];    // # rows should be 12'
                double[][] correctedPixelPts = new double[legPixelPts.Length][];

                for(int row=0; row<legPixelPts.Length; row++)
                {
                    legPixelPts[row] = new double[] { horizPos[i][row], vertPos[i][row]};
                }

                for (int j = 0; j < legPixelPts.Length; j++)
                {
                    correctedPixelPts[j] = CorrectRadialDistortion(legPixelPts[j][0], legPixelPts[j][1]);
                }

                double[][][] separatedPixelPts = SeparateTibiaFemur(correctedPixelPts);
                double[][] tibiaPixelPts = separatedPixelPts[0];
                double[][] femurPixelPts = separatedPixelPts[1];

                double[][] tibiaNC1 = GetMatrixNC(tibiaPixelPts);
                double[][] femurNC1 = GetMatrixNC(femurPixelPts);

                //need same clarification as above
                double[][] calibratedTibiaPixel = ncMultiplication(tibiaNC1, tibiaPixelPts);
                double[][] calibratedFemurPixel = ncMultiplication(femurNC1, femurPixelPts);

                double[][] tibiaMatrixT = HomographicMatrixT(calibratedTibFemGlob[0], calibratedTibiaPixel);
                double[][] femurMatrixT = HomographicMatrixT(calibratedTibFemGlob[1], calibratedFemurPixel);

                double[] tibiaMatrixU = MatrixInverseProgram.MatVecProd(MatrixInverseProgram.MatInverse(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatTranspose(tibiaMatrixT), tibiaMatrixT)), TransposeMultPixelPts(tibiaMatrixT, tibiaPixelPts));
                double[] femurMatrixU = MatrixInverseProgram.MatVecProd(MatrixInverseProgram.MatInverse(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatTranspose(femurMatrixT), femurMatrixT)), TransposeMultPixelPts(femurMatrixT, femurPixelPts));

                //matrixH
                double[][] tibiaMatrixH = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(tibiaNC1), PopulateHMultplierMatLeg(tibiaMatrixU)), tibiaNC2);
                double[][] femurMatrixH = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(femurNC1), PopulateHMultplierMatLeg(femurMatrixU)), femurNC2);
                //matrixR
                double[][] tibiaMatrixR = CrossVectors(tibiaMatrixH, true);
                double[][] femurMatrixR = CrossVectors(femurMatrixH, true);

                double[][] tibiaTimeOrigin = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(InitGoProInMat()), GetMultMatrixH(tibiaMatrixH));
                double[][] femurTimeOrigin = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(InitGoProInMat()), GetMultMatrixH(femurMatrixH));

                xt[i] = tibiaTimeOrigin[0][0];
                yt[i] = tibiaTimeOrigin[1][0];
                zt[i] = tibiaTimeOrigin[2][0];

                xf[i] = femurTimeOrigin[0][0];
                yf[i] = femurTimeOrigin[1][0];
                zf[i] = femurTimeOrigin[2][0];

                if (i == 0)
                {
                    tibMatrixR_init = globalToCameraTheta(tibiaMatrixR);
                    femMatrixR_init = globalToCameraTheta(femurMatrixR);
                }

                double[][] tibiaThetaVals = GetThetaValues(tibiaMatrixR, tibMatrixR_init);
                double[][] femurThetaVals = GetThetaValues(femurMatrixR, femMatrixR_init);

                tibiaTheta1[i] = tibiaThetaVals[0][0];
                tibiaTheta2[i] = tibiaThetaVals[1][0];
                tibiaTheta3[i] = tibiaThetaVals[2][0];

                femurTheta1[i] = femurThetaVals[0][0];
                femurTheta2[i] = femurThetaVals[1][0];
                femurTheta3[i] = femurThetaVals[2][0];
            }

            xtG = xt;
            ytG = yt;
            ztG = zt;

            xfG = xf;
            yfG = yf;
            zfG = zf;

            GtibiaTheta1 = tibiaTheta1;
            GtibiaTheta2 = tibiaTheta2;
            GtibiaTheta3 = tibiaTheta3;

            GfemurTheta1 = femurTheta1;
            GfemurTheta2 = femurTheta2;
            GfemurTheta3 = femurTheta3;

            double[][][] legThetas = new double[2][][];
            legThetas[0][0] = tibiaTheta1;
            legThetas[0][1] = tibiaTheta2;
            legThetas[0][2] = tibiaTheta3;
            legThetas[1][0] = femurTheta1;
            legThetas[1][1] = femurTheta2;
            legThetas[1][2] = femurTheta3;

            return legThetas;
        }

        private double[][] GetMultMatrixH(double[][] matrixH)
        {
            double[][] multiplierMatH = new double[matrixH.Length][];
            for (int j = 0; j < multiplierMatH.Length; j++)
            {
                multiplierMatH[j][0] = matrixH[j][3];
            }

            double[][] matToNorm = new double[3][];
            for(int j = 0; j<matToNorm.Length; j++)
            {
                matToNorm[j][0] = matrixH[2][j];
            }

            double[][] returnMat =  MatrixScalarDiv(multiplierMatH, Norm.Norm2(matToNorm));

            return returnMat;
        }

        private void CalculateAngularVelocity(int frames, double[][][] legThetas)
        {
            double interval = 1 / frames; //technically this should be pulled from video info about frame rate

            double[][] tibiaThetas = legThetas[0];
            double[][] femurThetas = legThetas[1];

            double[][] tibiaOmegaDots = GetOmegaDot(tibiaThetas, frames);
            double[][] femurOmegaDots = GetOmegaDot(femurThetas, frames);
        }

        private double[][] GetOmegaDot(double[][] thetas, int frames)
        {
            //angles
            double[] theta1dot = new double[frames];
            double[] theta2dot = new double[frames];
            double[] theta3dot = new double[frames];
            //velocity
            double[] omega1 = new double[frames];
            double[] omega2 = new double[frames];
            double[] omega3 = new double[frames];
            //acceleration
            double[] omega1dot = new double[frames];
            double[] omega2dot = new double[frames];
            double[] omega3dot = new double[frames];

            for (int i = 1; i < frames - 1; i++)
            {
                theta1dot[i] = (thetas[0][i + 1] - thetas[0][i - 1]) / (2 * (1 / frames));
                theta2dot[i] = (thetas[1][i + 1] - thetas[1][i - 1]) / (2 * (1 / frames));
                theta3dot[i] = (thetas[2][i + 1] - thetas[2][i - 1]) / (2 * (1 / frames));

                double[][] omegaPt1 = new double[3][];
                omegaPt1[0] = new double[] { Math.Cos(thetas[1][i]) * Math.Cos(thetas[2][i]), -Math.Sin(thetas[2][i]), 0};
                omegaPt1[1] = new double[] { Math.Cos(thetas[1][i]) * Math.Sin(thetas[2][i]), Math.Cos(thetas[2][i]), 0 };
                omegaPt1[2] = new double[] { -Math.Sin(thetas[2][i]), 0, 1 };

                double[] omegaPt2 = new double[3];
                omegaPt2[0] = theta1dot[i];
                omegaPt2[1] = theta2dot[i];
                omegaPt2[2] = theta3dot[i];

                double[] omegaPt3 = MatrixInverseProgram.MatVecProd(omegaPt1, omegaPt2);

                omega1[i] = omegaPt3[0];
                omega2[i] = omegaPt3[1];
                omega3[i] = omegaPt3[2];
            }

            for(int i=2; i<frames-2; i++)
            {
                omega1dot[i] = (omega1[i + 1] - omega1[i - 1]) / (2 * 1 / frames);
                omega2dot[i] = (omega2[i + 1] - omega2[i - 1]) / (2 * 1 / frames);
                omega3dot[i] = (omega3[i + 1] - omega3[i - 1]) / (2 * 1 / frames);
            }

            double[][] omegaDots = new double[3][];
            omegaDots[0] = omega1dot;
            omegaDots[1] = omega2dot;
            omegaDots[2] = omega3dot;

            return omegaDots;
        }

        public double[][][] CalibrateLegPts(List<double> horizPos, List<double> vertPos, double[][] matrixH)
        {
            double[][] correctedPixelPts = new double[horizPos.Count][];

            for(int i=0; i<horizPos.Count; i++)
            {
                correctedPixelPts[i] = CorrectRadialDistortion(horizPos[i], vertPos[i]);
            }

            //Find the global coordinates of the femur and femur markers
            double[][] globalPts = FindGlobalLegCoords(correctedPixelPts, matrixH);

            double[][][] separatedPts = SeparateTibiaFemur(globalPts);
            double[][] tibiaGlobalPts = separatedPts[0];
            double[][] femurGlobalPts = separatedPts[1];

            double[][] tibiaNC2 = GetMatrixNC(tibiaGlobalPts);
            double[][] femurNC2 = GetMatrixNC(femurGlobalPts);

            //NEED CLARIFICATION - identity matrix of NC2's - not sure if this step is only for testing purposes in matlab?

            double[][] calibratedTibiaGlobal = ncMultiplication(tibiaNC2, tibiaGlobalPts);
            double[][] calibratedFemurGlobal = ncMultiplication(femurNC2, femurGlobalPts);

            //okay stop here and this is where for loop begins
            double[][][] bundleCalibratedTibFemGlob = new double[2][][];
            bundleCalibratedTibFemGlob[0] = calibratedTibiaGlobal;
            bundleCalibratedTibFemGlob[1] = calibratedFemurGlobal;

            return bundleCalibratedTibFemGlob;

/*            double[][][] separatedPixelPts = SeparateTibiaFemur(correctedPixelPts);
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
            double[][] tibiaMatrixH = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(tibiaNC1), PopulateHMultplierMatLeg(tibiaMatrixU)), tibiaNC2);
            double[][] femurMatrixH = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(femurNC1), PopulateHMultplierMatLeg(femurMatrixU)), femurNC2);
            //matrixR
            double[][] tibiaThetaVals = CrossVectors(tibiaMatrixH, true);
            double[][] femurThetaVals = CrossVectors(femurMatrixH, true);*/
        }

        private double[][] CrossVectors(double[][] matrix, bool isLeg)
        {//hardcode for now
            double[][] firstM = new double[1][];
            double[][] secondM = new double[1][];

            firstM[0] = new double[] {matrix[1][0], matrix[1][1], matrix[1][2]};
            secondM[0] = new double[] { matrix[2][0], matrix[2][1], matrix[3][2] };

            double[][] result = MatrixInverseProgram.MatProduct(firstM, secondM);
            double norm = Norm.Norm2(result);
            double[][] q1 = MatrixScalarDiv(result, norm);

            double[][] q3 = MatrixScalarDiv(secondM, Norm.Norm2(secondM));
            double[][] q2 = MatrixInverseProgram.MatProduct(q3, q1);

            double[][] q1T = MatrixInverseProgram.MatTranspose(q1);
            double[][] q2T = MatrixInverseProgram.MatTranspose(q2);
            double[][] q3T = MatrixInverseProgram.MatTranspose(q3);

            double[][] toBeSVD = new double[3][];

            for(int i=0; i < toBeSVD.Length; i++)
            {
                toBeSVD[i] = new double[] { q1T[i][0], q2T[i][0], q3T[i][0]};
            }

            Accord.Math.Decompositions.SingularValueDecomposition svd = new Accord.Math.Decompositions.SingularValueDecomposition(ChangeArrayType(toBeSVD));

            double[][] matrixR = MatrixInverseProgram.MatTranspose(MatrixInverseProgram.MatProduct(ChangeArrayTypeBack(svd.LeftSingularVectors), MatrixInverseProgram.MatTranspose(ChangeArrayTypeBack(svd.RightSingularVectors))));

            return matrixR;
        }

        private double[][] GetThetaValues(double[][] matrixR, double[][] matrixR_init)
        {
            double[][] matrixR_new = MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(matrixR_init), matrixR);
            double theta3 = Math.Atan2(matrixR_new[1][0], matrixR_new[0][0]);
            double theta1 = Math.Atan2(matrixR_new[2][1], matrixR_new[2][2]);
            double theta2 = Math.Atan2(-(matrixR_new[2][0]), (matrixR_new[2][2] / Math.Cos(theta1)));

            double[][] thetaVals = new double[3][]; //{ theta1, theta2, theta3 };
            thetaVals[0][0] = theta1;
            thetaVals[1][0] = theta2;
            thetaVals[2][0] = theta3;

            return thetaVals;
        }

        private double[][] globalToCameraTheta(double[][] matrix)
        {
            double theta1;
            double theta2;
            double theta3;

            theta3 = Math.Atan2(matrix[1][0], matrix[0][0]);
            theta1 = Math.Atan2(matrix[2][1], matrix[2][2]);
            theta2 = Math.Atan2(-(matrix[2][0]), (matrix[2][2] / Math.Cos(theta1)));

            double[][] matrixR = new double[3][];
            matrixR[0] = new double[] {  Math.Cos(theta2) * Math.Cos(theta3), - Math.Sin(theta3) *  Math.Cos(theta1) +  Math.Sin(theta1) *  Math.Sin(theta2) *  Math.Cos(theta3),  Math.Sin(theta1) * Math.Sin(theta3) + Math.Cos(theta1) * Math.Sin(theta2) * Math.Cos(theta3) };
            matrixR[1] = new double[] {  Math.Cos(theta2) * Math.Sin(theta3),  Math.Cos(theta1) *  Math.Cos(theta3) +  Math.Sin(theta1) *  Math.Sin(theta2) *  Math.Sin(theta3), - Math.Sin(theta1) *  Math.Cos(theta3) +  Math.Cos(theta1) *  Math.Sin(theta2) *  Math.Sin(theta3) };
            matrixR[2] = new double[] { -Math.Sin(theta2),  Math.Sin(theta1) *  Math.Cos(theta2),  Math.Cos(theta1) *  Math.Cos(theta2) };

            return matrixR;
        }

        private double[][] MatrixScalarDiv(double[][] matrix, double scalar)
        {
            for(int i=0; i<matrix.Length; i++)
            {
                for(int j=0; j<matrix[i].Length; j++)
                {
                    matrix[i][j] = matrix[i][j] / scalar;
                }
            }

            return matrix;
        }

        private double[][] GetHMatrix(double[][] matrixU, double[][] nc1, double[][] nc2)
        {
            double[] matrixUcol12 = new double[matrixU.Length];
            for(int i=0; i<matrixU.Length; i++)
            {
                matrixUcol12[i] = matrixU[i][12];
            }

            double[][] matrixH = MatrixScalarProd(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(nc1), PopulateHMultplierMat(matrixUcol12)), nc2), matrixUcol12[12]);
            return matrixH;
        }

        private double[][] MatrixScalarProd(double[][] matrix, double scalar)
        {
            for(int i=0; i<matrix.Length; i++)
            {
                for(int j=0; j<matrix[i].Length; j++)
                {
                    matrix[i][j] = matrix[i][j] * scalar;
                }
            }
            return matrix;
        }

        private int SignOfSingleValue(double value)
        {
            if (value == 0)
            {
                return 0;
            }
            else if (value < 0)
            {
                return -1;
            }
            else if (value > 0)
            {
                return 1;
            }
            else
            {
                Console.WriteLine("The value passed is complex.");
                return 0;
            }
        }

        private double[][] GetUFromSVD(double[][] matrixT)
        {
            Accord.Math.Decompositions.SingularValueDecomposition svd = new Accord.Math.Decompositions.SingularValueDecomposition(ChangeArrayType(MatrixInverseProgram.MatProduct(MatrixInverseProgram.MatInverse(matrixT), matrixT)));
            double[][] matrixU = ChangeArrayTypeBack(svd.LeftSingularVectors);

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

        private double[][] ChangeArrayTypeBack(double[,] matrix)
        {
            double[][] returnMatrix = new double[matrix.GetLength(0)][];

            for (int i = 0; i < returnMatrix.Length; i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    returnMatrix[i][j] = matrix[i,j];
                }
            }

            return returnMatrix;
        }

        private double[][] PopulateHMultplierMat(double[] matrixU)
        {
            double[][] multiplierMat = new double[3][];
            multiplierMat[0] = new double[] { matrixU[0], matrixU[1], matrixU[2], matrixU[3] };
            multiplierMat[1] = new double[] { matrixU[4], matrixU[5], matrixU[6], matrixU[7] };
            multiplierMat[2] = new double[] { matrixU[8], matrixU[9], matrixU[10], matrixU[11] };

            return multiplierMat;
        }

        private double[][] PopulateHMultplierMatLeg(double[] matrixU)
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

        private double[][] InitGoProInMat()
        {
            // GoPro camera intrinsic matrix
            double[][] intrinsicMatrixGoPro = new double[3][];
            intrinsicMatrixGoPro[0] = new double[] { fx, 0, 0 };
            intrinsicMatrixGoPro[1] = new double[] { 0, fy, 0 };
            intrinsicMatrixGoPro[2] = new double[] { 0, 0, 1 };

            return intrinsicMatrixGoPro;
        }

        public List<double> GetFirstFrameVals(List<string> csvString)
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
                            homGraphT[row][col] = globalPts[row/2][col];
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
                            homGraphT[row][col] = globalPts[row / 2][col - 8]*pixelPts[row / 2][0];
                        }
                        else if(col == 11)
                        {
                            homGraphT[row][col] = pixelPts[row / 2][0];
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
                            homGraphT[row][col] = globalPts[row / 2][col-4];
                        }
                        else if (col == 7)
                        {
                            homGraphT[row][col] = 1;
                        }
                        else if (col > 7 && col < 11)
                        {
                            homGraphT[row][col] = -(globalPts[row / 2][col - 8] * pixelPts[row / 2][1]);
                        }
                        else if (col == 11)
                        {
                            homGraphT[row][col] = -(pixelPts[row / 2][1]);
                        }
                    }
                }
            }

            return homGraphT;
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
