﻿using System;
using Coolector.Common.Events;

namespace Coolector.Services.Users.Shared.Events
{
    public class UserSignedUp : IEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Email { get; }
        public string Name { get; }
        public string PictureUrl { get; }
        public string Role { get; }
        public string State { get; }
        public string Provider { get; }
        public string ExternalUserId { get; }
        public DateTime CreatedAt { get; }

        protected UserSignedUp()
        {
        }

        public UserSignedUp(Guid requestId, string userId, string email, string name,
            string pictureUrl, string role, string state, string provider,
            string externalUserId, DateTime createdAt)
        {
            RequestId = requestId;
            UserId = userId;
            Email = email;
            Name = name;
            PictureUrl = pictureUrl;
            Role = role;
            State = state;
            Provider = provider;
            ExternalUserId = externalUserId;
            CreatedAt = createdAt;
        }
    }
}