using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace net6_template_devstandard_api.Services
{
    public interface IAdService
    {
        bool ValidateCredentials(string empId, string password);
    }
    public class AdService : IAdService
    {
        private readonly IConfiguration configuration;
        public AdService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool ValidateCredentials(string empId, string password)
        {
            var ldapDomain = this.configuration.GetSection("LdapDomain").Value;
            using (var adContext = new PrincipalContext(ContextType.Domain, ldapDomain))
            {
                return adContext.ValidateCredentials(empId, password);
            }
        }
    }
}
