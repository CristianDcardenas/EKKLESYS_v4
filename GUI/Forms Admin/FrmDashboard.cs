using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BLL;
using ENTITY;

namespace GUI
{
    public partial class FrmDashboard : Form
    {
        private readonly CursoService _cursoService;
        private readonly EventoService _eventoService;
        private List<CursoDTO> _cursos;
        private List<EventoDTO> _eventos;

        public FrmDashboard()
        {
            InitializeComponent();
            _cursoService = new CursoService();
            _eventoService = new EventoService();

            ConfigurarDashboard();
        }

        private void ConfigurarDashboard()
        {
            try
            {
                // Cargar los datos
                _cursos = _cursoService.ConsultarDTO();
                _eventos = _eventoService.ConsultarDTO();

                // Cargar estadísticas y gráficos
                CargarEstadisticasCursos();
                CargarEstadisticasEventos();
                CargarGraficoCursosPorFecha();
                CargarGraficoEventosPorMes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstadisticasCursos()
        {
            if (_cursos == null || _cursos.Count == 0)
            {
                lblTotalCursos.Text = "0";
                lblCursosActivos.Text = "0";
                lblCursosInactivos.Text = "0";
                lblInscripcionesCursos.Text = "0";
                return;
            }

            // Total de cursos
            lblTotalCursos.Text = _cursos.Count.ToString();

            // Cursos activos (fecha fin posterior a hoy)
            int cursosActivos = _cursos.Count(c => c.fecha_fin_curso >= DateTime.Today);
            lblCursosActivos.Text = cursosActivos.ToString();

            // Cursos inactivos
            lblCursosInactivos.Text = (_cursos.Count - cursosActivos).ToString();

            // Total de inscripciones
            int totalInscripciones = _cursos.Sum(c => c.NumeroInscritos);
            lblInscripcionesCursos.Text = totalInscripciones.ToString();
        }

        private void CargarEstadisticasEventos()
        {
            if (_eventos == null || _eventos.Count == 0)
            {
                lblTotalEventos.Text = "0";
                lblEventosActivos.Text = "0";
                lblEventosInactivos.Text = "0";
                lblInscripcionesEventos.Text = "0";
                return;
            }

            // Total de eventos
            lblTotalEventos.Text = _eventos.Count.ToString();

            // Eventos activos (fecha fin posterior a hoy)
            int eventosActivos = _eventos.Count(e => e.fecha_fin_evento >= DateTime.Today);
            lblEventosActivos.Text = eventosActivos.ToString();

            // Eventos inactivos
            lblEventosInactivos.Text = (_eventos.Count - eventosActivos).ToString();

            // Total de asistentes
            int totalAsistentes = _eventos.Sum(e => e.NumeroAsistentes);
            lblInscripcionesEventos.Text = totalAsistentes.ToString();
        }

        private void CargarGraficoCursosPorFecha()
        {
            // Limpiar series existentes
            chartCursos.Series.Clear();
            chartCursos.Titles.Clear();

            if (_cursos == null || _cursos.Count == 0)
            {
                chartCursos.Titles.Add("No hay cursos disponibles");
                return;
            }

            // Agrupar cursos por mes de inicio
            var cursosPorMes = _cursos
                .GroupBy(c => new { Mes = c.fecha_inicio_curso.Month, Año = c.fecha_inicio_curso.Year })
                .Select(g => new
                {
                    Fecha = $"{g.Key.Mes}/{g.Key.Año}",
                    Cantidad = g.Count(),
                    Inscritos = g.Sum(c => c.NumeroInscritos)
                })
                .OrderBy(x => DateTime.Parse($"{x.Fecha}/1"))
                .ToList();

            // Crear serie para cantidad de cursos
            Series seriesCursos = new Series("Cantidad de Cursos");
            seriesCursos.ChartType = SeriesChartType.Column;
            seriesCursos.Color = Color.FromArgb(0, 191, 255);

            // Crear serie para cantidad de inscritos
            Series seriesInscritos = new Series("Inscritos");
            seriesInscritos.ChartType = SeriesChartType.Line;
            seriesInscritos.Color = Color.FromArgb(255, 128, 0);
            seriesInscritos.BorderWidth = 3;
            seriesInscritos.MarkerStyle = MarkerStyle.Circle;
            seriesInscritos.MarkerSize = 8;
            seriesInscritos.YAxisType = AxisType.Secondary;

            // Agregar puntos de datos
            foreach (var item in cursosPorMes)
            {
                seriesCursos.Points.AddXY(item.Fecha, item.Cantidad);
                seriesInscritos.Points.AddXY(item.Fecha, item.Inscritos);
            }

            // Configurar el gráfico
            chartCursos.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;

            // Agregar series al gráfico
            chartCursos.Series.Add(seriesCursos);
            chartCursos.Series.Add(seriesInscritos);

            // Configurar leyenda
            chartCursos.Legends[0].Enabled = true;
            chartCursos.Legends[0].Docking = Docking.Bottom;

            // Configurar título
            chartCursos.Titles.Add("Cursos e Inscripciones por Mes");
            chartCursos.Titles[0].Font = new Font("Segoe UI", 12, FontStyle.Bold);
        }

        private void CargarGraficoEventosPorMes()
        {
            // Limpiar series existentes
            chartEventos.Series.Clear();
            chartEventos.Titles.Clear();

            if (_eventos == null || _eventos.Count == 0)
            {
                chartEventos.Titles.Add("No hay eventos disponibles");
                return;
            }

            // Crear nueva serie
            Series series = new Series("Eventos por Mes");
            series.ChartType = SeriesChartType.Pie;

            // Agrupar eventos por mes
            var eventosPorMes = _eventos
                .GroupBy(e => e.fecha_inicio_evento.Month)
                .Select(g => new
                {
                    Mes = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Cantidad = g.Count()
                })
                .OrderBy(x => x.Mes)
                .ToList();

            // Agregar puntos de datos
            foreach (var item in eventosPorMes)
            {
                int puntoIndice = series.Points.AddXY(item.Mes, item.Cantidad);

                // Asignar colores diferentes a cada punto
                switch (puntoIndice % 5)
                {
                    case 0: series.Points[puntoIndice].Color = Color.FromArgb(255, 191, 0); break;
                    case 1: series.Points[puntoIndice].Color = Color.FromArgb(0, 191, 255); break;
                    case 2: series.Points[puntoIndice].Color = Color.FromArgb(0, 255, 191); break;
                    case 3: series.Points[puntoIndice].Color = Color.FromArgb(191, 0, 255); break;
                    case 4: series.Points[puntoIndice].Color = Color.FromArgb(191, 255, 0); break;
                }
            }

            // Configurar etiquetas
            series.Label = "#PERCENT{P0}";
            series.LegendText = "#VALX: #VAL";

            // Agregar serie al gráfico
            chartEventos.Series.Add(series);

            // Configurar leyenda
            chartEventos.Legends[0].Enabled = true;
            chartEventos.Legends[0].Docking = Docking.Bottom;

            // Configurar título
            chartEventos.Titles.Add("Distribución de Eventos por Mes");
            chartEventos.Titles[0].Font = new Font("Segoe UI", 12, FontStyle.Bold);
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            ConfigurarDashboard();
        }
    }
}