using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Pustok.Database.DomainModels;
using System.Linq;


namespace Pustok.Controllers.Admin;

public class MailController : Controller
{
    [Route("admin/emails")]


    [HttpGet]
    public IActionResult Index()
    {
        return View("Views/Admin/Mail/SendEmail.cshtml");
    }

    [HttpPost]
    public IActionResult Index(MailRequest mailRequest)
    {
        MimeMessage mimeMessage = new MimeMessage();

        MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin","abdullahmanafli38@gmail.com");

        mimeMessage.From.Add(mailboxAddressFrom);


        MailboxAddress mailboxAddressTo = new MailboxAddress("Admin", mailRequest.ReceiverMail);

        mimeMessage.To.Add(mailboxAddressTo);

        mimeMessage.Subject = mailRequest.Subject;



        SmtpClient client = new SmtpClient();

        client.Connect("smtp.gmail.com", 587,false);

        client.Authenticate("abdullahmanafli38@gmail.com","3859589am");

        client.Send(mimeMessage);

        client.Disconnect(true);


        return View();
    }
}
