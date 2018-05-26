using AbstractPizzeria;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AbstractPizzeriaService
{
    [Table("AbstractDatabase")]
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext()
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Ingridient> Ingridients { get; set; }

        public virtual DbSet<Worker> Workers { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<ArticleIngridient> ArticleIngridients { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        public virtual DbSet<ResourceIngridient> ResourceIngridients { get; set; }
    }
}