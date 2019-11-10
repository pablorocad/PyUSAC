using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PyUSAC
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnCrearArchivo_Click(object sender, EventArgs e)
        {
            newTab("", "Nueva Pestaña");
        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            String[] temp = readFile();
            newTab(temp[0], temp[1]);
        }


        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            guardar();
            
        }

        //------------------------------------------METODOS---------------------------------
        public String[] readFile()//Meotodo para leer un archivo de texto
        {
            String path;
            String[] texto = new String[2];

            OpenFileDialog directory = new OpenFileDialog();//Cuadro para buscar archivos
            directory.InitialDirectory = "C:\\";//Seteamos donde mepieza a buscar
            directory.Filter = "pyUSAC files (*.pyUSAC) | *.pyUSAC";//Filtro
            directory.FilterIndex = 2;
            directory.RestoreDirectory = true;

            if (directory.ShowDialog() == DialogResult.OK)//Si logro abrirse
            {
                path = directory.FileName;//Tomamos el path del archivo

                String[] b = path.Split('\\');

                texto[1] = b.Last();
                texto[0] = File.ReadAllText(path, Encoding.UTF7);//Leemos el texto
            }

            

            return texto;//Devolvemso el texto
        }

        public void newTab(String cadena, String name)
        {

            TabPage tp = new TabPage(name);//Creamos una nueva pestaña
            //tp.Name = "textoBox";

            AreaEdicion.TabPages.Add(tp);//La añadimos al Tab

            TextBox txt = new TextBox();//Creamos un text box
            txt.Text = cadena;

            txt.Dock = DockStyle.Fill;//Le decimos que se acople a su contenedor
            txt.Multiline = true;//Sera de multiples lineas
            txt.ScrollBars = ScrollBars.Vertical;//Tendra ScrollBar a lo vertical

            tp.Controls.Add(txt);//Lo insertamos en la pestaña
        }

        public void guardar()
        {
            TextBox temp = AreaEdicion.SelectedTab.Controls[0] as TextBox;
            MessageBox.Show(temp.Text);
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnCompilar_Click(object sender, EventArgs e)
        {
            TextBox temp = AreaEdicion.SelectedTab.Controls[0] as TextBox;
            Sintactico analisis = new Sintactico();

            bool result = analisis.Analizar(temp.Text, 8200);
            AreaReportes.SelectTab("Consola");

            (AreaReportes.SelectedTab.Controls[0] as TextBox).Text = "";

            if (result)
            {
                //(AreaReportes.SelectedTab.Controls[0] as TextBox).Text = "Cadena correcta";
                foreach (String s in Sintactico.listaImp)
                {
                    (AreaReportes.SelectedTab.Controls[0] as TextBox).Text =
                        (AreaReportes.SelectedTab.Controls[0] as TextBox).Text + s + "\r\n";
                }
            }
            else
            {
                (AreaReportes.SelectedTab.Controls[0] as TextBox).Text = "Cadena incorrecta";
            }

            if (Sintactico.listaErrores.Count != 0)
            {
                AreaReportes.SelectTab(1);
                (AreaReportes.SelectedTab.Controls[0] as DataGridView).Rows.Clear();

                foreach (Error err in Sintactico.listaErrores)
                {
                    (AreaReportes.SelectedTab.Controls[0] as DataGridView).Rows.Add(
                        err.getTipo().ToString(), 
                        err.getDescripcion().ToString(),
                        err.getFila().ToString(),
                        err.getColumna().ToString());
                }
            }

            //AreaReportes.SelectTab("Consola");
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            TextBox temp = AreaEdicion.SelectedTab.Controls[0] as TextBox;
            Sintactico analisis = new Sintactico();

            bool result = analisis.Analizar(temp.Text, 0);
            AreaReportes.SelectTab("Consola");

            if (result)
            {
                MessageBox.Show("Cadena correcta");

            }
            else
            {
                MessageBox.Show("Cadena incorrecta");
            }

            //    ArbolArreglo arbol = new ArbolArreglo("arr10");

            //    arbol.add(2);
            //    arbol.add(3);
            //    arbol.add(4);

            //    MessageBox.Show("");
        }
    }
}
