using Projeto.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Mapping
{
    public class Controle2ViaMap : EntityTypeConfiguration<Controle2Via>
    {
        public Controle2ViaMap()
        {
            ToTable("Controle2Via");
            HasKey(x => x.cdc2v);
        }
    }
}
