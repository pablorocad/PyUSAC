using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.CMF
{
    class Clase : Bloques
    {
        String identificador;
        ParseTreeNode esqueleto;

        public Clase(string identificador, ParseTreeNode esqueleto)
        {
            this.identificador = identificador;
            this.esqueleto = esqueleto;
        }

        public Entorno Ejecutar(Entorno ent)
        {
            Entorno entorno = new Entorno(ent);

            PrimeraPasada primera = new PrimeraPasada();
            primera.first(esqueleto.ChildNodes.ElementAt(1), entorno);

            SegundaPasada segunda = new SegundaPasada();
            LinkedList<Instruccion> l_ins = segunda.second(esqueleto.ChildNodes.ElementAt(1), entorno);

            foreach (Instruccion ins in l_ins)
            {
                ins.Ejecutar(entorno);
            }

            return entorno;
        }

        public ParseTreeNode getEsqueleto()
        {
            return esqueleto;
        }

        public string getName()
        {
            return identificador;
        }

        public Tipo.Simbolo getTipo()
        {
            return Tipo.Simbolo.clase;
        }
    }
}
