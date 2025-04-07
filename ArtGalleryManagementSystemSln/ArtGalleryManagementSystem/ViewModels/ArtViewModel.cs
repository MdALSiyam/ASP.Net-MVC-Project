using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGalleryManagementSystem.ViewModels
{
    public class ArtViewModel
    {
        public ArtViewModel()
        {
            this.ExhibitionList = new List<int>();
        }
        public int ArtID { get; set; }
        [Required(ErrorMessage = "Please Enter Art Title")]
        [Display(Name = "Art Title"), StringLength(50)]
        public string ArtTitle { get; set; }
        [Required(ErrorMessage = "Please Enter Artist Name")]
        [Display(Name = "Artist Name"), StringLength(50)]
        public string ArtistName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date of Creation")]
        [ValidateDate]
        public System.DateTime DateOfCreation { get; set; }
        [Range(5000, 50000)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please Enter Discount")]
        [Display(Name = "Discount (%)")]
        [StringLength(5, MinimumLength = 2)]
        public string Discount { get; set; }
        [Display(Name = "Available ?")]
        public bool IsAvailable { get; set; }
        [Display(Name = "Profile Picure")]
        public string Picture { get; set; }
        public HttpPostedFileBase PicturePath { get; set; }
        public List<int> ExhibitionList { get; set; }
    }
}