using Irony.Parsing;
using PyUSAC.Clases;
using PyUSAC.CMF;
using PyUSAC.Instrucciones;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyUSAC.Analisis
{
    class SegundaPasada
    {
        Resolve resolve = new Resolve();
        public static List<Error> listaErrores = new List<Error>();
        

        /*Se encarga de ejecutar lo que esta fuera de clases, metodos, funciones y leugo ejecuta el main()*/
        public LinkedList<Instruccion> second(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Instruccion> Ins = null;
            switch (temp.Term.ToString())
            {
                case "INICIO":
                    Ins = second(temp.ChildNodes.ElementAt(0), ent);
                    return Ins;

                case "S":
                    Ins = second(temp.ChildNodes.ElementAt(0), ent);
                    return Ins;

                case "L_INS":

                    if (temp.ChildNodes.Count == 2)//Si trae mas instrucciones
                    {
                        Ins = second(temp.ChildNodes.ElementAt(0), ent);//Tomamos la lista de instrucciones

                        Instruccion s = instructions(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones
                        if (s != null)//Para excluir declaracion y asignacion
                        {
                            Ins.AddLast(s);
                        }
                    }
                    else if (temp.ChildNodes.Count == 1)//La primera instruccion
                    {
                        Ins = new LinkedList<Instruccion>();//Creamso la lista de instrucciones
                        Instruccion s = instructions(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones

                        if (s != null)//Para excluir declaracion y asignacion
                        {
                            Ins.AddLast(s);
                        }

                    }
                    return Ins;

                case "L_INSB":

                    if (temp.ChildNodes.Count == 2)//Si trae mas instrucciones
                    {
                        Ins = second(temp.ChildNodes.ElementAt(0), ent);//Tomamos la lista de instrucciones

                        Instruccion s = instructions(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones
                        if (s != null)//Para excluir declaracion y asignacion
                        {
                            Ins.AddLast(s);
                        }
                    }
                    else if (temp.ChildNodes.Count == 1)//La primera instruccion
                    {
                        Ins = new LinkedList<Instruccion>();//Creamso la lista de instrucciones
                        Instruccion s = instructions(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones

                        if (s != null)//Para excluir declaracion y asignacion
                        {
                            Ins.AddLast(s);
                        }

                    }
                    return Ins;

            }
            return Ins;
        }

        public Instruccion instructions(ParseTreeNode temp, Entorno ent)
        {
            switch (temp.Term.ToString())
            {
                case "DECLARACION":
                    LinkedList<Declaracion> ID = L_ID(temp.ChildNodes.ElementAt(1), ent);//Guardamos la lista de ID

                    return new Dec(ID);

                case "LOG":
                    ParseTreeNode expLog = temp.ChildNodes.ElementAt(2);//Resolver la expresion
                    return new Log(expLog);//Retornar instruccion Log

                case "ALERT":
                    ParseTreeNode expAlert = temp.ChildNodes.ElementAt(2);//Resolver la expresion
                    return new Alert(expAlert);

                case "ASIGNACION":

                    ParseTreeNode asig = temp.ChildNodes.ElementAt(0);
                    ParseTreeNode expAs = temp.ChildNodes.ElementAt(3);

                    if (temp.ChildNodes.ElementAt(1).ChildNodes.Count != 0)
                    {
                        ParseTreeNode l_dim = temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0);
                        return new Asignacion(asig, l_dim, expAs);
                    }

                        return new Asignacion(asig, expAs);

                case "GRAPH":
                    ParseTreeNode nameGraph = temp.ChildNodes.ElementAt(2);
                    ParseTreeNode grafo = temp.ChildNodes.ElementAt(4);
                    
                    return new Graph(nameGraph, grafo);
                        

                case "AUM_DEC"://Pasar a clase abstracta-----------------------------------------------------------------------------

                    ParseTreeNode id = temp.ChildNodes.ElementAt(0);

                    if (temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0].Equals("++"))
                    {
                        return new Aum_Dec(id, true);
                    }
                    else if (temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0].Equals("--"))
                    {
                        return new Aum_Dec(id, false);
                    }
                    break;

                case "BLOQUE":
                    ParseTreeNode lista = temp.ChildNodes.ElementAt(1);
                    return new Bloque(lista);
                    break;

                case "IF":
                    ParseTreeNode condicionIF = temp.ChildNodes.ElementAt(2);
                    Instruccion bloqueIF = instructions(temp.ChildNodes.ElementAt(4), ent);
                    Instruccion relse = instructions(temp.ChildNodes.ElementAt(5), ent);

                    return new IF(condicionIF, bloqueIF, relse);
                    break;

                case "ELSE":
                    if (temp.ChildNodes.Count != 0)
                    {
                        Instruccion ins = instructions(temp.ChildNodes.ElementAt(1), ent);
                        return ins;
                    }
                    break;

                case "WHILE":
                    ParseTreeNode condicionWHILE = temp.ChildNodes.ElementAt(2);
                    Instruccion bloqueWHILE = instructions(temp.ChildNodes.ElementAt(4),ent);

                    return new While(condicionWHILE, bloqueWHILE);

                case "DO_WHILE":
                    ParseTreeNode condicionDO = temp.ChildNodes.ElementAt(4);
                    Instruccion bloqueDO = instructions(temp.ChildNodes.ElementAt(1), ent);

                    return new Do_While(condicionDO, bloqueDO);

                case "FOR":
                    ParseTreeNode inicializacion = temp.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0);
                    ParseTreeNode condicion = temp.ChildNodes.ElementAt(3);
                    ParseTreeNode actualizacion = temp.ChildNodes.ElementAt(5);

                    Entorno entornoFor = new Entorno(ent);//Creamos el entorno de la iteación del For

                    Instruccion bloqueFOR = instructions(temp.ChildNodes.ElementAt(7), entornoFor);
                    //Mandamos a traer el bloque de instrucciones for

                    return new For(inicializacion, condicion, actualizacion, entornoFor, bloqueFOR);

                case "SWITCH":
                    ParseTreeNode entrada = temp.ChildNodes.ElementAt(2);
                    LinkedList<Case> listaCase = BLOQUE_SW(temp.ChildNodes.ElementAt(5), ent);
                    Instruccion def = null;

                    if (temp.ChildNodes.ElementAt(6).ChildNodes.Count != 0)
                    {
                        Entorno DefEnt = new Entorno(ent);
                        ParseTreeNode listaDef = temp.ChildNodes.ElementAt(6).ChildNodes.ElementAt(2);
                        Bloque bloque = new Bloque(listaDef);
                        def = bloque;
                    }

                    return new Switch(entrada, listaCase, def);
                    break;

                case "BREAK":
                    return new Break();

                case "CONTINUE":
                    return new Continue();

                case "RETURN":
                    return new Return(temp.ChildNodes.ElementAt(1));

                case "LLAMADA":

                    ParseTreeNode lla = temp.ChildNodes.ElementAt(0);
                    ParseTreeNode parametros = temp.ChildNodes.ElementAt(1);

                    return new Llamada(lla, parametros);

                    break;
            }
            return null;
        }

        

        public LinkedList<Declaracion> L_ID(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Declaracion> l_id = null;

            if (temp.ChildNodes.Count == 4)
            {
                l_id = L_ID(temp.ChildNodes.ElementAt(0), ent);//Tomamos la lista
                String id = temp.ChildNodes.ElementAt(2).ToString().Split(' ')[0];//Tomamos el ID
                Declaracion dec = null;

                if (temp.ChildNodes.ElementAt(3).ChildNodes.Count != 0)//Si trae una asignacion
                {
                    if (!temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(0).Term.ToString().Equals("L_DIM"))//Si es variable normal
                    {
                        ParseTreeNode exp = temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(1);

                        dec = new Declaracion(id, exp);

                        for (int x = l_id.Count - 1; x >= 0; x--)//Recorremos la lista para atras para cambiar los nulos
                        {
                            if (l_id.ElementAt(x).getTipo().Equals(Tipo.Simbolo.variable))
                            {
                                if (l_id.ElementAt(x).getExpresion() == null)
                                {
                                    l_id.ElementAt(x).setExpresion(exp);
                                }
                            }
                        }
                    }
                    else
                    {//Si es un arreglo
                        ParseTreeNode l_dim = temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(0);
                        ParseTreeNode valores = null;

                        if (temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(1).ChildNodes.Count != 0)//si tra valores
                        {
                            
                            valores = temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(1).ChildNodes.ElementAt(1);
                            //Mandamos a traer los valores
                            //Console.WriteLine();
                        }

                        dec = new Declaracion(id, l_dim, valores);
                    }
                }
                else
                {
                    dec = new Declaracion(id, null);
                }

                l_id.AddLast(dec);//Guardamos
            }
            else if (temp.ChildNodes.Count == 2)
            {
                l_id = new LinkedList<Declaracion>();//El primer ID, crreamos la lista
                String id = temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0];
                Declaracion dec = null;

                if (temp.ChildNodes.ElementAt(1).ChildNodes.Count != 0)
                {
                    if (!temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.ToString().Equals("L_DIM"))//Si es variable normal
                    {
                        ParseTreeNode exp = temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1);

                        dec = new Declaracion(id, exp);
                    }
                    else
                    {//Si es un arreglo
                        ParseTreeNode l_dim = temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0);
                        ParseTreeNode valores = null;
                        if (temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).ChildNodes.Count != 0)
                        {
                            valores = temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).ChildNodes.ElementAt(1);
                            //Mandamos a traer los valores

                            //Console.WriteLine();
                        }

                        dec = new Declaracion(id, l_dim, valores);
                    }
                }
                else
                {
                    dec = new Declaracion(id, null);
                }

                l_id.AddLast(dec);//Guardamos
            }
            return l_id;
        }

        
        public LinkedList<Case> BLOQUE_SW(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Case> listaCase = null;

            if (temp.ChildNodes.Count == 5)
            {
                listaCase = BLOQUE_SW(temp.ChildNodes.ElementAt(0), ent);
                ParseTreeNode condicion = temp.ChildNodes.ElementAt(2);

                Entorno nuevo = new Entorno(ent);
                ParseTreeNode lista = temp.ChildNodes.ElementAt(4);
                Bloque bloque = new Bloque(lista);

                Case @case = new Case(condicion, bloque);

                listaCase.AddLast(@case);
            }
            else if (temp.ChildNodes.Count == 4)
            {
                listaCase = new LinkedList<Case>();
                ParseTreeNode condicion = temp.ChildNodes.ElementAt(1);

                Entorno nuevo = new Entorno(ent);
                ParseTreeNode lista = temp.ChildNodes.ElementAt(3);
                Bloque bloque = new Bloque(lista);

                Case @case = new Case(condicion, bloque);

                listaCase.AddLast(@case);

            }

            return listaCase;
        }
        

    }
}
