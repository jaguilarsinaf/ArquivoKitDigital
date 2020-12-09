using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.ClassesInsert
{
    public class TB_REGISTRO_PROCESSAMENTO
    {
        public int cd_regpss { get; set; }
        public int cd_logpss { get; set; }
        public string st_regpss { get; set; }
        public DateTime? dt_sitreg { get; set; }

        public TB_REGISTRO_PROCESSAMENTO()
        {

        }

        public TB_REGISTRO_PROCESSAMENTO(int cd_logpss, string st_regpss, DateTime? dt_sitreg)
        {
            this.cd_logpss = cd_logpss;
            this.st_regpss = st_regpss;
            this.dt_sitreg = dt_sitreg;

        }

        public TB_REGISTRO_PROCESSAMENTO(int cd_regpss, int cd_logpss, string st_regpss, DateTime? dt_sitreg)
        {
            this.cd_regpss = cd_regpss;
            this.cd_logpss = cd_logpss;
            this.st_regpss = st_regpss;
            this.dt_sitreg = dt_sitreg;

        }
    }
}
