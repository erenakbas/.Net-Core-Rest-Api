using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Data
{
    public class CustomersRepository : ICustomersRepository
    {
        private CustomerContext _Context;
        private ILogger _Logger;

        public CustomersRepository(CustomerContext Context, ILoggerFactory loggerFactory)
        {
            _Context = Context;
            _Logger = loggerFactory.CreateLogger("StatesRepository");
        }

        public async Task<bool> DeleteCustomersAsync(int id)
        {
            var customer = await _Context.Customers
                .Include(o => o.Orders)
                .SingleOrDefaultAsync(c => c.Id == id);
            _Context.Remove(customer);
            try
            {
                return (await _Context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error in {nameof(DeleteCustomersAsync)}:" + ex.Message);
            }
            return false;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _Context.Customers
                .OrderBy(o => o.LastName)
                .Include(s => s.States)
                .Include(o => o.Orders)
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _Context.Customers
                .Include(s => s.States)
                .Include(o => o.Orders)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> InsertCustomersAsync(Customer customer)
        {
            _Context.Add(customer);
            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _Logger.LogError($"Error in {nameof(InsertCustomersAsync)}:" + ex.Message);
            }
            return customer;
        }

        public async Task<bool> UpdateCustomersAsync(Customer customer)
        {
            _Context.Customers.Attach(customer);
            _Context.Entry(customer).State = EntityState.Modified;
            try
            {
                return(await _Context.SaveChangesAsync()>0 ? true : false);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Error in {nameof(UpdateCustomersAsync)}:" + ex.Message);
            }
            return false;
        }
    }
}
