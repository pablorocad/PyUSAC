using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Simbolo
    {

        Tipo.Valor tipo;
        Object contenido;

        public Simbolo(Tipo.Valor tipo, Object contenido)
        {
            this.tipo = tipo;
            this.contenido = contenido;
        }

        public Tipo.Valor getTipo()
        {
            return tipo;
        }

        public void setTipo(Tipo.Valor tipo)
        {
            this.tipo = tipo;
        }

        public Object getContenido()
        {
            return contenido;
        }

        public void setValor(Object contenido)
        {
            this.contenido = contenido;
        }

    }
}
