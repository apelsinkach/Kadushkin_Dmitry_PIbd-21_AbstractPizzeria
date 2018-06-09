using AbstractPizzeria;
using AbstractPizzeriaService;
using AbstractPizzeriaService.BindingModels;
using AbstractPizzeriaService.Interfaces;
using AbstractPizzeriaService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class IngridientServiceList : IIngridientService
    {
        private ListDataSingleton source;

        public IngridientServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<IngridientViewModel> GetList()
        {
            List<IngridientViewModel> result = source.Ingridients
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
            Ingridient element = source.Ingridients.FirstOrDefault(rec => rec.Id == id);
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
            Ingridient element = source.Ingridients.FirstOrDefault(rec => rec.IngridientName == model.IngridientName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxId = source.Ingridients.Count > 0 ? source.Ingridients.Max(rec => rec.Id) : 0;
            source.Ingridients.Add(new Ingridient
            {
                Id = maxId + 1,
                IngridientName = model.IngridientName
            });
        }

        public void UpdElement(IngridientBindingModel model)
        {
            Ingridient element = source.Ingridients.FirstOrDefault(rec =>
                                        rec.IngridientName == model.IngridientName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.Ingridients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.IngridientName = model.IngridientName;
        }

        public void DelElement(int id)
        {
            Ingridient element = source.Ingridients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Ingridients.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
