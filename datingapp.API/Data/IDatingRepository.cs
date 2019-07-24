using System.Collections.Generic;
using System.Threading.Tasks;
using datingapp.API.Helpers;
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

         Task<Photo> GetPhoto(int id);

         Task<Photo> GetMainPhotoForUser(int userId);

         Task<Like> GetLike(int userId, int recipientId);

         Task<IEnumerable<User>> GetLikees(int id);
         Task<IEnumerable<User>> GetLikers(int id);

         Task<Message> GetMessage(int id);
         Task<IEnumerable<Message>> GetMessagesForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
         

    }
}