using System.Collections.Generic;

namespace Network.Models
{
    public class ModelBundle
    {
        public User user { get; set; }
        public List<Connection> connections { get; set; }
        public List<Invitation> invitations { get; set; }
    }

}