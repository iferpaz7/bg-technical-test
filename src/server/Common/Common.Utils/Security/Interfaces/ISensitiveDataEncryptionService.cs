namespace Common.Utils.Security.Interfaces;

public interface ISensitiveDataEncryptionService
{
    Task<byte[]> EncryptToBytesAsync(string passphrase, string data);
    Task<string> DecryptFromBytesAsync(string passphrase, byte[] encryptedData);
}