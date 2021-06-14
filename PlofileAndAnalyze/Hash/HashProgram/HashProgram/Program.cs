using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace HashProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] salt = GetSalt(true);

            string passwordText = "passWord123  _";
            Console.WriteLine("passwordText: " + passwordText);
            var sw = new Stopwatch();

            sw.Start();
            var results = GeneratePasswordHashUsingSalt(passwordText, salt);
            sw.Stop();
            Console.WriteLine($"{nameof(GeneratePasswordHashUsingSalt)}: {sw.ElapsedMilliseconds}");
            sw.Reset();

            sw.Start();
            var optimizedResult = GeneratePasswordHashUsingSaltOptimized(passwordText, salt);
            sw.Stop();
            Console.WriteLine($"{nameof(GeneratePasswordHashUsingSaltOptimized)}: {sw.ElapsedMilliseconds}");

            Console.ReadLine();
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {

            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }


        public static string GeneratePasswordHashUsingSaltOptimized(string passwordText, byte[] salt)
        {

            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytesOptimized(new UTF8Encoding(false).GetBytes(passwordText), salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }

        private static byte[] GetSalt(bool useConst)
        {
            var saltConst = new byte[]
            {

                209,
                144,
                75,
                248,
                126,
                248,
                253,
                47,
                146,
                65,
                6,
                207,
                242,
                157,
                206,
                185,
            };

            if (useConst)
            {
                return saltConst;
            }
            var saltGenerated = new byte[16];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltGenerated);
            }

            return saltGenerated;

        }
    }
}
