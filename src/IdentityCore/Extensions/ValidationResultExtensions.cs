using FluentValidation.Results;

namespace IdentityCore.Extensions
{
    public static class ValidationResultExtensions
    {
        public static bool EnsureValidResult(this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join(Environment.NewLine, validationResult.Errors.Select(x => x.ErrorMessage)));
            }
            else
            {
                return true;
            }
        }
    }
}
