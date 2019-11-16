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

        public Instruccion Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            Expresion condicion = resolve.resolverExpresion(cond, ent);

            Instruccion aux = null;

            if (!condicion.getTipo().Equals(Tipo.Valor.rnull))
            {
                if (condicion.getTipo().Equals(Tipo.Valor.booleano))
                {
                    if (condicion.getValor().ToString().ToLower().Equals("true"))
                    {
                        aux = bloque.Ejecutar(ent);

                    }
                    else
                    {
                        if (relse != null)
                        {
                            aux = relse.Ejecutar(ent);
                        }
                    }
                }
                else
                {
                    int linea = cond.Span.Location.Line;
                    int columna = cond.Span.Location.Column;
                    Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La Expresion debe ser booleana", linea, columna));
                }
            }
            return aux;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.If;
        }
    }
}
