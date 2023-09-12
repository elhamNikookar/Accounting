using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Repositories;
using Accounting.ViewModels;

namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {

        private Accounting_DBEntities db;

        public CustomerRepository(Accounting_DBEntities context)
        {
            db = context;
        }
        public bool DeleteCustomer(Customer customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomerById(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Customer> GetAllCustomer()
        {
            return db.Customers.ToList();
        }

        public Customer GetCustomerById(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public int GetCustomerIdByName(string customerName)
        {
            return db.Customers.First(c => c.FullName == customerName).CustomerID;
        }

        public string GetCustomerNameById(int customerId)
        {
            return db.Customers.Find(customerId).FullName;
        }

        public List<ListCustomerViewModel> GetCustomerNames(string filter = "")
        {
            if (filter == "")
                return db.Customers.Select(c => new ListCustomerViewModel()
                {
                    CustomerID = c.CustomerID,
                    FullName = c.FullName,
                }).ToList();
            return db.Customers.Where(c => c.FullName.ToLower().Contains(filter)).Select(c => new ListCustomerViewModel()
            {
                CustomerID = c.CustomerID,
                FullName = c.FullName,
            }).ToList();
        }

        public IEnumerable<Customer> GetCustomersByFilter(string parameter)
        {
            return db.Customers.Where(
                n => n.FullName.Contains(parameter) || n.Mobile.Contains(parameter) ||
                n.Email.Contains(parameter)).ToList();
        }

        public bool InsertCustomer(Customer customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
