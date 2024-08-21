using Data.Context;
using Entity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace SignalRProject.Hub
{
    //add comment 1
    //add cooment 2
    public interface IChatHub
    {
        Task ReceiveNotification(string name, string content);
    }
    public class ChatHub : Hub<IChatHub>
    {
        private readonly DbSet<Entity.UserNotification> _userNotification;

        protected IUnitOfWork _uow;
        public ChatHub(IUnitOfWork uow) {
            _uow = uow;
            _userNotification = _uow.Set<Entity.UserNotification>();
        }

        public async Task SendMessage(string user, string message)
        {
            string name = "";
            UserNotification currentuser = new UserNotification();
            try
            {
                currentuser = await _userNotification.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefaultAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(currentuser?.UserName))
                {
                    await Clients.Client(Context.ConnectionId).ReceiveNotification("Sys", "متصل نیستی");
                    return;
                }

                UserNotification userdb = await _userNotification.Where(c => c.ConnectionId == user).FirstOrDefaultAsync().ConfigureAwait(false);
                name = userdb?.UserName;
            }
            catch (Exception ex)
            {
                name = "";
                await Clients.Client(Context.ConnectionId).ReceiveNotification(name, "این کاربر انلاین نیست");
            }
            //await Clients.Client(user).ReceiveNotification( message);
            if (!string.IsNullOrEmpty(name))
            {                                
                await Clients.Client(user).ReceiveNotification(currentuser.UserName, message);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).ReceiveNotification("", "این کاربر انلاین نیست");
            }
        }
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, "chatroom");

            return base.OnConnectedAsync();
        }

        public async Task Oisconnect()
        {
            var userdb = await _userNotification.Where(c => c.ConnectionId == Context.ConnectionId).FirstOrDefaultAsync();
            if (userdb != null)
            {
                _userNotification.Remove(userdb);
                _uow.SaveChanges();
            }
            
            await base.OnDisconnectedAsync(new Exception());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SetNameConnectionId(string user , string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var userdb = await _userNotification.Where(c=>c.ConnectionId == user)?.FirstOrDefaultAsync();
                if (userdb == null)
                {
                    _userNotification.Add(new UserNotification() { ConnectionId = Context.ConnectionId, UserName = name });
                    _uow.SaveChanges();
                }
                else
                {
                    userdb.UserName = name;
                    _userNotification.Update(userdb);
                }
            }
        }
    }

}
