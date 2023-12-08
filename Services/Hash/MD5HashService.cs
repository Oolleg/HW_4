namespace HW_4.Services.Hash
{
    public class MD5HashService : IHashService
    {
        public string HexString(string input)
        {
            using var hasher = System.Security.Cryptography.MD5.Create();

            byte[] bytes = hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

            return Convert.ToHexString(bytes);
        }
    }
}
