using DeloiteAssement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DeloiteAssement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmailSender _emailSender;

        // Constructor with dependency injection for both IEmailService and ILogger
        public HomeController(ILogger<HomeController> logger, EmailSender emailSender)
        {
           
            _logger = logger;
            this._emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ViewAppointment()
        {
            var model = new AppointmentViewModel
            {
                listClients = GetClients()
            };

            return View(model);
        }

        public IActionResult Appointment()
        {
            var model = new AppointmentFormModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAppointment(AppointmentFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Save appointment to the database
                    string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myAppointmentDb;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
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

                    var receiver = model.Email;  // Get the email from the submitted form
                    var subject = "Appointment Confirmation";
                    var message = $"Dear {model.Name},\n\nYour appointment has been scheduled on {model.DateTime}.";

                    await _emailSender.SendEmailAsync(receiver, subject, message);


                    // Store success message in TempData (optional)
                    TempData["SuccessMessage"] = "Appointment scheduled successfully and email sent to " + receiver;
                    return RedirectToAction("Appointment");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while submitting the appointment.");
                    TempData["ErrorMessage"] = "An error occurred while booking your appointment. Please try again later.";
                }
            }

            return View("Appointment", model); 
        }

        private List<ClientInfo> GetClients()
        {
            var clients = new List<ClientInfo>();
            string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myAppointmentDb;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM clients";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var client = new ClientInfo
                            {
                                id = reader.GetInt32(0),
                                name = reader.GetString(1),
                                surname = reader.GetString(2),
                                phone = reader.GetString(3),
                                email = reader.GetString(4),
                                appointment_date = reader.GetDateTime(5),
                                address = reader.GetString(6),
                                product = reader.GetString(7),
                                comments = reader.GetString(8),
                                created_at = reader.GetDateTime(9)
                            };

                            clients.Add(client);
                        }
                    }
                }
            }

            return clients;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
