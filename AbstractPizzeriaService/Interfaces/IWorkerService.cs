using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.Interfaces
{
    public interface IWorkerService
    {
        List<WorkerViewModel> GetList();

        WorkerViewModel GetElement(int id);

        void AddElement(WorkerBindingModel model);

        void UpdElement(WorkerBindingModel model);

        void DelElement(int id);
    }
}
