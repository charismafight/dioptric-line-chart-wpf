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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new DioptricContext())
            {
                DataContext = db.Dioptrics.OrderBy(p => p.Name).ToList();
            }

            new Chart().Show();
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

        private void BtnShowChart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
