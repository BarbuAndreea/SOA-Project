using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MyDent.DataAccess.Abstactions;
using System;

namespace MyDent.DataAccess.Helpers
{
    public class HashingString : IHashingString
    {
        public string HashString(string unhashed)
        {
            byte[] salt = new byte[128 / 8];
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: unhashed,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
    }
}
