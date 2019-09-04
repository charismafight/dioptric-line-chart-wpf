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
using System.Text.RegularExpressions;

namespace Dioptric
{
    /// <summary>
    /// CaseDetail.xaml 的交互逻辑
    /// </summary>
    public partial class PatientDetail : MetroWindow
    {
        public PatientDetail(object context = null)
        {
            InitializeComponent();
            if (context == null)
            {
                Title = "患者病例";
                context = new PatientModel();
            }

            DataContext = context;
        }

        private async void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            //因为身份证用于计算年龄，所以身份证必填且符合规范
            var m = DataContext as PatientModel;

            using (var db = new PatientContext())
            {
                if (m.Id == 0)
                {
                    db.Patients.Add(m);
                }
                else
                {
                    var modelInDb = db.Patients.Single(p => p.Id == m.Id);
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
