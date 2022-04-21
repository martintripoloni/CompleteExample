using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CompleteExample.Entities
{
    public interface ICompleteExampleDBContext
    {
        DbSet<Course> Courses { get; set; }
        DbSet<Enrollment> Enrollment { get; set; }
        DbSet<Instructor> Instructors { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<EnrollmentHistory> EnrollmentHistories { get; set; }
        Task<int> SaveChanges();
    }
}