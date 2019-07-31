using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

        bool leftEye;

        public Chart(PatientModel model, InspectionModel currentInspection, bool leftEye = true)
        {
            InitializeComponent();
            pwi = new PatientWithInspection();
            pwi.Patient = model;
            pwi.Inspection = currentInspection;
            this.leftEye = leftEye;

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
            double xmax = (int)canGraph.Width;

            const double yscale = 50;
            double ymin = yscale;
            double ymax = (int)canGraph.Height - yscale;
            double yLength = ymax - ymin;

            //坐标轴分成多少份
            var sectionCounts = 10;
            //x坐标使用年龄以月为单位，从0-216个月即18岁为止，所以步长为4，共216个刻度698宽
            double xstep = 3;
            //y1坐标使用屈光度，从-20到+30,步长6，共100个刻度600高
            double y1step = 6;
            //y2坐标眼轴，从 5到35，共30高度分为300个刻度
            double y2step = 3;

            //坐标原点，在y最大值的3分之2处
            var oiriginPositionY = ymax / 3 * 2;
            var originPostionX = 0;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            //x轴线
            //ymax / 3 * 2;表示x轴放在y轴的中间，形成+-效果
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, oiriginPositionY), new Point(canGraph.Width, oiriginPositionY)));
            //x轴点
            for (double x = xmin + xstep; x <= canGraph.Width - xstep; x += xstep)
            {
                var additional = 0;


                var y1 = oiriginPositionY - margin / 2;
                var y2 = oiriginPositionY + margin / 2;

                if ((x - xmin) % 36 == 0)
                {
                    additional = 3;

                    //x轴画刻度
                    //每一岁即12个月
                    var tb = new TextBlock
                    {
                        Text = ((x - xmin) / 36).ToString(),
                    };
                    Canvas.SetLeft(tb, x);
                    //往下移5个px防止重叠
                    Canvas.SetTop(tb, y1 + 5);
                    canGraph.Children.Add(tb);
                    //每10cm
                    //最终是18个节点
                    var tbHeight = new TextBlock
                    {
                        Text = ((x - xmin) / 36 * 10).ToString(),
                    };
                    Canvas.SetLeft(tbHeight, x);
                    Canvas.SetTop(tbHeight, y1 - 20);
                    canGraph.Children.Add(tbHeight);
                }

                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, y1 - additional),
                    new Point(x, y2 + additional)));
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
            for (double y = 0; y <= canGraph.Height; y += y1step)
            {
                var additional = 0;

                if (y % 24 == 0)
                {
                    additional = 3;

                    var tb = new TextBlock
                    {
                        Text = (y / 12 - 20).ToString(),
                    };
                    Canvas.SetLeft(tb, 15);
                    //往下移5个px防止重叠
                    Canvas.SetTop(tb, (int)canGraph.Height - y);
                    canGraph.Children.Add(tb);
                    //每10cm
                    //最终是18个节点
                }

                var x1 = xmin - margin / 2 - additional;
                var x2 = xmin + margin / 2 + additional;

                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(x1, y),
                    new Point(x2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;
            canGraph.Children.Add(yaxis_path);

            //y2轴,，眼轴
            GeometryGroup y2axis_geom = new GeometryGroup();
            y2axis_geom.Children.Add(new LineGeometry(new Point(xmax, 0), new Point(xmax, canGraph.Height)));
            //y2轴点
            for (double y = 0; y <= canGraph.Height; y += y2step)
            {
                var additional = 0;

                if (y % 30 == 0)
                {
                    additional = 3;

                    var tb = new TextBlock
                    {
                        Text = (y / 30 + 5).ToString(),
                    };
                    Canvas.SetLeft(tb, xmax + 10);
                    //往下移5个px防止重叠
                    Canvas.SetTop(tb, canGraph.Height - y);
                    canGraph.Children.Add(tb);
                }

                var x1 = xmax - margin / 2 - additional;
                var x2 = xmax + margin / 2 + additional;

                y2axis_geom.Children.Add(new LineGeometry(
                    new Point(x1, y),
                    new Point(x2, y)));
            }

            Path y2axis_path = new Path();
            y2axis_path.StrokeThickness = 1;
            y2axis_path.Stroke = Brushes.Black;
            y2axis_path.Data = y2axis_geom;
            canGraph.Children.Add(y2axis_path);


            var orderedModels = model.Inspections.OrderBy(p => p.Age).ToList();

            var pxPerAge = 3;
            var pxPerSPH = 12;

            //画点连线，年龄
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

                points.Add(new Point(x, leftEye ? yLeftEye : yRightEye));
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 2;
            polyline.Stroke = Brushes.Red;
            polyline.Points = points;

            canGraph.Children.Add(polyline);

            var pointsHeight = new PointCollection();
            var heightOrderedModels = model.Inspections.OrderBy(p => p.Height).ToList();
            var pxPerHeight = 3.6111;
            var pxPerEyeAxial = 3;
            //画点连线，身高
            for (int i = 0; i < heightOrderedModels.Count; i++)
            {
                var x = xmin + heightOrderedModels[i].Height * pxPerHeight;

                var yLeft = oiriginPositionY - (heightOrderedModels[i].LeftEye.EyeAxial * pxPerEyeAxial);
                var yRight = oiriginPositionY - (heightOrderedModels[i].RightEye.EyeAxial * pxPerEyeAxial);

                pointsHeight.Add(new Point(x, leftEye ? yLeft : yRight));
            }

            Polyline polylineHeight = new Polyline();
            polylineHeight.StrokeThickness = 2;
            polylineHeight.Stroke = Brushes.Blue;
            polylineHeight.Points = pointsHeight;

            canGraph.Children.Add(polylineHeight);
        }

        private async void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageDialogResult clickresult = await this.ShowMessageAsync(this.Title, "是否确定打印本页？", MessageDialogStyle.AffirmativeAndNegative);
            if (clickresult == MessageDialogResult.Negative)
            {
                return;
            }

            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                printTime.Text = DateTime.Now.ToString("yyyy年MM月dd日");
                dialog.PrintVisual(canvasMain, "Print");
            }
        }
    }

    class PatientWithInspection
    {
        public PatientModel Patient { get; set; }
        public InspectionModel Inspection { get; set; }

        public string OpDate
        {
            get
            {
                DateTime result;
                if (DateTime.TryParse(Inspection.OptometryDate, out result))
                {
                    return result.ToString("yyyy年MM月dd日");
                }

                return string.Empty;
            }
            set { }
        }
    }

}
