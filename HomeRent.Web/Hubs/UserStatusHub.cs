using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace HomeRent.Web.Hubs
{
    public class UserStatusHub : Hub
    {
        private static ConcurrentDictionary<string, string> onlineUsers = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                onlineUsers[userId] = Context.ConnectionId;

                if (!Context.User.IsInRole("Administrator"))
                {
                    Clients.Group("AdminGroup").SendAsync("UserStatusChanged", userId, "Online");
                }
                else
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, "AdminGroup");
                }
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                onlineUsers.TryRemove(userId, out _);

                if (!Context.User.IsInRole("Administrator"))
                {
                    Clients.Group("AdminGroup").SendAsync("UserStatusChanged", userId, "Offline");
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public Task<ConcurrentDictionary<string, string>> GetOnlineUsers()
        {
            return Task.FromResult(onlineUsers);
        }
    }
}
