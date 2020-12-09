using Projeto.Domain.ClassesInsert;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class TB_LOG_PROCESSAMENTOMap : EntityTypeConfiguration<TB_LOG_PROCESSAMENTO>
    {
        public TB_LOG_PROCESSAMENTOMap()
        {
            ToTable("TB_LOG_PROCESSAMENTO");
            HasKey(x => x.cd_logpss);
        }
    }
}
