namespace GUI
{
    partial class FrmPrincipalUser
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipalUser));
            this.pnMenuLateral = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new FontAwesome.Sharp.IconButton();
            this.btnVerPerfil = new FontAwesome.Sharp.IconButton();
            this.btnEventos = new FontAwesome.Sharp.IconButton();
            this.btnCursos = new FontAwesome.Sharp.IconButton();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.lblBienvenido = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.pnTitleBar = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.iconFormActual = new FontAwesome.Sharp.IconPictureBox();
            this.pnSeparador = new System.Windows.Forms.Panel();
            this.pnContenedor = new System.Windows.Forms.Panel();
            this.pnMenuLateral.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnTitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconFormActual)).BeginInit();
            this.SuspendLayout();
            // 
            // pnMenuLateral
            // 
            this.pnMenuLateral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(37)))), ((int)(((byte)(84)))));
            this.pnMenuLateral.Controls.Add(this.btnCerrarSesion);
            this.pnMenuLateral.Controls.Add(this.btnVerPerfil);
            this.pnMenuLateral.Controls.Add(this.btnEventos);
            this.pnMenuLateral.Controls.Add(this.btnCursos);
            this.pnMenuLateral.Controls.Add(this.panelLogo);
            this.pnMenuLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnMenuLateral.Location = new System.Drawing.Point(0, 0);
            this.pnMenuLateral.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnMenuLateral.Name = "pnMenuLateral";
            this.pnMenuLateral.Size = new System.Drawing.Size(259, 700);
            this.pnMenuLateral.TabIndex = 0;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.Cyan;
            this.btnCerrarSesion.IconChar = FontAwesome.Sharp.IconChar.RightFromBracket;
            this.btnCerrarSesion.IconColor = System.Drawing.Color.Cyan;
            this.btnCerrarSesion.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCerrarSesion.IconSize = 32;
            this.btnCerrarSesion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.Location = new System.Drawing.Point(0, 638);
            this.btnCerrarSesion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Padding = new System.Windows.Forms.Padding(11, 0, 20, 0);
            this.btnCerrarSesion.Size = new System.Drawing.Size(259, 62);
            this.btnCerrarSesion.TabIndex = 5;
            this.btnCerrarSesion.Text = "Cerrar Sesión";
            this.btnCerrarSesion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // btnVerPerfil
            // 
            this.btnVerPerfil.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnVerPerfil.FlatAppearance.BorderSize = 0;
            this.btnVerPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerPerfil.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnVerPerfil.ForeColor = System.Drawing.Color.Cyan;
            this.btnVerPerfil.IconChar = FontAwesome.Sharp.IconChar.User;
            this.btnVerPerfil.IconColor = System.Drawing.Color.Cyan;
            this.btnVerPerfil.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnVerPerfil.IconSize = 32;
            this.btnVerPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVerPerfil.Location = new System.Drawing.Point(0, 334);
            this.btnVerPerfil.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnVerPerfil.Name = "btnVerPerfil";
            this.btnVerPerfil.Padding = new System.Windows.Forms.Padding(11, 0, 20, 0);
            this.btnVerPerfil.Size = new System.Drawing.Size(259, 62);
            this.btnVerPerfil.TabIndex = 6;
            this.btnVerPerfil.Text = "Mi Perfil";
            this.btnVerPerfil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVerPerfil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVerPerfil.UseVisualStyleBackColor = true;
            this.btnVerPerfil.Click += new System.EventHandler(this.btnVerPerfil_Click);
            // 
            // btnEventos
            // 
            this.btnEventos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEventos.FlatAppearance.BorderSize = 0;
            this.btnEventos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEventos.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnEventos.ForeColor = System.Drawing.Color.Cyan;
            this.btnEventos.IconChar = FontAwesome.Sharp.IconChar.CalendarAlt;
            this.btnEventos.IconColor = System.Drawing.Color.Cyan;
            this.btnEventos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEventos.IconSize = 32;
            this.btnEventos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEventos.Location = new System.Drawing.Point(0, 272);
            this.btnEventos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEventos.Name = "btnEventos";
            this.btnEventos.Padding = new System.Windows.Forms.Padding(11, 0, 20, 0);
            this.btnEventos.Size = new System.Drawing.Size(259, 62);
            this.btnEventos.TabIndex = 3;
            this.btnEventos.Text = "Eventos";
            this.btnEventos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEventos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEventos.UseVisualStyleBackColor = true;
            this.btnEventos.Click += new System.EventHandler(this.btnEventos_Click);
            // 
            // btnCursos
            // 
            this.btnCursos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCursos.FlatAppearance.BorderSize = 0;
            this.btnCursos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCursos.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.btnCursos.ForeColor = System.Drawing.Color.Cyan;
            this.btnCursos.IconChar = FontAwesome.Sharp.IconChar.Book;
            this.btnCursos.IconColor = System.Drawing.Color.Cyan;
            this.btnCursos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCursos.IconSize = 32;
            this.btnCursos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCursos.Location = new System.Drawing.Point(0, 210);
            this.btnCursos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCursos.Name = "btnCursos";
            this.btnCursos.Padding = new System.Windows.Forms.Padding(11, 0, 20, 0);
            this.btnCursos.Size = new System.Drawing.Size(259, 62);
            this.btnCursos.TabIndex = 2;
            this.btnCursos.Text = "Cursos";
            this.btnCursos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCursos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCursos.UseVisualStyleBackColor = true;
            this.btnCursos.Click += new System.EventHandler(this.btnCursos_Click);
            // 
            // panelLogo
            // 
            this.panelLogo.Controls.Add(this.lblBienvenido);
            this.panelLogo.Controls.Add(this.picLogo);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(259, 210);
            this.panelLogo.TabIndex = 0;
            // 
            // lblBienvenido
            // 
            this.lblBienvenido.AutoSize = true;
            this.lblBienvenido.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold);
            this.lblBienvenido.ForeColor = System.Drawing.Color.White;
            this.lblBienvenido.Location = new System.Drawing.Point(12, 162);
            this.lblBienvenido.Name = "lblBienvenido";
            this.lblBienvenido.Size = new System.Drawing.Size(103, 19);
            this.lblBienvenido.TabIndex = 1;
            this.lblBienvenido.Text = "Bienvenid@";
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(37, 23);
            this.picLogo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(165, 126);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // pnTitleBar
            // 
            this.pnTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(37)))), ((int)(((byte)(84)))));
            this.pnTitleBar.Controls.Add(this.lblTitulo);
            this.pnTitleBar.Controls.Add(this.iconFormActual);
            this.pnTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTitleBar.Location = new System.Drawing.Point(259, 0);
            this.pnTitleBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnTitleBar.Name = "pnTitleBar";
            this.pnTitleBar.Size = new System.Drawing.Size(841, 80);
            this.pnTitleBar.TabIndex = 1;
            this.pnTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnTitleBar_MouseDown);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(67, 39);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(173, 23);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Panel de Usuario";
            // 
            // iconFormActual
            // 
            this.iconFormActual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(37)))), ((int)(((byte)(84)))));
            this.iconFormActual.ForeColor = System.Drawing.Color.Cyan;
            this.iconFormActual.IconChar = FontAwesome.Sharp.IconChar.User;
            this.iconFormActual.IconColor = System.Drawing.Color.Cyan;
            this.iconFormActual.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconFormActual.IconSize = 39;
            this.iconFormActual.Location = new System.Drawing.Point(20, 23);
            this.iconFormActual.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconFormActual.Name = "iconFormActual";
            this.iconFormActual.Size = new System.Drawing.Size(40, 39);
            this.iconFormActual.TabIndex = 0;
            this.iconFormActual.TabStop = false;
            // 
            // pnSeparador
            // 
            this.pnSeparador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.pnSeparador.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnSeparador.Location = new System.Drawing.Point(259, 80);
            this.pnSeparador.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnSeparador.Name = "pnSeparador";
            this.pnSeparador.Size = new System.Drawing.Size(841, 5);
            this.pnSeparador.TabIndex = 2;
            // 
            // pnContenedor
            // 
            this.pnContenedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(37)))), ((int)(((byte)(84)))));
            this.pnContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnContenedor.Location = new System.Drawing.Point(259, 85);
            this.pnContenedor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnContenedor.Name = "pnContenedor";
            this.pnContenedor.Size = new System.Drawing.Size(841, 615);
            this.pnContenedor.TabIndex = 3;
            this.pnContenedor.Paint += new System.Windows.Forms.PaintEventHandler(this.pnContenedor_Paint);
            // 
            // FrmPrincipalUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.pnContenedor);
            this.Controls.Add(this.pnSeparador);
            this.Controls.Add(this.pnTitleBar);
            this.Controls.Add(this.pnMenuLateral);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmPrincipalUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Panel de Usuario - Sistema de Gestión de Iglesia";
            this.Load += new System.EventHandler(this.FrmPrincipalUser_Load);
            this.pnMenuLateral.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            this.panelLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnTitleBar.ResumeLayout(false);
            this.pnTitleBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconFormActual)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMenuLateral;
        private FontAwesome.Sharp.IconButton btnCerrarSesion;
        private FontAwesome.Sharp.IconButton btnEventos;
        private FontAwesome.Sharp.IconButton btnVerPerfil;
        private FontAwesome.Sharp.IconButton btnCursos;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label lblBienvenido;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Panel pnTitleBar;
        private System.Windows.Forms.Label lblTitulo;
        private FontAwesome.Sharp.IconPictureBox iconFormActual;
        private System.Windows.Forms.Panel pnSeparador;
        private System.Windows.Forms.Panel pnContenedor;
    }
}