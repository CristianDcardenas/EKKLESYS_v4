using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class FrmInicio2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInicio2));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnRegistrarse = new System.Windows.Forms.Button();
            this.btnIniciarSesion = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.btnContacto = new System.Windows.Forms.Button();
            this.btnEventos = new System.Windows.Forms.Button();
            this.btnCursos = new System.Windows.Forms.Button();
            this.btnNosotros = new System.Windows.Forms.Button();
            this.btnInicio = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabInicio = new System.Windows.Forms.TabPage();
            this.panelBienvenida = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.lblDescripcionBienvenida = new System.Windows.Forms.Label();
            this.btnComenzarAhora = new System.Windows.Forms.Button();
            this.pictureBoxDashboard = new System.Windows.Forms.PictureBox();
            this.tabCursos = new System.Windows.Forms.TabPage();
            this.flpCursos = new System.Windows.Forms.FlowLayoutPanel();
            this.tabEventos = new System.Windows.Forms.TabPage();
            this.flpEventos = new System.Windows.Forms.FlowLayoutPanel();
            this.tabContacto = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelInfoContacto = new System.Windows.Forms.Panel();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.lblDireccionTitulo = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmailTitulo = new System.Windows.Forms.Label();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.lblTelefonoTitulo = new System.Windows.Forms.Label();
            this.panelFormContacto = new System.Windows.Forms.Panel();
            this.btnEnviarMensaje = new System.Windows.Forms.Button();
            this.txtMensaje = new System.Windows.Forms.TextBox();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.txtAsunto = new System.Windows.Forms.TextBox();
            this.lblAsunto = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmailForm = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblTituloContacto = new System.Windows.Forms.Label();
            this.lblDescripcionContacto = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.linkAyuda = new System.Windows.Forms.LinkLabel();
            this.linkPrivacidad = new System.Windows.Forms.LinkLabel();
            this.linkTerminos = new System.Windows.Forms.LinkLabel();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblFooterLogo = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabInicio.SuspendLayout();
            this.panelBienvenida.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDashboard)).BeginInit();
            this.tabCursos.SuspendLayout();
            this.tabEventos.SuspendLayout();
            this.tabContacto.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelInfoContacto.SuspendLayout();
            this.panelFormContacto.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.panelHeader.Controls.Add(this.btnRegistrarse);
            this.panelHeader.Controls.Add(this.btnIniciarSesion);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1365, 74);
            this.panelHeader.TabIndex = 0;
            // 
            // btnRegistrarse
            // 
            this.btnRegistrarse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegistrarse.BackColor = System.Drawing.Color.White;
            this.btnRegistrarse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegistrarse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrarse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.btnRegistrarse.Location = new System.Drawing.Point(1216, 18);
            this.btnRegistrarse.Margin = new System.Windows.Forms.Padding(4);
            this.btnRegistrarse.Name = "btnRegistrarse";
            this.btnRegistrarse.Size = new System.Drawing.Size(133, 37);
            this.btnRegistrarse.TabIndex = 2;
            this.btnRegistrarse.Text = "Registrarse";
            this.btnRegistrarse.UseVisualStyleBackColor = false;
            this.btnRegistrarse.Click += new System.EventHandler(this.btnRegistrarse_Click);
            // 
            // btnIniciarSesion
            // 
            this.btnIniciarSesion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIniciarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.btnIniciarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIniciarSesion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciarSesion.ForeColor = System.Drawing.Color.White;
            this.btnIniciarSesion.Location = new System.Drawing.Point(1061, 18);
            this.btnIniciarSesion.Margin = new System.Windows.Forms.Padding(4);
            this.btnIniciarSesion.Name = "btnIniciarSesion";
            this.btnIniciarSesion.Size = new System.Drawing.Size(147, 37);
            this.btnIniciarSesion.TabIndex = 1;
            this.btnIniciarSesion.Text = "Iniciar Sesión";
            this.btnIniciarSesion.UseVisualStyleBackColor = false;
            this.btnIniciarSesion.Click += new System.EventHandler(this.btnIniciarSesion_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(16, 18);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(125, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "EKKLESYS";
            // 
            // panelNavigation
            // 
            this.panelNavigation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(59)))), ((int)(((byte)(110)))));
            this.panelNavigation.Controls.Add(this.btnContacto);
            this.panelNavigation.Controls.Add(this.btnEventos);
            this.panelNavigation.Controls.Add(this.btnCursos);
            this.panelNavigation.Controls.Add(this.btnNosotros);
            this.panelNavigation.Controls.Add(this.btnInicio);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNavigation.Location = new System.Drawing.Point(0, 74);
            this.panelNavigation.Margin = new System.Windows.Forms.Padding(4);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(1365, 49);
            this.panelNavigation.TabIndex = 1;
            // 
            // btnContacto
            // 
            this.btnContacto.BackColor = System.Drawing.Color.Transparent;
            this.btnContacto.FlatAppearance.BorderSize = 0;
            this.btnContacto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContacto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContacto.ForeColor = System.Drawing.Color.White;
            this.btnContacto.Location = new System.Drawing.Point(533, 0);
            this.btnContacto.Margin = new System.Windows.Forms.Padding(4);
            this.btnContacto.Name = "btnContacto";
            this.btnContacto.Size = new System.Drawing.Size(133, 49);
            this.btnContacto.TabIndex = 4;
            this.btnContacto.Text = "Contacto";
            this.btnContacto.UseVisualStyleBackColor = false;
            this.btnContacto.Click += new System.EventHandler(this.btnContacto_Click);
            // 
            // btnEventos
            // 
            this.btnEventos.BackColor = System.Drawing.Color.Transparent;
            this.btnEventos.FlatAppearance.BorderSize = 0;
            this.btnEventos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEventos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEventos.ForeColor = System.Drawing.Color.White;
            this.btnEventos.Location = new System.Drawing.Point(400, 0);
            this.btnEventos.Margin = new System.Windows.Forms.Padding(4);
            this.btnEventos.Name = "btnEventos";
            this.btnEventos.Size = new System.Drawing.Size(133, 49);
            this.btnEventos.TabIndex = 3;
            this.btnEventos.Text = "Eventos";
            this.btnEventos.UseVisualStyleBackColor = false;
            this.btnEventos.Click += new System.EventHandler(this.btnEventos_Click);
            // 
            // btnCursos
            // 
            this.btnCursos.BackColor = System.Drawing.Color.Transparent;
            this.btnCursos.FlatAppearance.BorderSize = 0;
            this.btnCursos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCursos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCursos.ForeColor = System.Drawing.Color.White;
            this.btnCursos.Location = new System.Drawing.Point(267, 0);
            this.btnCursos.Margin = new System.Windows.Forms.Padding(4);
            this.btnCursos.Name = "btnCursos";
            this.btnCursos.Size = new System.Drawing.Size(133, 49);
            this.btnCursos.TabIndex = 2;
            this.btnCursos.Text = "Cursos";
            this.btnCursos.UseVisualStyleBackColor = false;
            this.btnCursos.Click += new System.EventHandler(this.btnCursos_Click);
            // 
            // btnNosotros
            // 
            this.btnNosotros.BackColor = System.Drawing.Color.Transparent;
            this.btnNosotros.FlatAppearance.BorderSize = 0;
            this.btnNosotros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNosotros.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNosotros.ForeColor = System.Drawing.Color.White;
            this.btnNosotros.Location = new System.Drawing.Point(133, 0);
            this.btnNosotros.Margin = new System.Windows.Forms.Padding(4);
            this.btnNosotros.Name = "btnNosotros";
            this.btnNosotros.Size = new System.Drawing.Size(133, 49);
            this.btnNosotros.TabIndex = 1;
            this.btnNosotros.Text = "Nosotros";
            this.btnNosotros.UseVisualStyleBackColor = false;
            this.btnNosotros.Click += new System.EventHandler(this.btnConocerMas_Click);
            // 
            // btnInicio
            // 
            this.btnInicio.BackColor = System.Drawing.Color.Transparent;
            this.btnInicio.FlatAppearance.BorderSize = 0;
            this.btnInicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInicio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInicio.ForeColor = System.Drawing.Color.White;
            this.btnInicio.Location = new System.Drawing.Point(0, 0);
            this.btnInicio.Margin = new System.Windows.Forms.Padding(4);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Size = new System.Drawing.Size(133, 49);
            this.btnInicio.TabIndex = 0;
            this.btnInicio.Text = "Inicio";
            this.btnInicio.UseVisualStyleBackColor = false;
            this.btnInicio.Click += new System.EventHandler(this.btnInicio_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlMain.Controls.Add(this.tabInicio);
            this.tabControlMain.Controls.Add(this.tabCursos);
            this.tabControlMain.Controls.Add(this.tabEventos);
            this.tabControlMain.Controls.Add(this.tabContacto);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ItemSize = new System.Drawing.Size(0, 1);
            this.tabControlMain.Location = new System.Drawing.Point(0, 123);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1365, 637);
            this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlMain.TabIndex = 2;
            // 
            // tabInicio
            // 
            this.tabInicio.Controls.Add(this.panelBienvenida);
            this.tabInicio.Location = new System.Drawing.Point(4, 5);
            this.tabInicio.Margin = new System.Windows.Forms.Padding(4);
            this.tabInicio.Name = "tabInicio";
            this.tabInicio.Padding = new System.Windows.Forms.Padding(4);
            this.tabInicio.Size = new System.Drawing.Size(1357, 628);
            this.tabInicio.TabIndex = 0;
            this.tabInicio.Text = "Inicio";
            this.tabInicio.UseVisualStyleBackColor = true;
            // 
            // panelBienvenida
            // 
            this.panelBienvenida.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.panelBienvenida.Controls.Add(this.panel1);
            this.panelBienvenida.Controls.Add(this.pictureBoxDashboard);
            this.panelBienvenida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBienvenida.Location = new System.Drawing.Point(4, 4);
            this.panelBienvenida.Margin = new System.Windows.Forms.Padding(4);
            this.panelBienvenida.Name = "panelBienvenida";
            this.panelBienvenida.Size = new System.Drawing.Size(1349, 620);
            this.panelBienvenida.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblBienvenida);
            this.panel1.Controls.Add(this.lblDescripcionBienvenida);
            this.panel1.Controls.Add(this.btnComenzarAhora);
            this.panel1.Location = new System.Drawing.Point(13, 159);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(579, 284);
            this.panel1.TabIndex = 5;
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBienvenida.ForeColor = System.Drawing.Color.White;
            this.lblBienvenida.Location = new System.Drawing.Point(27, 17);
            this.lblBienvenida.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(460, 54);
            this.lblBienvenida.TabIndex = 1;
            this.lblBienvenida.Text = "Bienvenido a EKKLESYS";
            // 
            // lblDescripcionBienvenida
            // 
            this.lblDescripcionBienvenida.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.lblDescripcionBienvenida.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionBienvenida.ForeColor = System.Drawing.Color.White;
            this.lblDescripcionBienvenida.Location = new System.Drawing.Point(32, 91);
            this.lblDescripcionBienvenida.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescripcionBienvenida.Name = "lblDescripcionBienvenida";
            this.lblDescripcionBienvenida.Size = new System.Drawing.Size(533, 98);
            this.lblDescripcionBienvenida.TabIndex = 2;
            this.lblDescripcionBienvenida.Text = "Conoce los cursos que tenemos para el crecimiento de tu ministerio y los próximos" +
    " eventos de nuestra iglesia, en una plataforma moderna y fácil de usar.";
            // 
            // btnComenzarAhora
            // 
            this.btnComenzarAhora.BackColor = System.Drawing.Color.White;
            this.btnComenzarAhora.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComenzarAhora.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComenzarAhora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.btnComenzarAhora.Location = new System.Drawing.Point(205, 193);
            this.btnComenzarAhora.Margin = new System.Windows.Forms.Padding(4);
            this.btnComenzarAhora.Name = "btnComenzarAhora";
            this.btnComenzarAhora.Size = new System.Drawing.Size(160, 43);
            this.btnComenzarAhora.TabIndex = 3;
            this.btnComenzarAhora.Text = "Comenzar Ahora";
            this.btnComenzarAhora.UseVisualStyleBackColor = false;
            // 
            // pictureBoxDashboard
            // 
            this.pictureBoxDashboard.BackColor = System.Drawing.Color.White;
            this.pictureBoxDashboard.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxDashboard.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxDashboard.Image")));
            this.pictureBoxDashboard.Location = new System.Drawing.Point(208, 0);
            this.pictureBoxDashboard.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxDashboard.Name = "pictureBoxDashboard";
            this.pictureBoxDashboard.Size = new System.Drawing.Size(1141, 620);
            this.pictureBoxDashboard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDashboard.TabIndex = 0;
            this.pictureBoxDashboard.TabStop = false;
            // 
            // tabCursos
            // 
            this.tabCursos.Controls.Add(this.flpCursos);
            this.tabCursos.Location = new System.Drawing.Point(4, 5);
            this.tabCursos.Margin = new System.Windows.Forms.Padding(4);
            this.tabCursos.Name = "tabCursos";
            this.tabCursos.Padding = new System.Windows.Forms.Padding(4);
            this.tabCursos.Size = new System.Drawing.Size(1357, 628);
            this.tabCursos.TabIndex = 1;
            this.tabCursos.Text = "Cursos";
            this.tabCursos.UseVisualStyleBackColor = true;
            // 
            // flpCursos
            // 
            this.flpCursos.AutoScroll = true;
            this.flpCursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCursos.Location = new System.Drawing.Point(4, 4);
            this.flpCursos.Margin = new System.Windows.Forms.Padding(4);
            this.flpCursos.Name = "flpCursos";
            this.flpCursos.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.flpCursos.Size = new System.Drawing.Size(1349, 620);
            this.flpCursos.TabIndex = 0;
            // 
            // tabEventos
            // 
            this.tabEventos.Controls.Add(this.flpEventos);
            this.tabEventos.Location = new System.Drawing.Point(4, 5);
            this.tabEventos.Margin = new System.Windows.Forms.Padding(4);
            this.tabEventos.Name = "tabEventos";
            this.tabEventos.Padding = new System.Windows.Forms.Padding(4);
            this.tabEventos.Size = new System.Drawing.Size(1357, 628);
            this.tabEventos.TabIndex = 2;
            this.tabEventos.Text = "Eventos";
            this.tabEventos.UseVisualStyleBackColor = true;
            // 
            // flpEventos
            // 
            this.flpEventos.AutoScroll = true;
            this.flpEventos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpEventos.Location = new System.Drawing.Point(4, 4);
            this.flpEventos.Margin = new System.Windows.Forms.Padding(4);
            this.flpEventos.Name = "flpEventos";
            this.flpEventos.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.flpEventos.Size = new System.Drawing.Size(1349, 620);
            this.flpEventos.TabIndex = 0;
            // 
            // tabContacto
            // 
            this.tabContacto.Controls.Add(this.tableLayoutPanel1);
            this.tabContacto.Controls.Add(this.lblTituloContacto);
            this.tabContacto.Controls.Add(this.lblDescripcionContacto);
            this.tabContacto.Location = new System.Drawing.Point(4, 5);
            this.tabContacto.Margin = new System.Windows.Forms.Padding(4);
            this.tabContacto.Name = "tabContacto";
            this.tabContacto.Padding = new System.Windows.Forms.Padding(4);
            this.tabContacto.Size = new System.Drawing.Size(1357, 628);
            this.tabContacto.TabIndex = 4;
            this.tabContacto.Text = "Contacto";
            this.tabContacto.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.panelInfoContacto, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelFormContacto, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(27, 123);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1301, 458);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panelInfoContacto
            // 
            this.panelInfoContacto.BackColor = System.Drawing.Color.White;
            this.panelInfoContacto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInfoContacto.Controls.Add(this.lblDireccion);
            this.panelInfoContacto.Controls.Add(this.lblDireccionTitulo);
            this.panelInfoContacto.Controls.Add(this.lblEmail);
            this.panelInfoContacto.Controls.Add(this.lblEmailTitulo);
            this.panelInfoContacto.Controls.Add(this.lblTelefono);
            this.panelInfoContacto.Controls.Add(this.lblTelefonoTitulo);
            this.panelInfoContacto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoContacto.Location = new System.Drawing.Point(4, 4);
            this.panelInfoContacto.Margin = new System.Windows.Forms.Padding(4);
            this.panelInfoContacto.Name = "panelInfoContacto";
            this.panelInfoContacto.Padding = new System.Windows.Forms.Padding(27, 25, 27, 25);
            this.panelInfoContacto.Size = new System.Drawing.Size(512, 450);
            this.panelInfoContacto.TabIndex = 0;
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccion.ForeColor = System.Drawing.Color.Gray;
            this.lblDireccion.Location = new System.Drawing.Point(31, 295);
            this.lblDireccion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(185, 20);
            this.lblDireccion.TabIndex = 5;
            this.lblDireccion.Text = "Calle Principal 123, Ciudad";
            // 
            // lblDireccionTitulo
            // 
            this.lblDireccionTitulo.AutoSize = true;
            this.lblDireccionTitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccionTitulo.Location = new System.Drawing.Point(31, 258);
            this.lblDireccionTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDireccionTitulo.Name = "lblDireccionTitulo";
            this.lblDireccionTitulo.Size = new System.Drawing.Size(85, 23);
            this.lblDireccionTitulo.TabIndex = 4;
            this.lblDireccionTitulo.Text = "Dirección";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.Gray;
            this.lblEmail.Location = new System.Drawing.Point(31, 185);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(166, 20);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "contacto@ekklesys.com";
            // 
            // lblEmailTitulo
            // 
            this.lblEmailTitulo.AutoSize = true;
            this.lblEmailTitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailTitulo.Location = new System.Drawing.Point(31, 148);
            this.lblEmailTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmailTitulo.Name = "lblEmailTitulo";
            this.lblEmailTitulo.Size = new System.Drawing.Size(54, 23);
            this.lblEmailTitulo.TabIndex = 2;
            this.lblEmailTitulo.Text = "Email";
            // 
            // lblTelefono
            // 
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefono.ForeColor = System.Drawing.Color.Gray;
            this.lblTelefono.Location = new System.Drawing.Point(31, 74);
            this.lblTelefono.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(111, 20);
            this.lblTelefono.TabIndex = 1;
            this.lblTelefono.Text = "+1 234 567 890";
            // 
            // lblTelefonoTitulo
            // 
            this.lblTelefonoTitulo.AutoSize = true;
            this.lblTelefonoTitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefonoTitulo.Location = new System.Drawing.Point(31, 37);
            this.lblTelefonoTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTelefonoTitulo.Name = "lblTelefonoTitulo";
            this.lblTelefonoTitulo.Size = new System.Drawing.Size(78, 23);
            this.lblTelefonoTitulo.TabIndex = 0;
            this.lblTelefonoTitulo.Text = "Teléfono";
            // 
            // panelFormContacto
            // 
            this.panelFormContacto.BackColor = System.Drawing.Color.White;
            this.panelFormContacto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFormContacto.Controls.Add(this.btnEnviarMensaje);
            this.panelFormContacto.Controls.Add(this.txtMensaje);
            this.panelFormContacto.Controls.Add(this.lblMensaje);
            this.panelFormContacto.Controls.Add(this.txtAsunto);
            this.panelFormContacto.Controls.Add(this.lblAsunto);
            this.panelFormContacto.Controls.Add(this.txtEmail);
            this.panelFormContacto.Controls.Add(this.lblEmailForm);
            this.panelFormContacto.Controls.Add(this.txtNombre);
            this.panelFormContacto.Controls.Add(this.lblNombre);
            this.panelFormContacto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFormContacto.Location = new System.Drawing.Point(524, 4);
            this.panelFormContacto.Margin = new System.Windows.Forms.Padding(4);
            this.panelFormContacto.Name = "panelFormContacto";
            this.panelFormContacto.Padding = new System.Windows.Forms.Padding(27, 25, 27, 25);
            this.panelFormContacto.Size = new System.Drawing.Size(773, 450);
            this.panelFormContacto.TabIndex = 1;
            // 
            // btnEnviarMensaje
            // 
            this.btnEnviarMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnviarMensaje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.btnEnviarMensaje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarMensaje.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviarMensaje.ForeColor = System.Drawing.Color.White;
            this.btnEnviarMensaje.Location = new System.Drawing.Point(31, 383);
            this.btnEnviarMensaje.Margin = new System.Windows.Forms.Padding(4);
            this.btnEnviarMensaje.Name = "btnEnviarMensaje";
            this.btnEnviarMensaje.Size = new System.Drawing.Size(709, 43);
            this.btnEnviarMensaje.TabIndex = 8;
            this.btnEnviarMensaje.Text = "Enviar Mensaje";
            this.btnEnviarMensaje.UseVisualStyleBackColor = false;
            // 
            // txtMensaje
            // 
            this.txtMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMensaje.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMensaje.Location = new System.Drawing.Point(31, 252);
            this.txtMensaje.Margin = new System.Windows.Forms.Padding(4);
            this.txtMensaje.Multiline = true;
            this.txtMensaje.Name = "txtMensaje";
            this.txtMensaje.Size = new System.Drawing.Size(708, 122);
            this.txtMensaje.TabIndex = 7;
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.Location = new System.Drawing.Point(31, 228);
            this.lblMensaje.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(64, 20);
            this.lblMensaje.TabIndex = 6;
            this.lblMensaje.Text = "Mensaje";
            // 
            // txtAsunto
            // 
            this.txtAsunto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAsunto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAsunto.Location = new System.Drawing.Point(31, 191);
            this.txtAsunto.Margin = new System.Windows.Forms.Padding(4);
            this.txtAsunto.Name = "txtAsunto";
            this.txtAsunto.Size = new System.Drawing.Size(708, 27);
            this.txtAsunto.TabIndex = 5;
            // 
            // lblAsunto
            // 
            this.lblAsunto.AutoSize = true;
            this.lblAsunto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsunto.Location = new System.Drawing.Point(31, 166);
            this.lblAsunto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAsunto.Name = "lblAsunto";
            this.lblAsunto.Size = new System.Drawing.Size(55, 20);
            this.lblAsunto.TabIndex = 4;
            this.lblAsunto.Text = "Asunto";
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(391, 62);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(348, 27);
            this.txtEmail.TabIndex = 3;
            // 
            // lblEmailForm
            // 
            this.lblEmailForm.AutoSize = true;
            this.lblEmailForm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailForm.Location = new System.Drawing.Point(391, 37);
            this.lblEmailForm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmailForm.Name = "lblEmailForm";
            this.lblEmailForm.Size = new System.Drawing.Size(46, 20);
            this.lblEmailForm.TabIndex = 2;
            this.lblEmailForm.Text = "Email";
            // 
            // txtNombre
            // 
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.Location = new System.Drawing.Point(31, 62);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(4);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(332, 27);
            this.txtNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(31, 37);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(64, 20);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre";
            // 
            // lblTituloContacto
            // 
            this.lblTituloContacto.AutoSize = true;
            this.lblTituloContacto.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloContacto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.lblTituloContacto.Location = new System.Drawing.Point(580, 25);
            this.lblTituloContacto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTituloContacto.Name = "lblTituloContacto";
            this.lblTituloContacto.Size = new System.Drawing.Size(192, 41);
            this.lblTituloContacto.TabIndex = 0;
            this.lblTituloContacto.Text = "Contáctanos";
            this.lblTituloContacto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDescripcionContacto
            // 
            this.lblDescripcionContacto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionContacto.ForeColor = System.Drawing.Color.Gray;
            this.lblDescripcionContacto.Location = new System.Drawing.Point(344, 74);
            this.lblDescripcionContacto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescripcionContacto.Name = "lblDescripcionContacto";
            this.lblDescripcionContacto.Size = new System.Drawing.Size(667, 25);
            this.lblDescripcionContacto.TabIndex = 1;
            this.lblDescripcionContacto.Text = "Estamos aquí para ayudarte con cualquier consulta sobre nuestro sistema.";
            this.lblDescripcionContacto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(43)))), ((int)(((byte)(94)))));
            this.panelFooter.Controls.Add(this.linkAyuda);
            this.panelFooter.Controls.Add(this.linkPrivacidad);
            this.panelFooter.Controls.Add(this.linkTerminos);
            this.panelFooter.Controls.Add(this.lblCopyright);
            this.panelFooter.Controls.Add(this.lblFooterLogo);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 760);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(4);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1365, 62);
            this.panelFooter.TabIndex = 3;
            // 
            // linkAyuda
            // 
            this.linkAyuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkAyuda.AutoSize = true;
            this.linkAyuda.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkAyuda.LinkColor = System.Drawing.Color.White;
            this.linkAyuda.Location = new System.Drawing.Point(1293, 21);
            this.linkAyuda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkAyuda.Name = "linkAyuda";
            this.linkAyuda.Size = new System.Drawing.Size(51, 20);
            this.linkAyuda.TabIndex = 4;
            this.linkAyuda.TabStop = true;
            this.linkAyuda.Text = "Ayuda";
            // 
            // linkPrivacidad
            // 
            this.linkPrivacidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkPrivacidad.AutoSize = true;
            this.linkPrivacidad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkPrivacidad.LinkColor = System.Drawing.Color.White;
            this.linkPrivacidad.Location = new System.Drawing.Point(1200, 21);
            this.linkPrivacidad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkPrivacidad.Name = "linkPrivacidad";
            this.linkPrivacidad.Size = new System.Drawing.Size(78, 20);
            this.linkPrivacidad.TabIndex = 3;
            this.linkPrivacidad.TabStop = true;
            this.linkPrivacidad.Text = "Privacidad";
            // 
            // linkTerminos
            // 
            this.linkTerminos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkTerminos.AutoSize = true;
            this.linkTerminos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkTerminos.LinkColor = System.Drawing.Color.White;
            this.linkTerminos.Location = new System.Drawing.Point(1120, 21);
            this.linkTerminos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkTerminos.Name = "linkTerminos";
            this.linkTerminos.Size = new System.Drawing.Size(69, 20);
            this.linkTerminos.TabIndex = 2;
            this.linkTerminos.TabStop = true;
            this.linkTerminos.Text = "Términos";
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.ForeColor = System.Drawing.Color.White;
            this.lblCopyright.Location = new System.Drawing.Point(507, 21);
            this.lblCopyright.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(339, 20);
            this.lblCopyright.TabIndex = 1;
            this.lblCopyright.Text = "© 2025 EKKLESYS. Todos los derechos reservados.";
            // 
            // lblFooterLogo
            // 
            this.lblFooterLogo.AutoSize = true;
            this.lblFooterLogo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooterLogo.ForeColor = System.Drawing.Color.White;
            this.lblFooterLogo.Location = new System.Drawing.Point(16, 21);
            this.lblFooterLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFooterLogo.Name = "lblFooterLogo";
            this.lblFooterLogo.Size = new System.Drawing.Size(78, 20);
            this.lblFooterLogo.TabIndex = 0;
            this.lblFooterLogo.Text = "EKKLESYS";
            // 
            // FrmInicio2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1365, 822);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelNavigation);
            this.Controls.Add(this.panelHeader);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1061, 728);
            this.Name = "FrmInicio2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Gestión de Iglesia - EKKLESYS";
            this.Load += new System.EventHandler(this.FrmInicio2_LoadAsync);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelNavigation.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabInicio.ResumeLayout(false);
            this.panelBienvenida.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDashboard)).EndInit();
            this.tabCursos.ResumeLayout(false);
            this.tabEventos.ResumeLayout(false);
            this.tabContacto.ResumeLayout(false);
            this.tabContacto.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelInfoContacto.ResumeLayout(false);
            this.panelInfoContacto.PerformLayout();
            this.panelFormContacto.ResumeLayout(false);
            this.panelFormContacto.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnRegistrarse;
        private System.Windows.Forms.Button btnIniciarSesion;
        private System.Windows.Forms.Panel panelNavigation;
        private System.Windows.Forms.Button btnContacto;
        private System.Windows.Forms.Button btnEventos;
        private System.Windows.Forms.Button btnCursos;
        private System.Windows.Forms.Button btnNosotros;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabInicio;
        private System.Windows.Forms.TabPage tabCursos;
        private System.Windows.Forms.TabPage tabEventos;
        private System.Windows.Forms.TabPage tabContacto;
        private System.Windows.Forms.Panel panelBienvenida;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Label lblDescripcionBienvenida;
        private System.Windows.Forms.Button btnComenzarAhora;
        private System.Windows.Forms.PictureBox pictureBoxDashboard;
        private System.Windows.Forms.FlowLayoutPanel flpCursos;
        private System.Windows.Forms.FlowLayoutPanel flpEventos;
        private System.Windows.Forms.Label lblTituloContacto;
        private System.Windows.Forms.Label lblDescripcionContacto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelInfoContacto;
        private System.Windows.Forms.Label lblTelefonoTitulo;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.Label lblEmailTitulo;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblDireccionTitulo;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.Panel panelFormContacto;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblEmailForm;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblAsunto;
        private System.Windows.Forms.TextBox txtAsunto;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.TextBox txtMensaje;
        private System.Windows.Forms.Button btnEnviarMensaje;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblFooterLogo;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.LinkLabel linkTerminos;
        private System.Windows.Forms.LinkLabel linkPrivacidad;
        private System.Windows.Forms.LinkLabel linkAyuda;
        private Panel panel1;
    }
}
