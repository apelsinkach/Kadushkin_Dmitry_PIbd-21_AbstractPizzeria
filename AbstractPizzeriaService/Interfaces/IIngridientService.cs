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
    [CustomInterface("Интерфейс для работы с компонентами")]
    public interface IIngridientService
    {
        [CustomMethod("Метод получения списка компонент")]
        List<IngridientViewModel> GetList();
        [CustomMethod("Метод получения компонента по id")]
        IngridientViewModel GetElement(int id);
        [CustomMethod("Метод добавления компонента")]
        void AddElement(IngridientBindingModel model);
        [CustomMethod("Метод изменения данных по компоненту")]
        void UpdElement(IngridientBindingModel model);
        [CustomMethod("Метод удаления компонента")]
        void DelElement(int id);
    }
}
