namespace BusinessCardAPI.Models.DTOs
{
    public class BusinessCardFilterDto
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
