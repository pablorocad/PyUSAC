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
    class Switch : Instruccion
    {
        ParseTreeNode entrada;
        LinkedList<Case> listaCasos;
        Instruccion rdefault;

        public Switch(ParseTreeNode entrada, LinkedList<Case> listaCasos, Instruccion def)
        {
            this.entrada = entrada;
            this.listaCasos = listaCasos;
            rdefault = def;
        }

        public void Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            Boolean z = true;
            Expresion cond = resolve.resolverExpresion(entrada, ent);//Resolvemos la condicion

            if (!cond.getTipo().Equals(Tipo.Valor.rnull))
            {
                foreach (Case cs in listaCasos)
                {
                    Expresion expCs = resolve.resolverExpresion(cs.getCondicion(), ent);//Resolvemos la condicion

                    if (cond.getValor().ToString().ToLower().Equals(expCs.getValor().ToString().ToLower()))
                    {
                        z = false;
                        cs.getBloque().Ejecutar(ent);
                        if (cs.getBreak())
                        {
                            break;
                        }
                        cond = resolve.resolverExpresion(entrada, ent);
                        if (cond.getTipo().Equals(Tipo.Valor.rnull))
                        {
                            cond = new Expresion(Tipo.Valor.booleano, "");
                        }
                    }
                }

                if (z && rdefault != null)
                {
                    rdefault.Ejecutar(ent);
                }
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Switch;
        }
    }
}
