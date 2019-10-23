using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Error
    {

        Tipo.Error tipo;
        String descripción;
        int fila, columna;

        public Error(Tipo.Error tipo, string descripción, int fila, int columna)
        {
            this.tipo = tipo;
            this.descripción = descripción;
            this.fila = fila;
            this.columna = columna;
        }

        public Tipo.Error getTipo()
        {
            return tipo;
        }

        public String getDescripcion()
        {
            return descripción;
        }

        public int getFila()
        {
            return fila;
        }

        public int getColumna()
        {
            return columna;
        }
    }
}
