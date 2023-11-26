using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentsProfileApplication.Models;
using StudentsProfileApplication.Interface;
using StudentsProfileApplication.ViewModels;

namespace StudentsProfileApplication.Services
{
    /// <summary>
    /// The StudentService
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly StudentsInfoDBContext _context;
        private readonly ProfileAuditDbContext _profileAuditDbContext;
        private readonly IConfiguration _configuration;
        public StudentService(ProfileAuditDbContext profileDataDbContext, StudentsInfoDBContext studentDBContext, IConfiguration configuration)
        {
            _context = studentDBContext;
            _profileAuditDbContext = profileDataDbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// The IndexData
        /// </summary>
        /// <returns></returns>
        public async Task<List<Student>> IndexData()
        {
            List<Student> students = new List<Student>();
            try
            {
                students = await _context.Students.ToListAsync();
                foreach (var student in students)
                {
                    Console.WriteLine($"StudentsCount{student.FirstName},CoursesCount{student.Courses.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return students;
        }

        /// <summary>
        /// The ViewStudentData
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentViewModel"></param>
        /// <returns></returns>
        public async Task<StudentViewModel> ViewStudentData(int id)
        {
            StudentViewModel ViewModel = new StudentViewModel();
            try
            {
                //Calling StoredProc having sql function
                //var student = _context.Students.FromSqlRaw<Student>("Student_Get {0}", id).ToList().FirstOrDefault();

                ViewModel = await StudentData(id, ViewModel);
                ConceptOfPOC(ViewModel.FirstName?.Replace(" ", ""));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ViewModel;
        }

        /// <summary>
        /// The SaveChanges
        /// </summary>
        /// <param name="studentData"></param>
        /// <returns></returns>
        public async Task SaveChanges(StudentViewModel studentViewModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Student student = await _context.Students.Include(z=>z.Address).FirstOrDefaultAsync(s=>s.StudentId ==studentViewModel.StudentId);
                    if (student == null)
                    {
                        student = new Student();
                    }
                    student.FirstName = studentViewModel.FirstName;
                    student.LastName = studentViewModel.LastName;
                    student.DateOfBirth = studentViewModel.DateOfBirth;
                    student.EnrollmentDate = studentViewModel.EnrollmentDate;
                    student.Gpa = studentViewModel.Gpa;
                    student.HasScholarShip = studentViewModel.HasScholarShip;
                    student.DepartmentID = studentViewModel.DepartmentID;
                    student.DepartmentID = studentViewModel.DepartmentID; 
                    student.DepartmentID = studentViewModel.DepartmentID; 
                    student.DepartmentID = studentViewModel.DepartmentID;
                    if (student.StudentId == 0)
                    {
                        _context.Add(student);
                    }
                    _context.SaveChanges();

                    Address address = student.Address?.FirstOrDefault(a=>a.StudentId == student.StudentId);
                    if(address == null)
                    {
                        address = new Address();
                    }
                    address.Street = studentViewModel.Street;
                    address.City = studentViewModel.City;
                    address.PostalCode = studentViewModel.PostalCode;
                    address.State = studentViewModel.State;
                    address.StudentId = student.StudentId;
                    if (address.AddressId == 0) { _context.Add(address); }
                    _context.SaveChanges();

                    transaction.CommitAsync();


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// The EditStudentRecord
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentViewModel"></param>
        /// <returns></returns>
        public async Task<StudentViewModel> EditStudentRecord(int id, StudentViewModel studentViewModel)
        {
            try
            {
                studentViewModel = await _context.Students.Include(x=>x.Address)
                    .Include(x => x.Departments)
                    .Where(x => x.StudentId == id)

                    .Select(s => new StudentViewModel
                    {
                        StudentId = s.StudentId,
                        FirstName = s.FirstName.ToString(),
                        LastName = s.LastName.ToString(),
                        DateOfBirth = s.DateOfBirth,
                        EnrollmentDate = s.EnrollmentDate,
                        Gpa = s.Gpa,
                        Age = s.Age,
                        HasScholarShip = s.HasScholarShip,
                        DepartmentID = s.DepartmentID,
                        Street = s.Address.Select(x => x.Street).FirstOrDefault(),
                        AddressId = s.Address.Select(x => x.AddressId).FirstOrDefault(),
                        City = s.Address.Select(x => x.City).FirstOrDefault(),
                        State = s.Address.Select(x => x.State).FirstOrDefault(),
                        PostalCode = s.Address.Select(x => x.PostalCode).FirstOrDefault(),
                        SelectedDepartment = s.Departments,
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return studentViewModel;
        }

        /// <summary>
        /// The DeleteRecord
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteRecord(int? id)
        {
            Student student;

            try
            {
                if (id == null)
                {
                    return;
                }
                student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return;
                }
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StudentViewModel> StudentData(int id, StudentViewModel studentViewModel)
        {
            try
            {
                var student = await _context.Students.Include(x => x.Departments).Include(x => x.Address).Where(s => s.StudentId == id).FirstOrDefaultAsync();
                if (student != null)
                {
                    studentViewModel.StudentId = student.StudentId;
                    studentViewModel.FirstName = student.FirstName;
                    studentViewModel.LastName = student.LastName;
                    studentViewModel.DateOfBirth = student.DateOfBirth;
                    studentViewModel.EnrollmentDate = student.EnrollmentDate;
                    studentViewModel.Gpa = student.Gpa;
                    studentViewModel.Age = student.Age;
                    studentViewModel.HasScholarShip = student.HasScholarShip;
                    studentViewModel.DepartmentID = student.DepartmentID;
                    studentViewModel.SelectedDepartment = student.Departments;
                    studentViewModel.State = student.Address.Select(x => x.State).FirstOrDefault();
                    studentViewModel.City = student.Address.Select(x => x.City).FirstOrDefault();
                    studentViewModel.PostalCode = student.Address.Select(x => x.PostalCode).FirstOrDefault();
                    studentViewModel.Street = student.Address.Select(x => x.Street).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return studentViewModel;
        }








        /// <summary>
        /// The ConceptOfPOC
        /// </summary>
        /// <param name="tableName"></param>
        public void ConceptOfPOC(string tableName)
        {
            bool CheckTableExists = DbTableExists(tableName);
            if (CheckTableExists)
            {
                List<string> dependentTables = GetDependentTables(tableName);
                List<string> DependentTables = dependentTables.ToList();
                string filePath = "D:\\File.sql";

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    List<Dictionary<string, string>> StudentsData = GetAllStudents(tableName);

                    foreach (var studendata in StudentsData)
                    {
                        // Generate and write insert queries for each dependent table
                        string insertQuery = GenerateInsertQuery(tableName, studendata, "FirstName");
                        writer.WriteLine(insertQuery);
                    }
                }
            }
        }

        /// <summary>
        /// DbTableExists
        /// </summary>
        /// <param name="strTableNameAndSchema"></param>
        /// <returns></returns>
        public bool DbTableExists(string strTableNameAndSchema)
        {
            // Retrieve the connection string from appsettings.json
            string connectionString = _configuration.GetConnectionString("constring");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string strCheckTable =
                   String.Format(
                      "IF OBJECT_ID('{0}', 'U') IS NOT NULL SELECT 'true' ELSE SELECT 'false'",
                      strTableNameAndSchema);

                SqlCommand command = new SqlCommand(strCheckTable, connection);
                command.CommandType = CommandType.Text;
                connection.Open();

                return Convert.ToBoolean(command.ExecuteScalar());
            }
        }

        public List<string> GetDependentTables(string tableName)
        {
            // Retrieve the connection string from appsettings.json
            string connectionString = _configuration.GetConnectionString("constring");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string strDependentTablesQuery = $@"
            SELECT DISTINCT
                FK.TABLE_NAME AS DependentTable
            FROM
                INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC
                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS PK ON RC.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS FK ON RC.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS CU ON CU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
            WHERE

                PK.TABLE_NAME = '{tableName}'";

                SqlCommand command = new SqlCommand(strDependentTablesQuery, connection);
                command.CommandType = CommandType.Text;
                connection.Open();

                var result = new List<string>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader["DependentTable"].ToString());
                    }

                }
                return result;
            }
        }

        /// <summary>
        /// The GenerateInsertQuery
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="keyValuePairs"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public string GenerateInsertQuery(string tableName, Dictionary<string, string> keyValuePairs, string ColumnName)
        {
            var value1 = "";
            if (keyValuePairs.ContainsKey(ColumnName))
            {
                string columnValue = keyValuePairs[ColumnName];
                value1 = $@"
                IF NOT EXISTS 
                (SELECT * FROM {tableName} WHERE {ColumnName} = '{columnValue}')
                BEGIN
                    INSERT INTO {tableName} ({string.Join(", ", keyValuePairs.Keys)})
                    VALUES ({string.Join(", ", keyValuePairs.Values.Select(value => $"'{value}'"))})
                END";
            }
            return value1;
        }

        /// <summary>
        /// The GetAllStudents
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetAllStudents(string tableName)
        {
            List<Dictionary<string, string>> tableData = new List<Dictionary<string, string>>();
            string sqlQuery = $"SELECT * FROM {tableName}";

            // Execute the raw SQL query using ExecuteSqlRaw
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlQuery;
                command.CommandType = CommandType.Text;
                _context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var row = new Dictionary<string, string>();

                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            var fieldName = result.GetName(i);
                            var fieldValue = result.GetValue(i);
                            row[fieldName] = fieldValue.ToString();
                        }
                        tableData.Add(row);
                    }
                }
            }
            return tableData;
        }

    }
}
