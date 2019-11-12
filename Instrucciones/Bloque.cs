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
            Entorno nuevo = new Entorno(ent);
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
