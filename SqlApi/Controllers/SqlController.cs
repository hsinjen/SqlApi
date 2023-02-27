using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SqlApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SqlController : ControllerBase
    {
        [HttpPost]
        [ActionName("readData")]
        public string readData(string commands)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=demo;Trusted_Connection=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(commands, conn))
                    {
                        if (command.Connection.State == ConnectionState.Closed)
                        {
                            command.Connection.Open();
                        }

                        using (DataTable dt = new DataTable())
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(command))
                            {
                                da.Fill(dt);

                                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                                Dictionary<string, object> row;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    row = new Dictionary<string, object>();
                                    foreach (DataColumn col in dt.Columns)
                                    {
                                        row.Add(col.ColumnName, dr[col]);
                                    }
                                    rows.Add(row);
                                }

                                JsonSerializerOptions options = new JsonSerializerOptions
                                {
                                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                                    WriteIndented = true
                                };
                                return JsonSerializer.Serialize(rows, options);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        [ActionName("writeDate")]
        public string writeDate(string commands)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=demo;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.Transaction = transaction;

                        try
                        {
                            command.CommandText = commands;
                            int rowsAffected = command.ExecuteNonQuery();//影響筆數
                            transaction.Commit();
                            return rowsAffected.ToString();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return ex.Message;
                        }
                    }
                }
                
            }
            
        }
    }
}
