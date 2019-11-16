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
    class Metodo : Bloques
    {

        String identificador;
        LinkedList<String> parametros;
        ParseTreeNode esqueleto;
        ParseTreeNode parametrosFun;

        Entorno externo;

        public Metodo(string identificador, LinkedList<String> parametros, ParseTreeNode esqueleto)
        {
            this.identificador = identificador;
            this.parametros = parametros;
            this.esqueleto = esqueleto;
        }

        public void setExterno(Entorno ent)
        {
            externo = ent;
        }

        public void setParametrosFun(ParseTreeNode n)
        {
            parametrosFun = n;
        }

        public LinkedList<String> getParametros()
        {
            return parametros;
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
            return Tipo.Simbolo.metodo;
        }

        public Entorno Ejecutar(Entorno ent)
        {
            Entorno entorno = new Entorno(externo);
            Resolve resolve = new Resolve();
            LinkedList<NodoArreglo> l_par = new LinkedList<NodoArreglo>();
            if (parametrosFun != null)
            {
                 l_par= resolve.L_EXP(parametrosFun, ent);
            }

            if (parametros.Count == l_par.Count)
            {

                for (int x = 0; x < parametros.Count; x++)
                {
                    Expresion exp = l_par.ElementAt(x).getValor();
                    Simbolo sim = new Simbolo(Tipo.Simbolo.variable, exp);
                    entorno.add(parametros.ElementAt(x), sim, 0, 0);
                }

                SegundaPasada segunda = new SegundaPasada();
                LinkedList<Instruccion> l_ins;
                l_ins = segunda.second(esqueleto.ChildNodes.ElementAt(1), entorno);

                foreach (Instruccion ins in l_ins)
                {
                    ins.Ejecutar(entorno);
                }
            }

            return entorno;
        }
    }
}
