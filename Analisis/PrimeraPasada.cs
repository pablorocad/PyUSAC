using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using PyUSAC.Clases;
using PyUSAC.CMF;
using PyUSAC.Interfaces;

namespace PyUSAC.Analisis
{
    class PrimeraPasada
    {

        public Entorno first(ParseTreeNode temp, Entorno ent)
        {
            Entorno entorno = null;

            switch (temp.Term.ToString())
            {
                case "INICIO":
                    entorno = first(temp.ChildNodes.ElementAt(0), ent);
                    return entorno;

                case "S":
                    entorno = first(temp.ChildNodes.ElementAt(0), ent);
                    return entorno;

                case "L_INS":

                    if (temp.ChildNodes.Count == 2)//Si trae mas instrucciones
                    {
                        entorno = first(temp.ChildNodes.ElementAt(0), ent);//Tomamos la lista de instrucciones

                        Bloques dec = bloques(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones
                        if (dec != null)//Para excluir declaracion y asignacion
                        {
                            String name = "";
                            Tipo.Simbolo t = dec.getTipo();

                            switch (t)
                            {
                                case Tipo.Simbolo.clase:
                                    name = "@" + dec.getName();
                                    break;

                                case Tipo.Simbolo.funcion:
                                    name = "#" + dec.getName();
                                    break;

                                case Tipo.Simbolo.metodo:
                                    name = "%" + dec.getName();
                                    break;
                            }

                            Simbolo sim = new Simbolo(dec.getTipo(), dec);

                            entorno.add(name, sim, temp.Span.Location.Line, temp.Span.Location.Column);
                        }
                    }
                    else if (temp.ChildNodes.Count == 1)//La primera instruccion
                    {
                        entorno = ent;//Creamso la lista de instrucciones
                        Bloques dec = bloques(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones

                        if (dec != null)//Para excluir declaracion y asignacion
                        {
                            String name = "";
                            Tipo.Simbolo t = dec.getTipo();

                            switch (t)
                            {
                                case Tipo.Simbolo.clase:
                                    name = "@" + dec.getName();
                                    break;

                                case Tipo.Simbolo.funcion:
                                    name = "#" + dec.getName();
                                    break;

                                case Tipo.Simbolo.metodo:
                                    name = "%" + dec.getName();
                                    break;
                            }

                            Simbolo sim = new Simbolo(dec.getTipo(), dec);

                            entorno.add(name, sim, temp.Span.Location.Line, temp.Span.Location.Column);
                        }

                    }
                    return entorno;

                case "L_INSB":

                    if (temp.ChildNodes.Count == 2)//Si trae mas instrucciones
                    {
                        entorno = first(temp.ChildNodes.ElementAt(0), ent);//Tomamos la lista de instrucciones

                        Bloques dec = bloques(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones
                        if (dec != null)//Para excluir declaracion y asignacion
                        {
                            String name = "";
                            Tipo.Simbolo t = dec.getTipo();

                            switch (t)
                            {
                                case Tipo.Simbolo.clase:
                                    name = "@" + dec.getName();
                                    break;

                                case Tipo.Simbolo.funcion:
                                    name = "#" + dec.getName();
                                    break;

                                case Tipo.Simbolo.metodo:
                                    name = "%" + dec.getName();
                                    break;
                            }

                            Simbolo sim = new Simbolo(dec.getTipo(), dec);

                            entorno.add(name, sim, temp.Span.Location.Line, temp.Span.Location.Column);
                        }
                    }
                    else if (temp.ChildNodes.Count == 1)//La primera instruccion
                    {
                        entorno = ent;//Creamso la lista de instrucciones
                        Bloques dec = bloques(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones

                        if (dec != null)//Para excluir declaracion y asignacion
                        {
                            String name = "";
                            Tipo.Simbolo t = dec.getTipo();

                            switch (t)
                            {
                                case Tipo.Simbolo.clase:
                                    name = "@" + dec.getName();
                                    break;

                                case Tipo.Simbolo.funcion:
                                    name = "#" + dec.getName();
                                    break;

                                case Tipo.Simbolo.metodo:
                                    name = "%" + dec.getName();
                                    break;
                            }

                            Simbolo sim = new Simbolo(dec.getTipo(), dec);

                            entorno.add(name, sim, temp.Span.Location.Line, temp.Span.Location.Column);
                        }

                    }
                    return entorno;
            }
            return entorno;
        }

        public Bloques bloques(ParseTreeNode temp, Entorno ent)
        {
            Bloques bloque = null;
            switch (temp.Term.ToString())
            {
                case "CLASE":
                    String idCL = temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                    ParseTreeNode esqueletoCL = temp.ChildNodes.ElementAt(2);
                    return new Clase(idCL, esqueletoCL);

                case "METODO":
                    String idMET = temp.ChildNodes.ElementAt(2).ToString().Split(' ')[0];
                    ParseTreeNode esqueletoMET = temp.ChildNodes.ElementAt(6);

                    LinkedList<Declaracion> l_parMET = L_PARR(temp.ChildNodes.ElementAt(4), ent);

                    return new Metodo(idMET, l_parMET, esqueletoMET);

                case "FUNCION":
                    String idFUN = temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                    ParseTreeNode esqueletoFUN = temp.ChildNodes.ElementAt(5);

                    LinkedList<Declaracion> l_parFUN = L_PARR(temp.ChildNodes.ElementAt(3), ent);

                    return new Funcion(idFUN, l_parFUN, esqueletoFUN);
            }
            return bloque;
        }

        public LinkedList<Declaracion> L_PARR(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Declaracion> l_par = null;
            if (temp.ChildNodes.Count == 0)
            {
                l_par = new LinkedList<Declaracion>();
            }
            else
            {
                l_par = L_PAR(temp.ChildNodes.ElementAt(0), ent);
            }
            return l_par;
        }

        public LinkedList<Declaracion> L_PAR(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Declaracion> l_par = null;
            
            if (temp.ChildNodes.Count == 4)
            {
                l_par = L_PAR(temp.ChildNodes.ElementAt(0), ent);
                String name = temp.ChildNodes.ElementAt(3).ToString().Split(' ')[0];
                Declaracion dec = new Declaracion(name, null);

                l_par.AddLast(dec);
            }
            else if (temp.ChildNodes.Count == 2)
            {
                l_par = new LinkedList<Declaracion>();
                String name = temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                Declaracion dec = new Declaracion(name, null);

                l_par.AddLast(dec);
            }
            return l_par;
        }
    }
}
