using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalcHistoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalcHistoryServiceController : ControllerBase
    {

        private IDbConnection CalcHistory = new MySqlConnection("Server=cache-db;Database=cache-database;Uid=div-cache;Pwd=C@ch3d1v;");

        public CalcHistoryServiceController()
        {
            CalcHistory.Open();
            var tables = CalcHistory.Query<string>("SHOW TABLES LIKE 'CalcHistory2'");
            if (!tables.Any())
            {
                CalcHistory.Execute("CREATE TABLE CalcHistory2 (numberA BIGINT NOT NULL, numberB BIGINT NOT NULL, Result BIGINT NOT NULL, operat varchar(10) NOT NULL)");
                Console.WriteLine("DEN FANDTES IKKE!");
            }
            else
            {
                Console.WriteLine("DEN FANDTES!");
            }
            CalcHistory.Close();
        }

        [HttpGet]
        public List<string> Get()
        {

            CalcHistory.Open();
            List<string> list = new List<string>();

            string query = "SELECT numberA, numberB, Result, operat FROM CalcHistory2";
            IDbCommand cmd = CalcHistory.CreateCommand();
            cmd.CommandText = query;

            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                string s =
                reader.GetInt64(0).ToString() + reader.GetString(3) +
                reader.GetInt64(1).ToString() + " = " +
                reader.GetInt64(2).ToString();
                list.Add(s);

            }

            reader.Close();
            CalcHistory.Close();
            return list;
        }

        [HttpPost]
        public long Post([FromQuery] long a, [FromQuery] long b, [FromQuery] long res, [FromQuery] long op)
        {
            string oper = "";
            CalcHistory.Open();
            if (op == 1)
            {
                oper = "-";
            }
            else if (op == 2)
            {
                oper = "+";
            }

            CalcHistory.Execute("INSERT INTO CalcHistory2 (numberA, numberB, Result, operat) VALUES (@numberA, @numberB, @Result, @operat)", new { numberA = a, numberB = b, Result = res, operat = oper });
            CalcHistory.Close();
            return res;

        }
    }
}