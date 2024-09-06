using Journey.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public (bool IsValid, bool IsAdmin) ValidateUser(string userName, string password)
        {
            // Define the return value
            bool isValid = false;
            bool isAdmin = false;

            using (SqlConnection connection = new SqlConnection(this.GetSqlConnection()))
            {
                connection.Open();

                // First, check in the Admin table
                using (SqlCommand cmd = new SqlCommand("AdminValidateSP", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", (object)userName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Password", (object)password ?? DBNull.Value);



                    SqlParameter outputParam = new SqlParameter
                    {
                        ParameterName = "@isValid",
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
            }
            // Establish a connection to the database
            using (SqlConnection con = new SqlConnection(this.GetSqlConnection()))
            {
                // Create a command to execute the stored procedure
                using (SqlCommand cmd = new SqlCommand("UserLoginValidate", con))
                {
                    // Specify that we are executing a stored procedure
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters with proper prefixes
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Password", password);

                    // Add output parameter
                    SqlParameter outputParam = new SqlParameter("@IsValid", System.Data.SqlDbType.Bit)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Open the connection
                    con.Open();

                    // Execute the command and get the result
                    cmd.ExecuteScalar();

                    // Retrieve the output parameter value
                    isValid = (bool)outputParam.Value;
                }
            }

            // Return the validation result
            return (isValid, false);
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

                        });
                    }
                    conn.Close();
                    return destinations;

                }

            }
        }
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
                    cmd.Parameters.AddWithValue("@Persons", addBookUserDTO.Persons);
                    cmd.Parameters.AddWithValue("@Category", addBookUserDTO.Categories);
                    cmd.Parameters.AddWithValue("@SpecialRequest", addBookUserDTO.SpecialRequest);

                    conn.Open();
                    cmd.ExecuteNonQuery();


                }


                
            }
        }

        /////////////////////////////////////////////// CRUD FOR BOOKING CUSTOMER///////////////////////////////



    }
}


        