using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dioptric
{
    public class InspectionModel : INotifyPropertyChanged
    {
        public InspectionModel()
        {
            LeftEye = new EyeModel();
            RightEye = new EyeModel();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public virtual PatientModel Patient { get; set; }

        [NotMapped]
        private DateTime _birthDate;
        [NotMapped]
        public DateTime BirthDate
        {
            get
            {
                if (Patient != null)
                {
                    return Patient.BirthDate;
                }

                return _birthDate;
            }
            set
            {
                _birthDate = value;
            }
        }

        /// <summary>
        /// 年龄以月为单位，根据出生日期计算
        /// </summary>
        [NotMapped]
        public int Age
        {
            get
            {
                if (Patient == null || string.IsNullOrWhiteSpace(OptometryDate))
                {
                    return 0;
                }
                else
                {
                    return (DateTime.Parse(OptometryDate).Year * 12 + DateTime.Parse(OptometryDate).Month) - (BirthDate.Year * 12 + BirthDate.Month);
                }
            }
            set
            {

            }
        }

        public float Height { get; set; }

        public float Weight { get; set; }

        public string Cycloplegia { get; set; }

        [NotMapped]
        private string _optometryDate;
        public string OptometryDate
        {
            get
            {
                return _optometryDate;
            }

            set
            {
                _optometryDate = value;
                NotifyPropertyChange("Age");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Memo { get; set; }

        public int LeftEyeId { get; set; }
        public virtual EyeModel LeftEye { get; set; }

        public int RightEyeId { get; set; }
        public virtual EyeModel RightEye { get; set; }

        public void GetValueOfModel(InspectionModel model)
        {
            Id = model.Id;
            PatientId = model.PatientId;
            Height = model.Height;
            Weight = model.Weight;
            Cycloplegia = model.Cycloplegia;
            OptometryDate = model.OptometryDate;
            Memo = model.Memo;
            LeftEyeId = model.LeftEyeId;
            RightEyeId = model.RightEyeId;
        }
    }
}
