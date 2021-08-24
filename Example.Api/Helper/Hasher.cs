using System;
using System.Security.Cryptography;
using System.Text;

namespace Example.Api.Helper
{
    public static class Hasher
    {
        private static SHA256 defaultHashAlgorithm = SHA256.Create();
        private static int saltLengthLimit = 10;
        private static RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
        /// <summary>
        /// Merge password and salt bytes and hash newly created byte array with hash algorith supplied
        /// </summary>
        /// <param name="password">Plain password</param>
        /// <param name="salt">Salt</param>
        /// <param name="hashAlgorithm">Cryptographic hash algorithms to be used for hashing</param>
        /// <returns></returns>
        public static string ComputeHash(string password, string salt, HashAlgorithm hashAlgorithm)
        {
            string passwordWithSalt = password + salt;
            byte[] passwordWithSaltBytes = Encoding.UTF8.GetBytes(passwordWithSalt);

            byte[] digestBytes = hashAlgorithm.ComputeHash(passwordWithSaltBytes);

            return BitConverter.ToString(digestBytes).Replace("-", string.Empty).ToUpperInvariant();
        }
        public static string ComputeHash(string password, string salt)
        {
            string passwordWithSalt = password + salt;
            byte[] passwordWithSaltBytes = Encoding.UTF8.GetBytes(passwordWithSalt);

            byte[] digestBytes = defaultHashAlgorithm.ComputeHash(passwordWithSaltBytes);

            return BitConverter.ToString(digestBytes).Replace("-", string.Empty).ToUpperInvariant();
        }

        public static string GetSalt()
        {
            return GetSalt(saltLengthLimit);
        }
        public static string GetSalt(int maximumSaltLength)
        {
            string salt = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpperInvariant();
            return salt.Length > maximumSaltLength ? salt.Substring(0, maximumSaltLength) : salt;
        }
    }
}
