using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Instrucciones;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.CMF
{
    class Funcion : Bloques
    {

        String identificador;
        LinkedList<String> parametros;
        ParseTreeNode esqueleto;
        Expresion retorno;
        LinkedList<NodoArreglo> parametrosFun;

        Entorno externo;

        public Funcion(string identificador, LinkedList<String> parametros, ParseTreeNode esqueleto)
        {
            this.identificador = identificador;
            this.parametros = parametros;
            this.esqueleto = esqueleto;
        }

        public void setExterno(Entorno ent)
        {
            externo = ent;
        }

        public void setParametrosFun(LinkedList<NodoArreglo> n)
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
            return Tipo.Simbolo.funcion;
        }

        public Entorno Ejecutar(Entorno ent)
        {
            Entorno entorno = new Entorno(externo);
            Resolve resolve = new Resolve();
            LinkedList<NodoArreglo> l_par = parametrosFun;

            if (parametros.Count == l_par.Count)
            {
                for (int x = 0; x < parametros.Count; x++)
                {
                    Expresion exp = l_par.ElementAt(x).getValor();
                    Simbolo sim = new Simbolo(Tipo.Simbolo.variable, exp);
                    entorno.add(parametros.ElementAt(x), sim, 0, 0);
                }
            }

            SegundaPasada segunda = new SegundaPasada();
            LinkedList<Instruccion> l_ins;
            l_ins = segunda.second(esqueleto.ChildNodes.ElementAt(1), entorno);

            Instruccion auxIns;
            foreach (Instruccion ins in l_ins)
            {
                auxIns = ins.Ejecutar(entorno);
                if (auxIns.getTipo().Equals(Tipo.Instruccion.Return))
                {
                    Return r = (Return)auxIns;
                    retorno = r.getExp();
                    break;
                }
            }

            return entorno;
        }

        public Expresion getRetorno()
        {
            return retorno;
        }
    }
}
