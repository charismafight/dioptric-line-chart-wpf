using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SQLite.CodeFirst;

namespace Dioptric
{
    public class DioptricModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

        public string IDCardNumber { get; set; }

        public float EyeSight { get; set; }
    }
}
