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
   public class BasicServiceList : IBasicService
    {
        private ListDataSingleton source;

        public BasicServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = new List<RequestViewModel>();
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                string clientFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if (source.Customers[j].Id == source.Requests[i].CustomerId)
                    {
                        clientFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string productName = string.Empty;
                for (int j = 0; j < source.Articles.Count; ++j)
                {
                    if (source.Articles[j].Id == source.Requests[i].ArticleId)
                    {
                        productName = source.Articles[j].ArticleName;
                        break;
                    }
                }
                string implementerFIO = string.Empty;
                if (source.Requests[i].WorkerId.HasValue)
                {
                    for (int j = 0; j < source.Workers.Count; ++j)
                    {
                        if (source.Workers[j].Id == source.Requests[i].WorkerId.Value)
                        {
                            implementerFIO = source.Workers[j].WorkerFIO;
                            break;
                        }
                    }
                }
                result.Add(new RequestViewModel
                {
                    Id = source.Requests[i].Id,
                    CustomerId = source.Requests[i].CustomerId,
                    CustomerFIO = clientFIO,
                    ArticleId = source.Requests[i].ArticleId,
                    ArticleName = productName,
                    WorkerId = source.Requests[i].WorkerId,
                    WorkerName = implementerFIO,
                    Count = source.Requests[i].Count,
                    Sum = source.Requests[i].Sum,
                    DateCreate = source.Requests[i].DateCreate.ToLongDateString(),
                    DateImplement = source.Requests[i].DateImplement?.ToLongDateString(),
                    Status = source.Requests[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateOrder(RequestBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Requests[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Requests[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for (int i = 0; i < source.ArticleIngridients.Count; ++i)
            {
                if (source.ArticleIngridients[i].ArticleId == source.Requests[index].ArticleId)
                {
                    int countOnStocks = 0;
                    for (int j = 0; j < source.ResourceIngridients.Count; ++j)
                    {
                        if (source.ResourceIngridients[j].IngridientId == source.ArticleIngridients[i].IngridientId)
                        {
                            countOnStocks += source.ResourceIngridients[j].Count;
                        }
                    }
                    if (countOnStocks < source.ArticleIngridients[i].Count * source.Requests[index].Count)
                    {
                        for (int j = 0; j < source.Ingridients.Count; ++j)
                        {
                            if (source.Ingridients[j].Id == source.ArticleIngridients[i].IngridientId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Ingridients[j].IngridientName +
                                    " требуется " + source.ArticleIngridients[i].Count + ", в наличии " + countOnStocks);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.ArticleIngridients.Count; ++i)
            {
                if (source.ArticleIngridients[i].ArticleId == source.Requests[index].ArticleId)
                {

                    int countOnStocks = source.ArticleIngridients[i].Count * source.Requests[index].Count;
                    for (int j = 0; j < source.ResourceIngridients.Count; ++j)
                    {
                        if (source.ResourceIngridients[j].IngridientId == source.ArticleIngridients[i].IngridientId)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.ResourceIngridients[j].Count >= countOnStocks)
                            {
                                source.ResourceIngridients[j].Count -= countOnStocks;
                                break;
                            }
                            else
                            {
                                countOnStocks -= source.ResourceIngridients[j].Count;
                                source.ResourceIngridients[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Requests[index].WorkerId = model.WorkerId;
            source.Requests[index].DateImplement = DateTime.Now;
            source.Requests[index].Status = RequestStatus.Выполняется;
        }

        public void FinishOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Requests[index].Status = RequestStatus.Готов;
        }

        public void PayOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Requests.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Requests[index].Status = RequestStatus.Оплачен;
        }

        public void PutComponentOnStock(ResourceIngridientBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.ResourceIngridients.Count; ++i)
            {
                if (source.ResourceIngridients[i].ResourceId == model.ResourceId &&
                    source.ResourceIngridients[i].IngridientId == model.IngridientId)
                {
                    source.ResourceIngridients[i].Count += model.Count;
                    return;
                }
                if (source.ResourceIngridients[i].Id > maxId)
                {
                    maxId = source.ResourceIngridients[i].Id;
                }
            }
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
