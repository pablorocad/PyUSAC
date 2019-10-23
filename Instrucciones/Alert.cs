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

        Expresion exp;

        public Alert(Expresion exp)
        {
            this.exp = exp;
        }

        public void Ejecutar(Entorno ent)
        {
            if (exp != null)
            {
                MessageBox.Show(exp.getValor().ToString());
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.alert;
        }
    }
}
