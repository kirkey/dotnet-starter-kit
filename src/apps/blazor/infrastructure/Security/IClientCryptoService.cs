namespace FSH.Starter.Blazor.Infrastructure.Security;
public interface IClientCryptoService
{
    string Encrypt(string plain);
    string Decrypt(string cipher);
}

public class ClientCryptoService : IClientCryptoService
{
    // Simple reversible placeholder (NOT secure). Replace with WebCrypto integration in production.
    public string Encrypt(string plain)
    {
        if (string.IsNullOrEmpty(plain)) return plain;
        var bytes = System.Text.Encoding.UTF8.GetBytes(plain);
        Array.Reverse(bytes);
        return Convert.ToBase64String(bytes);
    }
    
    public string Decrypt(string cipher)
    {
        if (string.IsNullOrEmpty(cipher)) return cipher;
        try
        {
            var bytes = Convert.FromBase64String(cipher);
            Array.Reverse(bytes);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        catch { return cipher; }
    }
}

