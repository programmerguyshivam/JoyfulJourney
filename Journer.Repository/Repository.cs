using Journey.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Journer.Repository
{
    public class Repository : IRepository
    {
        private readonly IConfiguration _configuration;
        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetSqlConnection()
        {
            return _configuration.GetConnectionString("MyConnectionCS");
        }

        // ADD CUSTOMER PAGE FOR REGISTER//
        public void AddCustomer(RegisterDTO registerDTO)
        {
            using (SqlConnection con = new SqlConnection(this.GetSqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CREATEDATACUSTOMER", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FirstName", registerDTO.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", registerDTO.LastName);
                    cmd.Parameters.AddWithValue("@Email", registerDTO.Email);
                    cmd.Parameters.AddWithValue("@Password", registerDTO.Password);
                    cmd.Parameters.AddWithValue("@PhoneNumber", registerDTO.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", registerDTO.Address);

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }

        public (bool IsValid, bool IsAdmin) LoginValidate(string UserName, string password)
        {
            bool isValid = false;
            bool isAdmin = false;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("MyConnectionCS")))
            {
                connection.Open();

                try
                {
                    // First, check in the Admin table
                    using (SqlCommand cmd = new SqlCommand("AdminValidateSP", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Password", password ?? (object)DBNull.Value);

                        SqlParameter outputParam = new SqlParameter
                        {
                            ParameterName = "@IsAdmin",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();

                        isAdmin = (bool)outputParam.Value;
                        if (isAdmin)
                        {
                            // If admin credentials are valid, return immediately
                            return (true, true);
                        }
                    }

                    // If not an admin, check in the User table
                    using (SqlCommand cmd = new SqlCommand("LoginValidate", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", UserName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Password", password ?? (object)DBNull.Value);

                        SqlParameter outputParam = new SqlParameter
                        {
                            ParameterName = "@IsValid",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();

                        isValid = (bool)outputParam.Value;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception (e.g., log it)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // Optionally, you can rethrow or handle the exception as needed
                }
            }

            return (isValid, isAdmin);
        }


        // CRUD FOR ADMIN PROJECTS ///////////////////////////////////////////////////
        // FOR ADD DESTINATION PAGE FOR ADMIN////////////////////////////////////////////////////
        public void AddDest(AddAdminDestinationDTO destinationDTO)
        {

            using (SqlConnection conn = new SqlConnection(this.GetSqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("CREATEDATADESTINATION", conn))

                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", destinationDTO.Name);
                    cmd.Parameters.AddWithValue("@Description", destinationDTO.Description);
                    cmd.Parameters.AddWithValue("@Country", destinationDTO.Country);
                    cmd.Parameters.AddWithValue("@ImageURL", destinationDTO.ImageURl);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                }

            }

        }
                 // GET DATA FOR ADMIN DESTINATION////////////////////////
        public List<GetAdminDestinationDTO> getAdminDestination()
        {
            using (SqlConnection conn = new SqlConnection(this.GetSqlConnection()))

            {
                using (SqlCommand cmd = new SqlCommand("GETDATADESTINATION", conn))
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);
                    List<GetAdminDestinationDTO> destinations = new List<GetAdminDestinationDTO>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        destinations.Add(new GetAdminDestinationDTO()
                        {

                            DestinationId = Convert.ToInt32(dr["DestinationId"]),

                            Name = Convert.ToString(dr["Name"]),

                            Description = Convert.ToString((string)dr["Description"]),

                            Country = Convert.ToString((string)dr["Country"]),

                            ImageURL = Convert.ToString(dr["ImageURL"])

                        });
                    }
                    conn.Close();
                    return destinations;

                }

            }
        }

         // GET DATA FOR ADMIN DESTINATION////////////////////////



        // UPDATE DESTINATION FOR ADMIN TABLE/////////////////////////////////////////////////////////
        public void UpdateDest(UpdateAdminDestinationDTO updateAdminDestinationDTO)
        {
            using (SqlConnection conn = new SqlConnection(this.GetSqlConnection()))

            {
                using (SqlCommand cmd = new SqlCommand("UPDATEDATADESTINATION", conn))

                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DestinationId", updateAdminDestinationDTO.DestinationId);
                    cmd.Parameters.AddWithValue("@Name", updateAdminDestinationDTO.Name);
                    cmd.Parameters.AddWithValue("@Description", updateAdminDestinationDTO.Description);
                    cmd.Parameters.AddWithValue("@Country", updateAdminDestinationDTO.Country);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();



                }


            }

        }
        // UPDATE DESTINATION FOR ADMIN TABLE/////////////////////////////////////////////////////////


        // DELETE DESTINATION FOR ADMIN ////////////////////////////////////////////////////////
        public void DeleteDest(int id )
        {
            using (SqlConnection conn = new SqlConnection(this.GetSqlConnection()))

            {
                using (SqlCommand cmd = new SqlCommand("DELETEDATADESTINATION", conn))

                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DestinationId", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        // DELETE DESTINATION FOR ADMIN ////////////////////////////////////////////////////////


        ///////////////////////////////////////////////  FOR BOOKING CUSTOMER///////////////////////////////

        public void AddBook(AddBookUserDTO addBookUserDTO)
        {

            using(SqlConnection conn = new SqlConnection(this.GetSqlConnection())) 
            
            {
                using (SqlCommand cmd = new SqlCommand("CREATEDATABOOKNOW", conn)) 
                
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", addBookUserDTO.Name);
                    cmd.Parameters.AddWithValue("@Email", addBookUserDTO.Email);
                    cmd.Parameters.AddWithValue("@DateAndTime", addBookUserDTO.DateAndTime);
                    cmd.Parameters.AddWithValue("@Destination", addBookUserDTO.Destinations);
                   
                    cmd.Parameters.AddWithValue("@SpecialRequest", addBookUserDTO.SpecialRequest);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                }


                
            }
        }

        public void AddPackageAdmin(AddPackageAdmin addPackageAdmin)
        {

            using (SqlConnection conn = new SqlConnection(this.GetSqlConnection()))
            
            {
                using (SqlCommand cmd = new SqlCommand("CREATEDATAPACKAGES", conn)) 
                
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Title", addPackageAdmin.Title);
                    cmd.Parameters.AddWithValue("@Description", addPackageAdmin.Description);
                    cmd.Parameters.AddWithValue("@Price", addPackageAdmin.Price);
                    cmd.Parameters.AddWithValue("@Duration", addPackageAdmin.Duration);
                    cmd.Parameters.AddWithValue("@Destination_id",addPackageAdmin.Destination_id);
                    cmd.Parameters.AddWithValue("@ImageURL", addPackageAdmin.ImageURL);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                }

            }

        }

        public List<GetAdminPackage> getAdminPackages()
        {
            using (SqlConnection conn = new SqlConnection(this.GetSqlConnection())) 
            
            { 
            
                using ( SqlCommand cmd = new SqlCommand ("GETDATAPACKAGES", conn)) 
                
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    List<GetAdminPackage> getAdminPackages = new List<GetAdminPackage>();
                    foreach (DataRow row in dt.Rows)
                    {
                        getAdminPackages.Add(new GetAdminPackage()
                        {
                            PackageId = Convert.ToInt32(row["PackageId"]),
                            Title = Convert.ToString(row["Title"]),
                            Description = Convert.ToString(row["Description"]),
                            Price = Convert.ToDecimal(row["Price"]),
                            Duration = Convert.ToString(row["Duration"]),
                            Destination_id = Convert.ToInt32(row["Destination_id"]),
                            ImageURL = Convert.ToString(row["ImageURL"])
                        });


                    }
                }
            
            }
            return getAdminPackages();

        }

        #region -- Email Sending -----
        public void SendEMAIL(string address, string subject, string body)
        {
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("corporatehuntofficial@gmail.com");
                mm.To.Add(address);
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;

                    NetworkCredential nc = new NetworkCredential("corporatehuntofficial@gmail.com", "tsjs nnlw kpim noqo");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = nc;
                    smtp.Port = 587;

                    smtp.Send(mm);
                }

            }

        }
        #endregion
    }

}


