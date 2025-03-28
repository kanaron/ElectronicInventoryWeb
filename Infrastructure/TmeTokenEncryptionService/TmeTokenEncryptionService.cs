using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.TmeTokenEncryptionService;

public class TmeTokenEncryptionService : ITmeTokenEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public TmeTokenEncryptionService(IConfiguration config)
    {
        var keyString = config["EncryptionSettings:Key"];
        var ivString = config["EncryptionSettings:IV"];

        _key = Encoding.UTF8.GetBytes(keyString!);
        _iv = Encoding.UTF8.GetBytes(ivString!);
    }

    public string Encrypt(string plainToken)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainToken);
            sw.Flush();
            cs.FlushFinalBlock();
        }

        return Convert.ToBase64String(ms.ToArray());
    }


    public string Decrypt(string cipherToken)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(cipherToken));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
