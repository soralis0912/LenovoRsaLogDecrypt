using System;
using System.IO;
using System.Reflection;

class LenovoRsaLogDecrypt
{
    static void Main(string[] args)
    {
        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromCustomFolder);

        if (args.Length != 2)
        {
            Console.WriteLine("引数が間違っています");
            return;
        }

        try
        {
            Assembly? logAssembly = Assembly.Load("lenovo.mbg.service.common.log");

            if (logAssembly == null)
            {
                throw new Exception("lenovo.mbg.service.common.log アセンブリがロードできませんでした。");
            }

            Type? decryptorType = logAssembly.GetType("lenovo.mbg.service.common.log.LogAesDecrypt");

            if (decryptorType == null)
            {
                throw new Exception("LogAesDecrypt クラスが見つかりませんでした。");
            }

            object? decryptorInstance = Activator.CreateInstance(decryptorType);

            if (decryptorInstance == null)
            {
                throw new Exception("LogAesDecrypt クラスのインスタンス作成に失敗しました。");
            }

            ILogAesDecrypt? decryptor = decryptorInstance as ILogAesDecrypt;

            if (decryptor == null)
            {
                throw new Exception("インターフェース ILogAesDecrypt へのキャストに失敗しました。");
            }

            string encryptFile = args[0];
            string decryptSaveFile = args[1];

            decryptor.Decrypt2File(encryptFile, decryptSaveFile);

            Console.WriteLine("処理が完了しました。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラーが発生しました: {ex.Message}");
        }
    }

    private static Assembly? LoadFromCustomFolder(object? sender, ResolveEventArgs args)
    {
        string dllFolder = AppDomain.CurrentDomain.BaseDirectory;

        string assemblyPath = Path.Combine(dllFolder, new AssemblyName(args.Name).Name + ".dll");

        if (File.Exists(assemblyPath))
        {
            return Assembly.LoadFrom(assemblyPath);
        }

        throw new FileNotFoundException($"DLL '{assemblyPath}' が見つかりませんでした。");
    }
}
