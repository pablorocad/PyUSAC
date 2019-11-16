using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyUSAC.Instrucciones
{
    class Alert : Instruccion
    {

        ParseTreeNode exp;

        public Alert(ParseTreeNode exp)
        {
            this.exp = exp;
        }

        public Instruccion Ejecutar(Entorno ent)
        {
            if (exp != null)
            {
                Resolve resolve = new Resolve();
                Expresion aux = resolve.resolverExpresion(exp, ent);
                MessageBox.Show(aux.getValor().ToString());
            }
            return null;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.alert;
        }
    }
}
