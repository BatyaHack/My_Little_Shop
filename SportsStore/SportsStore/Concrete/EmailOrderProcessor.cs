using System.Net.Mail;
using System.Text;
using SportsStore.Models.Abstract;
using SportsStore.Models.Entities;
using System.Net;
using System;

namespace SportsStore.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "kortyailya@gmail.com";
        public string MailFromAdress = "vasa11514@gmail.com";
        public bool UseSsh = true;
        public string Username = "vasa11514@gmail.com";
        public string Password = "qwertyzxcvb";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\sports_store_emails";
        //smtp.gmail.com
        //qwertyzxcvb
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }


        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsh;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username,
emailSettings.Password);
                if (emailSettings.WriteAsFile) //Если нету smpt сервера позволяет просто записывать файлы в дерикторию
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .Append("Новый заказ")
                    .Append("------------")
                    .Append("Предметы: ");

                foreach (var item in cart.Lines)
                {
                    var subtotal = item.Product.Price * item.Quantity;
                    body.AppendFormat("{0} x {1} Сумма {2:c}", item.Product.Name, item.Quantity, subtotal);
                }

                body.AppendFormat("Всего: {0}", cart.ComputeTotalValue())
                    .Append("-----------")
                    .Append("Доставить: ")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                    .AppendLine(shippingDetails.Line3 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.State ?? "")
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.Zip)
                    .AppendLine("---")
                    .AppendFormat("Подарок - {0}", shippingDetails.GifWrap == true ? "Да" : "Нет");

                MailMessage mail = new MailMessage(emailSettings.MailFromAdress, emailSettings.MailToAddress, "Новый Заказ", body.ToString());

                smtpClient.Send(mail);
            }

            //Теперь нужно связать реализацию и интерфейс в Ninject контейнере
        }
    }
}