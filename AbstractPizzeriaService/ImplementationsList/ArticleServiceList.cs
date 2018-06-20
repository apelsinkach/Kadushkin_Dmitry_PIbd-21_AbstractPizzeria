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
    public class ArticleServiceList : IArticleService
    {
        private ListDataSingleton source;

        public ArticleServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<ArticleViewModel> GetList()
        {
            List<ArticleViewModel> result = source.Articles
                .Select(rec => new ArticleViewModel
                {
                    Id = rec.Id,
                    ArticleName = rec.ArticleName,
                    Price = rec.Price,
                    ArticleIngridients = source.ArticleIngridients
                            .Where(recPC => recPC.ArticleId == rec.Id)
                            .Select(recPC => new ArticleIngridientViewModel
                            {
                                Id = recPC.Id,
                                ArticleId = recPC.ArticleId,
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

        public ArticleViewModel GetElement(int id)
        {
            Article element = source.Articles.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ArticleViewModel
                {
                    Id = element.Id,
                    ArticleName = element.ArticleName,
                    Price = element.Price,
                    ArticleIngridients = source.ArticleIngridients
                            .Where(recPC => recPC.ArticleId == element.Id)
                            .Select(recPC => new ArticleIngridientViewModel
                            {
                                Id = recPC.Id,
                                ArticleId = recPC.ArticleId,
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

        public void AddElement(ArticleBindingModel model)
        {
            Article element = source.Articles.FirstOrDefault(rec => rec.ArticleName == model.ArticleName);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            int maxId = source.Articles.Count > 0 ? source.Articles.Max(rec => rec.Id) : 0;
            source.Articles.Add(new Article
            {
                Id = maxId + 1,
                ArticleName = model.ArticleName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.ArticleIngridients.Count > 0 ?
                                    source.ArticleIngridients.Max(rec => rec.Id) : 0;
            // убираем дубли по компонентам
            var groupComponents = model.ArticleIngridients
                                        .GroupBy(rec => rec.IngridientId)
                                        .Select(rec => new
                                        {
                                            IngridientId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            // добавляем компоненты
            foreach (var groupComponent in groupComponents)
            {
                source.ArticleIngridients.Add(new ArticleIngridient
                {
                    Id = ++maxPCId,
                    ArticleId = maxId + 1,
                    IngridientId = groupComponent.IngridientId,
                    Count = groupComponent.Count
                });
            }
        }

        public void UpdElement(ArticleBindingModel model)
        {
            Article element = source.Articles.FirstOrDefault(rec =>
                                        rec.ArticleName == model.ArticleName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            element = source.Articles.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ArticleName = model.ArticleName;
            element.Price = model.Price;

            int maxPCId = source.ArticleIngridients.Count > 0 ? source.ArticleIngridients.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.ArticleIngridients.Select(rec => rec.IngridientId).Distinct();
            var updateComponents = source.ArticleIngridients
                                            .Where(rec => rec.ArticleId == model.Id &&
                                           compIds.Contains(rec.IngridientId));
            foreach (var updateComponent in updateComponents)
            {
                updateComponent.Count = model.ArticleIngridients
                                                .FirstOrDefault(rec => rec.Id == updateComponent.Id).Count;
            }
            source.ArticleIngridients.RemoveAll(rec => rec.ArticleId == model.Id &&
                                       !compIds.Contains(rec.IngridientId));
            // новые записи
            var groupComponents = model.ArticleIngridients
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.IngridientId)
                                        .Select(rec => new
                                        {
                                            IngridientId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupComponent in groupComponents)
            {
                ArticleIngridient elementPC = source.ArticleIngridients
                                        .FirstOrDefault(rec => rec.ArticleId == model.Id &&
                                                        rec.IngridientId == groupComponent.IngridientId);
                if (elementPC != null)
                {
                    elementPC.Count += groupComponent.Count;
                }
                else
                {
                    source.ArticleIngridients.Add(new ArticleIngridient
                    {
                        Id = ++maxPCId,
                        ArticleId = model.Id,
                        IngridientId = groupComponent.IngridientId,
                        Count = groupComponent.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Article element = source.Articles.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.ArticleIngridients.RemoveAll(rec => rec.ArticleId == id);
                source.Articles.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
