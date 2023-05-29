using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TV_DASH_API.Models
{
    public class Floor
    {
        public int FloorId { get; set; }
        public string FloorName { get; set; }
        public int IsActive { get; set; }
        public int BuildingId { get; set; }
    }
}
