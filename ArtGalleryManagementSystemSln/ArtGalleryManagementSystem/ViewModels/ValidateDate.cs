using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGalleryManagementSystem.ViewModels
{
    public class ValidateDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime CurrentDate = DateTime.Now;
            string message = "";
            if (Convert.ToDateTime(value) < CurrentDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                message = "Creation Date must be today's date or greater than today's date";
                return new ValidationResult(message);
            }
        }
    }
}