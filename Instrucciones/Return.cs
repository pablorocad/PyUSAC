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
    class Return : Instruccion
    {

        ParseTreeNode retorno;
        Expresion exp;

        public Return(ParseTreeNode retorno)
        {
            this.retorno = retorno;
        }

        public Expresion getExp()
        {
            return exp;
        }

        public Instruccion Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            exp = resolve.resolverExpresion(retorno, ent);
            return this;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Return;
        }
    }
}
