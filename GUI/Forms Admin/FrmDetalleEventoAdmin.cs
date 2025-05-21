using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
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

        public FrmDetalleEventoAdmin(int eventoId)
        {
            InitializeComponent();
            _eventoService = new EventoService();
            _eventoId = eventoId;
            CargarEvento();
            ConfigurarBotonEliminar();
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

            if (!string.IsNullOrEmpty(_evento.ruta_imagen_evento) && File.Exists(_evento.ruta_imagen_evento))
                {
                    try
                    {
                        // Usar una copia de la imagen para evitar problemas de "disposed object"
                        using (var originalImage = Image.FromFile(_evento.ruta_imagen_evento))
                        {
                            pictureBox.Image = new Bitmap(originalImage);
                        }
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                        // No mostramos ninguna imagen si hay error
                    }
                    _evento = _eventoService.BuscarPorId(_eventoId);

                    if (_evento == null)
                    {
                        MessageBox.Show("El evento no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    var eventosDto = _eventoService.ConsultarDTO();
                    _eventoDto = eventosDto.Find(e => e.id_evento == _eventoId);

                    // Cargar datos en los controles
                    lblTitulo.Text = _evento.nombre_evento;
                    lblFechas.Text = $"Del {_evento.fecha_inicio_evento:dd/MM/yyyy} al {_evento.fecha_fin_evento:dd/MM/yyyy}";
                    lblLugar.Text = $"Lugar: {_evento.lugar_evento}";
                    txtDescripcion.Text = _evento.descripcion_evento;
                    lblAsistentes.Text = $"Asistentes: {_eventoDto?.NumeroAsistentes ?? 0}/{_evento.capacidad_max_evento}";

                    // Configurar visibilidad de botones según rol
                    ConfigurarVisibilidadBotones();
                }
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
            // Mostrar botón de eliminar solo para administradores
            btnEliminarEvento.Visible = Session.CurrentUser?.es_administrador == "S";
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
            btnEliminarEvento.Click += btnEliminarEvento_Click;
        }

        private void btnEliminarEvento_Click(object sender, EventArgs e)
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
                try
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
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al procesar la eliminación: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnVerAsistentes_Click(object sender, EventArgs e)
        {
            FrmAsistentesEvento frmAsistentesEvento = new FrmAsistentesEvento(_eventoId);
            frmAsistentesEvento.Show();
        }

        private void FrmDetalleEventoAdmin_Load(object sender, EventArgs e)
        {
            // Configuraciones adicionales al cargar el formulario
        }
    }
}