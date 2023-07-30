using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Commands
{
    public class SignupCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;        

        public string FirstName { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string MirrorFirstName { get; set; } = string.Empty;

        public string MirrorLastName { get; set; } = string.Empty;

        public string ImageId { get; set; } = string.Empty;

        public Phone Phone { get; set; } = new Phone();

        public Address Address { get; set; } = new Address();
    }
}
