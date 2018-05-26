using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ViewModels
{
    public class ResourcesLoadViewModel
    {
        public string ResourceName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Ingridients { get; set; }
    }
}
