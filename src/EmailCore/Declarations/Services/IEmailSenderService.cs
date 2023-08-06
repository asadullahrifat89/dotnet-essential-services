using EmailCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Declarations.Services
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmailAsync(EmailMessage emailMessage, EmailTemplate emailTemplate);
    }
}
