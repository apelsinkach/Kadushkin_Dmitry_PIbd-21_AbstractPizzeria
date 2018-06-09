using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.Interfaces
{
   public interface IBasicService
    {
        List<RequestViewModel> GetList();

        void CreateOrder(RequestBindingModel model);

        void TakeOrderInWork(RequestBindingModel model);

        void FinishOrder(int id);

        void PayOrder(int id);

        void PutComponentOnStock(ResourceIngridientBindingModel model);
    }
}
