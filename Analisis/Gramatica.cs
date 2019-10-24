using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace PyUSAC.Analisis
{
    class Gramatica : Grammar
    {

        public Gramatica() : base (caseSensitive: false)
        {
            #region Expresiones Regulares
            //COMENTARIOS----------------------------------------------------------------
            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\r\n");
            CommentTerminal comentarioMulti = new CommentTerminal("comentarioMulti", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioMulti);

            //Expresiones----------------------------------------------------------------
            NumberLiteral numero = new NumberLiteral("numero");
            StringLiteral cadena = new StringLiteral("cadena", "\"");
            StringLiteral caracter = TerminalFactory.CreateCSharpChar("caracter");//Chars
            IdentifierTerminal identificador = new IdentifierTerminal("identificador");
            #endregion

            #region Terminales
            //Palabras reservadas--------------------------------------------------------
            var rvar = ToTerm("var");
            var rtrue = ToTerm("true");
            var rfalse = ToTerm("false");
            var rlog = ToTerm("log");
            var ralert = ToTerm("alert");
            var rgraph = ToTerm("graph");

            //Simbolos-------------------------------------------------------------------
            var igual = ToTerm("=");
            var puntocoma = ToTerm(";");
            var coma = ToTerm(",");
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var multi = ToTerm("*");
            var div = ToTerm("/");
            var potencia = ToTerm("pow");
            var parizq = ToTerm("(");
            var parder = ToTerm(")");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var igualigual = ToTerm("==");
            var desigual = ToTerm("<>");
            var mayorigual = ToTerm(">=");
            var menorigual = ToTerm("<=");
            var and = ToTerm("&&");
            var or = ToTerm("||");
            var xor = ToTerm("^");
            var not = ToTerm("!");
            #endregion

            #region Precedencia
            RegisterOperators(1, xor);
            RegisterOperators(2, or);
            RegisterOperators(3, and);
            RegisterOperators(4, igualigual, desigual);
            RegisterOperators(5, mayor, menor, mayorigual, menorigual);
            RegisterOperators(6, mas, menos);
            RegisterOperators(7, multi, div);
            RegisterOperators(8, not);
            #endregion

            #region No Terminales

            NonTerminal INICIO = new NonTerminal("INICIO"),
                S = new NonTerminal("S"),
                L_INS = new NonTerminal("L_INS"),
                INS = new NonTerminal("INS"),


                DECLARACION = new NonTerminal("DECLARACION"),
                LOG = new NonTerminal("LOG"),
                ALERT = new NonTerminal("ALERT"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                GRAPH = new NonTerminal("GRAPH"),

                L_ID = new NonTerminal("L_ID"),
                ZI = new NonTerminal("ZI"),
                E = new NonTerminal("E");
                ;

            #endregion

            #region Gramatica
            INICIO.Rule = S
                        |Empty //Epsilon
                        ;

            S.Rule = L_INS
                ;

            L_INS.Rule = L_INS + INS
                       | INS
                        ;

            INS.Rule = DECLARACION
                      | LOG
                      | ALERT
                      | ASIGNACION
                      |GRAPH
                ;

            DECLARACION.Rule = rvar + L_ID + puntocoma
                ;

            L_ID.Rule = L_ID + coma + identificador + ZI
                      | identificador + ZI
                       ;

            ZI.Rule = igual + E
                    | Empty
                    ;

            LOG.Rule = rlog + parizq + E + parder + puntocoma
                ;

            ALERT.Rule = ralert + parizq + E + parder + puntocoma
                ;

            ASIGNACION.Rule = identificador + igual + E + puntocoma
                ;

            GRAPH.Rule = rgraph + parizq + E + coma + E + parder + puntocoma
                ;


            E.Rule = E + mas + E
                    | E + menos + E
                    | E + multi + E
                    | E + div + E
                    | E + potencia + E
                    | E + menor + E
                    | E + mayor + E
                    | E + mayorigual + E
                    | E + menorigual + E
                    | E + igualigual + E
                    | E + desigual + E
                    | E + or + E
                    | E + and + E
                    | E + xor + E
                    | not + E
                    | parizq + E + parder
                    | menos + E
                    | numero
                    | cadena
                    | identificador
                    | caracter
                    | rtrue
                    | rfalse
                    ;
            #endregion

            #region Especificaciones
            this.Root = INICIO;
            #endregion
        }

    }
}
