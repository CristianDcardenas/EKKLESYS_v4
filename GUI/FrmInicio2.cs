using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
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
        private ReminderSchedulerService reminderService;

        public FrmInicio2()
        {
            InitializeComponent();
            ConfigureTabControl();
            SetActiveTab(tabInicio);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            cursoService = new CursoService();
            eventoService = new EventoService();
            _botService = new TelegramBotService();
        }

        private async void FrmInicio2Closing(object sender, FormClosingEventArgs e)
        {
            await _botService.StopBotAsync();
            // Detener el servicio al cerrar la aplicación
            reminderService?.DetenerServicio();
        }

        private void ConfigureTabControl()
        {
            tabControlMain.Appearance = TabAppearance.FlatButtons;
            tabControlMain.ItemSize = new Size(0, 1);
            tabControlMain.SizeMode = TabSizeMode.Fixed;
        }

        private void SetActiveTab(TabPage tabPage)
        {
            tabControlMain.SelectTab(tabPage);
            btnInicio.BackColor = tabPage == tabInicio ? Color.FromArgb(60, 80, 140) : Color.Transparent;
            btnCursos.BackColor = tabPage == tabCursos ? Color.FromArgb(60, 80, 140) : Color.Transparent;
            btnEventos.BackColor = tabPage == tabEventos ? Color.FromArgb(60, 80, 140) : Color.Transparent;
            btnContacto.BackColor = tabPage == tabContacto ? Color.FromArgb(60, 80, 140) : Color.Transparent;
        }

        private void CargarCursos()
        {
            cursos = cursoService.ConsultarDTO()
            .Where(c => c.fecha_inicio_curso <= DateTime.Now && c.fecha_fin_curso >= DateTime.Now)
            .ToList();
            flpCursos.Controls.Clear();

            foreach (var curso in cursos)
            {
                // Panel principal
                Panel panel = new Panel
                {
                    Width = 320,
                    Height = 400, // Ajustado a la altura de FrmEventosUser
                    Margin = new Padding(15),
                    BackColor = Color.White,
                    Tag = curso.id_curso
                };
                ApplyRoundedCorners(panel, 10);
                ApplyShadowEffect(panel);

                // Contenedor para la imagen
                Panel imageContainer = new Panel
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Top,
                    BackColor = Color.FromArgb(240, 240, 240)
                };

                PictureBox pictureBox = new PictureBox
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent
                };

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
                    Height = 220,
                    Location = new Point(0, 180),
                    BackColor = Color.White,
                    Padding = new Padding(15)
                };

                // Título del curso
                Label lblNombre = new Label
                {
                    Text = curso.nombre_curso,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(50, 50, 50),
                    Width = 290,
                    Height = 30,
                    Location = new Point(15, 10),
                    AutoEllipsis = true
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
                    BackColor = Color.Transparent,
                    Image = CreateCalendarIcon()
                };

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
                    BackColor = Color.Transparent,
                    Image = CreateUserIcon()
                };

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

                // Barra de progreso
                Panel progressContainer = new Panel
                {
                    Width = 290,
                    Height = 6,
                    Location = new Point(15, 105),
                    BackColor = Color.FromArgb(230, 230, 230)
                };
                ApplyRoundedCorners(progressContainer, 3);

                int progressWidth = curso.capacidad_max_curso > 0
                    ? (int)((double)curso.NumeroInscritos / curso.capacidad_max_curso * 290)
                    : 0;

                Panel progressBar = new Panel
                {
                    Width = Math.Max(progressWidth, 5),
                    Height = 6,
                    Location = new Point(0, 0),
                    BackColor = GetProgressColor(curso.NumeroInscritos, curso.capacidad_max_curso)
                };
                ApplyRoundedCorners(progressBar, 3);

                progressContainer.Controls.Add(progressBar);

                // Botón "Ver detalles"
                Button btnVerDetalles = new Button
                {
                    Text = "Ver detalles",
                    Width = 290,
                    Height = 40,
                    Location = new Point(15, 160),
                    Tag = curso.id_curso,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                btnVerDetalles.FlatAppearance.BorderSize = 0;
                ApplyRoundedCorners(btnVerDetalles, 5);
                btnVerDetalles.Click += BtnVerDetalles_Click;

                // Agregar controles al panel de contenido
                contentPanel.Controls.Add(lblNombre);
                contentPanel.Controls.Add(fechasPanel);
                contentPanel.Controls.Add(inscritosPanel);
                contentPanel.Controls.Add(progressContainer);
                contentPanel.Controls.Add(btnVerDetalles);

                panel.Controls.Add(contentPanel);
                flpCursos.Controls.Add(panel);
            }
        }

        private void CargarEventos()
        {
            eventos = eventoService.ConsultarDTO()
            .Where(e => e.fecha_inicio_evento <= DateTime.Now && e.fecha_fin_evento >= DateTime.Now)
            .ToList();
            flpEventos.Controls.Clear();

            foreach (var evento in eventos)
            {
                // Panel principal
                Panel panel = new Panel
                {
                    Width = 320,
                    Height = 400,
                    Margin = new Padding(15),
                    BackColor = Color.White,
                    Tag = evento.id_evento
                };
                ApplyRoundedCorners(panel, 10);
                ApplyShadowEffect(panel);

                // Contenedor para la imagen
                Panel imageContainer = new Panel
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Top,
                    BackColor = Color.FromArgb(240, 240, 240)
                };

                PictureBox pictureBox = new PictureBox
                {
                    Width = 320,
                    Height = 180,
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent
                };

                if (!string.IsNullOrEmpty(evento.ruta_imagen_evento) && File.Exists(evento.ruta_imagen_evento))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(evento.ruta_imagen_evento);
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
                    Height = 220,
                    Location = new Point(0, 180),
                    BackColor = Color.White,
                    Padding = new Padding(15)
                };

                // Título del evento
                Label lblNombre = new Label
                {
                    Text = evento.nombre_evento,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(50, 50, 50),
                    Width = 290,
                    Height = 30,
                    Location = new Point(15, 10),
                    AutoEllipsis = true
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
                    BackColor = Color.Transparent,
                    Image = CreateLocationIcon()
                };

                Label lblLugar = new Label
                {
                    Text = evento.lugar_evento,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Width = 270,
                    Height = 20,
                    Location = new Point(20, 0),
                    AutoEllipsis = true
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
                    BackColor = Color.Transparent,
                    Image = CreateCalendarIcon()
                };

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
                    BackColor = Color.Transparent,
                    Image = CreateUserIcon()
                };

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

                // Barra de progreso
                Panel progressContainer = new Panel
                {
                    Width = 290,
                    Height = 6,
                    Location = new Point(15, 135),
                    BackColor = Color.FromArgb(230, 230, 230)
                };
                ApplyRoundedCorners(progressContainer, 3);

                int progressWidth = evento.capacidad_max_evento > 0
                    ? (int)((double)evento.NumeroAsistentes / evento.capacidad_max_evento * 290)
                    : 0;

                Panel progressBar = new Panel
                {
                    Width = Math.Max(progressWidth, 5),
                    Height = 6,
                    Location = new Point(0, 0),
                    BackColor = GetProgressColor(evento.NumeroAsistentes, evento.capacidad_max_evento)
                };
                ApplyRoundedCorners(progressBar, 3);

                progressContainer.Controls.Add(progressBar);

                // Botón "Ver detalles"
                Button btnVerDetalles = new Button
                {
                    Text = "Ver detalles",
                    Width = 290,
                    Height = 40,
                    Location = new Point(15, 160),
                    Tag = evento.id_evento,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Cursor = Cursors.Hand
                };
                btnVerDetalles.FlatAppearance.BorderSize = 0;
                ApplyRoundedCorners(btnVerDetalles, 5);
                btnVerDetalles.Click += BtnVerDetalles_Click;

                // Agregar controles al panel de contenido
                contentPanel.Controls.Add(lblNombre);
                contentPanel.Controls.Add(lugarPanel);
                contentPanel.Controls.Add(fechasPanel);
                contentPanel.Controls.Add(asistentesPanel);
                contentPanel.Controls.Add(progressContainer);
                contentPanel.Controls.Add(btnVerDetalles);

                panel.Controls.Add(contentPanel);
                flpEventos.Controls.Add(panel);
            }
        }

        // Método para crear una imagen por defecto moderna
        private void CrearImagenPorDefectoModerna(PictureBox pictureBox)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, pictureBox.Width, pictureBox.Height),
                    Color.FromArgb(240, 240, 240),
                    Color.FromArgb(220, 220, 220),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, 0, 0, pictureBox.Width, pictureBox.Height);
                }

                int iconSize = 48;
                int iconX = (pictureBox.Width - iconSize) / 2;
                int iconY = (pictureBox.Height - iconSize) / 2 - 10;

                using (Pen pen = new Pen(Color.FromArgb(180, 180, 180), 2))
                {
                    g.DrawRectangle(pen, iconX, iconY, iconSize, iconSize);
                    Point[] trianglePoints = {
                        new Point(iconX + 10, iconY + iconSize - 10),
                        new Point(iconX + iconSize/2 - 5, iconY + iconSize/2),
                        new Point(iconX + iconSize/2 + 15, iconY + iconSize - 10)
                    };
                    g.DrawLines(pen, trianglePoints);
                    g.DrawEllipse(pen, iconX + iconSize - 20, iconY + 10, 10, 10);
                }

                using (Font font = new Font("Segoe UI", 11, FontStyle.Regular))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
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
                using (Pen pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };
        }

        // Método para determinar el color de la barra de progreso
        private Color GetProgressColor(int current, int max)
        {
            if (max == 0) return Color.FromArgb(76, 175, 80);
            double percentage = (double)current / max;
            if (percentage < 0.5)
                return Color.FromArgb(76, 175, 80); // Verde
            else if (percentage < 0.75)
                return Color.FromArgb(255, 152, 0); // Naranja
            else
                return Color.FromArgb(244, 67, 54); // Rojo
        }

        // Métodos para crear los iconos
        private Bitmap CreateLocationIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.FromArgb(100, 100, 100), 1))
                {
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

        private Bitmap CreateCalendarIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.FromArgb(100, 100, 100), 1))
                {
                    g.DrawRectangle(pen, 1, 3, 13, 12);
                    g.DrawLine(pen, 1, 7, 14, 7);
                    g.DrawLine(pen, 4, 1, 4, 3);
                    g.DrawLine(pen, 11, 1, 11, 3);
                }
            }
            return bmp;
        }

        private Bitmap CreateUserIcon()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.FromArgb(100, 100, 100), 1))
                {
                    g.DrawEllipse(pen, 5, 1, 6, 6);
                    g.DrawArc(pen, 2, 7, 12, 12, 180, 180);
                }
            }
            return bmp;
        }

        // Métodos de eventos (sin cambios)
        private void BtnVerDetalles_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            int id = (int)btn.Tag;
            var curso = cursos?.Find(c => c.id_curso == id);
            if (curso != null)
            {
                try
                {
                    using (var frmDetallesCurso = new FrmDetalleCursoUser(id))
                    {
                        frmDetallesCurso.ShowDialog();
                    }
                    CargarCursos();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al mostrar detalles del curso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var evento = eventos?.Find(ev => ev.id_evento == id);
            if (evento != null)
            {
                try
                {
                    using (var frmDetallesEvento = new FrmDetalleEventoUser(id))
                    {
                        frmDetallesEvento.ShowDialog();
                    }
                    CargarEventos();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al mostrar detalles del evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

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
            var frmQr = new FrmQr();
            frmQr.ShowDialog();
        }

        private void btnConocerMas_Click(object sender, EventArgs e)
        {
            FrmAcercaDe frmAcercaDe = new FrmAcercaDe();
            frmAcercaDe.ShowDialog();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            SetActiveTab(tabInicio);
        }

        private void btnCursos_Click(object sender, EventArgs e)
        {
            CargarCursos();
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

        private async void FrmInicio2_LoadAsync(object sender, EventArgs e)
        {
            try
            {
                await _botService.StartBotAsync();
                MessageBox.Show("Bot de Telegram iniciado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar el bot: {ex.Message}");
            }

            // Iniciar el servicio de recordatorios automáticos
            reminderService = new ReminderSchedulerService();
            reminderService.IniciarServicio();
        }
        private void btnEnviarRecordatorios_Click(object sender, EventArgs e)
        {
            //var emailService = new EmailNotificationService();
            //emailService.TestearEnvioCorreo();
            var emailService = new EmailNotificationService();
            string resultado = emailService.TestearEnvioCorreo();
            MessageBox.Show(resultado, "Resultado Test");

        }
    }
}