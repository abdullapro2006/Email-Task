using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Pustok.Database.DomainModels;
using System.Linq;


namespace Pustok.Controllers.Admin;

[Route("admin/emails")]
public class MailController : Controller
{


    [HttpGet]
    public IActionResult Index()
    {
        return View("Views/Mail/Index.cshtml");
    }


    [HttpGet("SendEmail", Name = "send-email")]
    public IActionResult SendEmail()
    {
        return View("Views/Mail/SendEmail.cshtml");
    }

    [HttpPost("SendEmail",Name ="send-email")]
    public IActionResult SendEmail(MailRequest mailRequest)
    {
        MimeMessage mimeMessage = new MimeMessage();

        MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin","abdullahmanafli38@gmail.com");

        mimeMessage.From.Add(mailboxAddressFrom);


        MailboxAddress mailboxAddressTo = new MailboxAddress("Admin", mailRequest.ReceiverMail);

        mimeMessage.To.Add(mailboxAddressTo);

        var bodybuilder = new BodyBuilder();
        bodybuilder.TextBody = mailRequest.Body;

        mimeMessage.Body = bodybuilder.ToMessageBody();

        mimeMessage.Subject = mailRequest.Subject;



        SmtpClient client = new SmtpClient();

        client.Connect("smtp.gmail.com", 587,false);

        client.Authenticate("abdullahmanafli38@gmail.com", "utczysoecaklhlff");

        client.Send(mimeMessage);

        client.Disconnect(true);


        return View("Views/Mail/Index.cshtml");
    }
}
