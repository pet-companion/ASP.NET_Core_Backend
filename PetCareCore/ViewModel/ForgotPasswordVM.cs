using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.ViewModel
{
    public class ForgotPasswordVM
    {
        public string UserEmail { get; set; }
        public string VerifiedCode { get; set; }
    }
}
