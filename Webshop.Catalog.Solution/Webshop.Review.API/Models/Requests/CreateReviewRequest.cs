namespace Webshop.Review.API.Models.Requests
{
    public class CreateReviewRequest
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
