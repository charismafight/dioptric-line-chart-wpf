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
        int currentId;
        public InspectionDetail(int id, InspectionModel context = null)
        {
            InitializeComponent();

            currentId = id;

            if (context == null)
            {
                context = new InspectionModel();
            }
            else
            {
                using (var db = new PatientContext())
                {
                    var ins = db.Inspections.Single(p => p.Id == id);
                    context.LeftEye = db.Eyes.Single(p => p.Id == ins.LeftEyeId);
                    context.RightEye = db.Eyes.Single(p => p.Id == ins.RightEyeId);
                }
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
                m.PatientId = currentId;
                if (m.Id == 0)
                {
                    db.Eyes.Add(m.LeftEye);
                    db.Eyes.Add(m.RightEye);
                    db.SaveChanges();

                    m.LeftEyeId = m.LeftEye.Id;
                    m.RightEyeId = m.RightEye.Id;

                    db.SaveChanges();
                    db.Inspections.Add(m);
                }
                else
                {
                    var leftEyeInDb = db.Eyes.Single(p => p.Id == m.LeftEye.Id);
                    //赋值
                    var rightEyeInDb = db.Eyes.Single(p => p.Id == m.RightEye.Id);
                    leftEyeInDb.GetValueOfModel(m.LeftEye);
                    rightEyeInDb.GetValueOfModel(m.RightEye);

                    var modelInDb = db.Inspections.Single(p => p.Id == m.Id);
                    modelInDb.GetValueOfModel(m);
                }
                db.SaveChanges();
            }

            Close();
        }
    }
}
