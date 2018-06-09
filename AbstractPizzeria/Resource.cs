using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeria
{
    public class Resource
    {
        public int Id { get; set; }

        [Required]
        public string ResourceName { get; set; }

        [ForeignKey("ResourceId")]
        public virtual List<ResourceIngridient> ResourceIngridients { get; set; }
    }
}
