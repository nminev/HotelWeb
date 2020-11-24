using System.ComponentModel.DataAnnotations;

namespace Models.OfferModels
{
    public class OffersViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        [Range(0,double.MaxValue)]
        public double Price { get; set; }

        public double Raiting { get; set; }

        public string ImageSrc { get; set; }
    }
}
