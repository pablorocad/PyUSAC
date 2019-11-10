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
    class IF : Instruccion
    {
        ParseTreeNode cond;
        Instruccion bloque, relse;

        public IF(ParseTreeNode condicion, Instruccion bloque, Instruccion relse)
        {
            this.cond = condicion;
            this.bloque = bloque;
            this.relse = relse;
        }

        public void Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            Expresion condicion = resolve.resolverExpresion(cond, ent);
            if (!condicion.getTipo().Equals(Tipo.Valor.rnull))
            {
                if (condicion.getValor().ToString().ToLower().Equals("true"))
                {
                    bloque.Ejecutar(ent);
                }
                else
                {
                    if (relse != null)
                    {
                        relse.Ejecutar(ent);
                    }
                }
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.If;
        }
    }
}
