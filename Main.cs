using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

class LenovoRsaLogDecrypt {
    static void Main(string[] args) {
        if (args.Length != 2) {
            Console.WriteLine("エラー：引数が間違っています。\r\n" +
                "使用方法：\r\n" +
                $"    {Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)} [暗号化済みファイル] [出力ファイル名]");
            return;

        } else if (!File.Exists(args[0])) {
            Console.WriteLine($"エラー：指定された暗号化済みファイルが見つかりませんでした。\r\n" +
                $"ファイル：{args[0]}");
            return;
        }

        try {
            string encryptSrcFile = args[0];
            string decryptDstFile = args[1];
            Assembly logAssembly = Assembly.LoadFrom("lenovo.mbg.service.common.log.dll");
            Type decryptorType = logAssembly.GetType("lenovo.mbg.service.common.log.LogAesDecrypt");
            object decryptorInstance = Activator.CreateInstance(decryptorType);
            MethodInfo decryptMethod = decryptorType.GetMethod("Decrypt2File");

            decryptMethod.Invoke(decryptorInstance, new object[] { encryptSrcFile, decryptDstFile });
            Console.WriteLine("処理が完了しました。");
            //Process.Start(decryptDstFile);

        } catch (FileNotFoundException e) {
            Console.WriteLine("エラー：DLLが見つかりませんでした。\r\n" +
                "詳細：\r\n" + e.Message);

        } catch (Exception e) {
            Console.WriteLine("エラー：実行に失敗しました。\r\n" +
                "詳細：\r\n" + e.Message);
        }

    }
}
