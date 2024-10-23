using System.Diagnostics;
using lenovo.mbg.service.common.log;

class LenovoRsaLogDecrypt {
    static void Main(string[] args) {
        if (args.Length != 2) {
            Console.WriteLine("エラー：引数が間違っています。");
            Console.WriteLine("使用方法：");
            Console.WriteLine($"    {Process.GetCurrentProcess().MainModule.FileName} [暗号化済みファイル] [出力ファイル名]");
            return;
        }

        LogAesDecrypt decryptor = new LogAesDecrypt();
        string encryptSrcFile = args[0];
        string decryptDstFile = args[1];

        try {
            decryptor.Decrypt2File(encryptSrcFile, decryptDstFile);
            Console.WriteLine("処理が完了しました。");
        } catch (Exception ex) {
            Console.WriteLine("エラー：実行に失敗しました。");
            Console.WriteLine("エラーログ：");
            Console.WriteLine(ex.Message.ToString());
        }
    }
}
