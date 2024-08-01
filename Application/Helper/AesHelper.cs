using System.Security.Cryptography;
using System.Text;

namespace Application.Helper;
public static class AesHelper
{
    private static byte[]? _key;
    private static byte[]? _initializationVector;

    public static void Initialize(string key, string initializationVector)
    {
        _key = Encoding.UTF8.GetBytes(key);
        _initializationVector = Encoding.UTF8.GetBytes(initializationVector);
    }

    public static byte[] Encrypt(byte[] data)
    {
        if (_key is null || _initializationVector is null)
        {
            throw new ArgumentNullException("Key or InitializationVector cannot be null");
        }

        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = _key;
        var encryptedData = aes.EncryptCbc(data, _initializationVector)
            ?? throw new InvalidDataException($"Invalid attmept for encrypt {Encoding.UTF8.GetString(data)}");

        return encryptedData;
    }

    public static byte[] Decrypt(byte[] data)
    {
        if (_key is null || _initializationVector is null)
        {
            throw new ArgumentNullException("Key or InitializationVector cannot be null");
        }

        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = _key;
        var decryptedData = aes.DecryptCbc(data, _initializationVector)
            ?? throw new InvalidDataException($"Invalid attmept for decrypt {Encoding.UTF8.GetString(data)}");

        return decryptedData;
    }
}