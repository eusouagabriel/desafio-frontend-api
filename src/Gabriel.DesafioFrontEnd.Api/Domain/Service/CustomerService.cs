using Gabriel.DesafioFrontEnd.Api.Domain.Data;
using Gabriel.DesafioFrontEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gabriel.DesafioFrontEnd.Api.Domain.Service
{
    public class CustomerService
    {

        public async Task<(int TotalRow, IEnumerable<Customer> Customers)> GetCustomers(string searchCriteria, int pageNumber, int pageSize)
        {
            var totalRow = _dbContext.Customer.Count();

            var customers = new List<Customer>();

            if (!string.IsNullOrWhiteSpace(searchCriteria))
                customers = await _dbContext.Customer
                   .Where(x => x.Name.Contains(searchCriteria) || x.Address.Contains(searchCriteria))
                   .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            else

                customers = await _dbContext.Customer
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return (totalRow, customers);

        }

        public async Task<Customer> GetCustomer(Guid Id)
            => await _dbContext
            .Customer
            .Include(x => x.Cameras)
            .FirstOrDefaultAsync(x => x.Id == Id);

        public async Task UpdateCustomer(Guid id, Customer customerToUdate)
        {
            var customer = await GetCustomer(id);
            customer.Address = customerToUdate.Address;
            customer.Name = customerToUdate.Name;
            customer.IsActive = customerToUdate.IsActive;
            
            await _dbContext.SaveChangesAsync();
        }



        private readonly ApplicationDbContext _dbContext;

        public CustomerService(ApplicationDbContext dbContext)
            => _dbContext = dbContext;
    }
}
