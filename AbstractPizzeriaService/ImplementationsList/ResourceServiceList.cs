using AbstractPizzeria;
using AbstractPizzeriaService;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class ResourceServiceList : IResourceService
    {
        private ListDataSingleton source;

        public ResourceServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<ResourceViewModel> GetList()
        {
            List<ResourceViewModel> result = source.Resources
                .Select(rec => new ResourceViewModel
                {
                    Id = rec.Id,
                    ResourceName = rec.ResourceName,
                    ResourceIngridients = source.ResourceIngridients
                            .Where(recPC => recPC.ResourceId == rec.Id)
                            .Select(recPC => new ResourceIngridientViewModel
                            {
                                Id = recPC.Id,
                                ResourceId = recPC.ResourceId,
                                IngridientId = recPC.IngridientId,
                                IngridientName = source.Ingridients
                                    .FirstOrDefault(recC => recC.Id == recPC.IngridientId)?.IngridientName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ResourceViewModel GetElement(int id)
        {
            Resource element = source.Resources.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ResourceViewModel
                {
                    Id = element.Id,
                    ResourceName = element.ResourceName,
                    ResourceIngridients = source.ResourceIngridients
                            .Where(recPC => recPC.ResourceId == element.Id)
                            .Select(recPC => new ResourceIngridientViewModel
                            {
                                Id = recPC.Id,
                                ResourceId = recPC.ResourceId,
                                IngridientId = recPC.IngridientId,
                                IngridientName = source.Ingridients
                                    .FirstOrDefault(recC => recC.Id == recPC.IngridientId)?.IngridientName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ResourceBindingModel model)
        {
            Resource element = source.Resources.FirstOrDefault(rec => rec.ResourceName == model.ResourceName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxId = source.Resources.Count > 0 ? source.Resources.Max(rec => rec.Id) : 0;
            source.Resources.Add(new Resource
            {
                Id = maxId + 1,
                ResourceName = model.ResourceName
            });
        }

        public void UpdElement(ResourceBindingModel model)
        {
            Resource element = source.Resources.FirstOrDefault(rec =>
                                        rec.ResourceName == model.ResourceName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = source.Resources.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ResourceName = model.ResourceName;
        }

        public void DelElement(int id)
        {
            Resource element = source.Resources.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // при удалении удаляем все записи о компонентах на удаляемом складе
                source.ResourceIngridients.RemoveAll(rec => rec.ResourceId == id);
                source.Resources.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
