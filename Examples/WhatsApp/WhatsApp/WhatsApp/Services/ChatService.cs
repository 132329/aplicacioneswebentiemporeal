﻿using System;
using System.Collections.Generic;
using System.Linq;
using WhatsApp.Model;
using WhatsApp.Repositories;

namespace WhatsApp.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public IEnumerable<Chat> GetAllChats()
        {
            return chatRepository.GetAllChats();
        }

        public Chat GetChatBy(Guid id)
        {
            return chatRepository.GetChatBy(id);
        }

        public IEnumerable<Chat> GetChatsBy(string username)
        {
            return chatRepository.GetChatsBy(username);
        }

        public WhatsAppResponse GetMe(string name)
        {
            var user = chatRepository.GetUserBy(name);
            var chats = chatRepository.GetChatsBy(user);

            return new WhatsAppResponse
            {
                Me = new UserDto { Id = user.Id, Name = user.Name, Avatar = user.Avatar  },
                Chats = chats.Select(c =>
                    new ChatDto
                    {
                        Id = c.Id,
                        Image = c.IsGroup ? "http://thecatapi.com/api/images/get?format=src&type=gif" : c.Participants.Single(p => p.Id != user.Id).Avatar,
                        IsGroup = c.IsGroup,
                        Name = c.IsGroup ? c.Name : c.Participants.Single(p => p.Id != user.Id).Name,
                        Participants = c.Participants,
                        Messages = c.Messages
                    })
            };
        }

        public User GetUserBy(int id)
        {
            return chatRepository.GetUserBy(id);
        }

        public User GetUserBy(string name)
        {
            return chatRepository.GetUserBy(name);
        }
    }
}
