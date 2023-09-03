using CogniFitRepo.Server.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Server.Models.UserModels
{
    public class Portrait
    {

        [Column(TypeName = "INT")]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(75)")]
        public string Path { get; set; } = null!;

        // Navigation properties:
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
    }
}
