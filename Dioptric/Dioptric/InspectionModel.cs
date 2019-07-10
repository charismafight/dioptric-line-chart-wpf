using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dioptric
{
    public class InspectionModel
    {
        public float Age { get; set; }

        public float Height { get; set; }

        public float Weight { get; set; }

        public string Cycloplegia { get; set; }

        public DateTime OptometryDate { get; set; }

        public string Memo { get; set; }

        public EyeModel LeftEye { get; set; }

        public EyeModel RightEye { get; set; }
    }
}
