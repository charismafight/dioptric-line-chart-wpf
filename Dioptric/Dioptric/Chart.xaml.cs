using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dioptric
{
    /// <summary>
    /// Chart.xaml 的交互逻辑
    /// </summary>
    public partial class Chart : Window
    {
        public Chart()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //data
            var dic = new List<KeyValuePair<int, int>>()
            {
                new KeyValuePair<int, int>(1,10),
                new KeyValuePair<int, int>(2,20),
                new KeyValuePair<int, int>(3,10),
                new KeyValuePair<int, int>(4,30),
            };


            PointCollection points = new PointCollection();
            //像素间隔，用来画轴线上的标记刻度的
            const double margin = 8;

            //x最小50-最大910
            const double xscale = 50;
            double xmin = xscale;
            double xmax = (int)canGraph.Width - xscale;
            //x轴的pixel
            double xLength = xmax - xmin;

            const double yscale = 50;
            double ymin = yscale;
            double ymax = (int)canGraph.Height - yscale;
            double yLength = ymax - ymin;
            //步长很简单x的就是x的max/count，y同理
            //count就由外部的记录数决定了
            var count = dic.Count;
            double xstep = xLength / count;
            double ystep = yLength / count;

            var xaxis = new List<double>();
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            //x轴线
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax), new Point(canGraph.Width, ymax)));
            //x轴点，点个数直接取决于传入的记录条数
            for (double x = xmin + xstep; x <= canGraph.Width - xstep; x += xstep)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - margin / 2),
                    new Point(x, ymax + margin / 2)));
                xaxis.Add(x);
                var tb = new TextBlock() { Text = dic[xaxis.Count].Key.ToString() };
                Canvas.SetLeft(tb, x);
                Canvas.SetTop(tb, ymax);
                canGraph.Children.Add(tb);
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            //// Make the Y ayis.
            var yaxis = new List<double>();
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = ystep; y <= canGraph.Height - ystep; y += ystep)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));
                yaxis.Add(y);
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

            for (int i = 0; i < xaxis.Count; i++)
            {
                if (i == 0)
                {
                    points.Add(new Point(xmin, ymin));
                }
                else
                {
                    points.Add(new Point(xaxis[i], yaxis[i]));
                }
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 1;
            polyline.Stroke = Brushes.Red;
            polyline.Points = points;

            canGraph.Children.Add(polyline);
        }
    }
}
