using Microsoft.AspNetCore.SignalR;

namespace IronDome.Hubs
{
    public class RealTime : Hub
    {
        public async Task AttackAlert(int id, int rt, string name)
        {
            await Clients.All.SendAsync("RedAlert", id, rt, name);
        }
    }
}
