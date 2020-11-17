using System.ComponentModel.DataAnnotations;

namespace Models.OfferModels
{
    public class EditOfferViewModel
    {
        [Required]
        public int ID { get; set; }

        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public string ImageName { get; set; }

    }
}
