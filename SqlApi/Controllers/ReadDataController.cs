using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SqlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadDataController : ControllerBase
    {
        // GET: api/<ReadDataController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ReadDataController>/5
        [HttpGet("{strcommand}")]
        public string Get(string strcommand)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=demo;Trusted_Connection=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(strcommand, conn))
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

        // POST api/<ReadDataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReadDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReadDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
