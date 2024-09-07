using Microsoft.AspNetCore.Mvc;
using DeloiteAssement.Models;

namespace DeloiteAssement.Controllers
{
    public class Home1Controller : Controller
    {
        private readonly EmailSender _emailSender;

        public Home1Controller(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAppointment(AppointmentFormModel model)
        {
            if (ModelState.IsValid)
            {
                var receiver = model.Email;
                var subject = "Appointment Confirmation";
                var message = $"Dear {model.Name},\n\nYour appointment has been scheduled on {model.DateTime} at your registered address {model.Address}.\n Should you require any further assistance or changes, do not hesitate to call us on 99467855 or messaging us through live chat";

                await _emailSender.SendEmailAsync(receiver, subject, message);

                TempData["SuccessMessage"] = "Appointment scheduled successfully and email sent to " + receiver;

                return RedirectToAction("Appointment","Home");
            }

            
            return View("~/Views/Home/Appointment.cshtml", model);
           
        }

        public IActionResult Appointment()
        {
            
            return View("~/Views/Home/Appointment.cshtml");
        }
    }
}
