using AbstractPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPizzeriaService
{
    class ListDataSingleton
    {
        private static ListDataSingleton instance;

        public List<Customer> Customers { get; set; }

        public List<Ingridient> Ingridients { get; set; }

        public List<Worker> Workers { get; set; }

        public List<Request> Requests { get; set; }

        public List<Article> Articles { get; set; }

        public List<ArticleIngridient> ArticleIngridients { get; set; }

        public List<Resource> Resources { get; set; }

        public List<ResourceIngridient> ResourceIngridients { get; set; }

        private ListDataSingleton()
        {
            Customers = new List<Customer>();
            Ingridients = new List<Ingridient>();
            Workers = new List<Worker>();
            Requests = new List<Request>();
            Articles = new List<Article>();
            ArticleIngridients = new List<ArticleIngridient>();
            Resources = new List<Resource>();
            ResourceIngridients = new List<ResourceIngridient>();
        }

        public static ListDataSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new ListDataSingleton();
            }

            return instance;
        }
    }
}
