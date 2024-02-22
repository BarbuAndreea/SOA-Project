using MyDent.Domain.Enum;
using MyDent.Domain.Models;
using MyDent.Services.Abstractions;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace MyDent.Services.Helpers
{
    public class EmailHandler : IEmailHandler
    {
        private string _subject;
        private string _body;
        private SmtpClient _smtpClient;

        public void GenerateSmtpClient()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("andreealicenta10@gmail.com", "jrkvyobteyjceqkj"),
                EnableSsl = true,
            };
        }

        public void GenerateSubjectAndBodyForNewUserEmail(User user, string stream)
        {
            _subject = $"Welcome to MyDent, {user.FirstName} {user.LastName}!";
            _body = "<h3>We hope this email finds you well.</h3>";
            _body += "<p>You have been added to the MyDent platform! Below you will find the code needed for the clinic to add you!</p>";
            _body += "<p>With this QR code the clinic and the medics registered in it can see your personal informations.</p>";
            _body+= $"<p>You personal code is: {user.PersonalCode}</p>";
            _body+= $"<img alt='QR Code' title='qr' style='display: block' style='width:200px;height:200px' src='data:image/png;base64,{stream}'>";
            _body += "<p>Have a wonderful day!</p>";
        }

        public void GenerateSubjectAndBodyForAppoiment(Appointment appointment,User user,ActionTypeEnum type)
        {
            _subject = $"MyDent {type} appointment!";
            _body = "<h3>We hope this email finds you well.<h3>";
            if (type == ActionTypeEnum.New)
            {
                _body += $"<p>You have a appoitment to {appointment.StartDate.ToString("MMMM dd, yyyy")} from {appointment.StartDate.ToString("H:mm")} to {appointment.EndDate.ToString("H:mm")}. </p>";
            }
            else if (type == ActionTypeEnum.Edit)
            {
                _body += $"<p>Your dentist edit your appoitment to {appointment.StartDate.ToString("MMMM dd, yyyy")} from {appointment.StartDate.ToString("H:mm")} to {appointment.EndDate.ToString("H:mm")}. </p>";
            }
            _body += "<p>Have a wonderful day!</p>";
        }

        public void SendEmailAppoitment(Appointment appointment,User user, ActionTypeEnum type)
        {
            GenerateSubjectAndBodyForAppoiment(appointment, user, type);

            GenerateSmtpClient();
            MailAddress from = new MailAddress("andreealicenta10@gmail.com", "MyDent");

            var newBody = $"<p><b>Greetings dear {user.FirstName}, </b></p>" + _body;

            var message = new MailMessage(from, new MailAddress("barbu.andreea1510@gmail.com"));
            message.Subject = _subject;
            message.Body = newBody;
            message.IsBodyHtml = true;
            _smtpClient.Send(message);
            _subject="";
            _body="";
        }

        public void SendEmailToNewUser(User user, IQrCodeGenerator _qrCodeService)
        {
            var qrCode = _qrCodeService.GenerateQRCode(user.PersonalCode);
            var stream = new MemoryStream();
            qrCode.Save(stream, ImageFormat.Jpeg);
            byte[] byteImage = stream.ToArray();

            var qrcodeString = Convert.ToBase64String(byteImage);

            GenerateSubjectAndBodyForNewUserEmail(user, qrcodeString);

            GenerateSmtpClient();

            MailAddress from = new MailAddress("andreealicenta10@gmail.com", "MyDent");

            var newBody = "Greetings dear " + user.FirstName + ", \n\n" + _body;

            var message = new MailMessage(from, new MailAddress("barbu.andreea1510@gmail.com"));
            message.Subject = _subject;
            message.Body = newBody;
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(stream, "image/jpg"));
            _smtpClient.Send(message);
            _subject="";
            _body="";
        }
    }
}
