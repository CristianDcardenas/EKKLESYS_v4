using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace GUI
{
    public partial class FrmInscritosCurso : Form
    {
        private readonly CursoService cursoService;
        private readonly int idCurso;

        public FrmInscritosCurso(int idCurso)
        {
            InitializeComponent();
            cursoService = new CursoService();
            this.idCurso = idCurso;
            // Configurar el evento KeyDown para el TextBox
            txtBusqueda.KeyDown += TxtBusqueda_KeyDown;
            CargarDatos();
        }

        public FrmInscritosCurso()
        {
            InitializeComponent();
        }

        private void CargarDatos(string filtro = null)
        {
            var curso = cursoService.BuscarPorId(idCurso);
            if (curso != null)
            {
                lblTitulo.Text = $"Inscritos en el curso: {curso.nombre_curso}";
                lblCapacidad.Text = $"Capacidad: {curso.NumeroInscritos} / {curso.capacidad_max_curso}";

                dgvInscritos.Rows.Clear();
                var estudiantesFiltrados = string.IsNullOrWhiteSpace(filtro)
                    ? curso.Estudiantes
                    : curso.Estudiantes.Where(e => e.NombreCompleto.ToLower().Contains(filtro.ToLower())).ToList();

                foreach (var estudiante in estudiantesFiltrados)
                {
                    dgvInscritos.Rows.Add(estudiante.id_usuario, estudiante.NombreCompleto, estudiante.email, estudiante.telefono);
                }

                // Limpiar el TextBox después de la búsqueda
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    txtBusqueda.Text = string.Empty;
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBusqueda.Text.Trim();
            CargarDatos(filtro);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = string.Empty;
            CargarDatos();
        }

        private void TxtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true; // Evita el sonido del Enter
                string filtro = txtBusqueda.Text.Trim();
                CargarDatos(filtro);
            }
        }
    }
}