using Microsoft.EntityFrameworkCore;

namespace DotNetAngularApplication.Models
{
    public class FormContext : DbContext
    {
        public FormContext(DbContextOptions<FormContext> options) : base(options) { }
        
        public DbSet<newForm> newForms { get; set; }
        public DbSet<User> user { get; set; }
    }
}
