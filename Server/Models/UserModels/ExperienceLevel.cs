using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CogniFitRepo.Server.Models.UserModels
{
    public class ExperienceLevel
    {
        [Key]
        [Column(TypeName = "SMALLINT")]
        public int Id { get; set; }

        [Column(TypeName = "INT")]
        public byte MinimumExperience { get; set; }



    }
}
