using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DotNetMastery.Models
{
    public class Company
    {
        [Key]
        public int companyId { get; set; }
        [Required]
        public string Name { get; set; }

        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public int? PhoneNumber { get; set; }

    }
}
