using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Instrucciones
{
    class Graph : Instruccion
    {

        String name;
        String grafo;

        public Graph(string name, string grafo)
        {
            this.name = name;
            this.grafo = grafo;
        }

        public String getName()
        {
            return name;
        }

        public String getGrafo()
        {
            return grafo;
        }

        public void Ejecutar(Entorno ent)
        {
            String extension = name.Split('.')[1];
            String path = Directory.GetCurrentDirectory() + "\\Carpetas\\Graph";
            File.WriteAllText(path + "\\Text.txt", grafo);

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("echo off");
            cmd.StandardInput.WriteLine("cd " + path);
            cmd.StandardInput.WriteLine($"dot -T" + extension + " Text.txt -o " + name);
            cmd.StandardInput.WriteLine(name);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.graph;
        }
    }
}
