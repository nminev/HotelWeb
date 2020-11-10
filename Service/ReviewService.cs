using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class ReviewService
    {
        //public async Task AddReview(CreateReviewViewModel review)
        //{
        //    var reviewOffer = _context.Offers.Single(x => x.ID == review.OfferId);
        //    var user = await _manager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
        //    var OfferReviews = _context.Reviews.Where(x => x.Offer.ID == review.OfferId).ToList();

        //    if (OfferReviews.Any())
        //    {
        //        var sum = OfferReviews.Sum(x => (double)x.Score) + review.Score;
        //        var count = OfferReviews.Count + 1;
        //        reviewOffer.Rating = sum / count;
        //    }
        //    else
        //    {
        //        reviewOffer.Rating = review.Score;
        //    }

        //    _context.Reviews.Add(
        //         new Review
        //         {
        //             Title = review.Title,
        //             Comment = review.Comment,
        //             Score = review.Score,
        //             Offer = reviewOffer,
        //             Reviewer = user
        //         });


        //    _context.SaveChanges();
        //}
    }
}
