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

        public Instruccion Ejecutar(Entorno ent)
        {
            Entorno nuevo = new Entorno(ent);
            SegundaPasada leer = new SegundaPasada();
            LinkedList<Instruccion> lista;

            lista = leer.second(instrucciones, nuevo);
            Instruccion aux;
            foreach (Instruccion ins in lista)
            {

                aux = ins.Ejecutar(nuevo);

                if (aux != null)
                {
                    return aux;
                }
            }

            return null;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Bloque;
        }
    }
}
