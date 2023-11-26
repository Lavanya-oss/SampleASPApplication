using System.ComponentModel.DataAnnotations;

namespace StudentsProfileApplication.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        // Other address properties
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public int StudentId { get; set; }
        public virtual Student Students { get; set; }
    }
}
