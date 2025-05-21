using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BLL;
using ENTITY;
using FontAwesome.Sharp;

namespace GUI
{
    public partial class FrmDetalleCursoAdmin : Form
    {
        private readonly CursoService _cursoService;
        private readonly int _cursoId;
        private Curso _curso;
        private CursoDTO _cursoDto;

        public FrmDetalleCursoAdmin(int cursoId)
        {
            InitializeComponent();
            _cursoService = new CursoService();
            _cursoId = cursoId;
            CargarCurso();
            ConfigurarBotonEliminar();
        }

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

                if (!string.IsNullOrEmpty(_curso.ruta_imagen_curso) && File.Exists(_curso.ruta_imagen_curso))
                {
                    try
                    {
                        // Usar una copia de la imagen para evitar problemas de "disposed object"
                        using (var originalImage = Image.FromFile(_curso.ruta_imagen_curso))
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

                    // Configurar visibilidad de botones según rol
                    ConfigurarVisibilidadBotones();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el curso: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ConfigurarVisibilidadBotones()
        {
            // Mostrar botón de eliminar solo para administradores
            btnEliminarCurso.Visible = Session.CurrentUser?.es_administrador == "S";
        }

        private void ConfigurarBotonEliminar()
        {
            btnEliminarCurso.BackColor = Color.FromArgb(192, 0, 0);
            btnEliminarCurso.FlatAppearance.BorderSize = 0;
            btnEliminarCurso.FlatStyle = FlatStyle.Flat;
            btnEliminarCurso.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEliminarCurso.ForeColor = Color.White;
            btnEliminarCurso.IconChar = IconChar.TrashAlt;
            btnEliminarCurso.IconColor = Color.White;
            btnEliminarCurso.IconSize = 24;
            btnEliminarCurso.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnEliminarCurso.Click += btnEliminarCurso_Click;
        }

        private void btnEliminarCurso_Click(object sender, EventArgs e)
        {
            if (Session.CurrentUser?.es_administrador != "S")
            {
                MessageBox.Show("Solo los administradores pueden eliminar cursos",
                    "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro que desea eliminar permanentemente el curso: {_curso.nombre_curso}?\n\nEsta acción no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    var resultado = _cursoService.Eliminar(_cursoId);

                    if (resultado.StartsWith("Error"))
                    {
                        MessageBox.Show(resultado, "Error al eliminar",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Curso eliminado exitosamente", "Éxito",
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

        private void btnVerEstudiantes_Click(object sender, EventArgs e)
        {
            FrmInscritosCurso frmInscritosCurso = new FrmInscritosCurso(_cursoId);
            frmInscritosCurso.Show();
        }

        private void FrmDetalleCursoAdmin_Load(object sender, EventArgs e)
        {
            // Configuraciones adicionales al cargar el formulario
        }

        // [...] (otros métodos existentes)
    }
}