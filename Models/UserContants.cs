using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TV_DASH_API.Models
{
    public class UserContants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username = "admin", Password = "admin", UserId=1, Role = "TVADMIN" },
            new UserModel() { Username = "manager", Password = "manager", UserId=1, Role = "TVMANAGER" },
           // new UserModel() { Username = "user", Password = "user", UserId=1, Role = "SeatR-03" },
        };
    }
}
