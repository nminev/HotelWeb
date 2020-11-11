using Models.ReviewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Service
{
    public interface IReviewService
    {
        public Task AddReview(CreateReviewViewModel review, ClaimsPrincipal _user);
        public CreateReviewViewModel GetReviewVM(int id);
    }
}