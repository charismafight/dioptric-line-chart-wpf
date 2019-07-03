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
            var win = new CaseDetail();
            win.ShowDialog();

            Window_Loaded(null, null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new DioptricContext())
            {
                DataContext = db.Dioptrics.OrderBy(p => p.Name).ToList();
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgCase.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个病例后编辑");
                return;
            }

            var win = new CaseDetail(dgCase.SelectedItems[0] as DioptricModel);
            win.ShowDialog();
        }

        private async void BtnShowChart_Click(object sender, RoutedEventArgs e)
        {
            if (dgCase.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个病例后查看");
                return;
            }

            var selectedItem = dgCase.SelectedItems[0] as DioptricModel;

            var all = DataContext as List<DioptricModel>;
            var individualModels = all.Where(p => p.IDCardNumber == selectedItem.IDCardNumber).ToList();

            var chart = new Chart(individualModels);
            chart.ShowDialog();
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCase.SelectedItems.Count <= 0)
            {
                await this.ShowMessageAsync("错误", "选择一个病例删除");
                return;
            }

            var selectedItem = dgCase.SelectedItems[0] as DioptricModel;

            using (var db = new DioptricContext())
            {
                db.Dioptrics.Attach(selectedItem);
                db.Dioptrics.Remove(selectedItem);
                db.SaveChanges();
            }

            Window_Loaded(null, null);
        }
    }
}
