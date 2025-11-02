using System.Security.Cryptography;
using System.Text;

namespace Portal.Usuario.Core.Utils
{
    public static class HashHelper
    {
        public static string ToSha256(string input)
        {
            // Convertemos a string para bytes
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convertendo para string hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2")); // x2 => formato hex com 2 dígitos
                }
                return builder.ToString();
            }
        }
    }
}