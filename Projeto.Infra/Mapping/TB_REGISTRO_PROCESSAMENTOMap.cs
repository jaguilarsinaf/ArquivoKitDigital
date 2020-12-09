using Projeto.Domain.ClassesInsert;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class TB_REGISTRO_PROCESSAMENTOMap : EntityTypeConfiguration<TB_REGISTRO_PROCESSAMENTO>
    {
        public TB_REGISTRO_PROCESSAMENTOMap()
        {
            ToTable("TB_REGISTRO_PROCESSAMENTO");
            HasKey(x => x.cd_regpss);
        }
    }
}
