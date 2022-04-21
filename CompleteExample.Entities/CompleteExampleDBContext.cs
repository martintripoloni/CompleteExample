using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CompleteExample.Entities
{
    public class CompleteExampleDBContext : DbContext, ICompleteExampleDBContext
    {

        public CompleteExampleDBContext(DbContextOptions<CompleteExampleDBContext> options) : base(options) { }

        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>()
                .ToTable("Students", schema: "dbo");

            modelBuilder.Entity<Course>()
                .ToTable("Courses", schema: "dbo");

            modelBuilder.Entity<Enrollment>()
                .ToTable("Enrollment", schema: "dbo");

            modelBuilder.Entity<Instructor>()
                .ToTable("Instructors", schema: "dbo");
            
            modelBuilder.Entity<EnrollmentHistory>()
                .ToTable("EnrollmentHistory", schema: "dbo");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<EnrollmentHistory> EnrollmentHistories { get; set; }

    }
}
