// -----------------------------------------------------------------------
// <copyright file="EmailOrderProcessor.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DrinkBuyer.Domain.Concrete
{
    #region

    using System.Net;
    using System.Net.Mail;
    using System.Text;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.Domain.Entities;

    #endregion

    public class EmailSettings
    {
        #region Fields

        public string FileLocation = @"l:\drink_buyer_emails";

        public string MailFromAddress = "ccdiocese@gmail.com";

        public string MailToAddress = "ecnalyr@gmail.com";

        public string Password = "pas5word";

        public string ServerName = "smtp.gmail.com";

        public int ServerPort = 587;

        public bool UseSsl = true;

        public string Username = "ccdiocese@gmail.com";

        public bool WriteAsFile;

        #endregion
    }

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class EmailOrderProcessor : IOrderProcessor
    {
        #region Fields

        private readonly EmailSettings emailSettings;

        #endregion

        #region Constructors and Destructors

        public EmailOrderProcessor(EmailSettings settings)
        {
            this.emailSettings = settings;
        }

        #endregion

        #region Public Methods and Operators

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = this.emailSettings.UseSsl;
                smtpClient.Host = this.emailSettings.ServerName;
                smtpClient.Port = this.emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(this.emailSettings.Username, this.emailSettings.Password);
                if (this.emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = this.emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body =
                    new StringBuilder().AppendLine("A new order has been submitted").AppendLine("---").AppendLine(
                        "Items:");
                foreach (CartLine line in cart.Lines)
                {
                    decimal subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);
                }

                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue()).AppendLine("---").AppendLine(
                    "Ship to:").AppendLine(shippingInfo.Name).AppendLine(shippingInfo.Line1).AppendLine(
                        shippingInfo.Line2 ?? string.Empty).AppendLine(shippingInfo.Line3 ?? string.Empty).AppendLine(
                            shippingInfo.City).AppendLine(shippingInfo.State ?? string.Empty).AppendLine(
                                shippingInfo.Country).AppendLine(shippingInfo.Zip).AppendLine("---").AppendFormat(
                                    "Gift wrap: {0}", shippingInfo.GiftWrap ? "Yes" : "No");
                var mailMessage = new MailMessage(
                    this.emailSettings.MailFromAddress, 
                    // From
                    this.emailSettings.MailToAddress, 
                    // To
                    "New order submitted!", 
                    // Subject
                    body.ToString()); // Body
                if (this.emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }
        }

        #endregion
    }
}