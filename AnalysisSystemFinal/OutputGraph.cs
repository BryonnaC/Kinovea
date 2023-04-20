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

            //Application.Run();
        }
    }
}
