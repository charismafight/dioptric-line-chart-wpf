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

            DrawArea(model);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //DrawArea(model);
            Title = $"姓名：{pwi.Patient.Name} 性别：{pwi.Patient.Sex} 身份证号：{pwi.Patient.IDCardNumber}";
        }

        void DrawArea(PatientModel model)
        {
            var maxAge = model.Inspections.Max(p => p.Age);
            var minAge = model.Inspections.Min(p => p.Age);

            //var maxEyeSight = models.Max(p => p.EyeSight);
            //var minEyeSight = models.Min(p => p.EyeSight);

            PointCollection points = new PointCollection();
            //刻度长度，用来画轴线上的标记刻度的
            const double margin = 5;

            //scale为轴的两端空余px
            const double xscale = 50;
            double xmin = xscale;

            const double yscale = 50;
            double ymin = yscale;
            double ymax = (int)canGraph.Height - yscale;
            double yLength = ymax - ymin;

            //坐标轴分成多少份
            var sectionCounts = 10;
            //x坐标使用年龄以月为单位，从0-216个月即18岁为止，所以步长为4，共216个刻度698宽
            double xstep = 3;
            //y1坐标使用屈光度，从-20到+30,步长6，共100个刻度600高
            double ystep = 6;

            //坐标原点
            var oiriginPositionY = ymax / 2;
            var originPostionX = 0;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            //x轴线
            //ymax/2表示x轴放在y轴的中间，形成+-效果
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, oiriginPositionY), new Point(canGraph.Width, oiriginPositionY)));
            //x轴点
            for (double x = xmin + xstep; x <= canGraph.Width - xstep; x += xstep)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, oiriginPositionY - margin / 2),
                    new Point(x, oiriginPositionY + margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            //// Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = ystep; y <= canGraph.Height - ystep; y += ystep)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

            var orderedModels = model.Inspections.OrderBy(p => p.Age).ToList();

            var pxPerAge = 4;
            var pxPerSPH = 12;

            //画点连线
            for (int i = 0; i < orderedModels.Count; i++)
            {
                //月份x每月占像素，先把岁转成月，后续按照实际月份来算
                var x = xmin + orderedModels[i].Age * 12 * pxPerAge;

                //y轴有些不同，y的最大值是坐标0点，但是在画布上是左下角点（即top大，left0的点）
                //所以用视力的最大值去减，得到的结果就从y的最小值上按比例加，越大越靠上方
                //ymax / 2作为0点，+-要根据屈光度取反（为了对应坐标轴）
                //左眼
                var yLeftEye = oiriginPositionY - (orderedModels[i].LeftEye.SPH * pxPerSPH);
                //右眼
                var yRightEye = oiriginPositionY - (orderedModels[i].RightEye.SPH * pxPerSPH);

                points.Add(new Point(x, yLeftEye));

                //var tbX = new TextBlock();
                //tbX.Text = orderedModels[i].Age.ToString();
                //Canvas.SetLeft(tbX, x);
                //Canvas.SetTop(tbX, ymax);
                //canGraph.Children.Add(tbX);

                //var tbY = new TextBlock();
                //tbY.Text = orderedModels[i].LeftEye.SPH.ToString();
                //if (orderedModels[i].LeftEye.SPH == 30)
                //{
                //    Canvas.SetTop(tbY, yLeftEye - 15);
                //}
                //else
                //{
                //    Canvas.SetTop(tbY, yLeftEye);
                //}

                //Canvas.SetLeft(tbY, xmin - 20);

                //canGraph.Children.Add(tbY);
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 2;
            polyline.Stroke = Brushes.Red;
            polyline.Points = points;

            canGraph.Children.Add(polyline);
        }
    }

    class PatientWithInspection
    {
        public PatientModel Patient { get; set; }
        public InspectionModel Inspection { get; set; }
    }

}
