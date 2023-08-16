using BaseModule.Domain.Entities;
using System.Text.Json.Serialization;

namespace IdentityModule.Domain.Entities
{
    public class AccountActivationRequest : EntityBase
    {
        public string Email { get; set; } = string.Empty;

        public string ActivationKey { get; set; } = string.Empty;

        public ActivationKeyStatus ActivationKeyStatus = ActivationKeyStatus.Active;

        public static int GenerateRandomNumber()
        {
            var rand = new Random();
            int min = 100000; // Minimum 6-digit number
            int max = 999999; // Maximum 6-digit number
            int randomNumber = rand.Next(min, max + 1);

            while (ContainsZero(randomNumber))
            {
                randomNumber = rand.Next(min, max + 1);
            }

            return randomNumber;
        }

        public static bool ContainsZero(int number)
        {
            while (number > 0)
            {
                if (number % 10 == 0)
                {
                    return true;
                }
                number /= 10;
            }

            return false;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActivationKeyStatus
    {
        Active = 0,
        Expired = 1,
    }
}