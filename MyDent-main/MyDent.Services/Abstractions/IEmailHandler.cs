using MyDent.Domain.Enum;
using MyDent.Domain.Models;

namespace MyDent.Services.Abstractions
{
    public interface IEmailHandler
    {
        void GenerateSmtpClient();

        void GenerateSubjectAndBodyForNewUserEmail(User user, string stream);

        void SendEmailToNewUser(User user, IQrCodeGenerator _qrCodeService);
        void SendEmailAppoitment(Appointment appointment,User user, ActionTypeEnum @new);
    }
}
