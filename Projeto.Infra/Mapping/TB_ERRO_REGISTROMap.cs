using Projeto.Domain.ClassesInsert;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class TB_ERRO_REGISTROMap : EntityTypeConfiguration<TB_ERRO_REGISTRO>
    {
        public TB_ERRO_REGISTROMap()
        {
            ToTable("TB_ERRO_REGISTRO");
            HasKey(x => x.cd_erreg);
        }
    }
}
