using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Helpers
{
    public static class StaticFunctionHelper
    {
        public static string GnenrateRandomNumber()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }
}
