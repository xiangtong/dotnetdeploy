
using System;

namespace Network.Models
{
    public class Invitation:BaseEntity
    {
        public int InvitationId {get; set;}
        public DateTime CreatedAt {get;set;}
        public int SenderId {get;set;}
        public User Sender {get;set;}
        public int ReceiverId {get;set;}
        public User Receiver {get;set;}

    }
}