using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

class LenovoRsaLogDecrypt
{

    private static readonly string DLL = "lenovo.mbg.service.common.log.dll";
    private static readonly string OriginalDLL = "C:\\Program Files\\Software Fix\\" + DLL;

    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("エラー：引数が間違っています。\r\n" +
                "使用方法：\r\n" +
                $"    {Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)} [暗号化済みファイル] [出力ファイル名]");
            return;

        }
        else if (!File.Exists(args[0]))
        {
            Console.WriteLine($"エラー：指定された暗号化済みファイルが見つかりませんでした。\r\n" +
                $"ファイル：{args[0]}");
            return;
        }

        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromCustomFolder);

        try
        {
            string encryptSrcFile = args[0];
            string decryptDstFile = args[1];
            Assembly logAssembly = Assembly.Load("lenovo.mbg.service.common.log");

            if (logAssembly == null)
            {
                throw new Exception("エラー: lenovo.mbg.service.common.log.dllがロードできませんでした。");
            }

            Type decryptorType = logAssembly.GetType("lenovo.mbg.service.common.log.LogAesDecrypt");
            object decryptorInstance = Activator.CreateInstance(decryptorType);
            MethodInfo decryptMethod = decryptorType.GetMethod("Decrypt2File");

            decryptMethod.Invoke(decryptorInstance, new object[] { encryptSrcFile, decryptDstFile });
            Console.WriteLine("処理が完了しました。");

        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("エラー: DLLが見つかりませんでした。\r\n" +
                "詳細：\r\n" + e.Message);

        }
        catch (Exception e)
        {
            Console.WriteLine("エラー: 実行に失敗しました。\r\n" +
                "詳細：\r\n" + e.Message);
        }

    }

    private static Assembly LoadFromCustomFolder(object sender, ResolveEventArgs args)
    {
        List<string> searchDirectories = new List<string>
        {
            AppDomain.CurrentDomain.BaseDirectory,
            @"C:\Program Files\Software Fix",
            @"C:\Program Files\Rescue and Smart Assistant",
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
