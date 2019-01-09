using System;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Diagnostics;

namespace CSScript
{
    class Program
    {
        private static readonly string LOGO = "   _____      _____ _                         _____           _       _   \n  / ____|    / ____| |                       / ____|         (_)     | |  \n | |   _____| (___ | |__   __ _ _ __ _ __   | (___   ___ _ __ _ _ __ | |_ \n | |  |______\\___ \\| '_ \\ / _` | '__| '_ \\   \\___ \\ / __| '__| | '_ \\| __|\n | |____     ____) | | | | (_| | |  | |_) |  ____) | (__| |  | | |_) | |_ \n  \\_____|   |_____/|_| |_|\\__,_|_|  | .__/  |_____/ \\___|_|  |_| .__/ \\__|\n                                    | |                        | |        \n                                    |_|                        |_|        \n";
        private static readonly string TEMPLATE_A = "namespace CSharp_Script_Namespace { class CSharp_Script_Class { static void Main(string[] args){";
        private static readonly string TEMPLATE_B = "}}}";
        private static readonly string[] REFERENCES = { "" };

        static void Main(string[] args)
        {
            try
            {
                throw new Exception();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(LOGO);

                if(args.Length == 0)
                {
                    Fatal("No input file!");
                    return;
                }
                else
                {
                    if(!File.Exists(args[0]))
                    {
                        Fatal("No input file!");
                        return;
                    }
                }

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                CompilerResults result = Compile(TEMPLATE_A + File.ReadAllText(args[0]) + TEMPLATE_B, Path.GetFileNameWithoutExtension(args[0]) + ".exe", REFERENCES);

                if(result.Errors.HasErrors)
                {
                    foreach(CompilerError error in result.Errors)
                    {
                        Error($"Compile Error : {error.ErrorText} #{error.Line}");
                    }

                    Exit();
                    return;
                }
                else
                {
                    stopwatch.Stop();
                    Console.WriteLine($"[i] Compile finished! {stopwatch.ElapsedMilliseconds}ms");
                    Exit();
                    return;
                }
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.CursorVisible = false;
                Console.Clear();

                Console.WriteLine(LOGO);
                Console.WriteLine(" C# Script Exception\n Press any key to continue...\n");
                Console.WriteLine(' ' + e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
            }
        }

        private static CompilerResults Compile(string source, string outputFile, params string[] references)
        {
            CompilerParameters parameters = new CompilerParameters(references, outputFile);
            parameters.GenerateExecutable = true;

            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                return provider.CompileAssemblyFromSource(parameters, source);
            }
        }

        private static void Fatal(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[f] " + message);
            Console.ForegroundColor = ConsoleColor.White;

            Exit();
        }

        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[e] " + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Exit()
        {
            Console.CursorVisible = false;
            Console.WriteLine("[i] Press any key to continue...");
            Console.ReadKey();
        }
    }
}
