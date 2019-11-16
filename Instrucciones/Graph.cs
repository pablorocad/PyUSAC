using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyUSAC.Instrucciones
{
    class Graph : Instruccion
    {

        ParseTreeNode name;
        ParseTreeNode grafo;

        public Graph(ParseTreeNode name, ParseTreeNode grafo)
        {
            this.name = name;
            this.grafo = grafo;
        }

        public ParseTreeNode getName()
        {
            return name;
        }

        public ParseTreeNode getGrafo()
        {
            return grafo;
        }

        public Instruccion Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            Expresion extension1 = resolve.resolverExpresion(name, ent);
            Expresion path1 = resolve.resolverExpresion(grafo, ent);


            if (extension1.getTipo().Equals(Tipo.Valor.cadena))
            {
                if (path1.getTipo().Equals(Tipo.Valor.cadena))
                {
                    String extension = extension1.getValor().ToString().Split('.')[1];
                    String path = Directory.GetCurrentDirectory() + "\\Carpetas\\Graph";
                    File.WriteAllText(path + "\\Text.txt", path1.getValor().ToString());

                    Process cmd = new Process();
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();

                    cmd.StandardInput.WriteLine("echo off");
                    cmd.StandardInput.WriteLine("cd " + path);
                    cmd.StandardInput.WriteLine($"dot -T" + extension + " Text.txt -o " + extension1.getValor().ToString());
                    cmd.StandardInput.WriteLine(extension1.getValor().ToString());
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                }
                else
                {
                    SegundaPasada.listaErrores.Add(new Error(Tipo.Error.semantico, "Graph(Cadena,Cadena), Se esperaba una cadena" +
                    " en el segundo parametro", 0, 0));

                    MessageBox.Show("Graph(Cadena,Cadena), Se esperaba una cadena" +
                        " en el segundo parametro");
                }
            }
            else
            {
                SegundaPasada.listaErrores.Add(new Error(Tipo.Error.semantico, "Graph(Cadena,Cadena), Se esperaba una cadena" +
                    " en el primer parametro", 0, 0));

                MessageBox.Show("Graph(Cadena,Cadena), Se esperaba una cadena" +
                    " en el primer parametro");
            }
            return null;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.graph;
        }
    }
}
