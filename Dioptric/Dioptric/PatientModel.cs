﻿using System;
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

        public float Age { get; set; }


        public void GetValueOfModel(PatientModel model)
        {
            Name = model.Name;
            Sex = model.Sex;
            Age = model.Age;
            IDCardNumber = model.IDCardNumber;
        }
    }
}
