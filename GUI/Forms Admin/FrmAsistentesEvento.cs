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
    public partial class FrmAsistentesEvento : Form
    {
        private readonly EventoService eventoService;
        private readonly int idEvento;

        public FrmAsistentesEvento(int idEvento)
        {
            InitializeComponent();
            eventoService = new EventoService();
            this.idEvento = idEvento;
            // Configurar el evento KeyDown para el TextBox
            txtBusqueda.KeyDown += TxtBusqueda_KeyDown;
            CargarDatos();
        }

        private void CargarDatos(string filtro = null)
        {
            var evento = eventoService.BuscarPorId(idEvento);
            if (evento != null)
            {
                lblTitulo.Text = $"Asistentes al evento: {evento.nombre_evento}";
                lblCapacidad.Text = $"Capacidad: {evento.NumeroAsistentes} / {evento.capacidad_max_evento}";

                dgvAsistentes.Rows.Clear();
                var asistentesFiltrados = string.IsNullOrWhiteSpace(filtro)
                    ? evento.Asistentes
                    : evento.Asistentes.Where(a => a.NombreCompleto.ToLower().Contains(filtro.ToLower())).ToList();

                foreach (var asistente in asistentesFiltrados)
                {
                    dgvAsistentes.Rows.Add(asistente.id_usuario, asistente.NombreCompleto, asistente.email, asistente.telefono);
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