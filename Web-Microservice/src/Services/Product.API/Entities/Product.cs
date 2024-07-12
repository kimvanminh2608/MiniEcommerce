using Contracts.Domains;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.API.Entities
{
    public class Product : EntityAuditBase<long>
    {
        
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string No { get; set; }

        [Required]
        [Column(TypeName = "Nvarchar(250)")]
        public string Name { get; set; }


        [Column(TypeName = "Nvarchar(250)")]
        public string Summary { get; set; }

        [Column(TypeName = "Text")]
        public string Decription { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        
    }
}
