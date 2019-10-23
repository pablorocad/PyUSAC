using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System.Windows.Forms;
using PyUSAC.Instrucciones;

namespace PyUSAC.Analisis
{
    class Sintactico : Grammar
    {
        public static List<Error> listaErrores = new List<Error>();
        LinkedList<Instruccion> listaIns;
        public static LinkedList<String> listaImp;

        public Sintactico()
        {
            listaIns = new LinkedList<Instruccion>();
            listaImp = new LinkedList<String>();
        }

        public LinkedList<Instruccion> getIns()
        {
            return listaIns;
        }

        public bool Analizar(String cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbolAST = parser.Parse(cadena);
            ParseTreeNode raiz = arbolAST.Root;

            generarArbol(raiz);
            ejecutarArbol(raiz);

            if (raiz != null)
            {
                return true;
            }

            return false;
        }

        public static void generarArbol(ParseTreeNode temp)
        {
            try
            {
                String DOT = graficarArbol.textoGraphviz(temp);
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Carpetas\\AST.txt", DOT);

                generarImagen(Directory.GetCurrentDirectory() + "\\Carpetas");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public static void generarImagen(String path)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("echo off");
            cmd.StandardInput.WriteLine("cd " + path);
            cmd.StandardInput.WriteLine($"dot -Tpng AST.txt -o AST.png");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }


        public void ejecutarArbol(ParseTreeNode temp)
        {
            Entorno global = new Entorno(null);
            listaIns = second(temp, global);

            foreach (Instruccion lista in listaIns)
            {
                lista.Ejecutar(global);
            }

        }

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
            }
            return Ins;
        }

        public Instruccion instructions(ParseTreeNode temp, Entorno ent)
        {
            switch (temp.Term.ToString())
            {
                case "DECLARACION":
                    LinkedList<String> ID = L_ID(temp.ChildNodes.ElementAt(1));//Guardamos la lista de ID
                    Simbolo sim = new Simbolo(Tipo.Valor.rnull, null);//Por defecto los simbolos seran null

                    if (temp.ChildNodes.ElementAt(2).ChildNodes.Count != 0)
                    {
                        Expresion exp = resolverExpresion(temp.ChildNodes.ElementAt(2).ChildNodes.ElementAt(1), ent);//Resolvemos la expresion
                        if (exp != null)
                        {
                            sim = new Simbolo(exp.getTipo(), exp);//Creamos el nuevo simbolo
                        }
                    }

                    foreach (String st in ID)
                    {
                        ent.add(st, sim, 0, 0);//Guardamos en el entorno
                    }

                    return null;

                case "LOG":
                    Expresion expLog = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                    return new Log(expLog);
            }
            return null;
        }

        public LinkedList<String> L_ID(ParseTreeNode temp)
        {
            LinkedList<String> id = null;
            if (temp.ChildNodes.Count == 3)
            {
                id = L_ID(temp.ChildNodes.ElementAt(0));//Tomamos la lista
                id.AddLast(temp.ChildNodes.ElementAt(2).ToString().Split(' ')[0].ToLower());//Guardamos
            }
            else if (temp.ChildNodes.Count == 1)
            {
                id = new LinkedList<string>();//El primer ID, crreamos la lista
                id.AddLast(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0].ToLower());//Guardamos
            }
            return id;
        }

        public Expresion resolverExpresion(ParseTreeNode temp, Entorno ent)
        {
            if (temp.ChildNodes.Count == 3)//Si vienen 3 hijos es (E) o E op E
            {
                if (temp.ChildNodes.ElementAt(1).Term.ToString().Equals("E"))//Si es (E)
                {
                    return resolverExpresion(temp.ChildNodes.ElementAt(1), ent);
                }
                else
                {
                    String operador = temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                    Expresion hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                    Expresion hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);

                    Object nuevoValor = null;
                    switch (operador)
                    {
                        case "+":
                            //CADENAS
                            if (hijo1.getTipo().Equals(Tipo.Valor.cadena) || hijo2.getTipo().Equals(Tipo.Valor.cadena))
                            {
                                nuevoValor = hijo1.getValor().ToString() + "" + hijo2.getValor().ToString();
                                return new Expresion(Tipo.Valor.cadena, nuevoValor);
                            }
                            //NUMEROS
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) + Double.Parse(hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) + car;
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = Double.Parse(hijo2.getValor().ToString()) + car;
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede sumarse con la expresion: " + hijo2.getValor().ToString(), 0,0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede sumarse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "-":
                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) - Double.Parse(hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) - car;
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car - Double.Parse(hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede restarse con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede restarse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "*":
                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) * Double.Parse(hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) * car;
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = Double.Parse(hijo2.getValor().ToString()) * car;
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede multiplicarse con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede multiplicarse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "/":
                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) / Double.Parse(hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) / car;
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car / Double.Parse(hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede dividirse con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede dividirse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "pow":
                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Math.Pow(Double.Parse(hijo1.getValor().ToString()), Double.Parse(hijo2.getValor().ToString()));
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Math.Pow(Double.Parse(hijo1.getValor().ToString()),car);
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = Math.Pow(car,Double.Parse(hijo2.getValor().ToString()));
                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede elevarse a la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede elevarse a la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;
                    }
                }
            }
            else if (temp.ChildNodes.Count == 2)
            {
                Expresion hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(1), ent);

                if (hijo2.getTipo().Equals(Tipo.Valor.numero))
                {
                    return new Expresion(Tipo.Valor.numero, -1*Double.Parse(hijo2.getValor().ToString()));
                }
                else if (hijo2.getTipo().Equals(Tipo.Valor.cadena))
                {
                    return new Expresion(Tipo.Valor.cadena, "-" + hijo2.getValor().ToString());
                }
                else if (hijo2.getTipo().Equals(Tipo.Valor.caracter))
                {
                    double car = Convert.ToChar(hijo2.getValor().ToString());

                    return new Expresion(Tipo.Valor.cadena, -1*car);
                }
            }
            else if (temp.ChildNodes.Count == 1)
            {
                String type = temp.ChildNodes.ElementAt(0).Term.ToString().ToLower();

                switch (type)
                {
                    case "numero":
                        return new Expresion(Tipo.Valor.numero, temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0]);

                    case "caracter":
                        return new Expresion(Tipo.Valor.caracter, temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0]);

                    case "cadena":

                        return new Expresion(Tipo.Valor.cadena, temp.ChildNodes.ElementAt(0).ToString().Replace(" (cadena)", ""));

                    case "true":
                        return new Expresion(Tipo.Valor.booleano, type);

                    case "false":
                        return new Expresion(Tipo.Valor.booleano, type);

                    case "identificador":
                        Simbolo sim = ent.search(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0].ToLower(), 0,0);

                        if (sim != null)
                        {
                            return new Expresion(((Expresion)sim.getContenido()).getTipo(), (sim.getContenido() as Expresion).getValor());
                        }
                        break;
                }
            }
            return null;
        }

    }

}
