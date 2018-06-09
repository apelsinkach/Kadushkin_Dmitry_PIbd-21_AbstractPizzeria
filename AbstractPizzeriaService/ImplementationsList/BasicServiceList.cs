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
    public class BasicServiceList : IBasicService
    {
        private ListDataSingleton source;

        public BasicServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = source.Requests
                .Select(rec => new RequestViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    ArticleId = rec.ArticleId,
                    WorkerId = rec.WorkerId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerId)?.CustomerFIO,
                    ArticleName = source.Articles
                                    .FirstOrDefault(recP => recP.Id == rec.ArticleId)?.ArticleName,
                    WorkerName = source.Workers
                                    .FirstOrDefault(recI => recI.Id == rec.WorkerId)?.WorkerFIO
                })
                .ToList();
            return result;
        }

        public void CreateOrder(RequestBindingModel model)
        {
            int maxId = source.Requests.Count > 0 ? source.Requests.Max(rec => rec.Id) : 0;
            source.Requests.Add(new Request
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                ArticleId = model.ArticleId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RequestStatus.Принят
            });
        }

        public void TakeOrderInWork(RequestBindingModel model)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var productComponents = source.ArticleIngridients.Where(rec => rec.ArticleId == element.ArticleId);
            foreach (var productComponent in productComponents)
            {
                int countOnStocks = source.ResourceIngridients
                                            .Where(rec => rec.IngridientId == productComponent.IngridientId)
                                            .Sum(rec => rec.Count);
                if (countOnStocks < productComponent.Count * element.Count)
                {
                    var componentName = source.Ingridients
                                    .FirstOrDefault(rec => rec.Id == productComponent.IngridientId);
                    throw new Exception("Не достаточно компонента " + componentName?.IngridientName +
                        " требуется " + productComponent.Count + ", в наличии " + countOnStocks);
                }
            }
            // списываем
            foreach (var productComponent in productComponents)
            {
                int countOnStocks = productComponent.Count * element.Count;
                var stockComponents = source.ResourceIngridients
                                            .Where(rec => rec.IngridientId == productComponent.IngridientId);
                foreach (var stockComponent in stockComponents)
                {
                    // компонентов на одном слкаде может не хватать
                    if (stockComponent.Count >= countOnStocks)
                    {
                        stockComponent.Count -= countOnStocks;
                        break;
                    }
                    else
                    {
                        countOnStocks -= stockComponent.Count;
                        stockComponent.Count = 0;
                    }
                }
            }
            element.WorkerId = model.WorkerId;
            element.DateImplement = DateTime.Now;
            element.Status = RequestStatus.Выполняется;
        }

        public void FinishOrder(int id)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Готов;
        }

        public void PayOrder(int id)
        {
            Request element = source.Requests.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Оплачен;
        }

        public void PutComponentOnStock(ResourceIngridientBindingModel model)
        {
            ResourceIngridient element = source.ResourceIngridients
                                                .FirstOrDefault(rec => rec.ResourceId == model.ResourceId &&
                                                                    rec.IngridientId == model.IngridientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.ResourceIngridients.Count > 0 ? source.ResourceIngridients.Max(rec => rec.Id) : 0;
                source.ResourceIngridients.Add(new ResourceIngridient
                {
                    Id = ++maxId,
                    ResourceId = model.ResourceId,
                    IngridientId = model.IngridientId,
                    Count = model.Count
                });
            }
        }
    }
}
