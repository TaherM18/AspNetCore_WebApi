using Microsoft.AspNetCore.Http;

namespace Repositories.Models
{
    public class t_Contact
    {
        public int? c_ContactId { get; set; }
        public int c_UserId { get; set; }
        public string c_ContactName { get; set; }
        public string c_Email { get; set; }
        public string c_Mobile { get; set; }
        public string? c_Address { get; set; }
        public string? c_Image { get; set; }
        public string? c_Group { get; set; }
        public string? c_Status { get; set; }
        public IFormFile? ContactPicture { get; set; }

        public override string ToString()
        {
            return $"Contact {{ ID: {c_ContactId}, User ID: {c_UserId}, Name: {c_ContactName}, " +
                $"Email: {c_Email}, Mobile: {c_Mobile}, Address: {c_Address ?? "N/A"}, " +
                $"Image: {c_Image ?? "No Image"}, Group: {c_Group ?? "N/A"}, " +
                $"Status: {c_Status ?? "N/A"} }}";
        }
    }
}