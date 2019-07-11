using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dioptric
{
    public class EyeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public float Sight { get; set; }

        /// <summary>
        /// 球镜
        /// </summary>
        public string SPH { get; set; }

        /// <summary>
        /// 柱镜
        /// </summary>
        public string CYL { get; set; }

        /// <summary>
        /// 轴向
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// 矫正视力
        /// </summary>
        public float CVA { get; set; }

        /// <summary>
        /// 眼压
        /// </summary>
        public float IOP { get; set; }

        /// <summary>
        /// 眼轴
        /// </summary>
        public string EyeAxial { get; set; }

        /// <summary>
        /// 角膜曲率
        /// </summary>
        public string ORBSCAN { get; set; }
    }
}
