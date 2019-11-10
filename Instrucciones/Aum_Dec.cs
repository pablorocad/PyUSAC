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
    class Aum_Dec : Instruccion
    {
        ParseTreeNode id;
        Boolean aum_dec;

        public Aum_Dec(ParseTreeNode id, bool aum_dec)
        {
            this.id = id;
            this.aum_dec = aum_dec;
        }

        public void Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            Expresion hijo1 = resolve.resolverExpresion(id, ent);
            Double num1 = 0;
            if (id.ChildNodes.ElementAt(0).Term.ToString().Equals("identificador"))
            {
                if (aum_dec)
                {
                    if (hijo1.getTipo().Equals(Tipo.Valor.numero))
                    {
                        num1 = double.Parse(hijo1.getValor().ToString());
                        num1++;

                        ent.edit(id.ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                            new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.numero, num1)));
                    }
                    else if (hijo1.getTipo().Equals(Tipo.Valor.caracter))
                    {
                        num1 = Convert.ToChar(hijo1.getValor().ToString());
                        num1++;

                        ent.edit(id.ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                            new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.caracter, (char)num1)));
                    }
                }
                else
                {

                        if (hijo1.getTipo().Equals(Tipo.Valor.numero))
                        {
                            num1 = double.Parse(hijo1.getValor().ToString());
                            num1--;

                            ent.edit(id.ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                                new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.numero, num1)));
                        }
                        else if (hijo1.getTipo().Equals(Tipo.Valor.caracter))
                        {
                            ent.edit(id.ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                                new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.caracter, (char)num1)));
                        }
                    
                }
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.asignacion;
        }
    }
}
