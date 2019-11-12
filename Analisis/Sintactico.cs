using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System.Windows.Forms;
using PyUSAC.Instrucciones;

namespace PyUSAC.Analisis
{
    class Sintactico : Grammar
    {
        public static List<Error> listaErrores = new List<Error>();
        LinkedList<Instruccion> listaIns;
        public static LinkedList<String> listaImp;
        Resolve resolve = new Resolve();
        public static Stack<Instruccion> pilaBreak = new Stack<Instruccion>();

        public Sintactico()
        {
            listaIns = new LinkedList<Instruccion>();
            listaImp = new LinkedList<String>();
        }

        public LinkedList<Instruccion> getIns()
        {
            return listaIns;
        }

        public bool Analizar(String cadena, int n)
        {
            listaErrores = new List<Error>();
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);

            //foreach (var item in lenguaje.Errors)
            //{
            //    Console.WriteLine(item);
            //    //Console.WriteLine(item.State + " " + item.GetType().ToString());
            //}

            Parser parser = new Parser(lenguaje);
            ParseTree arbolAST = parser.Parse(cadena);
            ParseTreeNode raiz = arbolAST.Root;

            if (n == 0)
            {
                generarArbol(raiz);
            }
            else if(n == 8200)
            {
                try
                {
                    ejecutarArbol(raiz);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (raiz != null)
            {
                return true;
            }

            foreach (var item in arbolAST.ParserMessages)
            {
                listaErrores.Add(new Error(Tipo.Error.sintactico, item.Message, item.Location.Line, item.Location.Column));
            }

            return false;
        }

        public static void generarArbol(ParseTreeNode temp)
        {
            try
            {
                String DOT = graficarArbol.textoGraphviz(temp);
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Carpetas\\Arbol\\AST.txt", DOT);

                generarImagen(Directory.GetCurrentDirectory() + "\\Carpetas\\Arbol");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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


        public void ejecutarArbol(ParseTreeNode temp)
        {
            Entorno global = new Entorno(null);

            PrimeraPasada primera = new PrimeraPasada();
            primera.first(temp, global);

            SegundaPasada segunda = new SegundaPasada();
            listaIns = segunda.second(temp, global);

            foreach (Instruccion lista in listaIns)
            {
                lista.Ejecutar(global);
            }

        }

        

    }

}
