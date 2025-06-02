using System.ComponentModel.DataAnnotations;

namespace contactAppMicroservice.Entities
{
    public class Subcategory
    {
        [Key]
        public Guid SubcategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
