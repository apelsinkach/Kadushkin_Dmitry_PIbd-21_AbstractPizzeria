using AbstractPizzeria;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AbstractPizzeriaService
{
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext() : base("AbstractDatabase")
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

        public virtual DbSet<MessageInfo> MessageInfos { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                throw;
            }
        }

    }
}