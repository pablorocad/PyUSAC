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
        Entorno nuevo;

        public Bloque(ParseTreeNode instrucciones, Entorno ent)
        {
            this.instrucciones = instrucciones;
            this.nuevo = ent;
        }

        public void Ejecutar(Entorno ent)
        {
            SegundaPasada leer = new SegundaPasada();
            LinkedList<Instruccion> lista;

            lista = leer.second(instrucciones, nuevo);

            foreach(Instruccion ins in lista)
            {
                ins.Ejecutar(nuevo);
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Bloque;
        }
    }
}
