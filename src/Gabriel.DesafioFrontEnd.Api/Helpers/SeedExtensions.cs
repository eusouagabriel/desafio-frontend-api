using Gabriel.DesafioFrontEnd.Api.Domain.Data;

namespace Gabriel.DesafioFrontEnd.Api.Helpers
{
    public static class SeedExtensions
    {
        public static void UseSeedCustomers(this WebApplication self, IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                ApplicationDbSeed.SeedAsync(dbContext);
            }
        } 
    }
}
