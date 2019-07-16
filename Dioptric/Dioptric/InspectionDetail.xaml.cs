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
    /// InspectionDetail.xaml 的交互逻辑
    /// </summary>
    public partial class InspectionDetail : MetroWindow
    {
        public InspectionDetail(object context = null)
        {
            InitializeComponent();

            if (context == null)
            {
                context = new InspectionModel();
            }

            DataContext = context;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var m = DataContext as InspectionModel;

            using (var db = new PatientContext())
            {
                if (m.Id == 0)
                {
                    db.Inspections.Add(m);
                }
                else
                {
                    var modelInDb = db.Inspections.Single(p => p.Id == m.Id);
                    modelInDb.GetValueOfModel(m);
                }
                db.SaveChanges();
            }

            Close();
        }
    }
}
