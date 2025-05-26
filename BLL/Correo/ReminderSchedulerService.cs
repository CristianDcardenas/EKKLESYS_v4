using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BLL
{
    /// <summary>
    /// Servicio para programar y ejecutar recordatorios automáticamente
    /// </summary>
    public class ReminderSchedulerService
    {
        private readonly EmailNotificationService _emailService;
        private System.Timers.Timer _timer;
        private readonly object _lockObject = new object();

        public ReminderSchedulerService()
        {
            _emailService = new EmailNotificationService();
        }

        /// <summary>
        /// Inicia el servicio de recordatorios programados
        /// Se ejecuta todos los días a las 9:00 AM
        /// </summary>
        public void IniciarServicio()
        {
            // Calcular el tiempo hasta las 9:00 AM del día siguiente
            var ahora = DateTime.Now;
            var proximaEjecucion = DateTime.Today.AddDays(1).AddHours(9); // 9:00 AM mañana

            // Si aún no son las 9:00 AM de hoy, ejecutar hoy
            if (ahora.Hour < 9)
            {
                proximaEjecucion = DateTime.Today.AddHours(9); // 9:00 AM hoy
            }

            var tiempoHastaProximaEjecucion = proximaEjecucion - ahora;

            Console.WriteLine($"Servicio de recordatorios iniciado. Próxima ejecución: {proximaEjecucion}");

            // Timer para la primera ejecución
            var timerInicial = new System.Timers.Timer(tiempoHastaProximaEjecucion.TotalMilliseconds);
            timerInicial.Elapsed += (sender, e) =>
            {
                timerInicial.Stop();
                timerInicial.Dispose();

                // Ejecutar recordatorios
                EjecutarRecordatorios();

                // Configurar timer diario (cada 24 horas)
                ConfigurarTimerDiario();
            };
            timerInicial.Start();
        }

        private void ConfigurarTimerDiario()
        {
            // Timer que se ejecuta cada 24 horas
            _timer = new System.Timers.Timer(TimeSpan.FromHours(24).TotalMilliseconds);
            _timer.Elapsed += (sender, e) => EjecutarRecordatorios();
            _timer.Start();

            Console.WriteLine("Timer diario configurado para ejecutarse cada 24 horas a las 9:00 AM");
        }

        private void EjecutarRecordatorios()
        {
            lock (_lockObject)
            {
                try
                {
                    Console.WriteLine($"=== Ejecutando recordatorios automáticos - {DateTime.Now} ===");
                    _emailService.EjecutarRecordatoriosDiarios();
                    Console.WriteLine("=== Recordatorios automáticos completados ===");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en la ejecución automática de recordatorios: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Ejecuta los recordatorios manualmente (para testing o ejecución manual)
        /// </summary>
        public void EjecutarRecordatoriosManual()
        {
            Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("Ejecutando recordatorios manualmente...");
                    _emailService.EjecutarRecordatoriosDiarios();
                    Console.WriteLine("Recordatorios manuales completados.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en recordatorios manuales: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Detiene el servicio de recordatorios
        /// </summary>
        public void DetenerServicio()
        {
            _timer?.Stop();
            _timer?.Dispose();
            Console.WriteLine("Servicio de recordatorios detenido.");
        }

        /// <summary>
        /// Verifica si el servicio está activo
        /// </summary>
        public bool EstaActivo => _timer?.Enabled ?? false;
    }
}