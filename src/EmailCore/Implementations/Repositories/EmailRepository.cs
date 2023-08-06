using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Implementations.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        public Task<ServiceResponse> CreateUser(CreateTemplateCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var user = User.Initialize(command, authCtx);

            var userRoleMaps = new List<UserRoleMap>();

            // if roles were sent map user to role
            if (command.Roles != null && command.Roles.Any())
            {
                var roles = await _roleRepository.GetRolesByNames(command.Roles);

                foreach (var role in roles)
                {
                    var roleMap = new UserRoleMap()
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                    };

                    userRoleMaps.Add(roleMap);
                }
            }

            await _mongoDbService.InsertDocument(user);

            if (userRoleMaps.Any())
                await _mongoDbService.InsertDocuments(userRoleMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(user, authCtx?.RequestUri);

        }
    }
}
