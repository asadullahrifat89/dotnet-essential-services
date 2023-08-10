using BaseCore.Extensions;
using BaseCore.Models.Entities;
using IdentityCore.Declarations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IdentityCore.Models.Entities
{
    public class AccountActivationRequest
    {
        public string Email = string.Empty;

        public string ActivationKey = generateActivatioKey();

        public ActivationStatus ActivationKeyStatus = ActivationStatus.Activated;


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
    public enum ActivationStatus
    {
        Activated = 0,
        Expired = 1,
    }


   
}