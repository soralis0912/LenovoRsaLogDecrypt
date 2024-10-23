using lenovo.mbg.service.common.log;

class LenovoRsaLogDecrypt
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("引数が間違っています");
            return;
        }
        LogAesDecrypt decryptor = new LogAesDecrypt();
        string encryptFile = args[0];

        string decryptSaveFile = args[1];
        try
        {
            decryptor.Decrypt2File(encryptFile, decryptSaveFile);
            Console.WriteLine("処理が完了しました。");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
    }
}