using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLDA_VehicleData_Watch.Models
{
    public class MyHub:Hub
    {
        public static  List<string> user=new List<string>();
        private readonly ILogger<MyHub> _logger;
        public MyHub(ILogger<MyHub> logger)
        {
            _logger = logger;
        }
       
        public async Task SendMessage(string user, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage",user,message);
        }

        public override async Task OnConnectedAsync()
        {
            //user.Clear();
                //_logger.LogInformation("此Signalr链接ID已开始：{0}", Context.User.Identity.Name + this.Context.ConnectionId);
                if (user != null)
            {
                if (!user.Contains(Context.User.Identity.Name + Context.ConnectionId))
                {
                    user.Add(Context.User.Identity.Name + Context.ConnectionId);
                }
            }
            else
            {
                user.Add(Context.User.Identity.Name + Context.ConnectionId);
            }

           
            //将同一个人的连接ID绑定到同一个分组，推送时就推送给这个分组
           await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name + Context.ConnectionId);
           
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
           await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name + Context.ConnectionId);
            //_logger.LogWarning(exception.ToString());
            //_logger.LogInformation("此Signalr链接ID已断开：{0}", this.Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
