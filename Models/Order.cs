using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Order:BaseEntity
    {
        public int OrderId {get;set;}

        [Range(0,Int32.MaxValue,ErrorMessage = "Please input the quantity of product you want to buy.")]
        public int Quantity { get; set; }
        public int ProductId {get;set;}

        public Product Product {get;set;}

        public int CustomerId {get;set;}

        public Customer Customer {get;set;}
        public DateTime CreatedAt {get;set;}
        
    }
}