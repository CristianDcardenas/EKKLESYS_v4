namespace GUI
{
    partial class FrmLogin
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
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.lblInstruccion = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblNoTieneCuenta = new System.Windows.Forms.Label();
            this.lblinkRegistrarse = new System.Windows.Forms.LinkLabel();
            this.bllinkKeyForgot = new System.Windows.Forms.LinkLabel();
            this.panelLineaEmail = new System.Windows.Forms.Panel();
            this.panelLineaPassword = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelDerecho = new System.Windows.Forms.Panel();
            this.checkBoxMostrarPassword = new System.Windows.Forms.CheckBox();
            this.panelIconoPassword = new System.Windows.Forms.Panel();
            this.panelIconoEmail = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelDerecho.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.White;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.ForeColor = System.Drawing.Color.DimGray;
            this.txtEmail.Location = new System.Drawing.Point(118, 205);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(347, 25);
            this.txtEmail.TabIndex = 2;
            this.txtEmail.Text = "correo@ejemplo.com";
            this.txtEmail.Enter += new System.EventHandler(this.txtEmail_Enter);
            this.txtEmail.Leave += new System.EventHandler(this.txtEmail_Leave);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.DimGray;
            this.txtPassword.Location = new System.Drawing.Point(118, 279);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.Size = new System.Drawing.Size(347, 25);
            this.txtPassword.TabIndex = 3;
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            this.btnIngresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIngresar.FlatAppearance.BorderSize = 0;
            this.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresar.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ForeColor = System.Drawing.Color.White;
            this.btnIngresar.Location = new System.Drawing.Point(88, 356);
            this.btnIngresar.Margin = new System.Windows.Forms.Padding(4);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(347, 50);
            this.btnIngresar.TabIndex = 4;
            this.btnIngresar.Text = "Iniciar sesión";
            this.btnIngresar.UseVisualStyleBackColor = false;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBienvenida.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            this.lblBienvenida.Location = new System.Drawing.Point(179, 59);
            this.lblBienvenida.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(182, 41);
            this.lblBienvenida.TabIndex = 8;
            this.lblBienvenida.Text = "Bienvenid@";
            // 
            // lblInstruccion
            // 
            this.lblInstruccion.AutoSize = true;
            this.lblInstruccion.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstruccion.ForeColor = System.Drawing.Color.Gray;
            this.lblInstruccion.Location = new System.Drawing.Point(84, 100);
            this.lblInstruccion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstruccion.Name = "lblInstruccion";
            this.lblInstruccion.Size = new System.Drawing.Size(342, 21);
            this.lblInstruccion.TabIndex = 9;
            this.lblInstruccion.Text = "Ingrese sus credenciales para acceder al sistema";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblEmail.Location = new System.Drawing.Point(84, 182);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(148, 21);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "Correo electrónico";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPassword.Location = new System.Drawing.Point(84, 255);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(92, 21);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "Contraseña";
            // 
            // lblNoTieneCuenta
            // 
            this.lblNoTieneCuenta.AutoSize = true;
            this.lblNoTieneCuenta.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoTieneCuenta.ForeColor = System.Drawing.Color.Gray;
            this.lblNoTieneCuenta.Location = new System.Drawing.Point(84, 431);
            this.lblNoTieneCuenta.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNoTieneCuenta.Name = "lblNoTieneCuenta";
            this.lblNoTieneCuenta.Size = new System.Drawing.Size(170, 21);
            this.lblNoTieneCuenta.TabIndex = 12;
            this.lblNoTieneCuenta.Text = "¿No tienes una cuenta?";
            // 
            // lblinkRegistrarse
            // 
            this.lblinkRegistrarse.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            this.lblinkRegistrarse.AutoSize = true;
            this.lblinkRegistrarse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblinkRegistrarse.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblinkRegistrarse.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            this.lblinkRegistrarse.Location = new System.Drawing.Point(320, 431);
            this.lblinkRegistrarse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblinkRegistrarse.Name = "lblinkRegistrarse";
            this.lblinkRegistrarse.Size = new System.Drawing.Size(120, 21);
            this.lblinkRegistrarse.TabIndex = 13;
            this.lblinkRegistrarse.TabStop = true;
            this.lblinkRegistrarse.Text = "Regístrate aquí";
            this.lblinkRegistrarse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblinkRegistrarse_LinkClicked);
            // 
            // bllinkKeyForgot
            // 
            this.bllinkKeyForgot.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            this.bllinkKeyForgot.AutoSize = true;
            this.bllinkKeyForgot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bllinkKeyForgot.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bllinkKeyForgot.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
            this.bllinkKeyForgot.Location = new System.Drawing.Point(253, 323);
            this.bllinkKeyForgot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.bllinkKeyForgot.Name = "bllinkKeyForgot";
            this.bllinkKeyForgot.Size = new System.Drawing.Size(187, 21);
            this.bllinkKeyForgot.TabIndex = 14;
            this.bllinkKeyForgot.TabStop = true;
            this.bllinkKeyForgot.Text = "¿Olvidaste tu contraseña?";
            // 
            // panelLineaEmail
            // 
            this.panelLineaEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panelLineaEmail.Location = new System.Drawing.Point(88, 232);
            this.panelLineaEmail.Margin = new System.Windows.Forms.Padding(4);
            this.panelLineaEmail.Name = "panelLineaEmail";
            this.panelLineaEmail.Size = new System.Drawing.Size(347, 2);
            this.panelLineaEmail.TabIndex = 16;
            // 
            // panelLineaPassword
            // 
            this.panelLineaPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panelLineaPassword.Location = new System.Drawing.Point(88, 306);
            this.panelLineaPassword.Margin = new System.Windows.Forms.Padding(4);
            this.panelLineaPassword.Name = "panelLineaPassword";
            this.panelLineaPassword.Size = new System.Drawing.Size(347, 2);
            this.panelLineaPassword.TabIndex = 17;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::GUI.Properties.Resources.ChatGPT_Image_19_may_2025__12_45_17;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(581, 550);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // panelDerecho
            // 
            this.panelDerecho.BackColor = System.Drawing.Color.White;
            this.panelDerecho.Controls.Add(this.checkBoxMostrarPassword);
            this.panelDerecho.Controls.Add(this.panelIconoPassword);
            this.panelDerecho.Controls.Add(this.panelIconoEmail);
            this.panelDerecho.Controls.Add(this.lblBienvenida);
            this.panelDerecho.Controls.Add(this.lblInstruccion);
            this.panelDerecho.Controls.Add(this.lblEmail);
            this.panelDerecho.Controls.Add(this.txtEmail);
            this.panelDerecho.Controls.Add(this.panelLineaEmail);
            this.panelDerecho.Controls.Add(this.lblPassword);
            this.panelDerecho.Controls.Add(this.txtPassword);
            this.panelDerecho.Controls.Add(this.panelLineaPassword);
            this.panelDerecho.Controls.Add(this.bllinkKeyForgot);
            this.panelDerecho.Controls.Add(this.btnIngresar);
            this.panelDerecho.Controls.Add(this.lblNoTieneCuenta);
            this.panelDerecho.Controls.Add(this.lblinkRegistrarse);
            this.panelDerecho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho.Location = new System.Drawing.Point(581, 0);
            this.panelDerecho.Name = "panelDerecho";
            this.panelDerecho.Size = new System.Drawing.Size(516, 550);
            this.panelDerecho.TabIndex = 18;
            // 
            // checkBoxMostrarPassword
            // 
            this.checkBoxMostrarPassword.AutoSize = true;
            this.checkBoxMostrarPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBoxMostrarPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxMostrarPassword.ForeColor = System.Drawing.Color.Gray;
            this.checkBoxMostrarPassword.Location = new System.Drawing.Point(88, 323);
            this.checkBoxMostrarPassword.Name = "checkBoxMostrarPassword";
            this.checkBoxMostrarPassword.Size = new System.Drawing.Size(158, 24);
            this.checkBoxMostrarPassword.TabIndex = 20;
            this.checkBoxMostrarPassword.Text = "Mostrar contraseña";
            this.checkBoxMostrarPassword.UseVisualStyleBackColor = true;
            this.checkBoxMostrarPassword.CheckedChanged += new System.EventHandler(this.checkBoxMostrarPassword_CheckedChanged);
            // 
            // panelIconoPassword
            // 
            this.panelIconoPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelIconoPassword.Location = new System.Drawing.Point(88, 279);
            this.panelIconoPassword.Name = "panelIconoPassword";
            this.panelIconoPassword.Size = new System.Drawing.Size(25, 25);
            this.panelIconoPassword.TabIndex = 19;
            // 
            // panelIconoEmail
            // 
            this.panelIconoEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelIconoEmail.Location = new System.Drawing.Point(88, 205);
            this.panelIconoEmail.Name = "panelIconoEmail";
            this.panelIconoEmail.Size = new System.Drawing.Size(25, 25);
            this.panelIconoEmail.TabIndex = 18;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1097, 550);
            this.Controls.Add(this.panelDerecho);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio de sesión - EKKELSYS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLogin_FormClosed);
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelDerecho.ResumeLayout(false);
            this.panelDerecho.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Label lblInstruccion;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblNoTieneCuenta;
        private System.Windows.Forms.LinkLabel lblinkRegistrarse;
        private System.Windows.Forms.LinkLabel bllinkKeyForgot;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelLineaEmail;
        private System.Windows.Forms.Panel panelLineaPassword;
        private System.Windows.Forms.Panel panelDerecho;
        private System.Windows.Forms.Panel panelIconoEmail;
        private System.Windows.Forms.Panel panelIconoPassword;
        private System.Windows.Forms.CheckBox checkBoxMostrarPassword;
    }
}