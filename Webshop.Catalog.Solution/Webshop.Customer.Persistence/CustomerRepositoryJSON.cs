using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Data.Persistence;

namespace Webshop.Customer.Persistence
{
    /// <summary>
    /// The purpose of this class is to save the customer to a JSON file that can be comitted to github
    /// </summary>
    public class CustomerRepositoryJSON : BaseRepository, ICustomerRepository
    {
        private readonly string foldername;
        private readonly string filename;
        private List<Domain.AggregateRoots.Customer> customers;
        public CustomerRepositoryJSON(DataContext dataContext) : base("customers.json", dataContext)
        {
            foldername = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database");
            filename = Path.Combine(foldername, this.TableName);
            EnsureFolder();
            this.customers = new List<Domain.AggregateRoots.Customer>();
            LoadCustomersFromFile();
        }

        public async Task CreateAsync(Domain.AggregateRoots.Customer entity)
        {
            entity.Id = CreateNextId();
            this.customers.Add(entity);
            await Task.Run(()=> this.SaveCustomersToFile());
        }

        public async Task DeleteAsync(int id)
        {
            var existingCustomer = this.customers.FirstOrDefault(x=>x.Id == id);
            if(existingCustomer != null)
            {
                this.customers.Remove(existingCustomer);
            }
            await Task.Run(()=> SaveCustomersToFile());
        }

        public async Task<IEnumerable<Domain.AggregateRoots.Customer>> GetAll()
        {
            return await Task.Run(()=>this.customers);
        }

        public async Task<Domain.AggregateRoots.Customer> GetById(int id)
        {
            return await Task.Run(()=>this.customers.FirstOrDefault(x=> x.Id == id));
        }

        public async Task UpdateAsync(Domain.AggregateRoots.Customer entity)
        {
            //updating is the same as removing old and adding new
            await DeleteAsync(entity.Id);
            //do not call create, it creates a new id, just add it to the list
            this.customers.Add(entity);            
            await Task.Run(()=>SaveCustomersToFile());
        }

        #region Load and Save from JSON file
        private void LoadCustomersFromFile()
        {
            if (File.Exists(this.filename))
            {
                string data = System.IO.File.ReadAllText(this.filename);
                this.customers = JsonConvert.DeserializeObject<List<Domain.AggregateRoots.Customer>>(data);
            }
        }

        private void SaveCustomersToFile()
        {
            string data = JsonConvert.SerializeObject(this.customers, Formatting.Indented);
            System.IO.File.WriteAllText(this.filename, data);
        }

        private void EnsureFolder()
        {            
            DirectoryInfo dir = Directory.CreateDirectory(this.foldername);
            if(!dir.Exists)
            {
                dir.Create();
            }
        }

        private int CreateNextId()
        {
            LoadCustomersFromFile();
            if (this.customers.Count > 0)
            {
                return this.customers.Max(x => x.Id) + 1;
            }
            else
            {
                return 1;
            }
        }
        #endregion
    }
}
