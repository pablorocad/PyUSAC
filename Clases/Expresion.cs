using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Expresion
    {

        Tipo.Valor tipo;
        Object valor;

        public Expresion(Tipo.Valor tipo, object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public Tipo.Valor getTipo()
        {
            return tipo;
        }

        public Object getValor()
        {
            return valor;
        }

        public void setValor(Object exp)
        {
            this.valor = exp;
        }
    }
}
