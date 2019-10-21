using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;

namespace PyUSAC.Analisis
{
    class Sintactico : Grammar
    {

        public bool Analizar(String cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbolAST = parser.Parse(cadena);
            ParseTreeNode raiz = arbolAST.Root;

            generarArbol(raiz);

            if (raiz != null)
            {
                return true;
            }

            return false;
        }

        public static void generarArbol(ParseTreeNode temp)
        {
            String DOT = graficarArbol.textoGraphviz(temp);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\Carpetas\\AST.txt", DOT);

            generarImagen(Directory.GetCurrentDirectory() + "\\Carpetas");

        }

        public static void generarImagen(String path)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("echo off");
            cmd.StandardInput.WriteLine("cd " + path);
            cmd.StandardInput.WriteLine($"dot -Tpng AST.txt -o AST.png");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

    }

}
