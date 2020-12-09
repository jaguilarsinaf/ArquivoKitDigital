using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.ClassesInsert
{
    public class TB_ERRO_REGISTRO
    {
        public int cd_erreg { get; set; }
        public int cd_regpss { get; set; }
        public int? cd_err { get; set; }
        public string ds_msgerr { get; set; }

        public TB_ERRO_REGISTRO()
        {
                
        }

        public TB_ERRO_REGISTRO(int cd_regpss, int? cd_err, string ds_msgerr)
        {
            this.cd_regpss = cd_regpss;
            this.cd_err = cd_err;
            this.ds_msgerr = ds_msgerr;
        }
    }
}
