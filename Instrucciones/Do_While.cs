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
    class Do_While : Instruccion
    {
        ParseTreeNode condicion;
        Instruccion bloque;

        public Do_While(ParseTreeNode condicion, Instruccion bloque)
        {
            this.condicion = condicion;
            this.bloque = bloque;
        }

        public void Ejecutar(Entorno ent)
        {

            Sintactico.pilaBreak.Push(this);

            Resolve resolve = new Resolve();
            Expresion cond = resolve.resolverExpresion(condicion, ent);

            if (!cond.getTipo().Equals(Tipo.Valor.rnull))
            {
                if (cond.getTipo().Equals(Tipo.Valor.booleano))
                {
                    do
                    {

                        if (Sintactico.pilaBreak.Count == 0 || !Sintactico.pilaBreak.Peek().Equals(this))
                        {
                            break;
                        }

                        bloque.Ejecutar(ent);
                        cond = resolve.resolverExpresion(condicion, ent);
                        if (cond.getTipo().Equals(Tipo.Valor.rnull))
                        {
                            cond = new Expresion(Tipo.Valor.booleano, false);
                        }

                    } while (cond.getValor().ToString().ToLower().Equals("true"));
                }
                else
                {
                    int linea = condicion.Span.Location.Line;
                    int columna = condicion.Span.Location.Column;
                    Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La Expresion debe ser booleana", linea, columna));
                }
            }
            if (Sintactico.pilaBreak.Count != 0 && Sintactico.pilaBreak.Peek().Equals(this))
            {
                Sintactico.pilaBreak.Pop();
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Do_While;
        }
    }
}
