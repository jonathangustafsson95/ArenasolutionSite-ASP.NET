using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLibrary.Enums
{
    public enum UserType
    {
        [Display(Name = "Player")]
        Player,
        [Display(Name = "Advertiser")]
        Advertiser,
        [Display(Name = "Operator")]
        Operator,
        [Display(Name = "LeagueOwner")]
        LeagueOwner
    }
}
