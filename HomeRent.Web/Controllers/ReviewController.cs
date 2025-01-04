using HomeRent.Data.Models.User;
using HomeRent.Models.DTOs.Review;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
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
        [Authorize]
        public async Task<IActionResult> Create(ReviewCreateDto reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.GetUserAsync(User);

            var result = await this.reviewService.CreateReviewAsync(reviewDto, user.Id);

            if (!result)
            {
                return StatusCode(500, "An error occurred while creating the review.");
            }

            return Ok(new { Message = "Review created successfully." });
        }

        public IActionResult Delete()
        {
            return this.NotFound();
        }
    }
}
