using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetAngularApplication.Models
{
    public class newForm
    {
        [Key]
        public int Id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? mobileNumber { get; set; }   
        public DateTime selectDate { get; set; }
        public bool status { get; set; }
        

    }
}
