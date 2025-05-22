using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmInicio2 : Form
    {
        private readonly CursoService cursoService;
        private List<CursoDTO> cursos;
        private readonly EventoService eventoService;
        private List<EventoDTO> eventos;
        private readonly TelegramBotService _botService;

        public FrmInicio2()
        {
            InitializeComponent();
            ConfigureTabControl();
            SetActiveTab(tabInicio); // Mostrar la pestaña de inicio por defecto
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            cursoService = new CursoService();
            eventoService = new EventoService();
            _botService = new TelegramBotService();

        }

        private async void FrmInicio2Closing(object sender, FormClosingEventArgs e)
        {
            await _botService.StopBotAsync();
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
                // Panel principal con esquinas redondeadas y sombra
                Panel panel = new Panel
                {
                    Width = 320,
                    Height = 380,
                    Margin = new Padding(15),
                    BackColor = Color.White
                };

                // Aplicar efecto de sombra y bordes redondeados
                ApplyRoundedCorners(panel, 10);
                ApplyShadowEffect(panel);

                // Contenedor para la imagen con altura fija
                Panel imageContainer = new Panel
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Top,
                    BackColor = Color.FromArgb(240, 240, 240)
                };

                // PictureBox para la imagen con mejor adaptación
                PictureBox pictureBox = new PictureBox
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom, // Mejor adaptación de la imagen
                    BackColor = Color.Transparent
                };

                // Cargar la imagen si existe
                if (!string.IsNullOrEmpty(curso.ruta_imagen_curso) && File.Exists(curso.ruta_imagen_curso))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(curso.ruta_imagen_curso);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                        CrearImagenPorDefectoModerna(pictureBox);
                    }
                }
                else
                {
                    CrearImagenPorDefectoModerna(pictureBox);
                }

                imageContainer.Controls.Add(pictureBox);
                panel.Controls.Add(imageContainer);

                // Panel para el contenido
                Panel contentPanel = new Panel
                {
                    Width = 320,
                    Height = 200,
                    Location = new Point(0, 180),
                    BackColor = Color.White,
                    Padding = new Padding(15)
                };

                // Título del curso con estilo moderno
                Label lblNombre = new Label
                {
                    Text = curso.nombre_curso,
                    Font = new Font("Segoe UI Semibold", 14),
                    ForeColor = Color.FromArgb(50, 50, 50),
                    Width = 290,
                    Height = 30,
                    Location = new Point(15, 10),
                    AutoEllipsis = true // Añade "..." si el texto es demasiado largo
                };

                // Fechas con icono
                Panel fechasPanel = new Panel
                {
                    Width = 290,
                    Height = 25,
                    Location = new Point(15, 45),
                    BackColor = Color.White
                };

                PictureBox calendarIcon = new PictureBox
                {
                    Size = new Size(16, 16),
                    Location = new Point(0, 2),
                    BackColor = Color.Transparent
                };
                calendarIcon.Image = CreateCalendarIcon();

                Label lblFechas = new Label
                {
                    Text = $"{curso.fecha_inicio_curso.ToShortDateString()} - {curso.fecha_fin_curso.ToShortDateString()}",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Width = 270,
                    Height = 20,
                    Location = new Point(20, 0)
                };

                fechasPanel.Controls.Add(calendarIcon);
                fechasPanel.Controls.Add(lblFechas);

                // Panel para inscritos con icono
                Panel inscritosPanel = new Panel
                {
                    Width = 290,
                    Height = 25,
                    Location = new Point(15, 75),
                    BackColor = Color.White
                };

                PictureBox userIcon = new PictureBox
                {
                    Size = new Size(16, 16),
                    Location = new Point(0, 2),
                    BackColor = Color.Transparent
                };
                userIcon.Image = CreateUserIcon();

                Label lblCapacidad = new Label
                {
                    Text = $"Inscritos: {curso.NumeroInscritos}/{curso.capacidad_max_curso}",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Width = 270,
                    Height = 20,
                    Location = new Point(20, 0)
                };

                inscritosPanel.Controls.Add(userIcon);
                inscritosPanel.Controls.Add(lblCapacidad);

                // Barra de progreso moderna
                Panel progressContainer = new Panel
                {
                    Width = 290,
                    Height = 6,
                    Location = new Point(15, 105),
                    BackColor = Color.FromArgb(230, 230, 230),
                    Padding = new Padding(0)
                };
                ApplyRoundedCorners(progressContainer, 3);

                // Calcular el ancho de la barra de progreso
                int progressWidth = curso.capacidad_max_curso > 0
                    ? (int)((double)curso.NumeroInscritos / curso.capacidad_max_curso * 290)
                    : 0;

                Panel progressBar = new Panel
                {
                    Width = Math.Max(progressWidth, 5), // Al menos 5px de ancho
                    Height = 6,
                    Location = new Point(0, 0),
                    BackColor = GetProgressColor(curso.NumeroInscritos, curso.capacidad_max_curso)
                };
                ApplyRoundedCorners(progressBar, 3);

                progressContainer.Controls.Add(progressBar);

                // Botón moderno para ver detalles
                Button btnVerDetalles = new Button
                {
                    Text = "Ver detalles",
                    Width = 290,
                    Height = 40,
                    Location = new Point(15, 160),
                    Tag = curso.id_curso,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(240, 240, 240),
                    ForeColor = Color.FromArgb(60, 60, 60),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                btnVerDetalles.FlatAppearance.BorderSize = 0;
                ApplyRoundedCorners(btnVerDetalles, 5);

                // Usar una variable local para el evento Click para evitar problemas de "disposed object"
                btnVerDetalles.Click += BtnVerDetalles_Click;


                // Agregar controles al panel de contenido
                contentPanel.Controls.Add(lblNombre);
                contentPanel.Controls.Add(fechasPanel);
                contentPanel.Controls.Add(inscritosPanel);
                contentPanel.Controls.Add(progressContainer);
                contentPanel.Controls.Add(btnVerDetalles);

                // Agregar el panel de contenido al panel principal
                panel.Controls.Add(contentPanel);

                // Agregar panel al FlowLayoutPanel
                flpCursos.Controls.Add(panel);
            }
        }

        // Método para crear una imagen por defecto más moderna
        private void CrearImagenPorDefectoModerna(PictureBox pictureBox)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Fondo degradado suave
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, pictureBox.Width, pictureBox.Height),
                    Color.FromArgb(240, 240, 240),
                    Color.FromArgb(220, 220, 220),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, 0, 0, pictureBox.Width, pictureBox.Height);
                }

                // Icono de imagen
                int iconSize = 48;
                int iconX = (pictureBox.Width - iconSize) / 2;
                int iconY = (pictureBox.Height - iconSize) / 2 - 10;

                // Dibujar un icono de imagen simple
                using (Pen pen = new Pen(Color.FromArgb(180, 180, 180), 2))
                {
                    // Rectángulo del marco de la imagen
                    g.DrawRectangle(pen, iconX, iconY, iconSize, iconSize);

                    // Montaña dentro del marco
                    Point[] trianglePoints = {
                new Point(iconX + 10, iconY + iconSize - 10),
                new Point(iconX + iconSize/2 - 5, iconY + iconSize/2),
                new Point(iconX + iconSize/2 + 15, iconY + iconSize - 10)
            };
                    g.DrawLines(pen, trianglePoints);

                    // Sol/círculo
                    g.DrawEllipse(pen, iconX + iconSize - 20, iconY + 10, 10, 10);
                }

                // Texto "Sin imagen" con fuente moderna
                using (Font font = new Font("Segoe UI", 11, FontStyle.Regular))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    // Dibujar el texto debajo del icono
                    RectangleF textRect = new RectangleF(0, iconY + iconSize + 10, pictureBox.Width, 30);
                    g.DrawString("Sin imagen", font, Brushes.Gray, textRect, sf);
                }
            }
            pictureBox.Image = bmp;
        }

        // Método para aplicar esquinas redondeadas a un control
        private void ApplyRoundedCorners(Control control, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
                path.AddArc(control.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
                path.AddArc(control.Width - radius * 2, control.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(0, control.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseAllFigures();

                control.Region = new Region(path);
            }
        }

        // Método para aplicar efecto de sombra
        private void ApplyShadowEffect(Panel panel)
        {
            panel.Paint += (sender, e) =>
            {
                // Dibujar borde suave
                using (Pen pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };
        }

        // Método para determinar el color de la barra de progreso según el porcentaje
        private Color GetProgressColor(int inscritos, int capacidad)
        {
            if (capacidad == 0) return Color.FromArgb(76, 175, 80); // Verde por defecto

            double porcentaje = (double)inscritos / capacidad;

            if (porcentaje < 0.5)
                return Color.FromArgb(76, 175, 80); // Verde
            else if (porcentaje < 0.75)
                return Color.FromArgb(255, 152, 0); // Naranja
            else
                return Color.FromArgb(244, 67, 54); // Rojo
        }

        // Método para crear un icono de calendario simple
        private Bitmap CreateCalendarIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.FromArgb(100, 100, 100), 1))
                {
                    // Dibujar el marco del calendario
                    g.DrawRectangle(pen, 1, 3, 13, 12);
                    // Dibujar líneas horizontales
                    g.DrawLine(pen, 1, 7, 14, 7);
                    // Dibujar "patas" del calendario
                    g.DrawLine(pen, 4, 1, 4, 3);
                    g.DrawLine(pen, 11, 1, 11, 3);
                }
            }
            return bmp;
        }

        // Método para crear un icono de usuario simple
        private Bitmap CreateUserIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.FromArgb(100, 100, 100), 1))
                {
                    // Dibujar la cabeza
                    g.DrawEllipse(pen, 5, 1, 6, 6);
                    // Dibujar el cuerpo
                    g.DrawArc(pen, 2, 7, 12, 12, 180, 180);
                }
            }
            return bmp;
        }

        

        private void CargarEventos()
        {
            eventos = eventoService.ConsultarDTO();
            flpEventos.Controls.Clear();

            foreach (var evento in eventos)
            {
                // Panel principal con esquinas redondeadas y sombra
                Panel panel = new Panel
                {
                    Width = 320,
                    Height = 400, // Un poco más alto para acomodar el campo de lugar
                    Margin = new Padding(15),
                    BackColor = Color.White
                };

                // Aplicar efecto de sombra y bordes redondeados
                ApplyRoundedCorners(panel, 10);
                ApplyShadowEffect(panel);

                // Contenedor para la imagen con altura fija
                Panel imageContainer = new Panel
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Top,
                    BackColor = Color.FromArgb(240, 240, 240)
                };

                // PictureBox para la imagen con mejor adaptación
                PictureBox pictureBox = new PictureBox
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom, // Mejor adaptación de la imagen
                    BackColor = Color.Transparent
                };

                // Cargar la imagen si existe
                if (!string.IsNullOrEmpty(evento.ruta_imagen_evento) && File.Exists(evento.ruta_imagen_evento))
                {
                    try
                    {
                        // Usar una copia de la imagen para evitar problemas de "disposed object"
                        using (var originalImage = Image.FromFile(evento.ruta_imagen_evento))
                        {
                            pictureBox.Image = new Bitmap(originalImage);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                        CrearImagenPorDefectoModerna(pictureBox);
                    }
                }
                else
                {
                    CrearImagenPorDefectoModerna(pictureBox);
                }

                imageContainer.Controls.Add(pictureBox);
                panel.Controls.Add(imageContainer);

                // Panel para el contenido
                Panel contentPanel = new Panel
                {
                    Width = 320,
                    Height = 220, // Más alto para acomodar el campo de lugar
                    Location = new Point(0, 180),
                    BackColor = Color.White,
                    Padding = new Padding(15)
                };

                // Título del evento con estilo moderno
                Label lblNombre = new Label
                {
                    Text = evento.nombre_evento,
                    Font = new Font("Segoe UI Semibold", 14),
                    ForeColor = Color.FromArgb(50, 50, 50),
                    Width = 290,
                    Height = 30,
                    Location = new Point(15, 10),
                    AutoEllipsis = true // Añade "..." si el texto es demasiado largo
                };

                // Panel para lugar con icono
                Panel lugarPanel = new Panel
                {
                    Width = 290,
                    Height = 25,
                    Location = new Point(15, 45),
                    BackColor = Color.White
                };

                PictureBox locationIcon = new PictureBox
                {
                    Size = new Size(16, 16),
                    Location = new Point(0, 2),
                    BackColor = Color.Transparent
                };
                locationIcon.Image = CreateLocationIcon();

                Label lblLugar = new Label
                {
                    Text = $"{evento.lugar_evento}",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Width = 270,
                    Height = 20,
                    Location = new Point(20, 0)
                };

                lugarPanel.Controls.Add(locationIcon);
                lugarPanel.Controls.Add(lblLugar);

                // Fechas con icono
                Panel fechasPanel = new Panel
                {
                    Width = 290,
                    Height = 25,
                    Location = new Point(15, 75),
                    BackColor = Color.White
                };

                PictureBox calendarIcon = new PictureBox
                {
                    Size = new Size(16, 16),
                    Location = new Point(0, 2),
                    BackColor = Color.Transparent
                };
                calendarIcon.Image = CreateCalendarIcon();

                Label lblFechas = new Label
                {
                    Text = $"{evento.fecha_inicio_evento.ToShortDateString()} - {evento.fecha_fin_evento.ToShortDateString()}",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Width = 270,
                    Height = 20,
                    Location = new Point(20, 0)
                };

                fechasPanel.Controls.Add(calendarIcon);
                fechasPanel.Controls.Add(lblFechas);

                // Panel para asistentes con icono
                Panel asistentesPanel = new Panel
                {
                    Width = 290,
                    Height = 25,
                    Location = new Point(15, 105),
                    BackColor = Color.White
                };

                PictureBox userIcon = new PictureBox
                {
                    Size = new Size(16, 16),
                    Location = new Point(0, 2),
                    BackColor = Color.Transparent
                };
                userIcon.Image = CreateUserIcon();

                Label lblAsistentes = new Label
                {
                    Text = $"Asistentes: {evento.NumeroAsistentes}/{evento.capacidad_max_evento}",
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Width = 270,
                    Height = 20,
                    Location = new Point(20, 0)
                };

                asistentesPanel.Controls.Add(userIcon);
                asistentesPanel.Controls.Add(lblAsistentes);

                // Barra de progreso moderna
                Panel progressContainer = new Panel
                {
                    Width = 290,
                    Height = 6,
                    Location = new Point(15, 135),
                    BackColor = Color.FromArgb(230, 230, 230),
                    Padding = new Padding(0)
                };
                ApplyRoundedCorners(progressContainer, 3);

                // Calcular el ancho de la barra de progreso
                int progressWidth = evento.capacidad_max_evento > 0
                    ? (int)((double)evento.NumeroAsistentes / evento.capacidad_max_evento * 290)
                    : 0;

                Panel progressBar = new Panel
                {
                    Width = Math.Max(progressWidth, 5), // Al menos 5px de ancho
                    Height = 6,
                    Location = new Point(0, 0),
                    BackColor = GetProgressColor(evento.NumeroAsistentes, evento.capacidad_max_evento)
                };
                ApplyRoundedCorners(progressBar, 3);

                progressContainer.Controls.Add(progressBar);

                // Botón moderno para ver detalles
                Button btnVerDetalles = new Button
                {
                    Text = "Ver detalles",
                    Width = 290,
                    Height = 40,
                    Location = new Point(15, 160),
                    Tag = evento.id_evento,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(240, 240, 240),
                    ForeColor = Color.FromArgb(60, 60, 60),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                btnVerDetalles.FlatAppearance.BorderSize = 0;
                ApplyRoundedCorners(btnVerDetalles, 5);

                // Usar una variable local para el evento Click para evitar problemas de "disposed object"
                btnVerDetalles.Click += BtnVerDetalles_Click;

                // Agregar controles al panel de contenido
                contentPanel.Controls.Add(lblNombre);
                contentPanel.Controls.Add(lugarPanel);
                contentPanel.Controls.Add(fechasPanel);
                contentPanel.Controls.Add(asistentesPanel);
                contentPanel.Controls.Add(progressContainer);
                contentPanel.Controls.Add(btnVerDetalles);

                // Agregar el panel de contenido al panel principal
                panel.Controls.Add(contentPanel);

                // Agregar panel al FlowLayoutPanel
                flpEventos.Controls.Add(panel);
            }
        }

        private Bitmap CreateLocationIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.FromArgb(100, 100, 100), 1))
                {
                    // Dibujar un marcador de ubicación
                    g.DrawEllipse(pen, 5, 2, 6, 6);
                    Point[] points = {
                new Point(8, 8),
                new Point(8, 14)
            };
                    g.DrawLines(pen, points);
                }
            }
            return bmp;
        }
        // Método para mostrar detalles del evento (evita problemas de "disposed object")


        private void BtnVerDetalles_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            // Obtener el ID del Tag del botón
            int id = (int)btn.Tag;

            // Buscar si el id existe en la lista de cursos
            var curso = cursos?.Find(c => c.id_curso == id);
            if (curso != null)
            {
                // Mostrar detalles del curso
                try
                {
                    using (var frmDetallesCurso = new FrmDetalleCursoUser(id))
                    {
                        frmDetallesCurso.ShowDialog();
                    }
                    // Recargar cursos después de cerrar el formulario
                    CargarCursos();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al mostrar detalles del curso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Buscar si el id existe en la lista de eventos
            var evento = eventos?.Find(ev => ev.id_evento == id);
            if (evento != null)
            {
                // Mostrar detalles del evento
                try
                {
                    using (var frmDetallesEvento = new FrmDetalleEventoUser(id))
                    {
                        frmDetallesEvento.ShowDialog();
                    }
                    // Recargar eventos después de cerrar el formulario
                    CargarEventos();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al mostrar detalles del evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Si no se encontró ni curso ni evento
            MessageBox.Show("No se encontró información para el elemento seleccionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void FrmInicio2_Load(object sender, EventArgs e)
        {

        }
    }
}