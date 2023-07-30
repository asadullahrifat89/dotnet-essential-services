using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
