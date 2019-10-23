using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;


namespace PyUSAC.Analisis
{
    class graficarArbol
    {

        private static int contador;
        private static String grafo;

        public static String textoGraphviz(ParseTreeNode temp)
        {
            grafo = "digraph AST{ \r\n";
            grafo += "nodo0[label=\"" + temp.ToString() + "\"];\r\n";
            contador = 1;
            recorrerAST("nodo0", temp);
            grafo += "}";

            return grafo;
        }

        public static void recorrerAST(String padre, ParseTreeNode temp)
        {
            foreach (ParseTreeNode hijo in temp.ChildNodes)
            {
                String nameChild = "nodo" + contador.ToString();
                grafo += nameChild + "[label=\"" + hijo.ToString() + "\"];\r\n";
                grafo += padre + "->" + nameChild + ";\r\n";
                contador++;
                recorrerAST(nameChild, hijo);
            }
        }
        
    }
}
