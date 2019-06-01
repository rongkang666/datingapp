using System;
using System.Threading.Tasks;
using datingapp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace datingapp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            
            var user = await _context.User.FirstOrDefaultAsync(x => x.Username == username);

            if(user == null)
            return null;

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
              using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
              
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i=0; i<computedHash.Length;i++){
                    if(computedHash[i] != passwordHash[i])
                    return false;
                }

            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            
            byte[] passwordHash, passwordSalt;

            createPasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;


        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()){

                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.User.AnyAsync(x =>x.Username == username))
                return true;

            return false;
        }
    }
}