using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.BindingModels
{
    public class ResourceIngridientBindingModel
    {
        public int Id { get; set; }

        public int ResourceId { get; set; }

        public int IngridientId { get; set; }

        public int Count { get; set; }
    }
}
