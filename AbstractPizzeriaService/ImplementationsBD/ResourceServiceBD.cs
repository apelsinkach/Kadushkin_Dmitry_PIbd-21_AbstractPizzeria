using AbstractPizzeria;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ImplementationsBD
{
   public class ResourceServiceBD : IResourceService
    {
        private AbstractDbContext context;

        public ResourceServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ResourceViewModel> GetList()
        {
            List<ResourceViewModel> result = context.Resources
                .Select(rec => new ResourceViewModel
                {
                    Id = rec.Id,
                    ResourceName = rec.ResourceName,
                    ResourceIngridients = context.ResourceIngridients
                            .Where(recPC => recPC.ResourceId == rec.Id)
                            .Select(recPC => new ResourceIngridientViewModel
                            {
                                Id = recPC.Id,
                                ResourceId = recPC.ResourceId,
                                IngridientId = recPC.IngridientId,
                                IngridientName = recPC.Ingridient.IngridientName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ResourceViewModel GetElement(int id)
        {
            Resource element = context.Resources.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ResourceViewModel
                {
                    Id = element.Id,
                    ResourceName = element.ResourceName,
                    ResourceIngridients = context.ResourceIngridients
                            .Where(recPC => recPC.ResourceId == element.Id)
                            .Select(recPC => new ResourceIngridientViewModel
                            {
                                Id = recPC.Id,
                                ResourceId = recPC.ResourceId,
                                IngridientId = recPC.IngridientId,
                                IngridientName = recPC.Ingridient.IngridientName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ResourceBindingModel model)
        {
            Resource element = context.Resources.FirstOrDefault(rec => rec.ResourceName == model.ResourceName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Resources.Add(new Resource
            {
                ResourceName = model.ResourceName
            });
            context.SaveChanges();
        }

        public void UpdElement(ResourceBindingModel model)
        {
            Resource element = context.Resources.FirstOrDefault(rec =>
                                        rec.ResourceName == model.ResourceName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Resources.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ResourceName = model.ResourceName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Resource element = context.Resources.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // при удалении удаляем все записи о компонентах на удаляемом складе
                        context.ResourceIngridients.RemoveRange(
                                            context.ResourceIngridients.Where(rec => rec.ResourceId == id));
                        context.Resources.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
