using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using webApp00.BL.Helper;

namespace webApp00.Controllers
{
    public class MailController : Controller
    {
        public IActionResult SendMail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendMail(string Title, string Msg)
        {
            //try
            //{
            //    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            //    {


            //        //smtp.Host = "smtp.gmail.com";
            //        //smtp.Port = "587";
            //        smtp.EnableSsl = true;
            //        smtp.Credentials = new NetworkCredential("as8338873@gmail.com", "A@123321A@");
            //        smtp.Send("as8338873@gmail.com", "abdeltwab7sin@gmail.com", Title, Msg);
            //    }

            //    TempData["Message"] = "Mail Sent";
            //    return View();
            //}
            //catch (Exception ex)
            //{
            //    TempData["Message"] = "Mail Faild";
            //    return View();

            //}

            TempData["Message"] = MailSender.Mail(Title, Msg);

            return View();
        }
    }
}
