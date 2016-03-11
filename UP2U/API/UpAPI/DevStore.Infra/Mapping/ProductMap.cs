using DevStore.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevStore.Infra.Mapping
{
    class ProductMap: EntityTypeConfiguration<Produto>
    {
        public ProductMap()
        {
            ToTable("Products");
            HasKey(x => x.Id);
            Property(x => x.Title).HasMaxLength(160).IsRequired();
            Property(x => x.Price).IsRequired();
            Property(x => x.IsActive).IsRequired();

            HasRequired(x => x.Category);
            

        }
    }
}
