using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SQLite.CodeFirst;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dioptric
{
    public class PatientModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public string IDCardNumber { get; set; }

        [NotMapped]
        public DateTime BirthDate
        {
            get
            {
                try
                {
                    //处理18位的身份证号码从号码中得到生日和性别代码
                    if (IDCardNumber.Length == 18)
                    {
                        return DateTime.Parse(IDCardNumber.Substring(6, 4) + "-" + IDCardNumber.Substring(10, 2) + "-" + IDCardNumber.Substring(12, 2));
                    }
                    //处理15位的身份证号码从号码中得到生日和性别代码
                    if (IDCardNumber.Length == 15)
                    {
                        return DateTime.Parse("19" + IDCardNumber.Substring(6, 2) + "-" + IDCardNumber.Substring(8, 2) + "-" + IDCardNumber.Substring(10, 2));
                    }

                    return DateTime.MinValue;
                }
                catch (Exception e)
                {
                    return DateTime.MinValue;
                }
            }
        }

        public virtual ICollection<InspectionModel> Inspections { get; set; } = new HashSet<InspectionModel>();


        public void GetValueOfModel(PatientModel model)
        {
            Name = model.Name;
            Sex = model.Sex;
            IDCardNumber = model.IDCardNumber;
        }
    }
}
