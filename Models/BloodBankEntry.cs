using System.ComponentModel.DataAnnotations;

namespace PrBloodBankAPI.Models
{
    public class BloodBankEntry
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DonorName { get; set; }

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        [StringLength(3)]
        public string BloodType { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactInfo { get; set; }

        [Required]
        [Range(200, 500)]
        public int Quantity { get; set; }  // in ml

        [Required]
        public DateTime CollectionDate { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }  // Available, Requested, Expired
    }
}
