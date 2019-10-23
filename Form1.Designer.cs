namespace PyUSAC
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.AreaReportes = new System.Windows.Forms.TabControl();
            this.Consola = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tablaErrores = new System.Windows.Forms.DataGridView();
            this.TipoError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fila = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Columna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AreaEdicion = new System.Windows.Forms.TabControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCrearArchivo = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAbrir = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGuardar = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGuardarComo = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCerrar = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCompilar = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGenerar = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.AreaReportes.SuspendLayout();
            this.Consola.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaErrores)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AreaReportes);
            this.panel1.Controls.Add(this.AreaEdicion);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(756, 674);
            this.panel1.TabIndex = 0;
            // 
            // AreaReportes
            // 
            this.AreaReportes.Controls.Add(this.Consola);
            this.AreaReportes.Controls.Add(this.tabPage2);
            this.AreaReportes.Location = new System.Drawing.Point(11, 398);
            this.AreaReportes.Name = "AreaReportes";
            this.AreaReportes.SelectedIndex = 0;
            this.AreaReportes.Size = new System.Drawing.Size(735, 263);
            this.AreaReportes.TabIndex = 2;
            // 
            // Consola
            // 
            this.Consola.Controls.Add(this.textBox1);
            this.Consola.Location = new System.Drawing.Point(4, 22);
            this.Consola.Name = "Consola";
            this.Consola.Padding = new System.Windows.Forms.Padding(3);
            this.Consola.Size = new System.Drawing.Size(727, 237);
            this.Consola.TabIndex = 0;
            this.Consola.Text = "Consola";
            this.Consola.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.textBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(727, 237);
            this.textBox1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tablaErrores);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(727, 237);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Errores";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tablaErrores
            // 
            this.tablaErrores.AllowUserToAddRows = false;
            this.tablaErrores.AllowUserToDeleteRows = false;
            this.tablaErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tablaErrores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TipoError,
            this.Descripcion,
            this.Fila,
            this.Columna});
            this.tablaErrores.Location = new System.Drawing.Point(0, 0);
            this.tablaErrores.Name = "tablaErrores";
            this.tablaErrores.ReadOnly = true;
            this.tablaErrores.Size = new System.Drawing.Size(724, 237);
            this.tablaErrores.TabIndex = 0;
            // 
            // TipoError
            // 
            this.TipoError.HeaderText = "Tipo de Error";
            this.TipoError.Name = "TipoError";
            this.TipoError.ReadOnly = true;
            this.TipoError.Width = 180;
            // 
            // Descripcion
            // 
            this.Descripcion.HeaderText = "Descripcion";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.ReadOnly = true;
            this.Descripcion.Width = 300;
            // 
            // Fila
            // 
            this.Fila.HeaderText = "Fila";
            this.Fila.Name = "Fila";
            this.Fila.ReadOnly = true;
            // 
            // Columna
            // 
            this.Columna.HeaderText = "Columna";
            this.Columna.Name = "Columna";
            this.Columna.ReadOnly = true;
            // 
            // AreaEdicion
            // 
            this.AreaEdicion.Location = new System.Drawing.Point(11, 40);
            this.AreaEdicion.Name = "AreaEdicion";
            this.AreaEdicion.SelectedIndex = 0;
            this.AreaEdicion.Size = new System.Drawing.Size(735, 339);
            this.AreaEdicion.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.btnCompilar,
            this.btnGenerar});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(756, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCrearArchivo,
            this.btnAbrir,
            this.btnGuardar,
            this.btnGuardarComo,
            this.btnCerrar});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // btnCrearArchivo
            // 
            this.btnCrearArchivo.Name = "btnCrearArchivo";
            this.btnCrearArchivo.Size = new System.Drawing.Size(159, 22);
            this.btnCrearArchivo.Text = "Crear archivo";
            this.btnCrearArchivo.Click += new System.EventHandler(this.BtnCrearArchivo_Click);
            // 
            // btnAbrir
            // 
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(159, 22);
            this.btnAbrir.Text = "Abrir archivo";
            this.btnAbrir.Click += new System.EventHandler(this.BtnAbrir_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(159, 22);
            this.btnGuardar.Text = "Guardar archivo";
            this.btnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // btnGuardarComo
            // 
            this.btnGuardarComo.Name = "btnGuardarComo";
            this.btnGuardarComo.Size = new System.Drawing.Size(159, 22);
            this.btnGuardarComo.Text = "Guardar como...";
            // 
            // btnCerrar
            // 
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(159, 22);
            this.btnCerrar.Text = "Cerrar archivo";
            this.btnCerrar.Click += new System.EventHandler(this.BtnCerrar_Click);
            // 
            // btnCompilar
            // 
            this.btnCompilar.Name = "btnCompilar";
            this.btnCompilar.Size = new System.Drawing.Size(68, 20);
            this.btnCompilar.Text = "Compilar";
            this.btnCompilar.Click += new System.EventHandler(this.BtnCompilar_Click);
            // 
            // btnGenerar
            // 
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(92, 20);
            this.btnGenerar.Text = "Generar Arbol";
            this.btnGenerar.Click += new System.EventHandler(this.BtnGenerar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 678);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "PyUSAC";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.AreaReportes.ResumeLayout(false);
            this.Consola.ResumeLayout(false);
            this.Consola.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablaErrores)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnCrearArchivo;
        private System.Windows.Forms.ToolStripMenuItem btnGuardar;
        private System.Windows.Forms.ToolStripMenuItem btnGuardarComo;
        private System.Windows.Forms.ToolStripMenuItem btnCerrar;
        private System.Windows.Forms.ToolStripMenuItem btnCompilar;
        private System.Windows.Forms.TabControl AreaEdicion;
        private System.Windows.Forms.TabPage Consola;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView tablaErrores;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoError;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fila;
        private System.Windows.Forms.DataGridViewTextBoxColumn Columna;
        private System.Windows.Forms.ToolStripMenuItem btnAbrir;
        public System.Windows.Forms.TabControl AreaReportes;
        private System.Windows.Forms.ToolStripMenuItem btnGenerar;
    }
}

