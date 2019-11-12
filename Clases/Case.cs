using Irony.Parsing;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Case
    {

        ParseTreeNode condicion;
        Instruccion bloque;
        Boolean rbreak;

        public Case(ParseTreeNode condicion, Instruccion bloque)
        {
            this.condicion = condicion;
            this.bloque = bloque;
            rbreak = false;
        }

        public void setBreak(Boolean b)
        {
            rbreak = b;
        }

        public Boolean getBreak()
        {
            return rbreak;
        }

        public ParseTreeNode getCondicion()
        {
            return condicion;
        }

        public Instruccion getBloque()
        {
            return bloque;
        }
    }
}
