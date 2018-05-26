using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerViewModel> GetList();

        CustomerViewModel GetElement(int id);

        void AddElement(CustomerBindingModel model);

        void UpdElement(CustomerBindingModel model);

        void DelElement(int id);
    }
}
