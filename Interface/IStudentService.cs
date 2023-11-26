using Microsoft.AspNetCore.Mvc;
using StudentsProfileApplication.Models;
using StudentsProfileApplication.ViewModels;

namespace StudentsProfileApplication.Interface
{
    /// <summary>
    /// IStudentService
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// The SaveChanges
        /// </summary>
        /// <param name="studentData"></param>
        /// <returns></returns>
        Task SaveChanges(StudentViewModel studentViewModel);

        /// <summary>
        /// The IndexData
        /// </summary>
        /// <returns></returns>
        Task<List<Student>> IndexData();

        /// <summary>
        /// The ViewStudentData
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentViewModel"></param>
        /// <returns></returns>
        Task<StudentViewModel> ViewStudentData(int id);

        /// <summary>
        /// The DeleteRecord
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteRecord(int? id);

        /// <summary>
        /// The EditStudentRecord
        /// </summary>
        /// <param name="id"></param>
        /// <param name="studentViewModel"></param>
        /// <returns></returns>
        Task<StudentViewModel> EditStudentRecord(int id,StudentViewModel studentViewModel);
    }
}