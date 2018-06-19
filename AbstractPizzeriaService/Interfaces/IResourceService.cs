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
    [CustomInterface("Интерфейс для работы со складами")]
    public interface IResourceService
    {

        [CustomMethod("Метод получения списка складов")]
        List<ResourceViewModel> GetList();
        [CustomMethod("Метод получения склада по id")]
        ResourceViewModel GetElement(int id);
        [CustomMethod("Метод добавления склада")]
        void AddElement(ResourceBindingModel model);
        [CustomMethod("Метод изменения данных по складу")]
        void UpdElement(ResourceBindingModel model);
        [CustomMethod("Метод удаления склада")]
        void DelElement(int id);
    }
}
