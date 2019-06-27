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
        public CaseDetail()
        {
            InitializeComponent();
        }

        private async void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            var m = new DioptricModel();
            try
            {
                m.Name = tbName.Text;
                m.Sex = tbSex.Text;
                m.Age = int.Parse(tbAge.Text);
                m.EyeSight = float.Parse(tbEyeSight.Text);
                m.IDCardNumber = tbIDCard.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("请确保输入数据正确");
                return;
            }

            using (var db = new DioptricContext())
            {
                db.Dioptrics.Add(m);
                db.SaveChanges();
            }

            await this.ShowMessageAsync("1", "2");


            Close();

            //var db = new DioptricDB();
            //var sp = db.DataProvider.GetSchemaProvider();
            //var schema = sp.GetSchema(db);

            //if (!schema.Tables.Any(p => p.TableName == "Dioptric"))
            //{
            //    using (var d = new DbDioptric())
            //    {

            //    }
            //}
        }
    }
}
