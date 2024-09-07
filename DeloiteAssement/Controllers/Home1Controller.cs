using Microsoft.AspNetCore.Mvc;
using DeloiteAssement.Models;
using System.Data.SqlClient;
using DeloiteAssement.Views.Home;

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

                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myAppointmentDb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //Checking if email already exists in the database. No duplicate appointments will be allowed
                    string emailCheckQuery = "SELECT COUNT(1) FROM clients WHERE email = @Email";

                    using(SqlCommand emailCheckCommand = new SqlCommand(emailCheckQuery, connection))
                    {
                        emailCheckCommand.Parameters.AddWithValue("@Email", model.Email);
                        int count = (int)emailCheckCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            ModelState.AddModelError("Email", "An appointment with this email already exists. You may opt to reschedule your appointment.");
                            return View("~/Views/Home/Appointment.cshtml", model);
                        }
                    }

                    string sql = "INSERT INTO clients (name, surname, phone, email, appointment_date, address, product, comments, created_at) " +
                                 "VALUES (@Name, @Surname, @MobileNumber, @Email, @DateTime, @Address, @Product, @Comments, @CreatedAt)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Surname", model.Surname);
                        command.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@DateTime", model.DateTime);
                        command.Parameters.AddWithValue("@Address", model.Address);
                        command.Parameters.AddWithValue("@Product", model.Product);
                        command.Parameters.AddWithValue("@Comments", model.Comments);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }

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
