using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ViewModels
{
    public class ArticleIngridientViewModel
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public int IngridientId { get; set; }

        public string IngridientName { get; set; }

        public int Count { get; set; }
    }
}
