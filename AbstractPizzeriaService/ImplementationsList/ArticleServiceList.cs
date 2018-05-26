using AbstractPizzeria;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ImplementationsList
{
   public class ArticleServiceList : IArticleService
    {
        private ListDataSingleton source;

        public ArticleServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<ArticleViewModel> GetList()
        {
            List<ArticleViewModel> result = new List<ArticleViewModel>();
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<ArticleIngridientViewModel> productComponents = new List<ArticleIngridientViewModel>();
                for (int j = 0; j < source.ArticleIngridients.Count; ++j)
                {
                    if (source.ArticleIngridients[j].ArticleId == source.Articles[i].Id)
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
                        productComponents.Add(new ArticleIngridientViewModel
                        {
                            Id = source.ArticleIngridients[j].Id,
                            ArticleId = source.ArticleIngridients[j].ArticleId,
                            IngridientId = source.ArticleIngridients[j].IngridientId,
                            IngridientName = componentName,
                            Count = source.ArticleIngridients[j].Count
                        });
                    }
                }
                result.Add(new ArticleViewModel
                {
                    Id = source.Articles[i].Id,
                    ArticleName = source.Articles[i].ArticleName,
                    Price = source.Articles[i].Price,
                    ArticleIngridients = productComponents
                });
            }
            return result;
        }

        public ArticleViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<ArticleIngridientViewModel> productComponents = new List<ArticleIngridientViewModel>();
                for (int j = 0; j < source.ArticleIngridients.Count; ++j)
                {
                    if (source.ArticleIngridients[j].ArticleId == source.Articles[i].Id)
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
                        productComponents.Add(new ArticleIngridientViewModel
                        {
                            Id = source.ArticleIngridients[j].Id,
                            ArticleId = source.ArticleIngridients[j].ArticleId,
                            IngridientId = source.ArticleIngridients[j].IngridientId,
                            IngridientName = componentName,
                            Count = source.ArticleIngridients[j].Count
                        });
                    }
                }
                if (source.Articles[i].Id == id)
                {
                    return new ArticleViewModel
                    {
                        Id = source.Articles[i].Id,
                        ArticleName = source.Articles[i].ArticleName,
                        Price = source.Articles[i].Price,
                        ArticleIngridients = productComponents
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(ArticleBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                if (source.Articles[i].Id > maxId)
                {
                    maxId = source.Articles[i].Id;
                }
                if (source.Articles[i].ArticleName == model.ArticleName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Articles.Add(new Article
            {
                Id = maxId + 1,
                ArticleName = model.ArticleName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.ArticleIngridients.Count; ++i)
            {
                if (source.ArticleIngridients[i].Id > maxPCId)
                {
                    maxPCId = source.ArticleIngridients[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.ArticleIngridients.Count; ++i)
            {
                for (int j = 1; j < model.ArticleIngridients.Count; ++j)
                {
                    if (model.ArticleIngridients[i].IngridientId ==
                        model.ArticleIngridients[j].IngridientId)
                    {
                        model.ArticleIngridients[i].Count +=
                            model.ArticleIngridients[j].Count;
                        model.ArticleIngridients.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.ArticleIngridients.Count; ++i)
            {
                source.ArticleIngridients.Add(new ArticleIngridient
                {
                    Id = ++maxPCId,
                    ArticleId = maxId + 1,
                    IngridientId = model.ArticleIngridients[i].IngridientId,
                    Count = model.ArticleIngridients[i].Count
                });
            }
        }

        public void UpdElement(ArticleBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                if (source.Articles[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Articles[i].ArticleName == model.ArticleName &&
                    source.Articles[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Articles[index].ArticleName = model.ArticleName;
            source.Articles[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.ArticleIngridients.Count; ++i)
            {
                if (source.ArticleIngridients[i].Id > maxPCId)
                {
                    maxPCId = source.ArticleIngridients[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.ArticleIngridients.Count; ++i)
            {
                if (source.ArticleIngridients[i].ArticleId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.ArticleIngridients.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.ArticleIngridients[i].Id == model.ArticleIngridients[j].Id)
                        {
                            source.ArticleIngridients[i].Count = model.ArticleIngridients[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.ArticleIngridients.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.ArticleIngridients.Count; ++i)
            {
                if (model.ArticleIngridients[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.ArticleIngridients.Count; ++j)
                    {
                        if (source.ArticleIngridients[j].ArticleId == model.Id &&
                            source.ArticleIngridients[j].IngridientId == model.ArticleIngridients[i].IngridientId)
                        {
                            source.ArticleIngridients[j].Count += model.ArticleIngridients[i].Count;
                            model.ArticleIngridients[i].Id = source.ArticleIngridients[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.ArticleIngridients[i].Id == 0)
                    {
                        source.ArticleIngridients.Add(new ArticleIngridient
                        {
                            Id = ++maxPCId,
                            ArticleId = model.Id,
                            IngridientId = model.ArticleIngridients[i].IngridientId,
                            Count = model.ArticleIngridients[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.ArticleIngridients.Count; ++i)
            {
                if (source.ArticleIngridients[i].ArticleId == id)
                {
                    source.ArticleIngridients.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Articles.Count; ++i)
            {
                if (source.Articles[i].Id == id)
                {
                    source.Articles.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
