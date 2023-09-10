using Accounting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Repositories
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomer();
        Customer GetCustomerById(int CustomerId);

        List<ListCustomerViewModel> GetCustomerNames(string filter = "");
        bool InsertCustomer(Customer Customer);
        bool UpdateCustomer(Customer Customer);
        bool DeleteCustomer(Customer customer);
        bool DeleteCustomer(int CustomerId);
        IEnumerable<Customer> GetCustomersByFilter(string parameter);
        int GetCustomerIdByName(string customerName);

        string GetCustomerNameById(int customerId);
    }
}
