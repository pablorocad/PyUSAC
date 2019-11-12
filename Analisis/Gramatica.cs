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
            var rif = ToTerm("if");
            var relse = ToTerm("else");
            var rwhile = ToTerm("while");
            var rdo = ToTerm("do");
            var rfor = ToTerm("for");
            var rswitch = ToTerm("switch");
            var rcase = ToTerm("case");
            var rcontinue = ToTerm("continue");
            var rbreak = ToTerm("break");
            var rdefault = ToTerm("default");
            var rclass = ToTerm("class");
            var rfunction = ToTerm("function");
            var rvoid = ToTerm("void");
            var rreturn = ToTerm("return");

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
            var llaveizq = ToTerm("{");
            var llaveder = ToTerm("}");
            var corcheteizq = ToTerm("[");
            var corcheteder = ToTerm("]");
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
            var aumento = ToTerm("++");
            var decremento = ToTerm("--");
            var dospuntos = ToTerm(":");
            #endregion

            #region Precedencia
            RegisterOperators(1, Associativity.Left, xor);
            RegisterOperators(2, Associativity.Left, or);
            RegisterOperators(3, Associativity.Left, and);
            RegisterOperators(4, igualigual, desigual);
            RegisterOperators(5, mayor, menor, mayorigual, menorigual);
            RegisterOperators(6, Associativity.Left, mas, menos);
            RegisterOperators(7, Associativity.Left, multi, div);
            RegisterOperators(8, potencia);
            RegisterOperators(9, Associativity.Right, not);
            RegisterOperators(10, Associativity.Right, aumento, decremento);
            #endregion

            #region No Terminales

            NonTerminal INICIO = new NonTerminal("INICIO"),
                S = new NonTerminal("S"),
                L_INS = new NonTerminal("L_INS"),
                INS = new NonTerminal("INS"),
                INSB = new NonTerminal("INSB"),
                L_INSB = new NonTerminal("L_INSB"),

                BLOQUE_CL = new NonTerminal("BLOQUE_CL"),
                L_INS_CL = new NonTerminal("L_INS_CL"),
                INS_CL = new NonTerminal("INS_CL"),

                BLOQUE_MET = new NonTerminal("BLOQUE_MET"),
                L_INS_MET = new NonTerminal("L_INS_MET"),
                INS_MET = new NonTerminal("INS_MET"),
                RETURN = new NonTerminal("RETURN"),

                DECLARACION = new NonTerminal("DECLARACION"),
                L_DIM = new NonTerminal("L_DIM"),
                VAL_ARR = new NonTerminal("VAL_ARR"),
                VAL_ARR2 = new NonTerminal("VAL_ARR2"),
                L_EXP = new NonTerminal("L_EXP"),

                LOG = new NonTerminal("LOG"),
                ALERT = new NonTerminal("ALERT"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                GRAPH = new NonTerminal("GRAPH"),
                AUM_DEC = new NonTerminal("AUM_DEC"),

                IF = new NonTerminal("IF"),
                ELSE = new NonTerminal("ELSE"),
                WHILE = new NonTerminal("WHILE"),
                DO_WHILE = new NonTerminal("DO_WHILE"),
                FOR = new NonTerminal("FOR"),
                DEC_ASIG = new NonTerminal("DEC_ASIG"),
                SWITCH = new NonTerminal("SWITCH"),
                BLOQUE_SW = new NonTerminal("BLOQUE_SW"),
                DEFAULT = new NonTerminal("DEFAULT"),
                BREAK_CONTINUE = new NonTerminal("BREAK_CONTINUE"),
                CLASE = new NonTerminal("CLASE"),
                METODO = new NonTerminal("METODO"),
                FUNCION = new NonTerminal("FUNCION"),
                L_PAR = new NonTerminal("L_PAR"),
                L_PARR = new NonTerminal("L_PARR"),

                BLOQUE = new NonTerminal("BLOQUE"),
                
                L_ID = new NonTerminal("L_ID"),
                ZI = new NonTerminal("ZI"),
                ZI2 = new NonTerminal("ZI2"),
                E = new NonTerminal("E")
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
                      | GRAPH
                      | AUM_DEC
                      | IF
                      | WHILE
                      | DO_WHILE
                      | FOR
                      | SWITCH
                      | CLASE
                      | METODO
                      | FUNCION
                ;

            L_INSB.Rule = L_INSB + INSB
                        | INSB
                        | Empty
                ;

            INSB.Rule = DECLARACION
                      | LOG
                      | ALERT
                      | ASIGNACION
                      | GRAPH
                      | AUM_DEC
                      | IF
                      | WHILE
                      | DO_WHILE
                      | FOR
                      | SWITCH
                      | BREAK_CONTINUE
                      | FUNCION
                      | METODO
                      | RETURN
                ;

            DECLARACION.Rule = rvar + L_ID + puntocoma
                ;

            L_ID.Rule = L_ID + coma + identificador + ZI
                      | identificador + ZI
                       ;

            ZI.Rule = igual + E
                    |L_DIM + VAL_ARR
                    | Empty
                    ;

            ZI2.Rule = L_DIM
                     | Empty
                     ;

            L_DIM.Rule = L_DIM + corcheteizq + E + corcheteder
                        | corcheteizq + E + corcheteder
                ;

            VAL_ARR.Rule = igual + VAL_ARR2
                         |Empty
                ;

            VAL_ARR2.Rule = VAL_ARR2 + coma + llaveizq + VAL_ARR2 + llaveder
                          | llaveizq + VAL_ARR2 + llaveder
                          | L_EXP
                ;

            L_EXP.Rule = L_EXP + coma + E
                       | E
                       ;

            LOG.Rule = rlog + parizq + E + parder + puntocoma
                ;

            ALERT.Rule = ralert + parizq + E + parder + puntocoma
                ;

            ASIGNACION.Rule = identificador + ZI2 + igual + E + puntocoma
                ;

            GRAPH.Rule = rgraph + parizq + E + coma + E + parder + puntocoma
                ;

            AUM_DEC.Rule = identificador + aumento + puntocoma
                      | identificador + decremento + puntocoma
                      ;


            IF.Rule = rif + parizq + E + parder + BLOQUE + ELSE
                ;

            BLOQUE.Rule = llaveizq + L_INSB + llaveder
                ;

            ELSE.Rule = relse + IF
                      | relse + BLOQUE
                      |Empty
                    ;

            WHILE.Rule = rwhile +parizq + E + parder + BLOQUE
                ;

            DO_WHILE.Rule = rdo + BLOQUE + rwhile + parizq + E + parder + puntocoma
                ;

            FOR.Rule = rfor + parizq + DEC_ASIG + E + puntocoma + E + parder + BLOQUE
                ;

            DEC_ASIG.Rule = DECLARACION
                          | ASIGNACION
                          ;

            SWITCH.Rule = rswitch + parizq + E + parder + llaveizq + BLOQUE_SW + DEFAULT + llaveder
                ;

            BLOQUE_SW.Rule = BLOQUE_SW + rcase + E + dospuntos + L_INSB + BREAK_CONTINUE
                            | rcase + E + dospuntos + L_INSB + BREAK_CONTINUE
                            ;

            BREAK_CONTINUE.Rule = rbreak + puntocoma
                        | rcontinue + puntocoma
                        | Empty
                        ;

            DEFAULT.Rule = rdefault + dospuntos + L_INSB + BREAK_CONTINUE
                          | Empty
                          ;

            CLASE.Rule = rclass + identificador + BLOQUE
                ;


            METODO.Rule = rfunction + rvoid + identificador + parizq + L_PARR + parder + BLOQUE
                ;


            FUNCION.Rule = rfunction + identificador + parizq + L_PARR + parder + BLOQUE
                ;

            L_PARR.Rule = L_PAR
                        | Empty
                        ;

            L_PAR.Rule = L_PAR + coma + rvar + identificador
                        | rvar + identificador
                        ;

            RETURN.Rule = rreturn + E + puntocoma
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
                    |E + aumento
                    |E + decremento
                    | not + E
                    | parizq + E + parder
                    | menos + E
                    | numero
                    | cadena
                    | identificador + ZI2
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
