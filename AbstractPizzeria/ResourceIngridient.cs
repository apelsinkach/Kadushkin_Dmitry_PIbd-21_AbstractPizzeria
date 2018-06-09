using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeria
{
    public class ResourceIngridient
    {
        public int Id { get; set; }

        public int ResourceId { get; set; }

        public int IngridientId { get; set; }

        public int Count { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual Ingridient Ingridient { get; set; }
    }
}
