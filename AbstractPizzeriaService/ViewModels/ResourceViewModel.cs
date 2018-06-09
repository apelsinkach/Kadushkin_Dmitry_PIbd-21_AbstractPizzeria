using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.ViewModels
{
    [DataContract]
    public class ResourceViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ResourceName { get; set; }
        [DataMember]
        public List<ResourceIngridientViewModel> ResourceIngridients { get; set; }
    }
}
