using System;
using CredentialManagement;

namespace eHoursModernUI
{
    public class CredentialManager
    {
        public static (string Username, string Password) ReadCredential(string target)
        {
            var cred = new Credential { Target = target };
            if (cred.Load())
            {
                return (cred.Username, cred.Password);
            }
            else
            {
                throw new Exception($"No credential found for target: {target}");
            }
        }

        public static void WriteCredential(string target, string username, string password)
        {
            var cred = new Credential
            {
                Target = target,
                Username = username,
                Password = password,
                PersistanceType = PersistanceType.LocalComputer
            };
            cred.Save();
        }
    }
}
