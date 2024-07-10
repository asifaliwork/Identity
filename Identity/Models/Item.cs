using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Identity.Models
{
    public class Item
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Item Name")]
        public string? ItemName { get; set; }
        [Required]
        public int Price { get; set; }

    }
}
