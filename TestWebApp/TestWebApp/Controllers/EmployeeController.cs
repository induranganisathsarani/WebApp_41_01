using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TestWebApp.Models;
using TestWebApp.Util;

namespace TestWebApp.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly DBConnection db = new DBConnection();

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var list = new List<Employee>();
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Employees";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Employee
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    MobileNo = reader["MobileNo"].ToString(),
                    Email = reader["Email"].ToString(),
                    Age = (int)reader["Age"],
                    DateOfBirth = (DateTime)reader["DateOfBirth"],
                    Role = reader["Role"].ToString()
                });
            }

         
            return Ok(list);
        }

        [HttpPost("add")]
        public IActionResult AddEmployee([FromBody] Employee emp)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "INSERT INTO Employees (Name, Address, MobileNo, Email, Age, DateOfBirth, Role) " +
                         "VALUES (@Name, @Address, @MobileNo, @Email, @Age, @DateOfBirth, @Role)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Address", emp.Address);
            cmd.Parameters.AddWithValue("@MobileNo", emp.MobileNo);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Age", emp.Age);
            cmd.Parameters.AddWithValue("@DateOfBirth", emp.DateOfBirth);
            cmd.Parameters.AddWithValue("@Role", emp.Role);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0 ? Ok("Employee added successfully") : StatusCode(500, "Error adding employee");
        }

        [HttpGet("get/{id}")]
        public IActionResult GetById(int id)
        {
            Employee emp = null;
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Employees WHERE Id = @id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                emp = new Employee
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    MobileNo = reader["MobileNo"].ToString(),
                    Email = reader["Email"].ToString(),
                    Age = (int)reader["Age"],
                    DateOfBirth = (DateTime)reader["DateOfBirth"],
                    Role = reader["Role"].ToString()
                };
            }

            db.ConClose();
            return emp == null ? NotFound("Employee not found") : Ok(emp);
        }

        [HttpPut("update")]
        public IActionResult UpdateEmployee([FromBody] Employee emp)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "UPDATE Employees SET Name=@Name, Address=@Address, MobileNo=@MobileNo, Email=@Email, " +
                         "Age=@Age, DateOfBirth=@DateOfBirth, Role=@Role WHERE Id=@Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", emp.Id);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Address", emp.Address);
            cmd.Parameters.AddWithValue("@MobileNo", emp.MobileNo);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Age", emp.Age);
            cmd.Parameters.AddWithValue("@DateOfBirth", emp.DateOfBirth);
            cmd.Parameters.AddWithValue("@Role", emp.Role);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0 ? Ok("Employee updated successfully") : NotFound("Employee not found");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "DELETE FROM Employees WHERE Id = @Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0 ? Ok("Employee deleted successfully") : NotFound("Employee not found");
        }

    }



}
