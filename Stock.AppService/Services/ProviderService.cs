using Stock.AppService.Base;
using Stock.Model.Entities;
using Stock.Repository.LiteDb.Interface;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Stock.AppService.Services
{
    public class ProviderService : BaseService<Provider>
    {
        public ProviderService(IRepository<Provider> repository) 
        : base(repository)
        {

        }

        public new Provider Create(Provider newProvider)
        {
            var isEmailUsed = GetByEmail(newProvider.Email).Any();

            if (isEmailUsed)
                throw new ArgumentException("Email already in use");
                
            return this.Repository.Add(newProvider);            
        }

        public new Provider Update(Provider updatedProvider)
        {
            var isEmailAvailable= !GetByEmail(updatedProvider.Email).Any() ||
                                   GetByEmail(updatedProvider.Email).FirstOrDefault().Id == updatedProvider.Id;

            if (!isEmailAvailable)
                throw new ArgumentException("Email already in use");
                
            this.Repository.Update(updatedProvider);       
            return updatedProvider;     
        }

        private IEnumerable<Provider> GetByEmail(string email)
        {
            return this.Repository.List().Where(Provider => Provider.Email == email);
        }



    }
}