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
    public class IngridientServiceBD : IIngridientService
    {
        private AbstractDbContext context;

        public IngridientServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<IngridientViewModel> GetList()
        {
            List<IngridientViewModel> result = context.Ingridients
                .Select(rec => new IngridientViewModel
                {
                    Id = rec.Id,
                    IngridientName = rec.IngridientName
                })
                .ToList();
            return result;
        }

        public IngridientViewModel GetElement(int id)
        {
            Ingridient element = context.Ingridients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new IngridientViewModel
                {
                    Id = element.Id,
                    IngridientName = element.IngridientName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(IngridientBindingModel model)
        {
            Ingridient element = context.Ingridients.FirstOrDefault(rec => rec.IngridientName == model.IngridientName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context.Ingridients.Add(new Ingridient
            {
                IngridientName = model.IngridientName
            });
            context.SaveChanges();
        }

        public void UpdElement(IngridientBindingModel model)
        {
            Ingridient element = context.Ingridients.FirstOrDefault(rec =>
                                        rec.IngridientName == model.IngridientName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = context.Ingridients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.IngridientName = model.IngridientName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Ingridient element = context.Ingridients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Ingridients.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
