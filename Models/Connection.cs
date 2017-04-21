
using System;

namespace Network.Models
{
    public class Connection:BaseEntity
    {
        public int ConnectionId {get; set;}
        
        public DateTime CreatedAt {get;set;}
        public int SelfId {get;set;}
        public User Self {get;set;}
        public int FriendId {get;set;}
        public User Friend {get;set;}

    }
}