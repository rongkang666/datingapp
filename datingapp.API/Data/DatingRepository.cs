using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datingapp.API.Helpers;
using datingapp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace datingapp.API.Data
{

    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;

        }
        public void add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
            
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u =>u.UserId == userId).FirstOrDefaultAsync(p =>p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
           var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

           return photo;
        }

        public async Task<User> GetUser(int id)
        {

            var user = await _context.User.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
            
        }


        public async Task<IEnumerable<User>> GetUsers()
        {
           var users = await _context.User.Include(p=> p.Photos).ToListAsync();

           return users;
        }

        public async Task<IEnumerable<User>> GetLikees(int id) {

           

            var users = await _context.User.Include(p=> p.Photos).ToListAsync();

            var likees = _context.Likes.Where(u => u.LikerId == id).Select(i => i.LikeeId);

            return users.Where(u => likees.Contains(u.Id));

            
        }
         public async Task<IEnumerable<User>> GetLikers(int id)
        {
             

            var users = await _context.User.Include(p=> p.Photos).ToListAsync();

            var likers = _context.Likes.Where(u => u.LikeeId == id).Select(i => i.LikerId);

            return users.Where(u => likers.Contains(u.Id));
            
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Message.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            
            var messages = _context.Message
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos).AsQueryable();

            switch (messageParams.MessageContainer) {
                case "Inbox":
                 messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false);
                 break;
                case "Outbox":
                 messages = messages.Where(m => m.SenderId == messageParams.UserId && m.SenderDeleted == false);
                 break;

                default:
                messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.IsRead == false && m.RecipientDeleted == false);
                break;

            }

            messages = messages.OrderByDescending(m => m.MessageSent);

            return await messages.ToListAsync();


        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Message
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(m => m.RecipientId == userId && m.SenderId == recipientId 
            && m.RecipientDeleted == false
            || m.SenderId == userId && m.RecipientId == recipientId && m.SenderDeleted == false)
            .OrderByDescending(m => m.MessageSent)
            .ToListAsync();

            return messages;

        }
    }
}