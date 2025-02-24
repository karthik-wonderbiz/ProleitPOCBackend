using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProleitPocBackend.Model
{
    [Table("Devices")]
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Machine { get; set; } = string.Empty;
        [Required]
        public string Property { get; set; } = string.Empty;
        [Required]
        public decimal Value { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }

    }
}