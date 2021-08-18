using System.ComponentModel.DataAnnotations;

namespace DataStore.Models
{
    public enum Role
    {
        [Display(Name = "CEO")]
        Ceo,
        [Display(Name = "Administrator")]
        Administrator,
        [Display(Name = "Seller")]
        Seller,
        [Display(Name = "Waiter")]
        Waiter
    }
}