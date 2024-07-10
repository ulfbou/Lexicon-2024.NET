using LMS.api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.api.Data
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {

            builder.HasKey(c => c.Id);


            builder.HasMany(c => c.Students)
                .WithOne(c => c.Course)
                .HasForeignKey(c => c.CourseID)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasMany(c => c.Modules)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseID);
        
        }
    }
}
