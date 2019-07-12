using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

       
    }
}