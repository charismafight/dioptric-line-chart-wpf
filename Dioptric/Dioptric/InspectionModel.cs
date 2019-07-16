using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dioptric
{
    public class InspectionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public virtual PatientModel Patient { get; set; }

        public float Age { get; set; }

        public float Height { get; set; }

        public float Weight { get; set; }

        public string Cycloplegia { get; set; }

        public string OptometryDate { get; set; }

        public string Memo { get; set; }

        public int LeftEyeId { get; set; }
        public virtual EyeModel LeftEye { get; set; }

        public int RightEyeId { get; set; }
        public virtual EyeModel RightEye { get; set; }
    }
}
