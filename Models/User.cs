
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Network.Models
{
    public abstract class BaseEntity{}
    public class User:BaseEntity
    {
        public int UserId {get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt {get;set;}
        [InverseProperty("Self")]
        public List<Connection> Connections {get;set;}
        [InverseProperty("Friend")]
        public List<Connection> FConnections {get;set;}
        
        [InverseProperty("Receiver")]
        public List<Invitation> Invitations {get;set;}
        [InverseProperty("Sender")]
        public List<Invitation> SInvitations {get;set;}
        public User()
        {
            Connections = new List<Connection>();
            Invitations = new List<Invitation>();
        }
        
    }
}