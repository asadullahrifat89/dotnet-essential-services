using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;
using EmailCore.Models.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Declarations.Repositories
{
    public interface IEmailRepository
    {
        Task<ServiceResponse> CreateTemplate(CreateTemplateCommand command);

        Task<bool> BeAnExistingEmailTemplate(string templateName);
    }
}
