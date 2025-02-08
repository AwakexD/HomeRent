using HomeRent.Common;
using HomeRent.Data.Models.User;
using HomeRent.Models.DTOs.Review;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReviewController : Controller
    {
        public UserManager<ApplicationUser> userManager;
        
        public IReviewService reviewService;

        public ReviewController(UserManager<ApplicationUser> userManager,
            IReviewService reviewService)
        {
            this.userManager = userManager;
            this.reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateDto reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(new { message = ErrorConstants.UserNotFoundError });
                }

                var result = await reviewService.CreateReviewAsync(reviewDto, user.Id);
                if (!result)
                {
                    return StatusCode(500, new { message = ErrorConstants.ReviewCreateError });
                }

                return Ok(new { message = "Review created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ErrorConstants.ReviewCreateError, error = ex.Message });
            }
        }

        //ToDO : Review Delete Implementation
        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
