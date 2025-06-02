using System.ComponentModel.DataAnnotations;

namespace contactAppMicroservice.Entities
{
    public class Category
    {
        [Key]
        public Guid CategoryId {  get; set; }

        public string Name { get; set; } = string.Empty;

    }
}
