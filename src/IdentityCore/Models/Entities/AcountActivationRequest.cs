using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Models.Entities
{
    public class AcountActivationRequest
    {
        public string Email = string.Empty;
        public string ActivationKey = string.Empty;
        public ActivationStatus ActivationStatus = ActivationStatus.Activated;

    }

    public enum ActivationStatus
    {
        Activated ,
        Expired 
    }

}