using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BLL;
using ENTITY;
using FontAwesome.Sharp;

namespace GUI
{
    public partial class FrmDetalleEventoUser : Form
    {
        private readonly EventoService _eventoService;
        private readonly int _eventoId;
        private Evento _evento;
        private EventoDTO _eventoDto;

        public FrmDetalleEventoUser(int eventoId)
        {
            InitializeComponent();
            _eventoService = new EventoService();
            _eventoId = eventoId;
            CargarEvento();
            ConfigurarBotonAsistir();
            
        }

        // Modificar el método CargarEvento para cargar la imagen
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

                var eventosDto = _eventoService.ConsultarDTO();
                _eventoDto = eventosDto.Find(e => e.id_evento == _eventoId);

                // Cargar datos en los controles
                lblTitulo.Text = _evento.nombre_evento;
                lblFechas.Text = $"Del {_evento.fecha_inicio_evento:dd/MM/yyyy} al {_evento.fecha_fin_evento:dd/MM/yyyy}";
                lblLugar.Text = $"Lugar: {_evento.lugar_evento}";
                txtDescripcion.Text = _evento.descripcion_evento;
                lblAsistentes.Text = $"Asistentes: {_eventoDto?.NumeroAsistentes ?? 0}/{_evento.capacidad_max_evento}";

                // Cargar la imagen si existe
                if (!string.IsNullOrEmpty(_evento.ruta_imagen_evento) && File.Exists(_evento.ruta_imagen_evento))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(_evento.ruta_imagen_evento);
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    catch (Exception ex)
                    {
                        // Si hay un error al cargar la imagen, simplemente no la mostramos
                        Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                    }
                }
                // Determinar si el usuario ya está registrado
                bool yaRegistrado = false;
                if (Session.CurrentUser != null)
                {
                    yaRegistrado = _eventoService.ConsultarAsistentes(_eventoId)
                        .Exists(u => u.id_usuario == Session.CurrentUser.id_usuario);
                }
                btnAsistir.Tag = yaRegistrado;
                ActualizarEstadoBotonAsistir();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el evento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ActualizarEstadoBotonAsistir()
        {
            if (Session.CurrentUser == null)
            {
                btnAsistir.Text = "Asistir";
                btnAsistir.BackColor = Color.FromArgb(0, 123, 255);
                return;
            }

            bool yaRegistrado = (bool)btnAsistir.Tag;

            if (yaRegistrado)
            {
                btnAsistir.Text = "Cancelar asistencia";
                btnAsistir.BackColor = Color.FromArgb(220, 53, 69); // Rojo
            }
            else
            {
                btnAsistir.Text = "Confirmar asistencia";
                btnAsistir.BackColor = Color.FromArgb(40, 167, 69); // Verde
            }
        }

        private void ConfigurarBotonAsistir()
        {
            btnAsistir.FlatAppearance.BorderSize = 0;
            btnAsistir.FlatStyle = FlatStyle.Flat;
            btnAsistir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAsistir.ForeColor = Color.White;
            btnAsistir.IconChar = IconChar.UserCheck;
            btnAsistir.IconColor = Color.White;
            btnAsistir.IconSize = 24;
            btnAsistir.TextImageRelation = TextImageRelation.ImageBeforeText;
            //btnAsistir.Click += btnAsistir_Click;
        }

        private void btnAsistir_Click(object sender, EventArgs e)
        {
            if (Session.CurrentUser == null)
            {
                MessageBox.Show("Debe iniciar sesión para registrar asistencia",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Validar si el evento ya finalizó
            if (_evento != null && _evento.fecha_fin_evento < DateTime.Now)
            {
                MessageBox.Show("No es posible registrar asistencia a un evento finalizado.",
                    "Evento finalizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool yaRegistrado = btnAsistir.Tag != null && (bool)btnAsistir.Tag;

            string mensaje;

            if (yaRegistrado)
            {
                mensaje = _eventoService.CancelarAsistencia(Session.CurrentUser.id_usuario, _eventoId);
            }
            else
            {
                if (_eventoDto?.NumeroAsistentes >= _evento.capacidad_max_evento)
                {
                    MessageBox.Show("El evento ha alcanzado su capacidad máxima",
                        "Cupo lleno",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                mensaje = _eventoService.RegistrarAsistencia(Session.CurrentUser.id_usuario, _eventoId);
            }

            MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarEvento(); // Recargar para actualizar el estado
            ActualizarEstadoBotonAsistir();
        }
        private void FrmDetalleEventoUser_Load(object sender, EventArgs e)
        {
            // Configuraciones adicionales al cargar el formulario
        }
    }
}