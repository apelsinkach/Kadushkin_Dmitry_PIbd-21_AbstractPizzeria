using AbstractPizzeria;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace AbstractPizzeriaService.ImplementationsBD
{
   public class BasicServiceBD : IBasicService
    {
        private AbstractDbContext context;

        public BasicServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<RequestViewModel> GetList()
        {
            List<RequestViewModel> result = context.Requests
                .Select(rec => new RequestViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    ArticleId = rec.ArticleId,
                    WorkerId = rec.WorkerId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    ArticleName = rec.Article.ArticleName,
                    WorkerName = rec.Worker.WorkerFIO
                })
                .ToList();
            return result;
        }

        public void CreateOrder(RequestBindingModel model)
        {
            var request = new Request
            {
                CustomerId = model.CustomerId,
                ArticleId = model.ArticleId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RequestStatus.Принят
            };
            context.Requests.Add(request);
            context.SaveChanges();
            var customer = context.Customers.FirstOrDefault(x => x.Id == model.CustomerId);
            SendEmail(customer.Mail, "Оповещение по заказам",
            string.Format("Заказ №{0} от {1} создан успешно", request.Id,
            request.DateCreate.ToShortDateString()));
        }

        public void TakeOrderInWork(RequestBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Request element = context.Requests.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var productComponents = context.ArticleIngridients
                                                .Include(rec => rec.Ingridient)
                                                .Where(rec => rec.ArticleId == element.ArticleId);
                    // списываем
                    foreach (var productComponent in productComponents)
                    {
                        int countOnStocks = productComponent.Count * element.Count;
                        var stockComponents = context.ResourceIngridients
                                                    .Where(rec => rec.IngridientId == productComponent.IngridientId);
                        foreach (var stockComponent in stockComponents)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (stockComponent.Count >= countOnStocks)
                            {
                                stockComponent.Count -= countOnStocks;
                                countOnStocks = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStocks -= stockComponent.Count;
                                stockComponent.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStocks > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                productComponent.Ingridient.IngridientName + " требуется " +
                                productComponent.Count + ", не хватает " + countOnStocks);
                        }
                    }
                    element.WorkerId = model.WorkerId;
                    element.DateImplement = DateTime.Now;
                    element.Status = RequestStatus.Выполняется;
                    context.SaveChanges();
                    SendEmail(element.Customer.Mail, "Оповещение по заказам",
                    string.Format("Заказ №{0} от {1} передеан в работу", element.Id, element.DateCreate.ToShortDateString()));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishOrder(int id)
        {
            Request element = context.Requests.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Готов;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
            string.Format("Заказ №{0} от {1} передан на оплату", element.Id,
            element.DateCreate.ToShortDateString()));
        }

        public void PayOrder(int id)
        {
            Request element = context.Requests.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = RequestStatus.Оплачен;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
            string.Format("Заказ №{0} от {1} оплачен успешно", element.Id, element.DateCreate.ToShortDateString()));
        }

        public void PutComponentOnStock(ResourceIngridientBindingModel model)
        {
            ResourceIngridient element = context.ResourceIngridients
                                                .FirstOrDefault(rec => rec.ResourceId == model.ResourceId &&
                                                                    rec.IngridientId == model.IngridientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.ResourceIngridients.Add(new ResourceIngridient
                {
                    ResourceId = model.ResourceId,
                    IngridientId = model.IngridientId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;

            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}
