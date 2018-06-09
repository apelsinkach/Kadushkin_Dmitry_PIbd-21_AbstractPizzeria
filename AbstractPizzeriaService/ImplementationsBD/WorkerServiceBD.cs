using AbstractPizzeria;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ImplementationsBD
{
   public class WorkerServiceBD : IWorkerService
    {
        private AbstractDbContext context;

        public WorkerServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<WorkerViewModel> GetList()
        {
            List<WorkerViewModel> result = context.Workers
                .Select(rec => new WorkerViewModel
                {
                    Id = rec.Id,
                    WorkerFIO = rec.WorkerFIO
                })
                .ToList();
            return result;
        }

        public WorkerViewModel GetElement(int id)
        {
            Worker element = context.Workers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WorkerViewModel
                {
                    Id = element.Id,
                    WorkerFIO = element.WorkerFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WorkerBindingModel model)
        {
            Worker element = context.Workers.FirstOrDefault(rec => rec.WorkerFIO == model.WorkerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            context.Workers.Add(new Worker
            {
                WorkerFIO = model.WorkerFIO
            });
            context.SaveChanges();
        }

        public void UpdElement(WorkerBindingModel model)
        {
            Worker element = context.Workers.FirstOrDefault(rec =>
                                        rec.WorkerFIO == model.WorkerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = context.Workers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WorkerFIO = model.WorkerFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Worker element = context.Workers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Workers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
