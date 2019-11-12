using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Instrucciones
{
    class Bloque : Instruccion
    {

        ParseTreeNode instrucciones;

        public Bloque(ParseTreeNode instrucciones)
        {
            this.instrucciones = instrucciones;
        }

        public void Ejecutar(Entorno ent)
        {
            int num = Sintactico.pilaBreak.Count;
            int numC = Sintactico.pilaContinue.Count;

            Entorno nuevo = new Entorno(ent);
            SegundaPasada leer = new SegundaPasada();
            LinkedList<Instruccion> lista;

            lista = leer.second(instrucciones, nuevo);

            foreach(Instruccion ins in lista)
            {
                if (ins.getTipo().Equals(Tipo.Instruccion.Break))
                {
                    Sintactico.pilaBreak.Pop();
                }
                else if (ins.getTipo().Equals(Tipo.Instruccion.Continue))
                {
                    Sintactico.pilaContinue.Pop();
                }
                else
                {
                    ins.Ejecutar(nuevo);

                    if (Sintactico.pilaBreak.Count < num)
                    {
                        break;
                    }
                    else if (Sintactico.pilaContinue.Count < numC)
                    {
                        continue;
                    }
                }
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Bloque;
        }
    }
}
