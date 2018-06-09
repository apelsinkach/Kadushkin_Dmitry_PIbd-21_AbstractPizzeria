using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerFIO { get; set; }

        public int ArticleId { get; set; }

        public string ArticleName { get; set; }

        public int? WorkerId { get; set; }

        public string WorkerName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }

        public string DateCreate { get; set; }

        public string DateImplement { get; set; }
    }
}
