using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmEventosAdmin : Form
    {
        private readonly EventoService eventoService;
        private List<EventoDTO> eventos;
        private List<EventoDTO> eventosFiltrados;

        public FrmEventosAdmin()
        {
            InitializeComponent();
            eventoService = new EventoService();
            CargarEventos();
        }

        private enum FiltroEventos
        {
            Todos,
            Activos,
            Proximos,
            Pasados
        }

        private void CargarEventos()
        {
            eventos = eventoService.ConsultarDTO();
            eventosFiltrados = null;
            txtSearch.Text = "Buscar evento...";
            MostrarEventos(eventos);
            label1.Text = $"Eventos Disponibles ({eventos.Count})";
        }

        private void MostrarEventos(List<EventoDTO> eventosAMostrar)
        {
            flpEventos.Controls.Clear();

            if (eventosAMostrar == null || eventosAMostrar.Count == 0)
            {
                MostrarMensajeNoHayEventos();
                return;
            }

            foreach (var evento in eventosAMostrar)
            {
                Panel panel = new Panel
                {
                    Width = 320,
                    Height = 400,
                    Margin = new Padding(15),
                    BackColor = Color.White
                };
                ApplyRoundedCorners(panel, 10);
                ApplyShadowEffect(panel);

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

                Panel contentPanel = new Panel
                {
                    Width = 320,
                    Height = 220,
                    Location = new Point(0, 180),
                    BackColor = Color.White,
                    Padding = new Padding(15)
                };

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
                    Text = evento.lugar_evento,
                    Font = new Font("Segoe UI", 9),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Width = 270,
                    Height = 20,
                    Location = new Point(20, 0)
                };

                lugarPanel.Controls.Add(locationIcon);
                lugarPanel.Controls.Add(lblLugar);

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

                int eventoId = evento.id_evento;
                btnVerDetalles.Enabled = !(evento.fecha_fin_evento < DateTime.Now);
                btnVerDetalles.Click += (sender, e) => MostrarDetallesEvento(eventoId);

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

        private void MostrarMensajeNoHayEventos()
        {
            Panel panelNoResults = new Panel
            {
                Width = flpEventos.Width - 100,
                Height = 150,
                BackColor = Color.White,
                Margin = new Padding(50, 100, 50, 0)
            };

            Label lblNoHayEventos = new Label
            {
                Text = "No se encontraron eventos con los criterios seleccionados",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = panelNoResults.Width - 40,
                Height = 40,
                Location = new Point(20, 55)
            };

            panelNoResults.Controls.Add(lblNoHayEventos);
            flpEventos.Controls.Add(panelNoResults);
        }

        private void MostrarDetallesEvento(int eventoId)
        {
            try
            {
                using (FrmDetalleEventoAdmin detalleForm = new FrmDetalleEventoAdmin(eventoId))
                {
                    detalleForm.ShowDialog(this);
                }
                CargarEventos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevoEvento_Click(object sender, EventArgs e)
        {
            FrmCrearEvento crearForm = new FrmCrearEvento();
            if (crearForm.ShowDialog() == DialogResult.OK)
            {
                CargarEventos();
                ActualizarOpcionesCombo(FiltroEventos.Todos);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarEventos();
            ActualizarOpcionesCombo(FiltroEventos.Todos);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != "Buscar evento...")
            {
                eventos = eventoService.ConsultarDTO();
                string busqueda = txtSearch.Text.ToLower();
                eventosFiltrados = eventos.Where(evento => evento.nombre_evento.ToLower().Contains(busqueda)).ToList();
                MostrarEventos(eventosFiltrados);
                label1.Text = $"Resultados de búsqueda ({eventosFiltrados.Count})";
            }
            else
            {
                CargarEventos();
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Buscar evento...")
            {
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Buscar evento...";
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearch_Click(sender, e);
            }
        }

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

        private Color GetProgressColor(int asistentes, int capacidad)
        {
            if (capacidad == 0) return Color.FromArgb(76, 175, 80);
            double porcentaje = (double)asistentes / capacidad;
            if (porcentaje < 0.5)
                return Color.FromArgb(76, 175, 80); // Verde
            else if (porcentaje < 0.75)
                return Color.FromArgb(255, 152, 0); // Naranja
            else
                return Color.FromArgb(244, 67, 54); // Rojo
        }

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

        private void EventosForm_Load(object sender, EventArgs e)
        {
            ActualizarOpcionesCombo(FiltroEventos.Todos);
            var eventosFiltrados = FiltrarEventos(FiltroEventos.Todos);
            MostrarEventos(eventosFiltrados);
        }

        private bool actualizandoCombo = false;
        private void ActualizarOpcionesCombo(FiltroEventos seleccionado)
        {
            actualizandoCombo = true;
            cmbFiltrarEventos.Items.Clear();

            foreach (FiltroEventos filtro in Enum.GetValues(typeof(FiltroEventos)))
            {
                cmbFiltrarEventos.Items.Add(filtro.ToString());
            }

            cmbFiltrarEventos.SelectedItem = seleccionado.ToString();
            actualizandoCombo = false;
        }

        private List<EventoDTO> FiltrarEventos(FiltroEventos filtro)
        {
            DateTime ahora = DateTime.Now;
            switch (filtro)
            {
                case FiltroEventos.Todos:
                    return eventos.ToList();
                case FiltroEventos.Activos:
                    return eventos.Where(e => e.fecha_inicio_evento <= ahora && e.fecha_fin_evento >= ahora).ToList();
                case FiltroEventos.Proximos:
                    return eventos.Where(e => e.fecha_inicio_evento > ahora).ToList();
                case FiltroEventos.Pasados:
                    return eventos.Where(e => e.fecha_fin_evento < ahora).ToList();
                default:
                    return eventos.ToList();
            }
        }

        private void cmbFiltrarEventos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizandoCombo || cmbFiltrarEventos.SelectedItem == null) return;
            if (Enum.TryParse(cmbFiltrarEventos.SelectedItem.ToString(), out FiltroEventos filtroSeleccionado))
            {
                var eventosFiltrados = FiltrarEventos(filtroSeleccionado);
                MostrarEventos(eventosFiltrados);
                label1.Text = $"Eventos ({eventosFiltrados.Count})";
                ActualizarOpcionesCombo(filtroSeleccionado);
            }
        }
    }
}