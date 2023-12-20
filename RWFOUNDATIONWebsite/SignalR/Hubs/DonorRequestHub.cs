using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.SignalR.Hubs
{
    public class DonorRequestHub: Hub
    {
        public async Task sendRequest(string user, string message)
        {
            await Clients.All.SendAsync("DonorRequest", user, message);
        }
        public async Task sendAlertTest()
        {
            await Clients.All.SendAsync("Added");
        }
    }
}
