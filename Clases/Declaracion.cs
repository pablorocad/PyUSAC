using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Declaracion
    {
        Tipo.Simbolo tipo;
        String identificador;
        ParseTreeNode exp = null;
        ParseTreeNode dimensiones = null;
        Boolean h;

        public Declaracion(string identificador, ParseTreeNode exp)
        {
            this.identificador = identificador;
            this.exp = exp;
            this.tipo = Tipo.Simbolo.variable;
        }

        public Declaracion(string identificador, ParseTreeNode dimensiones, ParseTreeNode valores)
        {
            this.identificador = identificador;
            this.dimensiones = dimensiones;
            exp = valores;
            this.tipo = Tipo.Simbolo.arreglo;
        }

        public String getIdentificador()
        {
            return identificador;
        }

        public ParseTreeNode getExpresion()
        {
            return exp;
        }


        public ParseTreeNode getArbol()
        {
            return dimensiones;
        }

        public void setExpresion(ParseTreeNode exp)
        {
            this.exp = exp;
        }

        public Tipo.Simbolo getTipo()
        {
            return tipo;
        }
    }
}
