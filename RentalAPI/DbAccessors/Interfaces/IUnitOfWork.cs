using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IUnitOfWork
    {
         Task SaveChangesAsync();
    }
}
