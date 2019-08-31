using CustomerManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Data
{
    public interface ICustomersRepository
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<Customer> InsertCustomersAsync(Customer customer);
        Task<bool> UpdateCustomersAsync(Customer customer);
        Task<bool> DeleteCustomersAsync(int id);

    }
}
