using MahApps.Metro.Controls;
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
    public partial class Chart : MetroWindow
    {
        PatientWithInspection pwi;

        public Chart(PatientModel model, InspectionModel currentInspection)
        {
            InitializeComponent();
            pwi = new PatientWithInspection();
            pwi.Patient = model;
            pwi.Inspection = currentInspection;

            DataContext = pwi;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //DrawArea(model);
            Title = $"姓名：{pwi.Patient.Name} 性别：{pwi.Patient.Sex} 身份证号：{pwi.Patient.IDCardNumber}";
        }

        void DrawArea(PatientModel model)
        {
            //var maxAge = models.Max(p => p.Age);
            //var minAge = models.Min(p => p.Age);

            ////var maxEyeSight = models.Max(p => p.EyeSight);
            ////var minEyeSight = models.Min(p => p.EyeSight);

            //PointCollection points = new PointCollection();
            ////像素间隔，用来画轴线上的标记刻度的
            //const double margin = 8;

            ////x最小50-最大910
            ////scale为轴的两端空余px
            //const double xscale = 50;
            //double xmin = xscale;
            //double xmax = (int)canGraph.Width - xscale;
            ////x轴的pixel
            //double xLength = xmax - xmin;
            ////用x长度来表示多少年龄区间的值，算出一个px表示多少年龄
            //var agePerPx = xLength / (maxAge - minAge);

            //const double yscale = 50;
            //double ymin = yscale;
            //double ymax = (int)canGraph.Height - yscale;
            //double yLength = ymax - ymin;
            ////用y长度来表示视力区间，算出一个px表示多少视力
            ////var eyeSightPerPx = yLength / (maxEyeSight - minEyeSight);
            ////步长很简单x的就是x的max/count，y同理
            ////坐标轴分成多少份
            //var sectionCounts = 10;
            //double xstep = xLength / sectionCounts;
            //double ystep = yLength / sectionCounts;

            //// Make the X axis.
            //GeometryGroup xaxis_geom = new GeometryGroup();
            ////x轴线
            //xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax), new Point(canGraph.Width, ymax)));
            ////x轴点，点个数直接取决于传入的记录条数
            //for (double x = xmin + xstep; x <= canGraph.Width - xstep; x += xstep)
            //{
            //    xaxis_geom.Children.Add(new LineGeometry(
            //        new Point(x, ymax - margin / 2),
            //        new Point(x, ymax + margin / 2)));
            //}

            //Path xaxis_path = new Path();
            //xaxis_path.StrokeThickness = 1;
            //xaxis_path.Stroke = Brushes.Black;
            //xaxis_path.Data = xaxis_geom;

            //canGraph.Children.Add(xaxis_path);

            ////// Make the Y ayis.
            //GeometryGroup yaxis_geom = new GeometryGroup();
            //yaxis_geom.Children.Add(new LineGeometry(
            //    new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            //for (double y = ystep; y <= canGraph.Height - ystep; y += ystep)
            //{
            //    yaxis_geom.Children.Add(new LineGeometry(
            //        new Point(xmin - margin / 2, y),
            //        new Point(xmin + margin / 2, y)));
            //}

            //Path yaxis_path = new Path();
            //yaxis_path.StrokeThickness = 1;
            //yaxis_path.Stroke = Brushes.Black;
            //yaxis_path.Data = yaxis_geom;

            //canGraph.Children.Add(yaxis_path);

            //var orderedModels = models.OrderBy(p => p.Age).ToList();
            ////画点连线
            //for (int i = 0; i < orderedModels.Count; i++)
            //{
            //    var ageDiff = orderedModels[i].Age - minAge;
            //    var x = xmin + ageDiff * agePerPx;

            //    //y轴有些不同，y的最大值是坐标0点，但是在画布上是左下角点（即top大，left0的点）
            //    //所以用视力的最大值去减，得到的结果就从y的最小值上按比例加，越大越靠上方
            //    var eyeSightDiff = maxEyeSight - orderedModels[i].EyeSight;
            //    var y = ymin + eyeSightDiff * eyeSightPerPx;

            //    points.Add(new Point(x, y));

            //    var tbX = new TextBlock();
            //    tbX.Text = orderedModels[i].Age.ToString();
            //    Canvas.SetLeft(tbX, x);
            //    Canvas.SetTop(tbX, ymax);
            //    canGraph.Children.Add(tbX);

            //    var tbY = new TextBlock();
            //    tbY.Text = orderedModels[i].EyeSight.ToString();
            //    if (orderedModels[i].EyeSight == minEyeSight)
            //    {
            //        Canvas.SetTop(tbY, y - 15);
            //    }
            //    else
            //    {
            //        Canvas.SetTop(tbY, y);
            //    }
            //    Canvas.SetLeft(tbY, xmin - 20);

            //    canGraph.Children.Add(tbY);
            //}

            //Polyline polyline = new Polyline();
            //polyline.StrokeThickness = 1;
            //polyline.Stroke = Brushes.Red;
            //polyline.Points = points;

            //canGraph.Children.Add(polyline);
        }
    }

    class PatientWithInspection
    {
        public PatientModel Patient { get; set; }
        public InspectionModel Inspection { get; set; }
    }

}
