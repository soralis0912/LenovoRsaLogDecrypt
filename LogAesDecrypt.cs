namespace lenovo.mbg.service.common.log
{
    public interface ILogAesDecrypt
    {
        void Decrypt2File(string encryptFile, string decryptSaveFile);
        public string Decrypt(string cipherText);
    }
}