using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class ModelBundle
    {
        public List<Product> products { get; set; }
        // public List<Customer> customers { get; set; }
        // public List<Order> orders { get; set; }
        public List<string> orders {get;set;}
        public List<string> customers_activity {get;set;}
    }
}