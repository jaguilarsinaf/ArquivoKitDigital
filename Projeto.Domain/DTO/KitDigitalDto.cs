using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Domain.DTO
{
    public class KitDigitalDto
    {
        public int tipoKit { get; set; }
        public string digital { get; set; }
        public int externalKey { get; set; }
        public int idControleImpressaoKitNova { get; set; }
        public Certificado certificado { get; set; }
        public Boleto boleto { get; set; }
        public Carteirinha carteirinha { get; set; }
    }
}
