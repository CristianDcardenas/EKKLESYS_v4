using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using ENTITY;
using static System.Collections.Specialized.BitVector32;

namespace GUI
{
    public partial class PerfilForm : Form
    {
        private readonly UsuarioService usuarioService;
        private readonly ComunaService comunaService;
        private readonly CursoService cursoService;
        private readonly EventoService eventoService;
        private string rutaImagenSeleccionada;

        public PerfilForm()
        {
            InitializeComponent();
            usuarioService = new UsuarioService();
            comunaService = new ComunaService();
            cursoService = new CursoService();
            eventoService = new EventoService();

            CargarComunas();
            CargarDatosUsuario();
            CargarCursos();
            CargarEventos();
        }

        private void CargarComunas()
        {
            var comunas = comunaService.ConsultarTodas();
            cmbComuna.DataSource = comunas;
            cmbComuna.DisplayMember = "nombre_comuna";
            cmbComuna.ValueMember = "id_comuna";
        }

        private void CargarDatosUsuario()
        {
            if (Session.CurrentUser != null)
            {
                txtNombre.Text = Session.CurrentUser.nombre;
                txtApellido.Text = Session.CurrentUser.apellido_paterno;
                dtpFechaNacimiento.Value = Session.CurrentUser.fecha_nacimiento;
                txtDireccion.Text = Session.CurrentUser.direccion;
                txtTelefono.Text = Session.CurrentUser.telefono;
                txtTelefono2.Text = Session.CurrentUser.telefono_2;
                txtEmail.Text = Session.CurrentUser.email;
                txtCedula.Text = Session.CurrentUser.cedula;

                if (Session.CurrentUser.es_miembro == "S")
                {
                    rbSi.Checked = true;
                }
                else
                {
                    rbNo.Checked = true;
                }
                cmbComuna.SelectedValue = Session.CurrentUser.id_comuna;

                if (!string.IsNullOrEmpty(Session.CurrentUser.ruta_imagen_usuario) && File.Exists(Session.CurrentUser.ruta_imagen_usuario))
                {
                    pictureBox.Image = System.Drawing.Image.FromFile(Session.CurrentUser.ruta_imagen_usuario);
                    txtRutaImagen.Text = Path.GetFileName(Session.CurrentUser.ruta_imagen_usuario);
                    rutaImagenSeleccionada = Session.CurrentUser.ruta_imagen_usuario;
                }
            }
        }


        private void CargarCursos()
        {
            if (Session.CurrentUser != null)
            {
                List<Curso> cursos;
                if (Session.CurrentUser.es_administrador == "S")
                {
                    try
                    {
                        int idAdministrador = cursoService.ObtenerIdAdministradorPorUsuario(Session.CurrentUser.id_usuario);
                        cursos = cursoService.Consultar()
                            .Where(c => c.id_administrador == idAdministrador)
                            .ToList();
                    }
                    catch
                    {
                        // Si no es admin registrado, no mostrar cursos
                        cursos = new List<Curso>();
                    }
                }
                else
                {
                    cursos = cursoService.ConsultarCursosPorUsuario(Session.CurrentUser.id_usuario);
                }
                dgvCursos.Rows.Clear();
                foreach (var curso in cursos)
                {
                    dgvCursos.Rows.Add(curso.nombre_curso, curso.fecha_inicio_curso.ToShortDateString(), curso.fecha_fin_curso.ToShortDateString());
                }
            }
        }

        private void CargarEventos()
        {
            if (Session.CurrentUser != null)
            {
                List<Evento> eventos;
                if (Session.CurrentUser.es_administrador == "S")
                {
                    // Obtener el id_administrador real
                    int idAdministrador = 0;
                    try
                    {
                        idAdministrador = eventoService.ObtenerIdAdministradorPorUsuario(Session.CurrentUser.id_usuario);
                    }
                    catch
                    {
                        // Si no es admin registrado, no mostrar eventos
                        eventos = new List<Evento>();
                        dgvEventos.Rows.Clear();
                        return;
                    }
                    eventos = eventoService.Consultar()
                        .Where(e => e.id_administrador == idAdministrador)
                        .ToList();
                }
                else
                {
                    eventos = eventoService.ConsultarEventosPorUsuario(Session.CurrentUser.id_usuario);
                }
                dgvEventos.Rows.Clear();
                foreach (var evento in eventos)
                {
                    dgvEventos.Rows.Add(evento.nombre_evento, evento.fecha_inicio_evento.ToShortDateString(), evento.fecha_fin_evento.ToShortDateString());
                }
            }
        }
        //private void CargarCursos()
        //{
        //    if (Session.CurrentUser != null)
        //    {
        //        //var cursos = cursoService.ConsultarCursosPorUsuario(Session.CurrentUser.id_usuario);
        //        List<Curso> cursos;
        //        if (Session.CurrentUser.es_administrador == "S")
        //        {
        //            // Mostrar cursos creados por el administrador
        //            cursos = cursoService.Consultar()
        //                .Where(c => c.id_administrador == Session.CurrentUser.id_usuario)
        //                .ToList();
        //        }
        //        else
        //        {
        //            // Mostrar cursos en los que el usuario está inscrito
        //            cursos = cursoService.ConsultarCursosPorUsuario(Session.CurrentUser.id_usuario);
        //        }
        //        dgvCursos.Rows.Clear();
        //        foreach (var curso in cursos)
        //        {
        //            dgvCursos.Rows.Add(curso.nombre_curso, curso.fecha_inicio_curso.ToShortDateString(), curso.fecha_fin_curso.ToShortDateString());
        //        }
        //    }
        //}

        //private void CargarEventos()
        //{
        //    if (Session.CurrentUser != null)
        //    {
        //        //var eventos = eventoService.ConsultarEventosPorUsuario(Session.CurrentUser.id_usuario);
        //        List<Evento> eventos;
        //        if (Session.CurrentUser.es_administrador == "S")
        //        {
        //            // Mostrar eventos creados por el administrador
        //            eventos = eventoService.Consultar()
        //                .Where(e => e.id_administrador == Session.CurrentUser.id_usuario)
        //                .ToList();
        //        }
        //        else
        //        {
        //            // Mostrar eventos en los que el usuario está inscrito
        //            eventos = eventoService.ConsultarEventosPorUsuario(Session.CurrentUser.id_usuario);
        //        }
        //        dgvEventos.Rows.Clear();
        //        foreach (var evento in eventos)
        //        {
        //            dgvEventos.Rows.Add(evento.nombre_evento, evento.fecha_inicio_evento.ToShortDateString(), evento.fecha_fin_evento.ToShortDateString());
        //        }
        //    }
        //}

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    Usuario usuario = new Usuario
                    {
                        id_usuario = Session.CurrentUser.id_usuario,
                        nombre = txtNombre.Text,
                        apellido_paterno = txtApellido.Text,
                        fecha_nacimiento = dtpFechaNacimiento.Value,
                        direccion = txtDireccion.Text,
                        telefono = txtTelefono.Text,
                        telefono_2 = string.IsNullOrWhiteSpace(txtTelefono2.Text) ? null : txtTelefono2.Text,
                        email = txtEmail.Text,
                        cedula = txtCedula.Text,
                        clave = Session.CurrentUser.clave,
                        es_miembro = rbSi.Checked ? "S" : "N",
                        ruta_imagen_usuario = rutaImagenSeleccionada ?? Session.CurrentUser.ruta_imagen_usuario,
                        id_comuna = (int)cmbComuna.SelectedValue,
                        es_administrador = Session.CurrentUser.es_administrador
                    };

                    string resultado = usuarioService.Modificar(usuario);
                    MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!resultado.StartsWith("Error"))
                    {
                        // Actualizar usuario en sesión
                        Session.CurrentUser = usuarioService.BuscarPorId(Session.CurrentUser.id_usuario);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingrese su nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Por favor, ingrese su apellido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Por favor, ingrese su dirección", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Por favor, ingrese su teléfono", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Por favor, ingrese su correo electrónico", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCedula.Text))
            {
                MessageBox.Show("Por favor, ingrese su cédula", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (cmbComuna.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, seleccione una comuna", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        //private void btnSeleccionarImagen_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //    {
        //        openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
        //        openFileDialog.Title = "Seleccionar imagen de perfil";

        //        if (openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            rutaImagenSeleccionada = openFileDialog.FileName;
        //            txtRutaImagen.Text = Path.GetFileName(rutaImagenSeleccionada);
        //            pictureBox.Image = System.Drawing.Image.FromFile(rutaImagenSeleccionada);
        //        }
        //    }
        //}
        private void btnSeleccionarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Seleccionar imagen de perfil";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string rutaOriginal = openFileDialog.FileName;
                    rutaImagenSeleccionada = CopiarImagenADirectorioApp(rutaOriginal);
                    txtRutaImagen.Text = Path.GetFileName(rutaImagenSeleccionada);
                    pictureBox.Image = System.Drawing.Image.FromFile(rutaImagenSeleccionada);
                }
            }
        }
        private string CopiarImagenADirectorioApp(string rutaOriginal)
        {
            try
            {
                string directorioDestino = Path.Combine(Application.StartupPath, "ImagenesUsuarios");

                // Crear el directorio si no existe
                if (!Directory.Exists(directorioDestino))
                    Directory.CreateDirectory(directorioDestino);

                string nombreArchivo = $"{Session.CurrentUser.id_usuario}_{DateTime.Now.Ticks}{Path.GetExtension(rutaOriginal)}";
                string rutaDestino = Path.Combine(directorioDestino, nombreArchivo);

                File.Copy(rutaOriginal, rutaDestino, true);
                return rutaDestino;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al copiar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return rutaOriginal; // Devolver la ruta original en caso de error
            }
        }

        private void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            FrmCambiarPassword frmCambiarPassword = new FrmCambiarPassword();
            frmCambiarPassword.ShowDialog();
        }

        private void PerfilForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvCursos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
