using System;
using System.Collections.Generic;

namespace DeloiteAssement.Models
{
    public class AppointmentViewModel
    {
        public List<ClientInfo> listClients { get; set; }
    }

    public class ClientInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime appointment_date { get; set; }
        public string address { get; set; }
        public string product { get; set; }
        public string comments { get; set; }
        public DateTime created_at { get; set; }
    }
}
