using System;
using System.Collections.Generic;
using System.Text;

namespace Roommates.Models
{
    public class RoommateChore
    {
        public int Id { get; set; }
        public int RoommateId { get; set; }
        public int ChoreId { get; set; }
    }
}
