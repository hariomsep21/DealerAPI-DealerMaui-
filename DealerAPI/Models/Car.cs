using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DealerAPI.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Do not auto-generate
        public int CarId { get; set; }

        public string CarName { get; set; }
        public string Variant { get; set; }
        public string? Image { get; set; }

        [ForeignKey("UserInfo")]
        public int UserInfoId { get; set; }
        public virtual UserInfo UserInfos { get; set; }

        // Navigation property for Payments related to this car
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Vehiclerecord> vehiclerecords { get; set; }
    }
}
