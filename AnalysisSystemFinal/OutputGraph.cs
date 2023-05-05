using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace AnalysisSystemFinal
{
    public partial class OutputGraph : Form
    {
        private string savableCSV;

        public OutputGraph()
        {
            InitializeComponent();

            PlotModel model = new PlotModel();
            model.PlotType = PlotType.XY;

            model.Title = "title";

            LinearAxis xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.MajorGridlineStyle = LineStyle.Solid;
            xAxis.MinorGridlineStyle = LineStyle.Dot;
            xAxis.MinimumPadding = 0.02;
            xAxis.MaximumPadding = 0.05;
            xAxis.Title = "Time";
            model.Axes.Add(xAxis);


            LinearAxis yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineStyle = LineStyle.Dot;
            yAxis.MinimumPadding = 0.05;
            yAxis.MaximumPadding = 0.1;
            yAxis.Title = "y title";
            model.Axes.Add(yAxis);

            plotView.Model = model;

            LineSeries series = new LineSeries();
            series.Title = "line";
            series.MarkerType = MarkerType.None;

            
        }

        public OutputGraph(string xAxisTitle, string yAxisTitle, int frames, double[] Mx, double[] My, double[] Mz, double[][] dataHorizontal)
        {
            InitializeComponent();

            PlotModel model = new PlotModel();
            model.PlotType = PlotType.XY;

            model.Title = "Moment vs Frame";

            LinearAxis xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.MajorGridlineStyle = LineStyle.Solid;
            xAxis.MinorGridlineStyle = LineStyle.Dot;
            xAxis.MinimumPadding = 0.02;
            xAxis.MaximumPadding = 0.05;
            xAxis.Title = xAxisTitle;
            model.Axes.Add(xAxis);


            LinearAxis yAxis = new LinearAxis();
            yAxis.Position = AxisPosition.Left;
            yAxis.MajorGridlineStyle = LineStyle.Solid;
            yAxis.MinorGridlineStyle = LineStyle.Dot;
            yAxis.MinimumPadding = 0.05;
            yAxis.MaximumPadding = 0.1;
            yAxis.Title = yAxisTitle;
            model.Axes.Add(yAxis);

            plotView.Model = model;

/*            LineSeries series = new LineSeries();
            series.Title = "marker 1";
            series.MarkerType = MarkerType.None;

            for(int i=0; i<frames-3; i++)
            {
                //series.Points.Add(new DataPoint(dataHorizontal[1][i], Mx[i]));
                series.Points.Add(new DataPoint(dataHorizontal[1][i], Mx[i]));
            }

            plotView.Model.Series.Add(series);

            LineSeries series1 = new LineSeries();
            series1.Title = "marker 2";
            series1.MarkerType = MarkerType.None;

            for (int i = 0; i < frames - 3; i++)
            {
                //series.Points.Add(new DataPoint(dataHorizontal[1][i], Mx[i]));
                series1.Points.Add(new DataPoint(dataHorizontal[2][i], Mx[i]));
            }

            plotView.Model.Series.Add(series1);

            LineSeries series2 = new LineSeries();
            series2.Title = "marker 3";
            series2.MarkerType = MarkerType.None;

            for (int i = 0; i < frames - 3; i++)
            {
                //series.Points.Add(new DataPoint(dataHorizontal[1][i], Mx[i]));
                series2.Points.Add(new DataPoint(dataHorizontal[3][i], Mx[i]));
            }

            plotView.Model.Series.Add(series2);

            LineSeries series3 = new LineSeries();
            series3.Title = "marker 4";
            series3.MarkerType = MarkerType.None;

            for (int i = 0; i < frames - 3; i++)
            {
                //series.Points.Add(new DataPoint(dataHorizontal[1][i], Mx[i]));
                series3.Points.Add(new DataPoint(dataHorizontal[4][i], Mx[i]));
            }

            plotView.Model.Series.Add(series3);

            LineSeries series4 = new LineSeries();
            series4.Title = "marker 5";
            series4.MarkerType = MarkerType.None;

            for (int i = 0; i < frames - 3; i++)
            {
                //series.Points.Add(new DataPoint(dataHorizontal[1][i], Mx[i]));
                series4.Points.Add(new DataPoint(dataHorizontal[5][i], Mx[i]));
            }

            plotView.Model.Series.Add(series4);

            LineSeries series5 = new LineSeries();
            series5.Title = "marker 6";
            series5.MarkerType = MarkerType.None;

            for (int i = 0; i < frames - 3; i++)
            {
                //series.Points.Add(new DataPoint(dataHorizontal[1][i], Mx[i]));
                series5.Points.Add(new DataPoint(dataHorizontal[6][i], Mx[i]));
            }

            plotView.Model.Series.Add(series5);*/

            LineSeries momentX = new LineSeries();
            LineSeries momentY = new LineSeries();
            LineSeries momentZ = new LineSeries();

            momentX.Title = "moment in X";
            momentY.Title = "moment in Y";
            momentZ.Title = "moment in Z";

            momentX.MarkerType = MarkerType.None;
            momentY.MarkerType = MarkerType.None;
            momentZ.MarkerType = MarkerType.None;

            for (int i = 0; i < frames-3; i++)
            {
                momentX.Points.Add(new DataPoint(i, Mx[i]));
                momentY.Points.Add(new DataPoint(i, My[i]));
                momentZ.Points.Add(new DataPoint(i, Mz[i]));
            }

            plotView.Model.Series.Add(momentX);
            plotView.Model.Series.Add(momentY);
            plotView.Model.Series.Add(momentZ);
        }

        public void SaveDataToFile(double[] Mx, double[] My, double[] Mz)
        {
            List<string> csvToSave = new List<string>();

            csvToSave.Add("Frame,X,Y,Z");

            for(int i = 1; i < Mx.Length; i++)
            {
                csvToSave.Add(i + "," + Mx[i] + "," + My[i] + "," + Mz[i]);
            }

            StringBuilder b = new StringBuilder();

            foreach (string line in csvToSave)
            {
                b.AppendLine(line);
            }

            savableCSV = b.ToString();

            saveFileDialog1.ShowDialog();
            saveFileDialog1.Dispose();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            File.WriteAllText(saveFileDialog1.FileName, savableCSV);
        }
    }
}
