namespace Infrastructure.TmeTokenEncryptionService
{
    public interface ITmeTokenEncryptionService
    {
        string Decrypt(string cipherToken);
        string Encrypt(string plainToken);
    }
}