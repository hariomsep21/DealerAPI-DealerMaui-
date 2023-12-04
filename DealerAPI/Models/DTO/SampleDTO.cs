using System.ComponentModel.DataAnnotations;

namespace DealerAPI.Models.DTO
{
    public class SampleDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
    }
}
