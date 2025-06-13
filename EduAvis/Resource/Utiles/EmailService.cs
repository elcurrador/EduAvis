using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EduAvis.Backend.Model;

namespace EduAvis.Resource.Utiles
{
    public class EmailService
    {
        private readonly string _fromEmail = "eduavis.system@gmail.com";
        private readonly string _appPassword = "xhcu xpzr lare zioi";

        public async Task<bool> SendEmailAsync(string toEmail, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_fromEmail, _appPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, "EduAvis System"),
                    Subject = "[EduAvis] Incident Notification",
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public string BuildIncidentEmailBody(Incident incident)
        {
            string descriptionRow = string.Empty;

            if (!string.IsNullOrWhiteSpace(incident.ReasonDescription))
            {
                descriptionRow = $@"
        <tr>
            <td style='padding: 6px 12px; font-weight: bold;'>Description:</td>
            <td style='padding: 6px 12px;'>{incident.ReasonDescription}</td>
        </tr>";
            }

            string typeLabel = incident.isSanction ? "Sanction" : "Warning";

            return $@"
<div style='font-family: Segoe UI, sans-serif; color: #333;'>
    <h2 style='color: #2C3E50;'>Disciplinary Incident Notification</h2>
    <p>Dear student,</p>
    <p>
        We would like to inform you of a disciplinary incident involving the following student:
    </p>
    <table style='border-collapse: collapse; margin-top: 10px;'>
        <tr>
            <td style='padding: 6px 12px; font-weight: bold;'>Student:</td>
            <td style='padding: 6px 12px;'>{incident.StudentNiaNavigation?.FullName}</td>
        </tr>
        <tr>
            <td style='padding: 6px 12px; font-weight: bold;'>Group:</td>
            <td style='padding: 6px 12px;'>{incident.StudentNiaNavigation?.GroupCode}</td>
        </tr>
        <tr>
            <td style='padding: 6px 12px; font-weight: bold;'>Date of Incident:</td>
            <td style='padding: 6px 12px;'>{incident.EventDatetime:dd/MM/yyyy}</td>
        </tr>
        <tr>
            <td style='padding: 6px 12px; font-weight: bold;'>Reason:</td>
            <td style='padding: 6px 12px;'>{incident.Reason?.ShortDescription}</td>
        </tr>
        <tr>
            <td style='padding: 6px 12px; font-weight: bold;'>Type:</td>
            <td style='padding: 6px 12px;'>{typeLabel}</td>
        </tr>
        {descriptionRow}
    </table>
    <p style='margin-top: 20px;'>
        This notice is part of the disciplinary protocol followed at EduAvis. Please review the incident accordingly.
    </p>
    <p>
        Sincerely,<br/>
        <strong>EduAvis Disciplinary System</strong>
    </p>
</div>";
        }




    }
}
