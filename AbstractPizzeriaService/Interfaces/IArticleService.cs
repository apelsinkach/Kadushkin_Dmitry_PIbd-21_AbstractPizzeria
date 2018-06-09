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
    [CustomInterface("Интерфейс для работы с изделиями")]
    public interface IArticleService
    {
        [CustomMethod("Метод получения списка изделий")]
        List<ArticleViewModel> GetList();
        [CustomMethod("Метод получения изделия по id")]
        ArticleViewModel GetElement(int id);
        [CustomMethod("Метод добавления изделия")]
        void AddElement(ArticleBindingModel model);
        [CustomMethod("Метод изменения данных по изделию")]
        void UpdElement(ArticleBindingModel model);
        [CustomMethod("Метод удаления изделия")]
        void DelElement(int id);
    }
}
