﻿using System;
using System.Threading.Tasks;
using WatchTyping.Core.Bus;
using WatchTyping.Core.Commands;
using WatchTyping.Core.Events;
using WatchTyping.Core.Models;
using WatchTyping.Core.Repositories;

namespace WatchTyping.Core.CommandHandlers
{
    public class UserWritingTextCommandHandler : IUserWritingTextCommandHandler
    {
        private readonly IBus _bus;
        private readonly IMessageRepository _messageRepository;

        public UserWritingTextCommandHandler(IBus bus, IMessageRepository messageRepository)
        {
            _bus = bus;
            _messageRepository = messageRepository;
        }

        public async Task ExecuteAsync(UserWritingTextCommand command)
        {
            await _messageRepository.UpdateMessageAsync(command.GroupId, Message.CreateNew(DateTime.Now, command.Message));
            await _bus.RaiseEventAsync(new UserWritingTextEvent { GroupId = command.GroupId, Message = command.Message });
        }
    }
}
