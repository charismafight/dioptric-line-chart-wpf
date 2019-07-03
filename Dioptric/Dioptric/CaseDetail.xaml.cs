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
using MahApps.Metro.Controls.Dialogs;

namespace Dioptric
{
    /// <summary>
    /// CaseDetail.xaml 的交互逻辑
    /// </summary>
    public partial class CaseDetail : MetroWindow
    {
        public CaseDetail(object context = null)
        {
            InitializeComponent();
            if (context == null)
            {
                Title = "新增病例";
                context = new DioptricModel();
            }

            DataContext = context;
        }

        private async void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            var m = DataContext as DioptricModel;

            using (var db = new DioptricContext())
            {
                if (m.Id == 0)
                {
                    db.Dioptrics.Add(m);
                }
                else
                {
                    var modelInDb = db.Dioptrics.Single(p => p.Id == m.Id);
                    modelInDb.GetValueOfModel(m);
                }
                db.SaveChanges();
            }

            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
