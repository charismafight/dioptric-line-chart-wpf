﻿using System;
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

        public void GetValueOfModel(InspectionModel model)
        {
            Id = model.Id;
            PatientId = model.PatientId;
            Age = model.Age;
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
