using System.ComponentModel.DataAnnotations;

namespace BlingIntegrationTagplus.Models
{
    public class TagPlusToken
    {
        public TagPlusToken(string name, string value, string expiresIn)
        {
            this.Name = name;
            this.Value = value;
            this.ExpiresIn = expiresIn;
        }

        [Key]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string ExpiresIn { get; set; }
    }
}
