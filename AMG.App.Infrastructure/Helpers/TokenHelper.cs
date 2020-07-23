using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AMG.App.Infrastructure.Helpers
{
    public class TokenHelper
    {
        public static string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
