using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class UserService: BaseService<User, IUserRepository>, IUserService
    {
        public UserService(IUserRepository repository, IUnitOfWork unitOfWork)
          : base(repository, unitOfWork)
        {
        }

        public async Task<User> FindByUserNameAndPasswordAsync(string userName, string password)
           => await _repository.FindByUserNameAndPasswordAsync(userName, password); 
    }
}
