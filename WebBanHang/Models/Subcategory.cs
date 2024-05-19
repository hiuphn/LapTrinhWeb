using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBanHang.Models
{
    public class Subcategory
    {
        
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            public string Description { get; set; }

            [Required]
            [ForeignKey("Category")]
            public int CategoryId { get; set; }

            public Category Category { get; set; }
        }



    }


