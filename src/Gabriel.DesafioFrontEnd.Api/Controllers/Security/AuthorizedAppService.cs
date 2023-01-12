namespace Gabriel.DesafioFrontEnd.Api.Controllers.Security
{
    public static class ClientApps
    {
        public static (string Id, string Secret) FrontEndChallenger = new("01878fb6-4206-40e0-b195-46f465a3a65b", "38c8a848d23e34f281b6dfcd35b11d79f0dbdbd0372269b38f46b6a5e9fe6006");
    
    }

    public class AuthorizedAppService
    {
        public virtual AuthorizedApp GetAppBy(Guid Id, string clientSecret) 
            => _authorizedApps.SingleOrDefault(x => x.Id == Id && x.ClientSecret == clientSecret);

        public AuthorizedApp GetAppById(Guid id) 
            => _authorizedApps.SingleOrDefault(x => x.Id == id);

        protected ICollection<AuthorizedApp> _authorizedApps;

        public AuthorizedAppService() 
            => _authorizedApps = new[]
            {
                new AuthorizedApp
                {
                    Name = "front end chalenger app",
                    Id = new Guid(ClientApps.FrontEndChallenger.Id),
                    ClientSecret = ClientApps.FrontEndChallenger.Secret,
                    Scopes = new[] { ScopeDefinition.CUSTOMER_READ }
                }
            };
    }


    public class AuthorizedApp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }
    }

    public static class ScopeDefinition
    {
        public const string CUSTOMER_READ = "customer.read";
       
    }
}
