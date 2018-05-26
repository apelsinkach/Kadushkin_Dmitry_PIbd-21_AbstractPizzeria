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
    public class WorkerServiceList : IWorkerService
    {
        private ListDataSingleton source;

        public WorkerServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<WorkerViewModel> GetList()
        {
            List<WorkerViewModel> result = new List<WorkerViewModel>();
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                result.Add(new WorkerViewModel
                {
                    Id = source.Workers[i].Id,
                    WorkerFIO = source.Workers[i].WorkerFIO
                });
            }
            return result;
        }

        public WorkerViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id == id)
                {
                    return new WorkerViewModel
                    {
                        Id = source.Workers[i].Id,
                        WorkerFIO = source.Workers[i].WorkerFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WorkerBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id > maxId)
                {
                    maxId = source.Workers[i].Id;
                }
                if (source.Workers[i].WorkerFIO == model.WorkerFIO)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            source.Workers.Add(new Worker
            {
                Id = maxId + 1,
                WorkerFIO = model.WorkerFIO
            });
        }

        public void UpdElement(WorkerBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Workers[i].WorkerFIO == model.WorkerFIO &&
                    source.Workers[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сотрудник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Workers[index].WorkerFIO = model.WorkerFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Workers.Count; ++i)
            {
                if (source.Workers[i].Id == id)
                {
                    source.Workers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
