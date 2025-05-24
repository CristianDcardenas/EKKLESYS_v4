using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BLL;
using ENTITY;
using FontAwesome.Sharp;

namespace GUI
{
    public partial class FrmDetalleCursoUser : Form
    {
        private readonly CursoService _cursoService;
        private readonly int _cursoId;
        private Curso _curso;
        private CursoDTO _cursoDto;

        public FrmDetalleCursoUser(int cursoId)
        {
            InitializeComponent();
            _cursoService = new CursoService();
            _cursoId = cursoId;
            CargarCurso();
            ConfigurarBotonInscribir();
            
        }



        // Modificar el método CargarCurso para cargar la imagen
        private void CargarCurso()
        {
            try
            {
                _curso = _cursoService.BuscarPorId(_cursoId);

                if (_curso == null)
                {
                    MessageBox.Show("El curso no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                var cursosDto = _cursoService.ConsultarDTO();
                _cursoDto = cursosDto.Find(c => c.id_curso == _cursoId);

                // Cargar datos en los controles
                lblTitulo.Text = _curso.nombre_curso;
                lblFechas.Text = $"Del {_curso.fecha_inicio_curso:dd/MM/yyyy} al {_curso.fecha_fin_curso:dd/MM/yyyy}";
                txtDescripcion.Text = _curso.descripcion_curso;
                lblInscritos.Text = $"Inscritos: {_cursoDto?.NumeroInscritos ?? 0}/{_curso.capacidad_max_curso}";

                // Cargar la imagen si existe
                if (!string.IsNullOrEmpty(_curso.ruta_imagen_curso) && File.Exists(_curso.ruta_imagen_curso))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(_curso.ruta_imagen_curso);
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    catch (Exception ex)
                    {
                        // Si hay un error al cargar la imagen, simplemente no la mostramos
                        Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                    }
                }
                // Determinar si el usuario ya está inscrito
                bool yaInscrito = false;
                if (Session.CurrentUser != null)
                {
                    yaInscrito = _cursoService.ConsultarEstudiantes(_cursoId)
                        .Exists(u => u.id_usuario == Session.CurrentUser.id_usuario);
                }
                btnInscribir.Tag = yaInscrito;
                ActualizarEstadoBotonInscribir();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el curso: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ActualizarEstadoBotonInscribir()
        {
            if (Session.CurrentUser == null)
            {
                btnInscribir.Text = "Asistir";
                btnInscribir.BackColor = Color.FromArgb(0, 123, 255);
                return;
            }

            bool yaRegistrado = (bool)btnInscribir.Tag;

            if (yaRegistrado)
            {
                btnInscribir.Text = "Cancelar asistencia";
                btnInscribir.BackColor = Color.FromArgb(220, 53, 69); // Rojo
            }
            else
            {
                btnInscribir.Text = "Confirmar asistencia";
                btnInscribir.BackColor = Color.FromArgb(40, 167, 69); // Verde
            }
        }
        

        private void ConfigurarBotonInscribir()
        {
            btnInscribir.BackColor = Color.FromArgb(0, 123, 255);
            btnInscribir.FlatAppearance.BorderSize = 0;
            btnInscribir.FlatStyle = FlatStyle.Flat;
            btnInscribir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnInscribir.ForeColor = Color.White;
            btnInscribir.IconChar = IconChar.UserPlus;
            btnInscribir.IconColor = Color.White;
            btnInscribir.IconSize = 24;
            btnInscribir.TextImageRelation = TextImageRelation.ImageBeforeText;
            //btnInscribir.Click += btnInscribir_Click;
        }

        private void btnInscribir_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session.CurrentUser == null)
                {
                    MessageBox.Show("Debe iniciar sesión o registrarse para inscribirse en un curso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Validar si el curso ya finalizó
                if (_curso != null && _curso.fecha_fin_curso < DateTime.Now)
                {
                    MessageBox.Show("No es posible inscribirse a un curso que ha finalizado.",
                        "Curso finalizado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                bool yaInscrito = btnInscribir.Tag != null && (bool)btnInscribir.Tag;
                string resultado;

                if (yaInscrito)
                {
                    // Cancelar inscripción
                    var confirmacion = MessageBox.Show(
                        $"¿Desea cancelar su inscripción en el curso: {_curso.nombre_curso}?",
                        "Cancelar inscripción",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                    if (confirmacion == DialogResult.Yes)
                    {
                        resultado = _cursoService.CancelarInscripcion(Session.CurrentUser.id_usuario, _cursoId);
                        MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarCurso();
                    }
                }
                else
                {
                    if (_cursoDto?.NumeroInscritos >= _curso.capacidad_max_curso)
                    {
                        MessageBox.Show("El curso ha alcanzado su capacidad máxima", "Cupo lleno",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var confirmacion = MessageBox.Show(
                        $"¿Desea inscribirse en el curso: {_curso.nombre_curso}?",
                        "Confirmar inscripción",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                    if (confirmacion == DialogResult.Yes)
                    {
                        resultado = _cursoService.Inscribirse(Session.CurrentUser.id_usuario, _cursoId);
                        MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarCurso();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la inscripción: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmDetalleCursoUser_Load(object sender, EventArgs e)
        {
            // Configuraciones adicionales al cargar el formulario
        }
    }
}