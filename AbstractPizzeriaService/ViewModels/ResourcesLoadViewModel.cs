using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ViewModels
{
    [DataContract]
    public class ResourcesLoadViewModel
    {
        [DataMember]
        public string ResourceName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<ResourcesIngridientLoadViewModel> Ingridients { get; set; }
    }

    [DataContract]
    public class ResourcesIngridientLoadViewModel
    {
        [DataMember]
        public string IngridientName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
