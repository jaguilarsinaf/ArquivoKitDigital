using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.ClassesInsert
{
    public class TB_LOG_PROCESSAMENTO
    {
        public int cd_logpss { get; set; }
        public string cd_pss { get; set; }
        public string cd_usu { get; set; }
        public int? qt_totreg { get; set; }
        public DateTime dt_inipss { get; set; }
        public DateTime? dt_fimpss { get; set; }
    }
}
