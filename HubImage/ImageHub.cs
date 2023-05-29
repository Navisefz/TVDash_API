using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TV_DASH_API.Models;

namespace TV_DASH_API.HubImage
{
    public class ImageHub:Hub
    {
        public async Task Message(MessageModel message)
        {
            await Clients.Others.SendAsync("message", message);
        }
    }
}
