using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dioptric
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new PatientDetail();
            win.ShowDialog();

            Window_Loaded(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new PatientContext())
            {
                var i = db.Patients.Count();
                DataContext = db.Patients.OrderBy(p => p.Name).ToList();
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCase.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个病例后编辑");
                return;
            }

            var win = new PatientDetail(dgCase.SelectedItems[0] as PatientModel);
            win.ShowDialog();
        }

        private async void BtnShowChart_Click(object sender, RoutedEventArgs e)
        {
            ShowChart(true);
        }

        private async void BtnShowRightEyeChart_Click(object sender, RoutedEventArgs e)
        {
            ShowChart(false);
        }

        private async void ShowChart(bool leftEye)
        {
            if (dgCase.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个病例后查看");
                return;
            }

            if (dgInspections.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个检查记录后查看");
                return;
            }

            var selectedItem = dgCase.SelectedItems[0] as PatientModel;

            var all = DataContext as List<PatientModel>;
            using (var db = new PatientContext())
            {
                var individualModel = db.Patients.Include("Inspections").SingleOrDefault(p => p.IDCardNumber == selectedItem.IDCardNumber);

                foreach (var inspection in individualModel.Inspections)
                {
                    inspection.LeftEye = db.Eyes.SingleOrDefault(p => p.Id == inspection.LeftEyeId);
                    inspection.RightEye = db.Eyes.SingleOrDefault(p => p.Id == inspection.RightEyeId);
                }

                var currentInspection = dgInspections.SelectedItem as InspectionModel;
                currentInspection.LeftEye = db.Eyes.SingleOrDefault(p => p.Id == currentInspection.LeftEyeId);
                currentInspection.RightEye = db.Eyes.SingleOrDefault(p => p.Id == currentInspection.RightEyeId);

                var chart = new Chart(individualModel, currentInspection, leftEye);
                chart.ShowDialog();
            }

            DgCase_SelectionChanged(null, null);
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCase.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个病例删除");
                return;
            }



            var selectedItem = dgCase.SelectedItems[0] as PatientModel;

            using (var db = new PatientContext())
            {
                db.Patients.Attach(selectedItem);
                db.Patients.Remove(selectedItem);
                db.SaveChanges();
            }

            Window_Loaded(null, null);
        }

        private async void BtnAddRecord_Click(object sender, RoutedEventArgs e)
        {
            if (dgCase.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个病例后操作");
                return;
            }

            var inspectionWindow = new InspectionDetail((dgCase.SelectedItem as PatientModel).Id);
            inspectionWindow.Closed += (ss, ee) => DgCase_SelectionChanged(null, null);
            inspectionWindow.ShowDialog();
        }

        private void DgCase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCase.SelectedItems.Count == 0)
            {
                return;
            }

            var selectedItem = dgCase.SelectedItems[0] as PatientModel;
            using (var db = new PatientContext())
            {
                var model = db.Patients.Include("Inspections").SingleOrDefault(p => p.Id == selectedItem.Id);
                var ins = model.Inspections;
                dgInspections.ItemsSource = ins.OrderByDescending(p => p.Id);
                if (ins.Any())
                {
                    dgInspections.SelectedIndex = 0;
                }
            }
        }

        private void DgInspections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //加载
        }

        private async void BtnEditInspection_Click(object sender, RoutedEventArgs e)
        {
            if (dgInspections.SelectedItems.Count == 0)
            {
                await this.ShowMessageAsync("错误", "选择一个检查记录后操作");
                return;
            }

            var inspectionWindow = new InspectionDetail((dgCase.SelectedItem as PatientModel).Id, dgInspections.SelectedItem as InspectionModel);
            inspectionWindow.Closed += (ss, ee) => DgCase_SelectionChanged(null, null);
            inspectionWindow.ShowDialog();
        }
    }
}
