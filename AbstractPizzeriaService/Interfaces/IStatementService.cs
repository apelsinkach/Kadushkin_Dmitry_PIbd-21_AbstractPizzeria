using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.Interfaces
{
    public interface IStatementService
    {
        void SaveProductPrice(StatementBindingModel model);

        List<ResourcesLoadViewModel> GetStocksLoad();

        void SaveStocksLoad(StatementBindingModel model);

        List<CustomerRequestsModel> GetClientOrders(StatementBindingModel model);

        void SaveClientOrders(StatementBindingModel model);

    }
}
