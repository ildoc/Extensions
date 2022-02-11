using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Settings
{
    [Table("__ApplicationSettings")]
    public class ApplicationSetting
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
