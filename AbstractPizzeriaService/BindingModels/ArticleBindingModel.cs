
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService.BindingModels
{
    [DataContract]
    public class ArticleBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ArticleName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<ArticleIngridientBindingModel> ArticleIngridients { get; set; }
    }
}
