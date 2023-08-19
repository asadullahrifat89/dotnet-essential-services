using Base.Application.DTOs.Responses;
using Email.Application.Commands;
using Email.Domain.Entities;
using Identity.Application.Commands;
using MediatR;

namespace Teams.UserManagement.Application.Commands
{
    public class OnboardUserCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string[] MetaTags { get; set; } = new string[] { };

        public static SubmitUserCommand MapSubmitUserCommand(OnboardUserCommand command)
        {
            return new SubmitUserCommand()
            {
                Email = command.Email,
                MetaTags = command.MetaTags,
            };
        }

        public static SendUserAccountActivationRequestCommand MapSendUserAccountActivationRequestCommand(OnboardUserCommand command)
        {
            return new SendUserAccountActivationRequestCommand()
            {
                Email = command.Email,
            };
        }

        public static EnqueueEmailMessageCommand MapEnqueueEmailMessageCommand(
            OnboardUserCommand command,            
            string activationKey,
            string activationLink,
            EmailTemplate? emailTemplate)
        {
            if (emailTemplate is not null)
            {
                return new EnqueueEmailMessageCommand()
                {
                    To = new[] { new EmailContact() { Email = command.Email, Name = command.Email.Split('@').FirstOrDefault() } },
                    Subject = "Account Activation - SELISE Teams",
                    EmailBodyType = EmailBodyType.Templated,
                    EmailTemplateConfiguration = new EmailTemplateConfiguration()
                    {
                        EmailTemplateId = emailTemplate.Id,
                        TagValues = new Dictionary<string, string>()
                        {
                            { "username", command.Email.Split('@').FirstOrDefault() },
                            { "activationkey", activationKey },
                            { "activationlink", activationLink }
                        }
                    },
                    Category = "Account Activation",
                };
            }
            else
            {
                return new EnqueueEmailMessageCommand()
                {
                    To = new[] { new EmailContact() { Email = command.Email, Name = command.Email.Split('@').FirstOrDefault() } },
                    Subject = "Account Activation - SELISE Teams",
                    EmailBodyType = EmailBodyType.NonTemplated,
                    EmailBody = new EmailBody()
                    {
                        EmailBodyContentType = EmailBodyContentType.Text,
                        Content = $"Hi {command.Email.Split('@').FirstOrDefault()}," + Environment.NewLine +
                        $"Welcome to SELISE! " + Environment.NewLine +
                        $"We have just received your quotation request. " + Environment.NewLine +
                        $"If this was you, you can set a password and activate your account to check the status of your quotation." + Environment.NewLine +
                        $"Here is your account activation code: {activationKey}." + Environment.NewLine +
                        $"Please go to this link to activate your account.\n{activationLink}" + Environment.NewLine +
                        $"If you didn't request this, just ignore and delete this message."
                    },
                    Category = "Account Activation",
                };
            }
        }
    }
}
