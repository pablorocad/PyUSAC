using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            TabPage tp = new TabPage("HEHE");//Creamos una nueva pestaña
            AreaEdicion.TabPages.Add(tp);//La añadimos al Tab

            TextBox txt = new TextBox();//Creamos un text box
            txt.Dock = DockStyle.Fill;//Le decimos que se acople a su contenedor
            txt.Multiline = true;//Sera de multiples lineas
            txt.ScrollBars = ScrollBars.Vertical;//Tendra ScrollBar a lo vertical

            tp.Controls.Add(txt);//Lo insertamos en la pestaña
        }
    }
}
