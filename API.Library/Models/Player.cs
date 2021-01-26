using System;
using System.Collections.Generic;
using System.Text;

namespace API.Library
{
    public class Player : User
    {
        public string ConnectionId { get; set; }
        public int Rating { get; set; }
        public string CurrentGameType { get; set; }
    }
}
