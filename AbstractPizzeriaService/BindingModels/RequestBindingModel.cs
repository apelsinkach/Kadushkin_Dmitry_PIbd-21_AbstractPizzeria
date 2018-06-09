using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.BindingModels
{
   public class RequestBindingModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ArticleId { get; set; }

        public int? WorkerId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
