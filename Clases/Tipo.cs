using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Tipo
    {


        public enum Simbolo
        {
            variable, clase, metodo, funcion
        };
        public enum Valor
        {
            numero, cadena, identificador, caracter, booleano, rnull, arreglo, objeto
        };

        public enum Error
        {
            lexico, sintactico, semantico
        };

        public enum Instruccion
        {
            declaracion, log, alert, graph, If, While, For, Do_While, Switch, Bloque, asignacion,
            Break, Continue, llamada, Return
        };

    }
}
