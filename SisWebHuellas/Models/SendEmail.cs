using Dominio.Entidades;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SisWebTickets.Models
{
    public class SendEmail
    {
        public string fromEmailAddress { get; set; }

        public string fromDisplayName { get; set; }

        public string fromEmailPassword { get; set; }

        public string smtpHost { get; set; }

        public string smtpPort { get; set; }

        public string urlWeb { get; set; }

        public SendEmail()
        {
            fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            fromDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            urlWeb = ConfigurationManager.AppSettings["urlWeb"].ToString();
        }

        public int enviarCorreo(TicketEN obj)
        {
            int send = 0;
            try
            {
                if (obj == null)
                    return send;
                string body = bodyEmail(obj).ToString();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmailAddress, fromDisplayName);
                message.To.Add(new MailAddress(obj.detalle.encargado.correo));
                message.Subject = "Control de Atención al Cliente - TECFLEX";
                message.IsBodyHtml = true;
                message.Body = body;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpHost;
                smtp.Port = int.Parse(smtpPort);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                smtp.Send(message);

                send= 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return send;
        }

        private StringBuilder bodyEmail(TicketEN objT)
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.AppendLine("<style> ");
            strBuild.AppendLine(".contain { ");
            strBuild.AppendLine("margin: auto; ");
            strBuild.AppendLine("width: 50%; ");
            strBuild.AppendLine("padding: 10px; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".card { ");
            strBuild.AppendLine("position: relative; ");
            strBuild.AppendLine("display: -ms-flexbox; ");
            strBuild.AppendLine("display: flex; ");
            strBuild.AppendLine("-ms-flex-direction: column; ");
            strBuild.AppendLine("flex-direction: column; ");
            strBuild.AppendLine("min-width: 0; ");
            strBuild.AppendLine("word-wrap: break-word; ");
            strBuild.AppendLine("background-color: #fff; ");
            strBuild.AppendLine("background-clip: border-box; ");
            strBuild.AppendLine("border: 1px solid rgba(0, 0, 0, 0.125); ");
            strBuild.AppendLine("border-radius: 0.25rem; ");
            strBuild.AppendLine("font-family: -apple-system, BlinkMacSystemFont, \"Segoe UI\", Roboto, \"Helvetica Neue\", Arial, sans-serif, \"Apple Color Emoji\", \"Segoe UI Emoji\", \"Segoe UI Symbol\", \"Noto Color Emoji\"; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".card-header { ");
            strBuild.AppendLine("padding: 0.75rem 1.25rem; ");
            strBuild.AppendLine("margin-bottom: 0; ");
            strBuild.AppendLine("background-color: rgba(0, 0, 0, 0.03); ");
            strBuild.AppendLine("border-bottom: 1px solid rgba(0, 0, 0, 0.125); ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".card-body { ");
            strBuild.AppendLine("-ms-flex: 1 1 auto; ");
            strBuild.AppendLine("flex: 1 1 auto; ");
            strBuild.AppendLine("padding: 1.25rem; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine("table, ");
            strBuild.AppendLine("td, ");
            strBuild.AppendLine("th { ");
            strBuild.AppendLine("border: 1px solid #ddd; ");
            strBuild.AppendLine("text-align: left; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine("table { ");
            strBuild.AppendLine("border-collapse: collapse; ");
            strBuild.AppendLine("width: 100%; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine("th, ");
            strBuild.AppendLine("td { ");
            strBuild.AppendLine("padding: 15px; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".col-md-6 { ");
            strBuild.AppendLine("width: 50%; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".btn { ");
            strBuild.AppendLine("border: none; ");
            strBuild.AppendLine("color: white; ");
            strBuild.AppendLine("padding: 10px; ");
            strBuild.AppendLine("text-align: center; ");
            strBuild.AppendLine("text-decoration: none; ");
            strBuild.AppendLine("display: inline-block; ");
            strBuild.AppendLine("font-size: 16px; ");
            strBuild.AppendLine("margin: 4px 2px; ");
            strBuild.AppendLine("cursor: pointer; ");
            strBuild.AppendLine("border-radius: 8px; ");
            strBuild.AppendLine("text-decoration: none; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".btn-danger { ");
            strBuild.AppendLine("color: #fff; ");
            strBuild.AppendLine("background-color: #dc3545; ");
            strBuild.AppendLine("border-color: #dc3545; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".col-sm-6 { ");
            strBuild.AppendLine("width: 50%; ");
            strBuild.AppendLine("float: left; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".text-logo-left { ");
            strBuild.AppendLine("text-align: right; ");
            strBuild.AppendLine("color: #cc1a20; ");
            strBuild.AppendLine("font-size: 200%; ");
            strBuild.AppendLine("margin-top: -5%; ");
            strBuild.AppendLine("margin-bottom: -5%; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".text-logo-right { ");
            strBuild.AppendLine("text-align: left; ");
            strBuild.AppendLine("color: #404343; ");
            strBuild.AppendLine("font-size: 200%; ");
            strBuild.AppendLine("margin-top: -5%; ");
            strBuild.AppendLine("margin-bottom: -5%; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".eslogan-left { ");
            strBuild.AppendLine("text-align: right; ");
            strBuild.AppendLine("font-size: 80%; ");
            strBuild.AppendLine("color: #404343; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".eslogan-right { ");
            strBuild.AppendLine("text-align: left; ");
            strBuild.AppendLine("font-size: 80%; ");
            strBuild.AppendLine("color: #404343; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine(".logo { ");
            strBuild.AppendLine("width: 20px; ");
            strBuild.AppendLine("height: 20px; ");
            strBuild.AppendLine("border: 3px solid #cc1a20; ");
            strBuild.AppendLine("background: #cc1a20; ");
            strBuild.AppendLine("-webkit-transform: rotate(45deg); ");
            strBuild.AppendLine("-moz-transform: rotate(45deg); ");
            strBuild.AppendLine("-ms-transform: rotate(45deg); ");
            strBuild.AppendLine("-o-transform: rotate(45deg); ");
            strBuild.AppendLine("transform: rotate(45deg); ");
            strBuild.AppendLine("display: inline-block; ");
            strBuild.AppendLine("} ");
            strBuild.AppendLine("    </style>   ");
            strBuild.AppendLine("    <main class=\"contain\">  ");
            strBuild.AppendLine("<div class=\"card\"> ");
            strBuild.AppendLine("<div class=\"card-header\"> ");
            strBuild.AppendLine("<div class=\"col-sm-6\"> ");
            strBuild.AppendLine("Sistema Control de Tickets - TECFLEX ");
            strBuild.AppendLine("</div> ");
            strBuild.AppendLine("</div> ");
            strBuild.AppendLine("<div class=\"card-body\"> ");
            strBuild.AppendLine("<p>Hola Estimado(a) " + objT.detalle.encargado.Nombre + ",</p> ");
            strBuild.AppendLine("<p>Se le asignó el ticket Nro <strong>" + objT.Nro + "</strong> desde el <strong>sistema de control de ");
            strBuild.AppendLine("tickets</strong> para que usted lo pueda atender ");
            strBuild.AppendLine("con la finalidad de poder brindar una solución al problema, a continuación se detalla el motivo de ");
            strBuild.AppendLine("su creación. </p> ");
            strBuild.AppendLine("<div class=\"col-md-6\"> ");
            strBuild.AppendLine("<table> ");
            strBuild.AppendLine("<tr> ");
            strBuild.AppendLine("<th>Contacto</th> ");
            strBuild.AppendLine("<td>" + objT.contacto + "</td> ");
            strBuild.AppendLine("</tr> ");
            strBuild.AppendLine("<tr> ");
            strBuild.AppendLine("<th>Empresa</th> ");
            strBuild.AppendLine("<td>" + objT.empresa + "</td> ");
            strBuild.AppendLine("</tr> ");
            strBuild.AppendLine("<tr> ");
            strBuild.AppendLine("<th>Motivo</th> ");
            strBuild.AppendLine("<td>" + objT.motivo.DescMotivo + "</td> ");
            strBuild.AppendLine("</tr> ");
            strBuild.AppendLine("<tr> ");
            strBuild.AppendLine("<th>Prioridad</th> ");
            strBuild.AppendLine("<td>" + objT.detalle.prioridad.DesPrio + "</td> ");
            strBuild.AppendLine("</tr> ");
            strBuild.AppendLine("</table> ");
            strBuild.AppendLine("</div> ");
            strBuild.AppendLine("<h5 class=\"card-title\">Descripción</h5> ");
            strBuild.AppendLine("<p class=\"card-text\">" + objT.detalle.descripcion + "</p> ");
            strBuild.AppendLine("<a href=\"" + urlWeb + "\" class=\"btn btn-danger\">Abrir en la Web</a> ");
            strBuild.AppendLine("</div> ");
            strBuild.AppendLine("</div> ");
            strBuild.AppendLine("</main>");

            return strBuild;
        }
    }
}