using BaseCore.Models.Entities;
using IdentityCore.Declarations.Commands;
using System.Text.Json.Serialization;

namespace IdentityCore.Models.Entities
{

    public class AccountActivationRequest : EntityBase
    {
        public string Email { get; set; } = string.Empty;

        public string ActivationKey = generateActivatioKey();

        public ActivationKeyStatus ActivationKeyStatus = ActivationKeyStatus.Activated;


        public static string generateActivatioKey()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 1000000);
            string sixDigitRandomNumber = randomNumber.ToString("000000");

            return sixDigitRandomNumber;

        }

        public static AccountActivationRequest Initialize(SendUserAccountActivationRequestCommand command, AuthenticationContext authenticationContext)
        {
            return new AccountActivationRequest()
            {
                Email = command.Email
            };
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActivationKeyStatus
    {
        Activated = 0,
        Expired = 1,
    }


   
}