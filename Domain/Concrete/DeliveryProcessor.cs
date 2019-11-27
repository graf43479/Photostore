using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
   public class DeliveryProcessor : IDeliveryProcessor
    {

       private EmailSettings emailSettings;
       //private EmailSettings operatorEmailSettings;
         private PhotoDBContext context;


         public DeliveryProcessor(PhotoDBContext context, EmailSettings settings, EmailSettings operatorSettings)
        {
            this.context = context;
            emailSettings = settings;
           // operatorEmailSettings = operatorSettings;
            
            
          
        }


       public void EmailRecovery(User user, string host)
       {
           EmailTest();
           using (var smtpClient = new SmtpClient())
           {
           //    emailSettings.MailToAddress = user.Email;
               smtpClient.EnableSsl = emailSettings.UseSsl;
               smtpClient.Host = emailSettings.ServerName;
               smtpClient.Port = emailSettings.ServerPort;
               smtpClient.UseDefaultCredentials = false;
               smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);

               if (emailSettings.WriteAsFile)
               {
                   smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                   smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                   smtpClient.EnableSsl = false;
               }

               StringBuilder body = new StringBuilder()
                   .AppendLine("<p>Здравствуйте " + user.Login + "! Ваш логин и пароль для авторизации на сайте: </p>")
                   .AppendLine("<p>------------------------------------</p>")
                   .AppendLine("<p>Логин: " + user.Login + "</p>")
                   .AppendLine("<p>Пароль: " + user.Password + "</p>")
                   .AppendLine("<p>------------------------------------</p>")
                   .AppendLine("<p>Для авторизации пройдите <a href='http://" + host + "/Account/Login'>по ссылке</a></p>");

               MailMessage mailMessage = new MailMessage(
                   emailSettings.MailFromAddress,
               //    emailSettings.MailToAddress,
               user.Email,
                   "Восстановление пароля",
                   body.ToString()
                   );




               mailMessage.IsBodyHtml = true;

               if (emailSettings.WriteAsFile)
               {
                   mailMessage.BodyEncoding = Encoding.UTF8;
               }


               try
               {
                   smtpClient.Send(mailMessage);
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex);
               }
           }
       }

       public void EmailActivation(User user, string host)
       {
           EmailTest();
           string activationLink = "<p>Добрый день, " + user.Login + "</p><p>Спасибо за интерес, проявленный к нашему сайту</p>" +
               "<p>Вы получили уведомление, потому что была произведена регистрация Вашего адреса</p>" +
               "<p>Для активизации Вашего аккаунта пройдите по ниже следующей ссылке</p>";
           //   activationLink= activationLink + "<p> <a href='http://localhost:57600/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "'> http://localhost:57600/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "</a></p>";
           activationLink = activationLink + "<p> <a href='http://" + host + "/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "'> http://" + host + "/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "</a></p>";
           activationLink = activationLink + "<p>Ваш логин: " + user.Login + "</p>" + "<p>Ваш пароль: " + user.Password + "</p>" +
           "<p>Если Вы не предпринимали попытку регистрации на сайте, то, пожалуйста, проигнорируйте данное письмо</p>";

           //string url = HttpWebRequest.
           /* MailMessage mailMessage = new MailMessage(
               emailSettings.MailFromAddress,
               emailSettings.MailToAddress,
               "Активация аккаунта",
               activationLink
               );
            
            mailMessage.IsBodyHtml = true;*/

           MailMessage mailMessage = new MailMessage(
                       emailSettings.MailFromAddress,
               //emailSettings.MailToAddress,
                       user.Email,
                       "Активация аккаунта",
                       activationLink
                       );
           mailMessage.IsBodyHtml = true;


           if (emailSettings.WriteAsFile)
           {
               mailMessage.BodyEncoding = Encoding.UTF8;
           }

           using (var smtpClient = new SmtpClient())
           {
               // emailSettings.MailToAddress = user.Email;
               smtpClient.EnableSsl = emailSettings.UseSsl;
               smtpClient.Host = emailSettings.ServerName;
               smtpClient.Port = emailSettings.ServerPort;
               smtpClient.UseDefaultCredentials = false;
               smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);
               try
               {
                   smtpClient.Send(mailMessage);
               }
               catch (Exception)
               {

               }
           }
       }

       public void FeedBackRequest(Message message)
       {
           EmailTest();
           string activationLink = "<p>Добрый день! Клиент сайта " + Constants.SITE_NAME + ", представившийся " +
                                   "как " + message.Name + ", направил вам сообщение с обратным адресом: " +
                                   message.Email + "</p>" +
                                   "<p>Тест сообщения ниже</p>" +
                                   "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
                                   message.Text + "</p>" +
                                   "<p>Отвечайте на письмо на адрес: " + message.Email + "</p>"; 

           //string url = HttpWebRequest.
           /* MailMessage mailMessage = new MailMessage(
               emailSettings.MailFromAddress,
               emailSettings.MailToAddress,
               "Активация аккаунта",
               activationLink
               );
            
            mailMessage.IsBodyHtml = true;*/

           MailMessage mailMessage = new MailMessage(
                       emailSettings.MailFromAddress,
               emailSettings.MailFromAddress,
               //        user.Email,
                       Constants.SITE_NAME + ": " + message.Name + " написал сообщение",
                       activationLink
                       );
           mailMessage.IsBodyHtml = true;


           if (emailSettings.WriteAsFile)
           {
               mailMessage.BodyEncoding = Encoding.UTF8;
           }

           using (var smtpClient = new SmtpClient())
           {
               // emailSettings.MailToAddress = user.Email;
               smtpClient.EnableSsl = emailSettings.UseSsl;
               smtpClient.Host = emailSettings.ServerName;
               smtpClient.Port = emailSettings.ServerPort;
               smtpClient.UseDefaultCredentials = false;
               smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);
               try
               {
                   smtpClient.Send(mailMessage);
               }
               catch (Exception)
               {

               }
           }
       }


       public void EmailTest()
       {
           
               emailSettings.MailFromAddress = Constants.MAIL_FROM_ADDRESS;
               emailSettings.UseSsl = Constants.USE_SSL;
               emailSettings.UserName = Constants.USERNAME;
               emailSettings.Password = Constants.PASSWORD;
               emailSettings.ServerName = Constants.SERVERNAME;
               emailSettings.ServerPort = Constants.SERVER_PORT;
               emailSettings.WriteAsFile = Constants.WRITE_AS_FILE;
               emailSettings.FileLocation = @"c:/sportstore";//Constants.FILE_LOCATION;
           

        
               //operatorEmailSettings.MailFromAddress = Constants.MAIL_FROM_ADDRESS;
               //operatorEmailSettings.UseSsl = Constants.USE_SSL;
               //operatorEmailSettings.UserName = Constants.USERNAME;
               //operatorEmailSettings.Password = Constants.PASSWORD;
               //operatorEmailSettings.ServerName = Constants.SERVERNAME;
               //operatorEmailSettings.ServerPort = Constants.SERVER_PORT;
               //operatorEmailSettings.WriteAsFile = Constants.WRITE_AS_FILE;
               //operatorEmailSettings.FileLocation = @"c:/sportstore";
        
       }


       public class EmailSettings
       {
           public string MailToAddress;
           public string MailFromAddress;
           public bool UseSsl;
           public string UserName;
           public string Password;
           public string ServerName;
           public int ServerPort;
           public bool WriteAsFile;
           public string FileLocation;
       }
    }
}
