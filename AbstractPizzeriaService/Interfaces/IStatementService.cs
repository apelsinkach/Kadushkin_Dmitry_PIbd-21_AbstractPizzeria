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
    [CustomInterface("Интерфейс для работы с отчетами")]
    public interface IStatementService
    {
        [CustomMethod("Метод сохранения списка изделий в doc-файл")]
        void SaveProductPrice(StatementBindingModel model);
        [CustomMethod("Метод получения списка складов с количество компонент на них")]
        List<ResourcesLoadViewModel> GetStocksLoad();
        [CustomMethod("Метод сохранения списка списка складов с количество компонент на них в xls-файл")]
        void SaveStocksLoad(StatementBindingModel model);
        [CustomMethod("Метод получения списка заказов клиентов")]
        List<CustomerRequestsModel> GetClientOrders(StatementBindingModel model);
        [CustomMethod("Метод сохранения списка заказов клиентов в pdf-файл")]
        void SaveClientOrders(StatementBindingModel model);

    }
}
