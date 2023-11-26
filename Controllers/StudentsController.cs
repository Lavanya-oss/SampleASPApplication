using Microsoft.AspNetCore.Mvc;
using StudentsProfileApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentsProfileApplication.Interface;
using StudentsProfileApplication.ViewModels;

namespace PractiseSamples.Controllers
{
    /// <summary>
    /// StudentsController
    /// </summary>
    public class StudentsController : Controller
    {
        private IStudentService _studentService;
        private readonly ProfileAuditDbContext _auditDbContext;
        private readonly StudentsInfoDBContext _context;

        /// <summary>
        /// StudentsController constructor
        /// </summary>
        /// <param name="profileAuditDb"></param>
        /// <param name="studentsInfoDB"></param>
        /// <param name="studentService"></param>
        public StudentsController(ProfileAuditDbContext profileAuditDb, StudentsInfoDBContext studentsInfoDB, IStudentService studentService)
        {
            _auditDbContext = profileAuditDb;
            _context = studentsInfoDB;
            _studentService = studentService;
        }

        /// <summary>
        /// Get Students Data
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {

            var stduents = await _studentService.IndexData();

            return View(stduents);
        }

        /// <summary>
        /// Add New Student
        /// </summary>
        /// <returns></returns>
        public IActionResult AddStudent()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments.ToList(), "DepartmentID", "Name");
            return View();
            
        }

        /// <summary>
        /// View Student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> View(int id)
        {
            StudentViewModel studentViewModel = new StudentViewModel();
            try
            {
                studentViewModel = await _studentService.ViewStudentData(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(studentViewModel);
        }

        /// <summary>
        /// Edit students
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            StudentViewModel studentViewModel = new StudentViewModel();
            try
            {

                studentViewModel = await _studentService.EditStudentRecord(id, studentViewModel);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", studentViewModel.DepartmentID);
            return View(studentViewModel);
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {

            await _studentService.DeleteRecord(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Save Changes
        /// </summary>
        /// <param name="studentViewModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveChanges(StudentViewModel studentViewModel)
        {
            await _studentService.SaveChanges(studentViewModel);
            return RedirectToAction("Index");
        }
    }
}

