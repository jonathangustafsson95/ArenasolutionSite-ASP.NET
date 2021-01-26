using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CommonLibrary
{
    /// <summary>
    /// This class contains the functionality for Advertisements
    /// </summary>
    public class Advert
    {
        [Key]
        public int advertId { get; set; }

        [Required]
        [Display(Name = "Name of advert:")]
        [StringLength(30, ErrorMessage = "Your advert name can't be longer than 30 characters", MinimumLength = 3)]
        public string AdvertName { get; set; } 

        [Required]
        [Display(Name = "Sponsor:")]
        [StringLength(30, ErrorMessage = "Your input can't be longer than 30 characters.", MinimumLength = 1)]
        public string Sponsoring { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Startdate")]
        public DateTime BeginDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Enddate")]
        public DateTime DeadlineDateTime { get; set; }

        public byte[] productImage { get; set; }

        [Required]
        [Display(Name = "Link to website:")]
        [StringLength(500, ErrorMessage = "Link cant be over 600 characters.", MinimumLength = 1)]
        public string Link { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual Advertiser User { get; set; }
    }
}
