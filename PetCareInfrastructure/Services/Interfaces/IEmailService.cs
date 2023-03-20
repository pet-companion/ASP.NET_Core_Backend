using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareInfrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string emailTo, string Subject, string Body);
    }
}
