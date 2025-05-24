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
    public partial class FrmCursosUser : Form
    {
        private readonly CursoService cursoService;
        private List<CursoDTO> cursos;
        private List<CursoDTO> cursosFiltrados;

        private enum FiltroCurso
        {
            Todos,
            Activos,
            Proximos,
            Pasados
        }

        public FrmCursosUser()
        {
            InitializeComponent();
            cursoService = new CursoService();
            CargarCursos();
        }

        private void CargarCursos()
        {
            cursos = cursoService.ConsultarDTO();
            cursosFiltrados = null;
            txtSearch.Text = "Buscar curso...";
            MostrarCursos(cursos);
            label1.Text = $"Cursos Disponibles ({cursos.Count})";
        }

        private void MostrarCursos(List<CursoDTO> cursosAMostrar)
        {
            flpCursos.Controls.Clear();

            if (cursosAMostrar == null || cursosAMostrar.Count == 0)
            {
                MostrarMensajeNoHayCursos();
                return;
            }

            foreach (var curso in cursosAMostrar)
            {
                Panel panel = new Panel
                {
                    Width = 320,
                    Height = 380,
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

                Panel contentPanel = new Panel
                {
                    Width = 320,
                    Height = 200,
                    Location = new Point(0, 180),
                    BackColor = Color.White,
                    Padding = new Padding(15)
                };

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

                int cursoId = curso.id_curso;
                btnVerDetalles.Click += (sender, e) => MostrarDetallesCurso(cursoId);

                contentPanel.Controls.Add(lblNombre);
                contentPanel.Controls.Add(fechasPanel);
                contentPanel.Controls.Add(inscritosPanel);
                contentPanel.Controls.Add(progressContainer);
                contentPanel.Controls.Add(btnVerDetalles);

                panel.Controls.Add(contentPanel);
                flpCursos.Controls.Add(panel);
            }
        }

        private void MostrarMensajeNoHayCursos()
        {
            Panel panelNoResults = new Panel
            {
                Width = flpCursos.Width - 100,
                Height = 150,
                BackColor = Color.White,
                Margin = new Padding(50, 100, 50, 0)
            };

            Label lblNoHayCursos = new Label
            {
                Text = "No se encontraron cursos con los criterios seleccionados",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = panelNoResults.Width - 40,
                Height = 40,
                Location = new Point(20, 55)
            };

            panelNoResults.Controls.Add(lblNoHayCursos);
            flpCursos.Controls.Add(panelNoResults);
        }

        private void MostrarDetallesCurso(int cursoId)
        {
            try
            {
                using (FrmDetalleCursoUser detalleForm = new FrmDetalleCursoUser(cursoId))
                {
                    detalleForm.ShowDialog(this);
                }
                CargarCursos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarCursos();
            ActualizarOpcionesCombo(FiltroCurso.Todos); // Reset filter to "Todos"
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text) && txtSearch.Text != "Buscar curso...")
            {
                cursos = cursoService.ConsultarDTO();
                string busqueda = txtSearch.Text.ToLower();
                cursosFiltrados = cursos.Where(c => c.nombre_curso.ToLower().Contains(busqueda)).ToList();
                MostrarCursos(cursosFiltrados);
                label1.Text = $"Resultados de búsqueda ({cursosFiltrados.Count})";
            }
            else
            {
                CargarCursos();
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Buscar curso...")
            {
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Buscar curso...";
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

        private Color GetProgressColor(int inscritos, int capacidad)
        {
            if (capacidad == 0) return Color.FromArgb(76, 175, 80);
            double porcentaje = (double)inscritos / capacidad;
            if (porcentaje < 0.5)
                return Color.FromArgb(76, 175, 80); // Verde
            else if (porcentaje < 0.75)
                return Color.FromArgb(255, 152, 0); // Naranja
            else
                return Color.FromArgb(244, 67, 54); // Rojo
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

        private void FrmCursosUser_Load(object sender, EventArgs e)
        {
            ActualizarOpcionesCombo(FiltroCurso.Todos);
            var cursosFiltrados = FiltrarCursos(FiltroCurso.Todos);
            MostrarCursos(cursosFiltrados);
        }

        private void flpCursos_Paint(object sender, PaintEventArgs e)
        {
            // Mantener evento vacío si no se necesita
        }

        private bool actualizandoCombo = false;
        private void ActualizarOpcionesCombo(FiltroCurso seleccionado)
        {
            actualizandoCombo = true;
            cmbFiltroCursos.Items.Clear();

            foreach (FiltroCurso filtro in Enum.GetValues(typeof(FiltroCurso)))
            {
                cmbFiltroCursos.Items.Add(filtro.ToString());
            }

            // Seleccionar la opción correspondiente
            cmbFiltroCursos.SelectedItem = seleccionado.ToString();
            actualizandoCombo = false;
        }

        private List<CursoDTO> FiltrarCursos(FiltroCurso filtro)
        {
            DateTime ahora = DateTime.Now;
            switch (filtro)
            {
                case FiltroCurso.Todos:
                    return cursos.ToList();
                case FiltroCurso.Activos:
                    return cursos.Where(c => c.fecha_inicio_curso <= ahora && c.fecha_fin_curso >= ahora).ToList();
                case FiltroCurso.Proximos:
                    return cursos.Where(c => c.fecha_inicio_curso > ahora).ToList();
                case FiltroCurso.Pasados:
                    return cursos.Where(c => c.fecha_fin_curso < ahora).ToList();
                default:
                    return cursos.ToList();
            }
        }

        private void cmbFiltroCursos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actualizandoCombo || cmbFiltroCursos.SelectedItem == null) return;
            FiltroCurso filtroSeleccionado = (FiltroCurso)Enum.Parse(typeof(FiltroCurso), cmbFiltroCursos.SelectedItem.ToString());
            var cursosFiltrados = FiltrarCursos(filtroSeleccionado);
            MostrarCursos(cursosFiltrados);
            label1.Text = $"Cursos ({cursosFiltrados.Count})";
            ActualizarOpcionesCombo(filtroSeleccionado);
        }
    }
}