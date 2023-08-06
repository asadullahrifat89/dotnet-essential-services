﻿using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;

namespace EmailCore.Declarations.Repositories
{
    public interface IEmailMessageRepository
    {
        Task<ServiceResponse> EnqueueEmailMessage(EnqueueEmailMessageCommand command);
    }
}