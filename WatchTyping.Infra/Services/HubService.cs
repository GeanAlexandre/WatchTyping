﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WatchTyping.Core.Events;
using WatchTyping.Core.Services;
using WatchTyping.Infra.Hubs;

namespace WatchTyping.Infra.Services
{
    public class HubService : IHubService
    {
        private readonly IHubContext<WritingTypingHub> _hub;

        public HubService(IHubContext<WritingTypingHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyUserCreateNewPaperAsync(UserCreateNewPaperEvent @event)
        {
            await _hub.Groups.AddAsync(@event.ConnectionId, @event.GroupId);
            await _hub.Clients.Group(@event.GroupId).InvokeAsync(nameof(UserCreateNewPaperEvent), @event.GroupId);
        }

        public async Task NotifyUserJoinGroupAsync(UserJoinGroupEvent @event)
        {
            await _hub.Groups.AddAsync(@event.ConnectionId, @event.GroupId);
            await _hub.Clients.Client(@event.ConnectionId).InvokeAsync(nameof(UserJoinGroupEvent), @event.LastMessage);
        }

        public async Task NotifyUserWritingTextAsync(UserWritingTextEvent @event)
        {
            await _hub.Groups.RemoveAsync(@event.ConnectionId, @event.GroupId);
            await _hub.Clients.Group(@event.GroupId).InvokeAsync(nameof(UserWritingTextEvent), @event.Message);
            await _hub.Groups.AddAsync(@event.ConnectionId, @event.GroupId);
        }
    }
}
