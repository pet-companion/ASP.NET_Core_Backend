using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace PetCareInfrastructure.Helpers
{
    public static class StaticFunctionHelper
    {
        public static string GnenrateRandomNumber()
        {
            Random random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }

        public static bool IsValidEmail(string email)
        {
            //Regex pattern for email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public static bool IsStrongPassword(string password)
        {
            // Regex pattern for password validation
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }
    }
}
