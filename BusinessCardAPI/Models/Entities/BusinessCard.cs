using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardAPI.Models.Entities
{
    public class BusinessCard
    {
        public BusinessCard(string name, string gender, DateTime dateOfBirth, string email, string phone, string address, string? photo, DateTime createdAt)
        {
            Name = name;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Email = email;
            Phone = phone;
            Address = address;
            Photo = photo;
            CreatedAt = createdAt;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Photo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public void Edit(string name, string gender, DateTime dateOfBirth, string email, string phone, string address, string? photo)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;
            if (!string.IsNullOrWhiteSpace(gender))
                Gender = gender;
            if (dateOfBirth > DateTime.UtcNow)
                DateOfBirth = dateOfBirth;
            if (!string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
                Email = email;
            if (!string.IsNullOrWhiteSpace(phone) || !new PhoneAttribute().IsValid(phone))
                Phone = phone;

            Address = address;
            Photo = photo;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}