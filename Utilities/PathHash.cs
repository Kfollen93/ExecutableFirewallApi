using System.Security.Cryptography;
using System.Text;

namespace ExecutableFirewallApi.Utilities
{
    public static class PathHash
    {
        public static string GetShortPathHash(string path)
        {
            string normalizedPath = path.ToUpperInvariant();

            byte[] inputBytes = Encoding.UTF8.GetBytes(normalizedPath);
            byte[] hashBytes = SHA256.HashData(inputBytes);

            string hashStr = Convert.ToHexString(hashBytes);

            return hashStr[..8];
        }
    }
}
