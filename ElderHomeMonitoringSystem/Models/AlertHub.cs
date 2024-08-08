using ElderHomeMonitoringSystem.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ElderHomeMonitoringSystem.Models
{
    public class AlertHub: Hub
    {
        public readonly IAccountRepository _accountRepository;
        public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        public AlertHub(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task SendNotification(int recipientId, string messageContent)
        {
            Console.WriteLine($"SendNotification: recipientId={recipientId}, messageContent={messageContent}");

            var recipient = await _accountRepository.GetUserByID(recipientId);

            if (recipient == null)
            {
                Console.WriteLine("SendNotification: Recipient not found.");
                throw new ArgumentException("Recipient not found.");
            }

            var connectionIds = ConnectedUsers.Where(kvp => kvp.Value == recipientId.ToString()).Select(kvp => kvp.Key).ToList();
            if (connectionIds.Count > 0)
            {
                foreach (var connectionId in connectionIds)
                {
                    Console.WriteLine($"SendNotification: Sending notification to connection ID {connectionId}.");
                    await Clients.Client(connectionId).SendAsync("ReceiveNotification", messageContent);
                }
                Console.WriteLine($"SendNotification: Sent notification to {recipientId} with content: {messageContent}");
            }
            else
            {
                Console.WriteLine($"SendNotification: User {recipientId} is not connected.");
            }
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                var existingConnections = ConnectedUsers.Where(kvp => kvp.Value == userId).Select(kvp => kvp.Key).ToList();
                foreach (var connectionId in existingConnections)
                {
                    ConnectedUsers.TryRemove(connectionId, out _);
                    Console.WriteLine($"OnConnectedAsync: Removed existing connection ID {connectionId} for user {userId}");
                }

                ConnectedUsers[Context.ConnectionId] = userId;
                Console.WriteLine($"OnConnectedAsync: User {userId} connected with connection ID {Context.ConnectionId}");

                PrintConnectedUsers();
            }
            else
            {
                Console.WriteLine("OnConnectedAsync: User ID not found in query string.");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnectedUsers.TryRemove(Context.ConnectionId, out var userId))
            {
                Console.WriteLine($"OnDisconnectedAsync: User {userId} disconnected.");
            }
            else
            {
                Console.WriteLine("OnDisconnectedAsync: User connection ID not found in connected users list.");
            }

            PrintConnectedUsers();

            await base.OnDisconnectedAsync(exception);
        }

        public static string[] GetConnectedUsers()
        {
            var users = ConnectedUsers.Values.Distinct().ToArray();
            Console.WriteLine($"GetConnectedUsers: {string.Join(", ", users)}");
            return users;
        }

        public async Task Ping()
        {
            Console.WriteLine("Ping method called");
            await Clients.Caller.SendAsync("Pong", "Ping received successfully");
        }


        private void PrintConnectedUsers()
        {
            var connectedUsers = ConnectedUsers.Values.Distinct().ToList();
            Console.WriteLine("Currently connected users: ");
            foreach (var user in connectedUsers)
            {
                Console.WriteLine($"User ID: {user}");
            }
        }
    }
}
