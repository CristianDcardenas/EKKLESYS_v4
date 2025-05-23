using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BLL;
using ENTITY;
using FontAwesome.Sharp;

namespace GUI
{
    public partial class FrmDetalleEventoAdmin : Form
    {
        private readonly EventoService _eventoService;
        private readonly int _eventoId;
        private Evento _evento;
        private EventoDTO _eventoDto;
        private bool _isProcessing = false; // Flag to prevent re-entry

        public FrmDetalleEventoAdmin(int eventoId)
        {
            InitializeComponent();
            _eventoService = new EventoService();
            _eventoId = eventoId;
            ConfigurarBotonEliminar(); // Configure button styling
            // Ensure single event subscription
            btnEliminarEvento.Click -= btnEliminarEvento_Click; // Remove any existing handler
            btnEliminarEvento.Click += btnEliminarEvento_Click; // Add handler
            Console.WriteLine("btnEliminarEvento.Click handler subscribed");
            CargarEvento();
        }

        private void CargarEvento()
        {
            try
            {
                _evento = _eventoService.BuscarPorId(_eventoId);
                if (_evento == null)
                {
                    MessageBox.Show("El evento no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Load image if available
                if (!string.IsNullOrEmpty(_evento.ruta_imagen_evento) && File.Exists(_evento.ruta_imagen_evento))
                {
                    try
                    {
                        using (var originalImage = Image.FromFile(_evento.ruta_imagen_evento))
                        {
                            pictureBox.Image = new Bitmap(originalImage);
                        }
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                    }
                }

                // Load DTO data
                var eventosDto = _eventoService.ConsultarDTO();
                _eventoDto = eventosDto.Find(e => e.id_evento == _eventoId);

                // Update UI controls
                lblTitulo.Text = _evento.nombre_evento;
                lblFechas.Text = $"Del {_evento.fecha_inicio_evento:dd/MM/yyyy} al {_evento.fecha_fin_evento:dd/MM/yyyy}";
                lblLugar.Text = $"Lugar: {_evento.lugar_evento}";
                txtDescripcion.Text = _evento.descripcion_evento;
                lblAsistentes.Text = $"Asistentes: {_eventoDto?.NumeroAsistentes ?? 0}/{_evento.capacidad_max_evento}";

                // Configure button visibility and log state
                ConfigurarVisibilidadBotones();
                Console.WriteLine($"btnEliminarEvento.Visible: {btnEliminarEvento.Visible}, Enabled: {btnEliminarEvento.Enabled}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el evento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ConfigurarVisibilidadBotones()
        {
            bool isAdmin = Session.CurrentUser?.es_administrador == "S";
            btnEliminarEvento.Visible = isAdmin;
            btnEliminarEvento.Enabled = isAdmin; // Ensure button is enabled for admins
            Console.WriteLine($"ConfigurarVisibilidadBotones: isAdmin={isAdmin}");
        }

        private void ConfigurarBotonEliminar()
        {
            btnEliminarEvento.BackColor = Color.FromArgb(192, 0, 0);
            btnEliminarEvento.FlatAppearance.BorderSize = 0;
            btnEliminarEvento.FlatStyle = FlatStyle.Flat;
            btnEliminarEvento.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminarEvento.ForeColor = Color.White;
            btnEliminarEvento.IconChar = IconChar.TrashAlt;
            btnEliminarEvento.IconColor = Color.White;
            btnEliminarEvento.IconSize = 24;
            btnEliminarEvento.TextImageRelation = TextImageRelation.ImageBeforeText;
            // Event subscription moved to constructor
        }

        private void btnEliminarEvento_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"btnEliminarEvento_Click triggered at: {DateTime.Now:HH:mm:ss.fff}");
            if (_isProcessing) return;
            _isProcessing = true;
            btnEliminarEvento.Enabled = false;

            try
            {
                if (Session.CurrentUser?.es_administrador != "S")
                {
                    MessageBox.Show("Solo los administradores pueden eliminar eventos",
                        "Acceso denegado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmacion = MessageBox.Show(
                    $"¿Está seguro que desea eliminar permanentemente el evento: {_evento.nombre_evento}?\n\nEsta acción no se puede deshacer.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (confirmacion == DialogResult.Yes)
                {
                    var resultado = _eventoService.Eliminar(_eventoId);
                    if (resultado.StartsWith("Error"))
                    {
                        MessageBox.Show(resultado, "Error al eliminar",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Evento eliminado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la eliminación: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessing = false;
                btnEliminarEvento.Enabled = btnEliminarEvento.Visible; // Re-enable only if visible
            }
        }

        private void btnVerAsistentes_Click(object sender, EventArgs e)
        {
            FrmAsistentesEvento frmAsistentesEvento = new FrmAsistentesEvento(_eventoId);
            frmAsistentesEvento.Show();
        }

        private void FrmDetalleEventoAdmin_Load(object sender, EventArgs e)
        {
            // No additional logic needed
        }
    }
}