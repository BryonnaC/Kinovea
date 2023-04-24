using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace AnalysisSystemFinal
{
    public partial class OutputGraph : Form
    {
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

            model.Title = "Moment vs Horizontal Position";

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

            LineSeries series = new LineSeries();
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

            plotView.Model.Series.Add(series5);

            //OxyPlot.

            //series.Points
        }
    }
}
