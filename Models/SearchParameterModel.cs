using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TV_DASH_API.Models
{
    public class SearchParameterModel
    {
        public string Keyword { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Username { get; set; }
        public int LocationId { get; set; }
        public int BuildingId { get; set; }
        public int ZoneId { get; set; }
        public int FloorId { get; set; }

    }
}
