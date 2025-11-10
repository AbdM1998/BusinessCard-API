using System.Text.RegularExpressions;

namespace BusinessCardAPI.Utilities
{
    public static class ValidationHelper
    {
        private const int MaxPhotoSizeBytes = 1024 * 1024; // 1MB

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                return Regex.IsMatch(email, emailPattern);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Allow international format with +, digits, spaces, hyphens, and parentheses
            // Sample : 1234567 ,  +1 234 - 567 - 8900, (123) 456 - 7890 ,123 456 7890
            var phonePattern = @"^[\d\s\-\+\(\)]{7,20}$";
            return Regex.IsMatch(phone, phonePattern);
        }

        public static bool IsValidBase64Photo(string? photo)
        {
            if (string.IsNullOrWhiteSpace(photo))
                return true; 

            try
            {
                var base64Data = photo.Contains(",") ? photo.Split(',')[1] : photo;

                // Validate base64 format
                var bytes = Convert.FromBase64String(base64Data);

                // Check size limit
                if (bytes.Length > MaxPhotoSizeBytes)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender))
                return false;

            var validGenders = new[] { "Male", "Female", "Other" };
            return validGenders.Contains(gender, StringComparer.OrdinalIgnoreCase);
        }

        public static string? GetBase64PhotoWithoutPrefix(string? photo)
        {
            if (string.IsNullOrWhiteSpace(photo))
                return null;

            return photo.Contains(",") ? photo.Split(',')[1] : photo;
        }

        public static long GetBase64PhotoSizeInBytes(string? photo)
        {
            if (string.IsNullOrWhiteSpace(photo))
                return 0;

            try
            {
                var base64Data = GetBase64PhotoWithoutPrefix(photo);
                if (string.IsNullOrWhiteSpace(base64Data))
                    return 0;

                var bytes = Convert.FromBase64String(base64Data);
                return bytes.Length;
            }
            catch
            {
                return 0;
            }
        }
    }
}