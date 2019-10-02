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

        bool ageChart;

        public Chart(PatientModel model, InspectionModel currentInspection, bool ageChart = true)
        {
            InitializeComponent();
            pwi = new PatientWithInspection();
            pwi.Patient = model;
            pwi.Inspection = currentInspection;
            this.leftEye = leftEye;

            DataContext = pwi;

            this.ageChart = ageChart;
            if (!ageChart)
            {
                tbDesc.Text = "身 高（0-180cm）";
            }

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
            var oiriginPositionYSPH = 332;
            var oiriginPositionYAxial = 601;
            var originPostionX = 0;

            //x轴
            DrawAxis(new Point(xmin, oiriginPositionYSPH), new Point(canGraph.Width, oiriginPositionYSPH), 180, 18);
            //y轴
            DrawAxis(new Point(xmin, 0), new Point(xmin, canGraph.Height), 180, 45);
            //DrawAxis(new Point(xmax, 0), new Point(xmax, canGraph.Height), 180);


            var orderedModels = model.Inspections.OrderBy(p => p.Age).ToList();

            var pxPerAge = 3.6111;
            var pxPerHeight = 3.6111;
            var pxPerSPH = 13.33;
            var pxPerEyeAxial = 33.33;

            var pointsAgeLeftAxial = new PointCollection();
            var pointsAgeRightAxial = new PointCollection();
            var pointsAgeLeftSPH = new PointCollection();
            var pointsAgeRightSPH = new PointCollection();

            var pointsHeightLeftAxial = new PointCollection();
            var pointsHeightRightAxial = new PointCollection();
            var pointsHeightLeftSPH = new PointCollection();
            var pointsHeightRightSPH = new PointCollection();

            var heightOrderedModels = model.Inspections.OrderBy(p => p.Height).ToList();

            if (!ageChart)
            {
                //画点连线，身高
                for (int i = 0; i < heightOrderedModels.Count; i++)
                {
                    var x = xmin + orderedModels[i].Height * pxPerHeight;

                    var yLeftEyeSPH = oiriginPositionYSPH + (orderedModels[i].LeftEye.SPH * pxPerSPH);
                    var yRightEyeSPH = oiriginPositionYSPH + (orderedModels[i].RightEye.SPH * pxPerSPH);
                    var yLeftEyeAxial = oiriginPositionYAxial - ((orderedModels[i].LeftEye.EyeAxial - 17) * pxPerEyeAxial);
                    var yRightEyeAxial = oiriginPositionYAxial - ((orderedModels[i].RightEye.EyeAxial - 17) * pxPerEyeAxial);

                    pointsHeightLeftSPH.Add(new Point(x, yLeftEyeSPH));
                    pointsHeightRightSPH.Add(new Point(x, yRightEyeSPH));
                    pointsHeightLeftAxial.Add(new Point(x, yLeftEyeAxial));
                    pointsHeightRightAxial.Add(new Point(x, yRightEyeAxial));
                }
            }
            else
            {
                //画点连线，年龄
                for (int i = 0; i < orderedModels.Count; i++)
                {
                    //月份x每月占像素，先把岁转成月，后续按照实际月份来算
                    var x = xmin + orderedModels[i].Age * pxPerAge;

                    //y轴有些不同，y的最大值是坐标0点，但是在画布上是左下角点（即top大，left0的点）
                    //所以用视力的最大值去减，得到的结果就从y的最小值上按比例加，越大越靠上方
                    //ymax / 2作为0点，+-要根据屈光度取反（为了对应坐标轴）
                    //左眼
                    var yLeftEyeSPH = oiriginPositionYSPH + (orderedModels[i].LeftEye.SPH * pxPerSPH);
                    var yRightEyeSPH = oiriginPositionYSPH + (orderedModels[i].RightEye.SPH * pxPerSPH);
                    var yLeftEyeAxial = oiriginPositionYAxial - ((orderedModels[i].LeftEye.EyeAxial - 17) * pxPerEyeAxial);
                    var yRightEyeAxial = oiriginPositionYAxial - ((orderedModels[i].RightEye.EyeAxial - 17) * pxPerEyeAxial);

                    pointsAgeLeftSPH.Add(new Point(x, yLeftEyeSPH));
                    pointsAgeRightSPH.Add(new Point(x, yRightEyeSPH));
                    pointsAgeLeftAxial.Add(new Point(x, yLeftEyeAxial));
                    pointsAgeRightAxial.Add(new Point(x, yRightEyeAxial));
                }
            }

            //年龄
            Polyline polylineAgeLeftSPH = new Polyline();
            polylineAgeLeftSPH.StrokeThickness = 2;
            polylineAgeLeftSPH.Stroke = Brushes.Red;
            polylineAgeLeftSPH.Points = pointsAgeLeftSPH;

            Polyline polylineRightSPH = new Polyline();
            polylineRightSPH.StrokeThickness = 2;
            polylineRightSPH.Stroke = Brushes.Blue;
            polylineRightSPH.Points = pointsAgeRightSPH;

            Polyline polylineAgeLeftAxial = new Polyline();
            polylineAgeLeftAxial.StrokeThickness = 2;
            polylineAgeLeftAxial.Stroke = Brushes.Yellow;
            polylineAgeLeftAxial.Points = pointsAgeLeftAxial;

            Polyline polylineRightAxial = new Polyline();
            polylineRightAxial.StrokeThickness = 2;
            polylineRightAxial.Stroke = Brushes.Green;
            polylineRightAxial.Points = pointsAgeRightAxial;

            canGraph.Children.Add(polylineAgeLeftSPH);
            canGraph.Children.Add(polylineRightSPH);
            canGraph.Children.Add(polylineAgeLeftAxial);
            canGraph.Children.Add(polylineRightAxial);



            //身高
            Polyline p1 = new Polyline();
            p1.StrokeThickness = 2;
            p1.Stroke = Brushes.Red;
            p1.Points = pointsHeightLeftSPH;

            Polyline p2 = new Polyline();
            p2.StrokeThickness = 2;
            p2.Stroke = Brushes.Blue;
            p2.Points = pointsHeightRightSPH;

            Polyline p3 = new Polyline();
            p3.StrokeThickness = 2;
            p3.Stroke = Brushes.Yellow;
            p3.Points = pointsHeightLeftAxial;

            Polyline p4 = new Polyline();
            p4.StrokeThickness = 2;
            p4.Stroke = Brushes.Green;
            p4.Points = pointsHeightRightAxial;

            canGraph.Children.Add(p1);
            canGraph.Children.Add(p2);
            canGraph.Children.Add(p3);
            canGraph.Children.Add(p4);
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


        /// <summary>
        /// 画轴线，刻度只在轴线为水平或垂直状态下有效
        /// </summary>
        /// <param name="start">开始点位置</param>
        /// <param name="end">结束点位置</param>
        /// <param name="scaleCount">刻度数量</param>
        /// <param name="sectionCount">大刻度数量（以多少小刻度为一个大刻度）</param>
        void DrawAxis(Point start, Point end, int scaleCount, int sectionCount = 10)
        {
            var geom = new GeometryGroup();
            //轴线
            var line = new LineGeometry(start, end);
            geom.Children.Add(line);
            //在点间画刻度
            var diffX = end.X - start.X - 1;
            diffX = diffX < 0 ? 0 : diffX;

            var diffY = end.Y - start.Y - 1;
            diffY = diffY < 0 ? 0 : diffY;

            //差值除以刻度数量
            var stepX = diffX / scaleCount;
            var stepY = diffY / scaleCount;

            const double margin = 3;

            //刻度数比分格多1
            for (int i = 0; i < scaleCount + 1; i++)
            {
                var tableCell = false;
                var addition = 0;
                if (i == 0 || i % (scaleCount / sectionCount) == 0)
                {
                    addition = 3;
                    //加网格
                    tableCell = true;
                }
                //把轴线分割成n份
                var x = start.X + stepX * (i);
                var y = start.Y + stepY * (i);

                //不做三角距离处理（即斜线的刻度）
                //只考虑横竖的话，那么diffX和diffY一定会有一个为0
                var xM = diffX == 0 ? 0 : 1;
                var yM = diffY == 0 ? 0 : 1;

                var startX = x + margin * yM + addition * yM;
                var startY = y + margin * xM + addition * xM;
                var endX = x - margin * yM - addition * yM;
                var endY = y - margin * xM - addition * xM;

                //在画y轴
                if (xM == 0 && tableCell)
                {
                    startX = x;
                    endX = (int)canGraph.Width;
                }

                //在画x轴
                if (yM == 0 && tableCell)
                {
                    startY = 0;
                    endY = (int)canGraph.Height;
                }

                //这里x和y轴是相反的，当xM为0的时候表示x要波动形成垂直于y轴的刻度
                var scaleLine = new LineGeometry(new Point(startX, startY), new Point(endX, endY));
                geom.Children.Add(scaleLine);
            }

            var path = new Path();
            path.StrokeThickness = 1;
            path.Stroke = Brushes.Black;
            path.Data = geom;

            canGraph.Children.Add(path);
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
