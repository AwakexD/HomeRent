using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HomeRent.Models.Validation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly HashSet<string> extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            this.extensions = new HashSet<string>(extensions.Select(e => e.ToLower()));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable<IFormFile> files)
            {
                foreach (var file in files)
                {
                    if (!IsValidFile(file))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }
            else if (value is IFormFile file)
            {
                if (!IsValidFile(file))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            else
            {
                return new ValidationResult("Invalid file format.");
            }

            return ValidationResult.Success;
        }

        private bool IsValidFile(IFormFile file)
        {
            if (file == null)
            {
                return true;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            return this.extensions.Contains(extension);
        }

        private string GetErrorMessage()
        {
            return "Снимки с това резширение не са разрешени!";
        }
    }
}
