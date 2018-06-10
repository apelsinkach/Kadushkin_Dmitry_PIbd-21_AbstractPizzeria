
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.BindingModels
{
   public class ArticleBindingModel
    {

        public int Id { get; set; }

        public string ArticleName { get; set; }

        public decimal Price { get; set; }

        public List<ArticleIngridientBindingModel> ArticleIngridients { get; set; }
    }
}
