using System.ComponentModel.DataAnnotations;

namespace BlingIntegrationTagplus.Models
{
    public class SettingString
    {
        public SettingString(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        [Key]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
