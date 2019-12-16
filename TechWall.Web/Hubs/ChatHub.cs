using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using TechWall.Services;
using TechWall.ViewModels;

namespace TechWall.Hubs
{
    public class ChatHub : Hub
    {
        public static IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

        public override Task OnConnected()
        {
            if (HttpContext.Current.User.Identity.GetUserId() != null)
            {
                string userId = new AppService().AddUserConnection(Guid.Parse(Context.ConnectionId));
                Clients.All.BroadcastOnlineUser(userId);
            }
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userId = new AppService().RemoveUserConnection(Guid.Parse(Context.ConnectionId));
            Clients.All.BroadcastOfflineUser(userId);
            return base.OnDisconnected(stopCalled);
        }

        public void GetUsersToChat()
        {
            string UserId = HttpContext.Current.User.Identity.GetUserId();
            List<UserDTO> users = new AppService().GetUsersToChat();
            Clients.Clients(new AppService().GetUserConnections(UserId)).BroadcastUsersToChat(users);
        }

        public static void OfflineUser(string userId)
        {
            context.Clients.All.BroadcastOfflineUser(userId);
        }

        public static void RecieveMessage(string fromUserId, string toUserId, string message)
        {
            context.Clients.Clients(new AppService().GetUserConnections(toUserId)).BroadcastRecieveMessage(fromUserId, message);
        }
    }
}