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

namespace GUI
{
    public partial class FrmEditarCurso : Form
    {
        private readonly CursoService cursoService;
        private string rutaImagen;
        private readonly int? cursoId; // Nullable to indicate if we're editing or creating
        private Curso cursoExistente; // To store the existing course data when editing

        public FrmEditarCurso(int? cursoId = null)
        {
            InitializeComponent();
            cursoService = new CursoService();
            this.cursoId = cursoId;

            if (cursoId.HasValue)
            {
                // Editing mode
                CargarCurso(cursoId.Value);
                btnGuardar.Text = "Guardar Cambios"; // Change button text for editing
                this.Text = "Editar Curso";
            }
            else
            {
                // Creation mode
                dtpFechaInicio.Value = DateTime.Now;
                dtpFechaFin.Value = DateTime.Now.AddDays(30);
                this.Text = "Crear Curso";
            }
        }

        private void CargarCurso(int cursoId)
        {
            try
            {
                cursoExistente = cursoService.BuscarPorId(cursoId);
                if (cursoExistente == null)
                {
                    MessageBox.Show("El curso no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Populate the form fields with the existing course data
                txtNombreCurso.Text = cursoExistente.nombre_curso;
                txtDescripcion.Text = cursoExistente.descripcion_curso;
                dtpFechaInicio.Value = cursoExistente.fecha_inicio_curso;
                dtpFechaFin.Value = cursoExistente.fecha_fin_curso;
                nudCapacidad.Value = cursoExistente.capacidad_max_curso;

                // Load the image if it exists
                if (!string.IsNullOrEmpty(cursoExistente.ruta_imagen_curso) && File.Exists(cursoExistente.ruta_imagen_curso))
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(cursoExistente.ruta_imagen_curso);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        rutaImagen = cursoExistente.ruta_imagen_curso;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar la imagen existente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el curso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    // Crear una carpeta para las imágenes si no existe
                    string carpetaImagenes = Path.Combine(Application.StartupPath, "Imagenes", "Cursos");
                    if (!Directory.Exists(carpetaImagenes))
                    {
                        Directory.CreateDirectory(carpetaImagenes);
                    }

                    // Copiar la imagen a la carpeta de la aplicación si se seleccionó una nueva
                    string rutaImagenGuardada = cursoExistente?.ruta_imagen_curso; // Keep the existing image by default
                    if (!string.IsNullOrEmpty(rutaImagen) && (cursoExistente == null || rutaImagen != cursoExistente.ruta_imagen_curso))
                    {
                        string nombreArchivo = $"curso_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Path.GetFileName(rutaImagen)}";
                        rutaImagenGuardada = Path.Combine(carpetaImagenes, nombreArchivo);
                        File.Copy(rutaImagen, rutaImagenGuardada, true);
                    }

                    Curso curso = new Curso
                    {
                        id_curso = cursoId ?? 0, // Set the ID if editing, 0 if creating
                        nombre_curso = txtNombreCurso.Text,
                        descripcion_curso = txtDescripcion.Text,
                        fecha_inicio_curso = dtpFechaInicio.Value,
                        fecha_fin_curso = dtpFechaFin.Value,
                        capacidad_max_curso = (int)nudCapacidad.Value,
                        ruta_imagen_curso = rutaImagenGuardada
                    };

                    string resultado;
                    if (cursoId.HasValue)
                    {
                        // Editing mode: Update the existing course
                        resultado = cursoService.Modificar(curso);
                    }
                    else
                    {
                        // Creation mode: Save a new course
                        resultado = cursoService.Guardar(curso);
                    }

                    MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!resultado.StartsWith("Error"))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al procesar el curso: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreCurso.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del curso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor, ingrese la descripción del curso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (dtpFechaInicio.Value > dtpFechaFin.Value)
            {
                MessageBox.Show("La fecha de inicio no puede ser posterior a la fecha de fin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (nudCapacidad.Value < 1)
            {
                MessageBox.Show("La capacidad debe ser al menos 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FrmEditarCurso_Load(object sender, EventArgs e)
        {
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Seleccionar imagen del curso";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    rutaImagen = openFileDialog.FileName;

                    // Mostrar la imagen en el PictureBox
                    try
                    {
                        pictureBox1.Image = Image.FromFile(rutaImagen);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}