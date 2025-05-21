using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmAcercaDe : Form
    {
        public FrmAcercaDe()
        {
            InitializeComponent();

            // Cargar información de versión
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Versión {version.Major}.{version.Minor}.{version.Build}";

            // Establecer fecha de copyright
            lblCopyright.Text = $"© {DateTime.Now.Year} EKKLESYS. Todos los derechos reservados.";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Abre el navegador con la URL de tu sitio web
                Process.Start("https://www.ekklesys.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el enlace: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmAcercaDe_Paint(object sender, PaintEventArgs e)
        {
            // Dibujar un borde redondeado
            using (Pen pen = new Pen(Color.FromArgb(40, 103, 178), 2))
            {
                e.Graphics.DrawRoundedRectangle(pen, 1, 1, this.Width - 3, this.Height - 3, 10);
            }
        }

        private void panelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // Permitir mover el formulario sin bordes
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void panelPrincipal_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // Extensión para dibujar rectángulos redondeados
    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = GetRoundedRect(rectangle, radius);
            graphics.DrawPath(pen, path);
        }

        private static GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            // Si el radio es demasiado grande, ajustarlo
            if (radius > (Math.Min(baseRect.Width, baseRect.Height)) / 2)
                radius = (Math.Min(baseRect.Width, baseRect.Height)) / 2;

            // Crear el path para el rectángulo redondeado
            GraphicsPath path = new GraphicsPath();

            path.AddArc(baseRect.X, baseRect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(baseRect.Right - radius * 2, baseRect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(baseRect.Right - radius * 2, baseRect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(baseRect.X, baseRect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            return path;
        }
    }

    // Clase para llamadas a la API de Windows
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}