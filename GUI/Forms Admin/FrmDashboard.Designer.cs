namespace GUI
{
    partial class FrmDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.lblTituloDashboard = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelCursos = new System.Windows.Forms.Panel();
            this.lblInscripcionesCursos = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCursosInactivos = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCursosActivos = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalCursos = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelEventos = new System.Windows.Forms.Panel();
            this.lblInscripcionesEventos = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblEventosInactivos = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblEventosActivos = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblTotalEventos = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.chartCursos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartEventos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelPrincipal.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelCursos.SuspendLayout();
            this.panelEventos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCursos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartEventos)).BeginInit();
            this.SuspendLayout();
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.BackColor = System.Drawing.Color.White;
            this.panelPrincipal.Controls.Add(this.btnRefrescar);
            this.panelPrincipal.Controls.Add(this.lblTituloDashboard);
            this.panelPrincipal.Controls.Add(this.tableLayoutPanel1);
            this.panelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Padding = new System.Windows.Forms.Padding(20);
            this.panelPrincipal.Size = new System.Drawing.Size(841, 615);
            this.panelPrincipal.TabIndex = 0;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefrescar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.btnRefrescar.FlatAppearance.BorderSize = 0;
            this.btnRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefrescar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefrescar.ForeColor = System.Drawing.Color.White;
            this.btnRefrescar.Location = new System.Drawing.Point(668, 20);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(150, 35);
            this.btnRefrescar.TabIndex = 2;
            this.btnRefrescar.Text = "Refrescar Datos";
            this.btnRefrescar.UseVisualStyleBackColor = false;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // lblTituloDashboard
            // 
            this.lblTituloDashboard.AutoSize = true;
            this.lblTituloDashboard.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(37)))), ((int)(((byte)(84)))));
            this.lblTituloDashboard.Location = new System.Drawing.Point(23, 20);
            this.lblTituloDashboard.Name = "lblTituloDashboard";
            this.lblTituloDashboard.Size = new System.Drawing.Size(363, 37);
            this.lblTituloDashboard.TabIndex = 1;
            this.lblTituloDashboard.Text = "Dashboard de Estadísticas";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelCursos, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelEventos, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chartCursos, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chartEventos, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(23, 70);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(795, 522);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelCursos
            // 
            this.panelCursos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.panelCursos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCursos.Controls.Add(this.lblInscripcionesCursos);
            this.panelCursos.Controls.Add(this.label7);
            this.panelCursos.Controls.Add(this.lblCursosInactivos);
            this.panelCursos.Controls.Add(this.label5);
            this.panelCursos.Controls.Add(this.lblCursosActivos);
            this.panelCursos.Controls.Add(this.label3);
            this.panelCursos.Controls.Add(this.lblTotalCursos);
            this.panelCursos.Controls.Add(this.label1);
            this.panelCursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCursos.Location = new System.Drawing.Point(3, 3);
            this.panelCursos.Name = "panelCursos";
            this.panelCursos.Size = new System.Drawing.Size(391, 150);
            this.panelCursos.TabIndex = 0;
            // 
            // lblInscripcionesCursos
            // 
            this.lblInscripcionesCursos.AutoSize = true;
            this.lblInscripcionesCursos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblInscripcionesCursos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.lblInscripcionesCursos.Location = new System.Drawing.Point(290, 110);
            this.lblInscripcionesCursos.Name = "lblInscripcionesCursos";
            this.lblInscripcionesCursos.Size = new System.Drawing.Size(36, 28);
            this.lblInscripcionesCursos.TabIndex = 7;
            this.lblInscripcionesCursos.Text = "00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label7.Location = new System.Drawing.Point(290, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Inscripciones";
            // 
            // lblCursosInactivos
            // 
            this.lblCursosInactivos.AutoSize = true;
            this.lblCursosInactivos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCursosInactivos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblCursosInactivos.Location = new System.Drawing.Point(200, 110);
            this.lblCursosInactivos.Name = "lblCursosInactivos";
            this.lblCursosInactivos.Size = new System.Drawing.Size(36, 28);
            this.lblCursosInactivos.TabIndex = 5;
            this.lblCursosInactivos.Text = "00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.Location = new System.Drawing.Point(200, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Inactivos";
            // 
            // lblCursosActivos
            // 
            this.lblCursosActivos.AutoSize = true;
            this.lblCursosActivos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCursosActivos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblCursosActivos.Location = new System.Drawing.Point(110, 110);
            this.lblCursosActivos.Name = "lblCursosActivos";
            this.lblCursosActivos.Size = new System.Drawing.Size(36, 28);
            this.lblCursosActivos.TabIndex = 3;
            this.lblCursosActivos.Text = "00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.Location = new System.Drawing.Point(110, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Activos";
            // 
            // lblTotalCursos
            // 
            this.lblTotalCursos.AutoSize = true;
            this.lblTotalCursos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalCursos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(37)))), ((int)(((byte)(84)))));
            this.lblTotalCursos.Location = new System.Drawing.Point(20, 110);
            this.lblTotalCursos.Name = "lblTotalCursos";
            this.lblTotalCursos.Size = new System.Drawing.Size(36, 28);
            this.lblTotalCursos.TabIndex = 1;
            this.lblTotalCursos.Text = "00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Estadísticas de Cursos";
            // 
            // panelEventos
            // 
            this.panelEventos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.panelEventos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEventos.Controls.Add(this.lblInscripcionesEventos);
            this.panelEventos.Controls.Add(this.label8);
            this.panelEventos.Controls.Add(this.lblEventosInactivos);
            this.panelEventos.Controls.Add(this.label10);
            this.panelEventos.Controls.Add(this.lblEventosActivos);
            this.panelEventos.Controls.Add(this.label12);
            this.panelEventos.Controls.Add(this.lblTotalEventos);
            this.panelEventos.Controls.Add(this.label14);
            this.panelEventos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEventos.Location = new System.Drawing.Point(400, 3);
            this.panelEventos.Name = "panelEventos";
            this.panelEventos.Size = new System.Drawing.Size(392, 150);
            this.panelEventos.TabIndex = 1;
            // 
            // lblInscripcionesEventos
            // 
            this.lblInscripcionesEventos.AutoSize = true;
            this.lblInscripcionesEventos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblInscripcionesEventos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(191)))), ((int)(((byte)(0)))));
            this.lblInscripcionesEventos.Location = new System.Drawing.Point(290, 110);
            this.lblInscripcionesEventos.Name = "lblInscripcionesEventos";
            this.lblInscripcionesEventos.Size = new System.Drawing.Size(36, 28);
            this.lblInscripcionesEventos.TabIndex = 15;
            this.lblInscripcionesEventos.Text = "00";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label8.Location = new System.Drawing.Point(290, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "Inscripciones";
            // 
            // lblEventosInactivos
            // 
            this.lblEventosInactivos.AutoSize = true;
            this.lblEventosInactivos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblEventosInactivos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblEventosInactivos.Location = new System.Drawing.Point(200, 110);
            this.lblEventosInactivos.Name = "lblEventosInactivos";
            this.lblEventosInactivos.Size = new System.Drawing.Size(36, 28);
            this.lblEventosInactivos.TabIndex = 13;
            this.lblEventosInactivos.Text = "00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label10.Location = new System.Drawing.Point(200, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "Inactivos";
            // 
            // lblEventosActivos
            // 
            this.lblEventosActivos.AutoSize = true;
            this.lblEventosActivos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblEventosActivos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblEventosActivos.Location = new System.Drawing.Point(110, 110);
            this.lblEventosActivos.Name = "lblEventosActivos";
            this.lblEventosActivos.Size = new System.Drawing.Size(36, 28);
            this.lblEventosActivos.TabIndex = 11;
            this.lblEventosActivos.Text = "00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label12.Location = new System.Drawing.Point(110, 90);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 20);
            this.label12.TabIndex = 10;
            this.label12.Text = "Activos";
            // 
            // lblTotalEventos
            // 
            this.lblTotalEventos.AutoSize = true;
            this.lblTotalEventos.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalEventos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(37)))), ((int)(((byte)(84)))));
            this.lblTotalEventos.Location = new System.Drawing.Point(20, 110);
            this.lblTotalEventos.Name = "lblTotalEventos";
            this.lblTotalEventos.Size = new System.Drawing.Size(36, 28);
            this.lblTotalEventos.TabIndex = 9;
            this.lblTotalEventos.Text = "00";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(20, 20);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(214, 28);
            this.label14.TabIndex = 8;
            this.label14.Text = "Estadísticas de Eventos";
            // 
            // chartCursos
            // 
            this.chartCursos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.chartCursos.BorderlineColor = System.Drawing.Color.Silver;
            this.chartCursos.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.chartCursos.ChartAreas.Add(chartArea1);
            this.chartCursos.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartCursos.Legends.Add(legend1);
            this.chartCursos.Location = new System.Drawing.Point(3, 159);
            this.chartCursos.Name = "chartCursos";
            this.chartCursos.Size = new System.Drawing.Size(391, 360);
            this.chartCursos.TabIndex = 2;
            this.chartCursos.Text = "chart1";
            // 
            // chartEventos
            // 
            this.chartEventos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.chartEventos.BorderlineColor = System.Drawing.Color.Silver;
            this.chartEventos.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea2.Name = "ChartArea1";
            this.chartEventos.ChartAreas.Add(chartArea2);
            this.chartEventos.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartEventos.Legends.Add(legend2);
            this.chartEventos.Location = new System.Drawing.Point(400, 159);
            this.chartEventos.Name = "chartEventos";
            this.chartEventos.Size = new System.Drawing.Size(392, 360);
            this.chartEventos.TabIndex = 3;
            this.chartEventos.Text = "chart2";
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 615);
            this.Controls.Add(this.panelPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmDashboard";
            this.Text = "Dashboard";
            this.panelPrincipal.ResumeLayout(false);
            this.panelPrincipal.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelCursos.ResumeLayout(false);
            this.panelCursos.PerformLayout();
            this.panelEventos.ResumeLayout(false);
            this.panelEventos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCursos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartEventos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPrincipal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelCursos;
        private System.Windows.Forms.Panel panelEventos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCursos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartEventos;
        private System.Windows.Forms.Label lblTituloDashboard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalCursos;
        private System.Windows.Forms.Label lblInscripcionesCursos;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCursosInactivos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCursosActivos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblInscripcionesEventos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblEventosInactivos;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblEventosActivos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblTotalEventos;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnRefrescar;
    }
// To fix the error CS0234, you need to ensure that the required assembly reference for  
// System.Windows.Forms.DataVisualization is added to your project.  

// Step 1: Add a reference to the "System.Windows.Forms.DataVisualization" assembly.  
// In Visual Studio, right-click on your project in the Solution Explorer,  
// select "Add Reference...", go to the ".NET" tab, and search for "System.Windows.Forms.DataVisualization".  
// Add it to your project.  

// Step 2: Ensure the namespace is correctly imported at the top of your file.  
using System.Windows.Forms.DataVisualization.Charting;
}