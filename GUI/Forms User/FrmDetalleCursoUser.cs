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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el curso: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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
            btnInscribir.Click += btnInscribir_Click;
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
                    var resultado = _cursoService.Inscribirse(Session.CurrentUser.id_usuario, _cursoId);

                    if (resultado.StartsWith("Error"))
                    {
                        MessageBox.Show(resultado, "Error al inscribir",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Inscripción exitosa", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Actualizar el número de inscritos
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