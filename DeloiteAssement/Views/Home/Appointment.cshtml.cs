using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;



namespace DeloiteAssement.Views.Home
{
    public class Appointments
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                //Adding SQL connection string 
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=myAppointmentDb;Integrated Security=True";

                //Creating Sql connection
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.surname = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.email = reader.GetString(4);
                                clientInfo.DateTime = reader.GetString(5);
                                clientInfo.address = reader.GetString(6);
                                clientInfo.product = reader.GetString(7);
                                clientInfo.comments = reader.GetString(8);
                                clientInfo.created_at = reader.GetString(9);

                                listClients.Add(clientInfo);
                                
                            }
                        }   
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

        }
    }

    public class ClientInfo
    {
        public String id;
        public String name;
        public String surname;
        public String phone;
        public String email;
        public String DateTime;
        public String address;
        public String product;
        public String comments;
        public String created_at;




    }
}
