using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Net.Mail;
using System.Net;

namespace BoardGameQuizAPI.Services
{
    public class CertificateService : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CertificateService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public byte[] GenerateCertificate(string playerName, string achievement, string department ,string level)
        //, string date, string signature)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Certificate";

            // Add a new page
            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            page.Orientation = PdfSharp.PageOrientation.Landscape;

            // Create XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Load background certificate template image
            string bgImagePath = string.Empty;
            if(level == "Gold")
                bgImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "certificate_template.png");
            else if(level == "Silver")
                bgImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "Silver_Certificate.png");
            else if(level == "Bronze")
                bgImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "Bronze_Certificate.png");

            XImage bgImage = XImage.FromFile(bgImagePath);
            gfx.DrawImage(bgImage, 0, 0, page.Width, page.Height);



            XFont magnoliaFont = new XFont("Magnolia Script", 25, XFontStyleEx.Regular);


            //XFont bodyFont = new XFont("Arial", 18, XFontStyleEx.Regular);
            XFont bodyFont = new XFont("Poppins", 13, XFontStyleEx.Regular);

            gfx.DrawString(playerName, magnoliaFont, XBrushes.Black,
                new XRect(0, 208, page.Width.Point, 50), XStringFormats.TopCenter);


            string finalstr = "(" + achievement +" - " + department + ")";

            gfx.DrawString(finalstr, bodyFont, XBrushes.Black,
                new XRect(0, 260, page.Width.Point, 50), XStringFormats.TopCenter);



            // Date and signature
            gfx.DrawString($"{DateTime.Now:dd/MM/yyyy}", bodyFont, XBrushes.Black,
                new XRect(665, page.Height.Point - 166, 200, 30), XStringFormats.TopLeft);



            // Save the document to memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                byte[] pdfData = stream.ToArray();

                // Define the path for saving the file in the temp folder
                string tempFilePath = Path.Combine(Path.GetTempPath(), $"{level}_Certificate.pdf");

                // Save the PDF file to the temp directory
                System.IO.File.WriteAllBytes(tempFilePath, pdfData);

                // Send the email with the generated PDF attachment
                SendEmailWithGeneratedCertificate(tempFilePath);

                // Return the file as a download response
                return pdfData;
            }
        }

        static void SendEmailWithGeneratedCertificate(string pdfFilePath)
        {
            // Configure the email client
            string fromAddress = "arunbalaji.sf3996@gmail.com";
            string toAddress = "arunjegan21@gmail.com";
            string subject = "Subject: Certificate of Winning";
            string body = "Hello, please find the attached Certification of winning.";

            // Create the email message
            MailMessage mail = new MailMessage(fromAddress, toAddress, subject, body);

            // Attach the PDF file
            mail.Attachments.Add(new Attachment(pdfFilePath));

            // Set up SMTP client
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("arunbalaji.sf3996@gmail.com", "Arunbalaji@21");
            SmtpServer.EnableSsl = true;

            // Send the email
            try
            {
                SmtpServer.Send(mail);
                Console.WriteLine("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                //Console.WriteLine("Failed recipient: " + ex.FailedRecipient);
                Console.WriteLine("Status code: " + ex.StatusCode);
            }
        }
    }
}
