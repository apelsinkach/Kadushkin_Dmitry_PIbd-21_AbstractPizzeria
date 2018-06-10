using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ViewModels
{
    public class ResourceViewModel
    {
        public int Id { get; set; }

        public string ResourceName { get; set; }

        public List<ResourceIngridientViewModel> ResourceIngridients { get; set; }
    }
}
