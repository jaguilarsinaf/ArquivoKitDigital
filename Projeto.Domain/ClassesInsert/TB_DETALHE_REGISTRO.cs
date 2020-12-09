using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.ClassesInsert
{
    public class TB_DETALHE_REGISTRO
    {
        public int cd_dtreg { get; set; }
        public int cd_regpss { get; set; }
        public int cd_cam { get; set; }
        public string vl_cam { get; set; }

        public TB_DETALHE_REGISTRO(int cd_regpss, int cd_cam, string vl_cam)
        {
            this.cd_regpss = cd_regpss;
            this.cd_cam = cd_cam;
            this.vl_cam = vl_cam;
        }
        public TB_DETALHE_REGISTRO()
        {

        }
    }
}
