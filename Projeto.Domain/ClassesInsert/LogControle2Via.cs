using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.ClassesInsert
{
    public class LogControle2Via
    {
        public int idLogControle2Via { get; set; }
        public int idControle2Via { get; set; }
        public string usuarioSolicitacao { get; set; }

        public LogControle2Via()
        {
                
        }

        public LogControle2Via(int idControle2Via, string usuarioSolicitacao)
        {
            this.idControle2Via = idControle2Via;
            this.usuarioSolicitacao = usuarioSolicitacao;
        }
    }
}
