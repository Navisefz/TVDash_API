using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TV_DASH_API.Models
{
    public class UserPreferenceModel
    {
        public int Sr_Preference_Id { get; set; }
        public int BuildingId { get; set; }
        public int FloorId { get; set; }
        public int ZoneId { get; set; }
        public int Theme { get; set; }
        public int UserTelecommuting { get; set; }
        public int FixedSeat { get; set; }
        public string Username { get; set; }
    }
}
