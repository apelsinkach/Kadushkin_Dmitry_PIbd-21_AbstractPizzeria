using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeria
{
    public class ArticleIngridient
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public int IngridientId { get; set; }

        public int Count { get; set; }

        public virtual Article Article { get; set; }

        public virtual Ingridient Ingridient { get; set; }
    }
}
