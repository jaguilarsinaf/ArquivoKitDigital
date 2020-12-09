using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain
{
    public class TB_CAMPO
    {
        [Key]
        public int cd_cam { get; set; }
        public string nm_cam { get; set; }
        public string ds_cam { get; set; }
    }
}
