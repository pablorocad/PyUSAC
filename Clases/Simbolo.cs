using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Simbolo
    {

        Tipo.Simbolo tipo;
        Object contenido;

        public Simbolo(Tipo.Simbolo tipo, Object contenido)
        {
            this.tipo = tipo;
            this.contenido = contenido;
        }

        public Tipo.Simbolo getTipo()
        {
            return tipo;
        }

        public void setTipo(Tipo.Simbolo tipo)
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
