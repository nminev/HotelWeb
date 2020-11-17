using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ReviewModels
{
    public class CreateReviewViewModel
    {
        public string OfferName { get; set; }

        [Required]
        public int OfferId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [Range(1,5)]
        public int Score { get; set; }

        public List<ReviewViewModel> Reviews { get; set; }
    }
}
