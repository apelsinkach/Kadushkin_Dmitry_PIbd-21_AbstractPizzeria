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
   public class IngridientServiceList : IIngridientService
    {
        private ListDataSingleton source;

        public IngridientServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<IngridientViewModel> GetList()
        {
            List<IngridientViewModel> result = new List<IngridientViewModel>();
            for (int i = 0; i < source.Ingridients.Count; ++i)
            {
                result.Add(new IngridientViewModel
                {
                    Id = source.Ingridients[i].Id,
                    IngridientName = source.Ingridients[i].IngridientName
                });
            }
            return result;
        }

        public IngridientViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Ingridients.Count; ++i)
            {
                if (source.Ingridients[i].Id == id)
                {
                    return new IngridientViewModel
                    {
                        Id = source.Ingridients[i].Id,
                        IngridientName = source.Ingridients[i].IngridientName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(IngridientBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Ingridients.Count; ++i)
            {
                if (source.Ingridients[i].Id > maxId)
                {
                    maxId = source.Ingridients[i].Id;
                }
                if (source.Ingridients[i].IngridientName == model.IngridientName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Ingridients.Add(new Ingridient
            {
                Id = maxId + 1,
                IngridientName = model.IngridientName
            });
        }

        public void UpdElement(IngridientBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Ingridients.Count; ++i)
            {
                if (source.Ingridients[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Ingridients[i].IngridientName == model.IngridientName &&
                    source.Ingridients[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Ingridients[index].IngridientName = model.IngridientName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Ingridients.Count; ++i)
            {
                if (source.Ingridients[i].Id == id)
                {
                    source.Ingridients.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
