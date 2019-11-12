using Irony.Parsing;
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
        LinkedList<Declaracion> parametros;
        ParseTreeNode esqueleto;

        public Metodo(string identificador, LinkedList<Declaracion> parametros, ParseTreeNode esqueleto)
        {
            this.identificador = identificador;
            this.parametros = parametros;
            this.esqueleto = esqueleto;
        }

        public LinkedList<Declaracion> getParametros()
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
    }
}
