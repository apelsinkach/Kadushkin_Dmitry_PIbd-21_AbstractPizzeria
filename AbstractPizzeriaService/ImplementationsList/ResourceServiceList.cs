using AbstractPizzeria;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ImplementationsList
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
            List<ResourceViewModel> result = new List<ResourceViewModel>();
            for (int i = 0; i < source.Resources.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<ResourceIngridientViewModel> ResourceIngridients = new List<ResourceIngridientViewModel>();
                for (int j = 0; j < source.ResourceIngridients.Count; ++j)
                {
                    if (source.ResourceIngridients[j].ResourceId == source.Resources[i].Id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Ingridients.Count; ++k)
                        {
                            if (source.ArticleIngridients[j].IngridientId == source.Ingridients[k].Id)
                            {
                                componentName = source.Ingridients[k].IngridientName;
                                break;
                            }
                        }
                        ResourceIngridients.Add(new ResourceIngridientViewModel
                        {
                            Id = source.ResourceIngridients[j].Id,
                            ResourceId = source.ResourceIngridients[j].ResourceId,
                            IngridientId = source.ResourceIngridients[j].IngridientId,
                            IngridientName = componentName,
                            Count = source.ResourceIngridients[j].Count
                        });
                    }
                }
                result.Add(new ResourceViewModel
                {
                    Id = source.Resources[i].Id,
                    ResourceName = source.Resources[i].ResourceName,
                    ResourceIngridients = ResourceIngridients
                });
            }
            return result;
        }

        public ResourceViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Resources.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<ResourceIngridientViewModel> ResourceIngridients = new List<ResourceIngridientViewModel>();
                for (int j = 0; j < source.ResourceIngridients.Count; ++j)
                {
                    if (source.ResourceIngridients[j].ResourceId == source.Resources[i].Id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Ingridients.Count; ++k)
                        {
                            if (source.ArticleIngridients[j].IngridientId == source.Ingridients[k].Id)
                            {
                                componentName = source.Ingridients[k].IngridientName;
                                break;
                            }
                        }
                        ResourceIngridients.Add(new ResourceIngridientViewModel
                        {
                            Id = source.ResourceIngridients[j].Id,
                            ResourceId = source.ResourceIngridients[j].ResourceId,
                            IngridientId = source.ResourceIngridients[j].IngridientId,
                            IngridientName = componentName,
                            Count = source.ResourceIngridients[j].Count
                        });
                    }
                }
                if (source.Resources[i].Id == id)
                {
                    return new ResourceViewModel
                    {
                        Id = source.Resources[i].Id,
                        ResourceName = source.Resources[i].ResourceName,
                        ResourceIngridients = ResourceIngridients
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ResourceBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Resources.Count; ++i)
            {
                if (source.Resources[i].Id > maxId)
                {
                    maxId = source.Resources[i].Id;
                }
                if (source.Resources[i].ResourceName == model.ResourceName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Resources.Add(new Resource
            {
                Id = maxId + 1,
                ResourceName = model.ResourceName
            });
        }

        public void UpdElement(ResourceBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Resources.Count; ++i)
            {
                if (source.Resources[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Resources[i].ResourceName == model.ResourceName &&
                    source.Resources[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Resources[index].ResourceName = model.ResourceName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.ResourceIngridients.Count; ++i)
            {
                if (source.ResourceIngridients[i].ResourceId == id)
                {
                    source.ResourceIngridients.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Resources.Count; ++i)
            {
                if (source.Resources[i].Id == id)
                {
                    source.Resources.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
