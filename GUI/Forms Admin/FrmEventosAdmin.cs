using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmEventosAdmin : Form
    {
        private readonly EventoService eventoService;
        private List<EventoDTO> eventos;

        public FrmEventosAdmin()
        {
            InitializeComponent();
            eventoService = new EventoService();
            CargarEventos();
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
                int eventoId = evento.id_evento;
                btnVerDetalles.Click += (sender, e) => {
                    MostrarDetallesEvento(eventoId);
                };

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

        // Método para mostrar detalles del evento (evita problemas de "disposed object")
        private void MostrarDetallesEvento(int eventoId)
        {
            try
            {
                using (FrmDetalleEventoAdmin detalleForm = new FrmDetalleEventoAdmin(eventoId))
                {
                    detalleForm.ShowDialog(this);
                }

                // Recargar eventos al cerrar el formulario de detalles
                CargarEventos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para crear un icono de ubicación simple
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

        // Método para aplicar efecto de sombra a un panel
        private void ApplyShadowEffect(Panel panel)
        {
            panel.Paint += (sender, e) =>
            {
                // Dibujar sombra sutil
                Rectangle shadowRect = new Rectangle(0, 0, panel.Width, panel.Height);
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    shadowRect,
                    Color.FromArgb(10, 0, 0, 0),
                    Color.FromArgb(30, 0, 0, 0),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, shadowRect);
                }
            };
        }

        // Método para crear una imagen por defecto moderna
        private void CrearImagenPorDefectoModerna(PictureBox pictureBox)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Fondo degradado
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, pictureBox.Width, pictureBox.Height),
                    Color.FromArgb(240, 240, 240),
                    Color.FromArgb(220, 220, 220),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, 0, 0, pictureBox.Width, pictureBox.Height);
                }

                // Icono de imagen
                using (Pen pen = new Pen(Color.FromArgb(180, 180, 180), 2))
                {
                    // Dibujar un marco de imagen
                    int margin = pictureBox.Width / 4;
                    g.DrawRectangle(pen, margin, margin, pictureBox.Width - margin * 2, pictureBox.Height - margin * 2);

                    // Dibujar una montaña simple
                    Point[] points = {
                new Point(margin, pictureBox.Height - margin),
                new Point(pictureBox.Width / 2, margin + pictureBox.Height / 4),
                new Point(pictureBox.Width - margin, pictureBox.Height - margin)
            };
                    g.DrawLines(pen, points);

                    // Dibujar un sol
                    g.DrawEllipse(pen, pictureBox.Width - margin - 20, margin + 10, 15, 15);
                }

                // Texto
                using (Font font = new Font("Segoe UI", 9, FontStyle.Regular))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString("Sin imagen", font, Brushes.Gray,
                        new Rectangle(0, pictureBox.Height - 30, pictureBox.Width, 20), sf);
                }
            }
            pictureBox.Image = bmp;
        }

        // Método para obtener el color de la barra de progreso según el porcentaje de ocupación
        private Color GetProgressColor(int asistentes, int capacidadMax)
        {
            if (capacidadMax <= 0) return Color.FromArgb(100, 200, 100); // Verde por defecto

            double porcentaje = (double)asistentes / capacidadMax;

            if (porcentaje < 0.5)
                return Color.FromArgb(100, 200, 100); // Verde
            else if (porcentaje < 0.8)
                return Color.FromArgb(255, 180, 0);   // Amarillo/Naranja
            else
                return Color.FromArgb(220, 80, 80);   // Rojo
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
                    // Dibujar un calendario
                    g.DrawRectangle(pen, 2, 3, 12, 11);
                    g.DrawLine(pen, 5, 3, 5, 1);
                    g.DrawLine(pen, 11, 3, 11, 1);

                    // Líneas horizontales para representar líneas de texto
                    g.DrawLine(pen, 4, 7, 12, 7);
                    g.DrawLine(pen, 4, 10, 12, 10);
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
                    // Dibujar cabeza
                    g.DrawEllipse(pen, 5, 2, 6, 6);

                    // Dibujar cuerpo
                    g.DrawLine(pen, 8, 8, 8, 12);

                    // Dibujar brazos
                    g.DrawLine(pen, 4, 10, 12, 10);
                }
            }
            return bmp;
        }
        private void CrearImagenPorDefecto(PictureBox pictureBox)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(pictureBox.Image))
            {
                g.Clear(Color.LightGray);
                using (Font font = new Font("Arial", 10))
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString("Sin imagen", font, Brushes.Gray, new Rectangle(0, 0, pictureBox.Width, pictureBox.Height), sf);
                }
            }
        }

        private void BtnVerDetalles_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int eventoId = (int)btn.Tag;

            FrmDetalleEventoAdmin detalleForm = new FrmDetalleEventoAdmin(eventoId);
            detalleForm.ShowDialog();

            // Recargar eventos al cerrar el formulario de detalles
            CargarEventos();
        }

       
        private void btnNuevoEvento_Click(object sender, EventArgs e)
        {
            FrmCrearEvento crearForm = new FrmCrearEvento();
            if (crearForm.ShowDialog() == DialogResult.OK)
            {
                CargarEventos();
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarEventos();
        }


        private void EventosForm_Load(object sender, EventArgs e)
        {
            // Puedes agregar código de inicialización adicional aquí si es necesario
        }
    }
}