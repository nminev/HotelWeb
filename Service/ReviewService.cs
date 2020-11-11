using Database.Data;
using Microsoft.AspNetCore.Identity;
using Models.ReviewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Service
{
    public class ReviewService : IReviewService
    {
        private ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _manager;
        public ReviewService(ApplicationDbContext context, UserManager<IdentityUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        public async Task AddReview(CreateReviewViewModel review, ClaimsPrincipal _user)
        {
            var reviewOffer = _context.Offers.Single(x => x.ID == review.OfferId);
            var user = await _manager.GetUserAsync(_user).ConfigureAwait(false);
            var OfferReviews = _context.Reviews.Where(x => x.Offer.ID == review.OfferId).ToList();

            if (OfferReviews.Any())
            {
                var sum = OfferReviews.Sum(x => (double)x.Score) + review.Score;
                var count = OfferReviews.Count + 1;
                reviewOffer.Rating = sum / count;
            }
            else
            {
                reviewOffer.Rating = review.Score;
            }

            _context.Reviews.Add(
                 new Review
                 {
                     Title = review.Title,
                     Comment = review.Comment,
                     Score = review.Score,
                     Offer = reviewOffer,
                     Reviewer = user
                 });


            _context.SaveChanges();
        }

        public CreateReviewViewModel GetReviewVM(int id)
        {
            var offerName = _context.Offers.Single(x => x.ID == id).Name;
            var reviews = _context.Reviews.Where(x => x.Offer.ID == id)?.ToList();
            List<ReviewViewModel> reviewVM = new List<ReviewViewModel>();
            foreach (var item in reviews)
            {
                reviewVM.Add(new ReviewViewModel { 
                    Score = item.Score, 
                    Title = item.Title, 
                    Comment = item.Comment });
            }
            var offerReview = new CreateReviewViewModel
            {
                OfferId = id,
                OfferName = offerName,
                Reviews = reviewVM
            };
            return offerReview;
        }
    }
}
