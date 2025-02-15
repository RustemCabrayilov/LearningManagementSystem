using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LearningManagementSystem.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public static Dictionary<string, string> Users = new();
        public ChatHub(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        
        public async Task Connect(string userId)
        {
            Users.Add(Context.ConnectionId, userId);
            string connectionId = Context.ConnectionId;
                var appUser = await _userManager.FindByIdAsync(userId);
                appUser.ConnectionId = connectionId;
                await _userManager.UpdateAsync(appUser);
                await Clients.All.SendAsync("Login", appUser.Id);
            
            await base.OnConnectedAsync();
        }
        /*public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser? appUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                appUser.ConnectionId = connectionId;
                await _userManager.UpdateAsync(appUser);
                await Clients.All.SendAsync("Login", appUser.Id);
            }
            await base.OnConnectedAsync();
        }*/

        public async Task Disconnect(string userId)
        {
            Users.TryGetValue(Context.ConnectionId, out userId);
            Users.Remove(Context.ConnectionId);
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                user.ConnectionId =null;
              await  _userManager.UpdateAsync(user);
                await Clients.All.SendAsync("Logout", user);
            }
        }
    }

}