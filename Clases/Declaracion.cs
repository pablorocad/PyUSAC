using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Declaracion
    {

        String identificador;
        Expresion exp;

        public Declaracion(string identificador, Expresion exp)
        {
            this.identificador = identificador;
            this.exp = exp;
        }

        public String getIdentificador()
        {
            return identificador;
        }

        public Expresion getExpresion()
        {
            return exp;
        }

        public void setExpresion(Expresion exp)
        {
            this.exp = exp;
        }
    }
}
