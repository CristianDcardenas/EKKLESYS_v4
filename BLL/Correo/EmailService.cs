using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Configuration;

namespace BLL
{
    public class EmailService
    {
        // Tu método existente
        public void EnviarCorreoRegistroEvento(string emailCliente, string nombreCliente, string nombreEvento, DateTime fechaEvento)
        {
            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 80%;
                        margin: 20px auto;
                        background-color: white;
                        padding: 20px;
                        border: 1px solid #ddd;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        color: #266593;
                        padding-bottom: 20px;
                        border-bottom: 2px solid #266593;
                    }}
                    .content {{
                        padding: 20px 0;
                    }}
                    .footer {{
                        padding-top: 20px;
                        border-top: 2px solid #266593;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color:#266593;'>¡Hola, {nombreCliente}!</h1>
                    </div>
                    <div class='content'>
                        <p style='font-size:1.2em;'>Has sido registrado exitosamente al evento:</p>
                        <h2 style='color:#333;'>{nombreEvento}</h2>
                        <p>Fecha del evento: <strong>{fechaEvento:dd/MM/yyyy}</strong></p>
                        <p>Gracias por participar. ¡Te esperamos!</p>
                    </div>
                    <div class='footer'>
                        <p style='color:#266593;'>Equipo de Eventos - EKKLESYS</p>
                    </div>
                </div>
            </body>
            </html>";

            EnviarCorreoHTML(emailCliente, "Confirmación de Registro al Evento", html);
        }

        // Nuevos métodos para notificaciones
        public void EnviarCorreoNuevoCurso(string emailCliente, string nombreCliente, string nombreCurso, DateTime fechaInicio, string descripcion)
        {
            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 80%;
                        margin: 20px auto;
                        background-color: white;
                        padding: 20px;
                        border: 1px solid #ddd;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        color: #28a745;
                        padding-bottom: 20px;
                        border-bottom: 2px solid #28a745;
                    }}
                    .content {{
                        padding: 20px 0;
                    }}
                    .footer {{
                        padding-top: 20px;
                        border-top: 2px solid #28a745;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color:#28a745;'>¡Nuevo Curso Disponible!</h1>
                    </div>
                    <div class='content'>
                        <p>Hola <strong>{nombreCliente}</strong>,</p>
                        <p style='font-size:1.2em;'>Te informamos que hay un nuevo curso disponible:</p>
                        <h2 style='color:#333;'>{nombreCurso}</h2>
                        <p><strong>Descripción:</strong> {descripcion}</p>
                        <p><strong>Fecha de inicio:</strong> {fechaInicio:dd/MM/yyyy}</p>
                        <p>¡No te pierdas esta oportunidad de aprender algo nuevo!</p>
                    </div>
                    <div class='footer'>
                        <p style='color:#28a745;'>Equipo Académico - EKKLESYS</p>
                    </div>
                </div>
            </body>
            </html>";

            EnviarCorreoHTML(emailCliente, "Nuevo Curso Disponible", html);
        }

        public void EnviarCorreoNuevoEvento(string emailCliente, string nombreCliente, string nombreEvento, DateTime fechaInicio, string lugar, string descripcion)
        {
            string html = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 80%;
                        margin: 20px auto;
                        background-color: white;
                        padding: 20px;
                        border: 1px solid #ddd;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    }}
                    .header {{
                        color: #dc3545;
                        padding-bottom: 20px;
                        border-bottom: 2px solid #dc3545;
                    }}
                    .content {{
                        padding: 20px 0;
                    }}
                    .footer {{
                        padding-top: 20px;
                        border-top: 2px solid #dc3545;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1 style='color:#dc3545;'>¡Nuevo Evento Programado!</h1>
                    </div>
                    <div class='content'>
                        <p>Hola <strong>{nombreCliente}</strong>,</p>
                        <p style='font-size:1.2em;'>Te invitamos a nuestro próximo evento:</p>
                        <h2 style='color:#333;'>{nombreEvento}</h2>
                        <p><strong>Descripción:</strong> {descripcion}</p>
                        <p><strong>Fecha:</strong> {fechaInicio:dd/MM/yyyy HH:mm}</p>
                        <p><strong>Lugar:</strong> {lugar}</p>
                        <p>¡Esperamos verte allí!</p>
                    </div>
                    <div class='footer'>
                        <p style='color:#dc3545;'>Equipo de Eventos - EKKLESYS </p>
                    </div>
                </div>
            </body>
            </html>";

            EnviarCorreoHTML(emailCliente, "Nuevo Evento Programado", html);
        }


        public void EnviarCorreoHTML(string emailCliente, string asunto, string htmlContent)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "EKKLESYS");
                mail.To.Add(emailCliente);
                mail.Subject = asunto;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlContent, Encoding.UTF8, MediaTypeNames.Text.Html);
                mail.AlternateViews.Add(htmlView);

                SmtpClient smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SmtpHost"],
                    Port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]),
                    Credentials = new NetworkCredential(
                        ConfigurationManager.AppSettings["EmailFrom"],
                        ConfigurationManager.AppSettings["EmailPassword"]
                    ),
                    EnableSsl = true
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar correo: {ex.Message}", ex);
            }
        }
    }
}