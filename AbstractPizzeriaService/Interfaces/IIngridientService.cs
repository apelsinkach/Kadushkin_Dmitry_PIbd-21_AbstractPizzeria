using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.Interfaces
{
   public interface IIngridientService
    {
        List<IngridientViewModel> GetList();

        IngridientViewModel GetElement(int id);

        void AddElement(IngridientBindingModel model);

        void UpdElement(IngridientBindingModel model);

        void DelElement(int id);
    }
}
