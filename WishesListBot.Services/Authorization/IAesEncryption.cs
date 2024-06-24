public interface IAesEncryption
{
    string Decrypt(string cipherText);
    string Encrypt(string plainText);
}