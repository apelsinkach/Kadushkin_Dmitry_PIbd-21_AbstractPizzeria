using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.BindingModels
{
    [DataContract]
    public class ArticleIngridientBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ArticleId { get; set; }
        [DataMember]
        public int IngridientId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
