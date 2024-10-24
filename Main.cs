using lenovo.mbg.service.common.log;
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


            ILogAesDecrypt? decryptor = decryptorInstance as ILogAesDecrypt;

            string encryptFile = args[0];
            string decryptSaveFile = args[1];
            if (decryptor == null)
            {
                throw new Exception("Decrypt2File メソッドが見つかりませんでした。");
            }
            else
            {
                decryptor.Decrypt2File(encryptFile, decryptSaveFile);
            }

            Console.WriteLine("処理が完了しました。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラーが発生しました: {ex.Message}");
        }
    }

    private static Assembly? LoadFromCustomFolder(object? sender, ResolveEventArgs args)
    {
        List<string> searchDirectories = new List<string>
        {
            @"C:\Program Files\Software Fix",
            AppDomain.CurrentDomain.BaseDirectory,  
        };

        foreach (var directory in searchDirectories)
        {
            string assemblyPath = Path.Combine(directory, new AssemblyName(args.Name).Name + ".dll");

            if (File.Exists(assemblyPath))
            {
                return Assembly.LoadFrom(assemblyPath);
            }
        }

        throw new FileNotFoundException($"DLL '{args.Name}' が見つかりませんでした。");
    }
}
