using AbstractPizzeriaService.Attributies;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IBasicService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<RequestViewModel> GetList();
        [CustomMethod("Метод создания заказа")]
        void CreateOrder(RequestBindingModel model);
        [CustomMethod("Метод передачи заказа в работу")]
        void TakeOrderInWork(RequestBindingModel model);
        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishOrder(int id);
        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayOrder(int id);
        [CustomMethod("Метод пополнения компонент на складе")]
        void PutComponentOnStock(ResourceIngridientBindingModel model);
    }
}
