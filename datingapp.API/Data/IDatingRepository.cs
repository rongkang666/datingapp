using System.Collections.Generic;
using System.Threading.Tasks;
using datingapp.API.Model;

namespace datingapp.API.Data
{
    public interface IDatingRepository
    {
         void add<T>(T entity) where T: class;

         void delete<T>(T entity) where T: class;

         Task<bool> SaveAll();

         Task<IEnumerable<User>> GetUsers();

         Task<User> GetUser(int id);
         

    }
}