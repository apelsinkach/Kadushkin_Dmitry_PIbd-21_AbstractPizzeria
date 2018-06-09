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
   public class ArticleServiceBD: IArticleService
    {
        private AbstractDbContext context;

        public ArticleServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ArticleViewModel> GetList()
        {
            List<ArticleViewModel> result = context.Articles
                .Select(rec => new ArticleViewModel
                {
                    Id = rec.Id,
                    ArticleName = rec.ArticleName,
                    Price = rec.Price,
                    ArticleIngridients = context.ArticleIngridients
                            .Where(recPC => recPC.ArticleId == rec.Id)
                            .Select(recPC => new ArticleIngridientViewModel
                            {
                                Id = recPC.Id,
                                ArticleId = recPC.ArticleId,
                                IngridientId = recPC.IngridientId,
                                IngridientName = recPC.Ingridient.IngridientName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ArticleViewModel GetElement(int id)
        {
            Article element = context.Articles.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ArticleViewModel
                {
                    Id = element.Id,
                    ArticleName = element.ArticleName,
                    Price = element.Price,
                    ArticleIngridients = context.ArticleIngridients
                            .Where(recPC => recPC.ArticleId == element.Id)
                            .Select(recPC => new ArticleIngridientViewModel
                            {
                                Id = recPC.Id,
                                ArticleId = recPC.ArticleId,
                                IngridientId = recPC.IngridientId,
                                IngridientName = recPC.Ingridient.IngridientName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ArticleBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Article element = context.Articles.FirstOrDefault(rec => rec.ArticleName == model.ArticleName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Article
                    {
                        ArticleName = model.ArticleName,
                        Price = model.Price
                    };
                    context.Articles.Add(element);
                    context.SaveChanges();
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
                        context.ArticleIngridients.Add(new ArticleIngridient
                        {
                            ArticleId = element.Id,
                            IngridientId = groupComponent.IngridientId,
                            Count = groupComponent.Count
                        });
                        context.SaveChanges();
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

        public void UpdElement(ArticleBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Article element = context.Articles.FirstOrDefault(rec =>
                                        rec.ArticleName == model.ArticleName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Articles.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.ArticleName = model.ArticleName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compIds = model.ArticleIngridients.Select(rec => rec.IngridientId).Distinct();
                    var updateComponents = context.ArticleIngridients
                                                    .Where(rec => rec.ArticleId == model.Id &&
                                                        compIds.Contains(rec.IngridientId));
                    foreach (var updateComponent in updateComponents)
                    {
                        updateComponent.Count = model.ArticleIngridients
                                                        .FirstOrDefault(rec => rec.Id == updateComponent.Id).Count;
                    }
                    context.SaveChanges();
                    context.ArticleIngridients.RemoveRange(
                                        context.ArticleIngridients.Where(rec => rec.ArticleId == model.Id &&
                                                                            !compIds.Contains(rec.IngridientId)));
                    context.SaveChanges();
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
                        ArticleIngridient elementPC = context.ArticleIngridients
                                                .FirstOrDefault(rec => rec.ArticleId == model.Id &&
                                                                rec.IngridientId == groupComponent.IngridientId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupComponent.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.ArticleIngridients.Add(new ArticleIngridient
                            {
                                ArticleId = model.Id,
                                IngridientId = groupComponent.IngridientId,
                                Count = groupComponent.Count
                            });
                            context.SaveChanges();
                        }
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

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Article element = context.Articles.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.ArticleIngridients.RemoveRange(
                                            context.ArticleIngridients.Where(rec => rec.ArticleId == id));
                        context.Articles.Remove(element);
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
