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
    public partial class FrmCrearEvento : Form
    {
        
        private readonly EventoService eventoService;
        private string rutaImagen;

        public FrmCrearEvento()
        {
            InitializeComponent();
            eventoService = new EventoService();
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now.AddDays(1);
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {
                    // Crear una carpeta para las imágenes si no existe
                    string carpetaImagenes = Path.Combine(Application.StartupPath, "Imagenes", "Eventos");
                    if (!Directory.Exists(carpetaImagenes))
                    {
                        Directory.CreateDirectory(carpetaImagenes);
                    }

                    // Copiar la imagen a la carpeta de la aplicación si se seleccionó una
                    string rutaImagenGuardada = null;
                    if (!string.IsNullOrEmpty(rutaImagen))
                    {
                        string nombreArchivo = $"evento_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Path.GetFileName(rutaImagen)}";
                        rutaImagenGuardada = Path.Combine(carpetaImagenes, nombreArchivo);
                        File.Copy(rutaImagen, rutaImagenGuardada, true);
                    }

                    Evento evento = new Evento
                    {
                        nombre_evento = txtNombreEvento.Text,
                        lugar_evento = txtLugar.Text,
                        descripcion_evento = txtDescripcion.Text,
                        fecha_inicio_evento = dtpFechaInicio.Value,
                        fecha_fin_evento = dtpFechaFin.Value,
                        capacidad_max_evento = (int)nudCapacidad.Value,
                        ruta_imagen_evento = rutaImagenGuardada
                    };

                    string resultado = eventoService.Guardar(evento);
                    MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!resultado.StartsWith("Error"))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al crear evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private void btnGuardar_Click(object sender, EventArgs e)
        //{

        //    if (ValidarCampos())
        //    {
        //        try
        //        {
        //            Evento evento = new Evento
        //            {
        //                nombre_evento = txtNombreEvento.Text,
        //                lugar_evento = txtLugar.Text,
        //                descripcion_evento = txtDescripcion.Text,
        //                fecha_inicio_evento = dtpFechaInicio.Value,
        //                fecha_fin_evento = dtpFechaFin.Value,
        //                capacidad_max_evento = (int)nudCapacidad.Value

        //            };

        //            string resultado = eventoService.Guardar(evento);
        //            MessageBox.Show(resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            if (!resultado.StartsWith("Error"))
        //            {
        //                this.DialogResult = DialogResult.OK;
        //                this.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error al crear evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombreEvento.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLugar.Text))
            {
                MessageBox.Show("Por favor, ingrese el lugar del evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Por favor, ingrese la descripción del evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Seleccionar imagen del evento";

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
