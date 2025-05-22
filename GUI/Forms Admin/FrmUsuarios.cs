using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmUsuarios : Form
    {
        private readonly UsuarioService _usuarioService;

        public FrmUsuarios()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
            ConfigurarDataGridView();
            CargarUsuarios();
        }

        private void ConfigurarDataGridView()
        {
            // Configuración básica del DataGridView
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Establecer el estilo de las filas alternas
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = _usuarioService.ConsultarDTO();
                dgvUsuarios.Rows.Clear();

                foreach (var usuario in usuarios)
                {
                    dgvUsuarios.Rows.Add(
                        usuario.id_usuario,
                        usuario.NombreCompleto,
                        usuario.email,
                        usuario.es_miembro,
                        usuario.es_administrador
                    );
                }

                // Ajustar el ancho de las columnas después de cargar los datos
                dgvUsuarios.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los usuarios: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            // Configurar visibilidad de botones según rol
            bool esAdmin = Session.CurrentUser?.es_administrador == "S";
            btnCambiarAdmin.Visible = esAdmin;
            btnCambiarMiembro.Visible = esAdmin;
        }

        private void btnCambiarAdmin_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un usuario para cambiar su rol", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Session.CurrentUser?.es_administrador != "S")
            {
                MessageBox.Show("Solo los administradores pueden cambiar roles", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int usuarioId = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["colId"].Value);
            string nombreUsuario = dgvUsuarios.SelectedRows[0].Cells["colNombre"].Value.ToString();
            bool esAdmin = dgvUsuarios.SelectedRows[0].Cells["colEsAdmin"].Value.ToString() == "Sí";

            string accion = esAdmin ? "quitar" : "otorgar";
            string mensaje = $"¿Está seguro que desea {accion} el rol de administrador a {nombreUsuario}?";

            var confirmacion = MessageBox.Show(
                mensaje,
                "Confirmar cambio de rol",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    // Cambiar el estado de administrador
                    string nuevoEstado = esAdmin ? "N" : "S";
                    var resultado = _usuarioService.CambiarRolAdministrador(usuarioId, nuevoEstado);

                    if (resultado.StartsWith("Error"))
                    {
                        MessageBox.Show(resultado, "Error al cambiar rol",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Rol de usuario actualizado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al procesar el cambio de rol: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCambiarMiembro_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un usuario para cambiar su estado de membresía", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Session.CurrentUser?.es_administrador != "S")
            {
                MessageBox.Show("Solo los administradores pueden cambiar el estado de membresía", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int usuarioId = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["colId"].Value);
            string nombreUsuario = dgvUsuarios.SelectedRows[0].Cells["colNombre"].Value.ToString();
            bool esMiembro = dgvUsuarios.SelectedRows[0].Cells["colEsMiembro"].Value.ToString() == "Sí";

            string accion = esMiembro ? "quitar" : "otorgar";
            string mensaje = $"¿Está seguro que desea {accion} el estado de miembro a {nombreUsuario}?";

            var confirmacion = MessageBox.Show(
                mensaje,
                "Confirmar cambio de membresía",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    // Cambiar el estado de miembro
                    string nuevoEstado = esMiembro ? "N" : "S";
                    var resultado = _usuarioService.CambiarEstadoMiembro(usuarioId, nuevoEstado);

                    if (resultado.StartsWith("Error"))
                    {
                        MessageBox.Show(resultado, "Error al cambiar estado de membresía",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Estado de membresía actualizado exitosamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al procesar el cambio de membresía: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}