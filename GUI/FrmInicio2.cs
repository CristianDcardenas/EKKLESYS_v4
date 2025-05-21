using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmInicio2 : Form
    {
        private readonly CursoService cursoService;
        private List<CursoDTO> cursos;
        private readonly EventoService eventoService;
        private List<EventoDTO> eventos;

        public FrmInicio2()
        {
            InitializeComponent();
            ConfigureTabControl();
            SetActiveTab(tabInicio); // Mostrar la pestaña de inicio por defecto
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            cursoService = new CursoService();
            eventoService = new EventoService();
        }

        private void ConfigureTabControl()
        {
            // Ocultar los headers de las pestañas
            tabControlMain.Appearance = TabAppearance.FlatButtons;
            tabControlMain.ItemSize = new Size(0, 1);
            tabControlMain.SizeMode = TabSizeMode.Fixed;
        }

        private void SetActiveTab(TabPage tabPage)
        {
            tabControlMain.SelectTab(tabPage);

            // Actualizar el estado de los botones de navegación
            btnInicio.BackColor = tabPage == tabInicio ? Color.FromArgb(60, 80, 140) : Color.Transparent;
            btnCursos.BackColor = tabPage == tabCursos ? Color.FromArgb(60, 80, 140) : Color.Transparent;
            btnEventos.BackColor = tabPage == tabEventos ? Color.FromArgb(60, 80, 140) : Color.Transparent;
            btnContacto.BackColor = tabPage == tabContacto ? Color.FromArgb(60, 80, 140) : Color.Transparent;
        }



        private void CargarCursos()
        {
            cursos = cursoService.ConsultarDTO();
            flpCursos.Controls.Clear();

            foreach (var curso in cursos)
            {
                Panel panel = new Panel
                {
                    Width = 300,
                    Height = 200,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    BackColor = Color.White
                };

                Label lblNombre = new Label
                {
                    Text = curso.nombre_curso,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Width = 280,
                    Height = 30,
                    Location = new Point(10, 10)
                };

                Label lblFechas = new Label
                {
                    Text = $"Inicio: {curso.fecha_inicio_curso.ToShortDateString()} - Fin: {curso.fecha_fin_curso.ToShortDateString()}",
                    Width = 280,
                    Height = 20,
                    Location = new Point(10, 45)
                };

                Label lblCapacidad = new Label
                {
                    Text = $"Inscritos: {curso.NumeroInscritos}/{curso.capacidad_max_curso}",
                    Width = 150,
                    Height = 20,
                    Location = new Point(10, 70)
                };

                ProgressBar progressBar = new ProgressBar
                {
                    Minimum = 0,
                    Maximum = curso.capacidad_max_curso,
                    Value = curso.NumeroInscritos,
                    Width = 280,
                    Height = 20,
                    Location = new Point(10, 95)
                };

                Button btnVerDetalles = new Button
                {
                    Text = "Ver detalles",
                    Width = 120,
                    Height = 30,
                    Location = new Point(10, 125),
                    Tag = curso.id_curso,
                    BackColor = Color.LightSteelBlue,
                    FlatStyle = FlatStyle.Flat
                };
                btnVerDetalles.Click += BtnVerDetalles_Click;

                panel.Controls.Add(lblNombre);
                panel.Controls.Add(lblFechas);
                panel.Controls.Add(lblCapacidad);
                panel.Controls.Add(progressBar);
                panel.Controls.Add(btnVerDetalles);

                flpCursos.Controls.Add(panel);
            }
        }



        private void CargarEventos()
        {
            eventos = eventoService.ConsultarDTO();
            flpEventos.Controls.Clear();

            foreach (var evento in eventos)
            {
                // Panel principal para el evento
                Panel panel = new Panel
                {
                    Width = 300,
                    Height = 190, // Reducida la altura al quitar el botón
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    BackColor = Color.White
                };

                // Título del evento
                Label lblNombre = new Label
                {
                    Text = evento.nombre_evento,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Width = 280,
                    Height = 30,
                    Location = new Point(10, 10)
                };

                // Lugar del evento
                Label lblLugar = new Label
                {
                    Text = $"Lugar: {evento.lugar_evento}",
                    Width = 280,
                    Height = 20,
                    Location = new Point(10, 45)
                };

                // Fechas
                Label lblFechas = new Label
                {
                    Text = $"Inicio: {evento.fecha_inicio_evento.ToShortDateString()} - Fin: {evento.fecha_fin_evento.ToShortDateString()}",
                    Width = 280,
                    Height = 20,
                    Location = new Point(10, 70)
                };

                // Capacidad e inscritos
                Label lblCapacidad = new Label
                {
                    Text = $"Asistentes: {evento.NumeroAsistentes}/{evento.capacidad_max_evento}",
                    Width = 150,
                    Height = 20,
                    Location = new Point(10, 95)
                };

                // Barra de progreso para visualizar ocupación
                ProgressBar progressBar = new ProgressBar
                {
                    Minimum = 0,
                    Maximum = evento.capacidad_max_evento,
                    Value = evento.NumeroAsistentes,
                    Width = 280,
                    Height = 20,
                    Location = new Point(10, 120)
                };

                // Botón para ver detalles
                Button btnVerDetalles = new Button
                {
                    Text = "Ver detalles",
                    Width = 120,
                    Height = 30,
                    Location = new Point(10, 150),
                    Tag = evento.id_evento,
                    BackColor = Color.LightSteelBlue,
                    FlatStyle = FlatStyle.Flat
                };
                btnVerDetalles.Click += BtnVerDetalles_Click;

                // Agregar controles al panel
                panel.Controls.Add(lblNombre);
                panel.Controls.Add(lblLugar);
                panel.Controls.Add(lblFechas);
                panel.Controls.Add(lblCapacidad);
                panel.Controls.Add(progressBar);
                panel.Controls.Add(btnVerDetalles);

                // Agregar panel al FlowLayoutPanel
                flpEventos.Controls.Add(panel);
            }
        }

        private void BtnVerDetalles_Click(object sender, EventArgs e)
        {

            var btn = sender as Button;
            if (btn == null) return;

            // Determinar si el botón pertenece a un curso o evento
            int id = (int)btn.Tag;

            // Buscar si el id existe en la lista de cursos
            var curso = cursos?.Find(c => c.id_curso == id);
            if (curso != null)
            {
                // Mostrar detalles del curso
                var frmDetallesCurso = new FrmDetalleCursoUser(id);
                frmDetallesCurso.ShowDialog();
                return;
            }

            // Buscar si el id existe en la lista de eventos
            var evento = eventos?.Find(ev => ev.id_evento == id);
            if (evento != null)
            {
                // Mostrar detalles del evento
                var frmDetallesEvento = new FrmDetalleEventoUser(id);
                frmDetallesEvento.ShowDialog();
                return;
            }
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            var loginForm = new FrmLogin();
            
            loginForm.ShowDialog();
            
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            var registroForm = new RegistroForm();
            
            registroForm.ShowDialog();
            
        }

        private void btnComenzarAhora_Click(object sender, EventArgs e)
        {
            SetActiveTab(tabCursos); // Cambiar a la pestaña de servicios/cursos
        }

        private void btnConocerMas_Click(object sender, EventArgs e)
        {
            FrmAcercaDe frmAcercaDe = new FrmAcercaDe();
            frmAcercaDe.ShowDialog();
        }

        private void btnVerDetallesCurso_Click(object sender, EventArgs e)
        {
            //var detalleForm = new FrmDetalleEventoUser();
            //detalleForm.ShowDialog();
        }

        // Manejadores para los botones de navegación
        private void btnInicio_Click(object sender, EventArgs e)
        {
            SetActiveTab(tabInicio);
        }

        private void btnCursos_Click(object sender, EventArgs e)
        {
            
            CargarCursos();
            // Cambiar a la pestaña de cursos
            SetActiveTab(tabCursos);
        }

        private void btnEventos_Click(object sender, EventArgs e)
        {
            CargarEventos();
            SetActiveTab(tabEventos);
        }

        private void btnContacto_Click(object sender, EventArgs e)
        {
            SetActiveTab(tabContacto);
        }

        private void pictureBoxDashboard_Click(object sender, EventArgs e)
        {

        }
    }
}