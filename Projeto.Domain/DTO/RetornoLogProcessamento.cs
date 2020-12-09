using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.DTO
{
    public class RetornoLogProcessamento
    {
        public int idLogProcessamento { get; set; }
        public int idRegistroProcessamento { get; set; }

        public RetornoLogProcessamento()
        {

        }

        public RetornoLogProcessamento(int idlog)
        {
            this.idLogProcessamento = idlog;
            this.idRegistroProcessamento = 0;
        }

        public RetornoLogProcessamento(int idlog, int idregistro)
        {
            this.idLogProcessamento = idlog;
            this.idRegistroProcessamento = idregistro;
        }
    }
}
