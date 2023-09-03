using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Server.Models.UserModels
{
    public class ApplicationUser : IdentityUser
    {
        //[Column(TypeName = "INT")]
        //[ForeignKey("Address")]
        //public int? UserAddressId { get; set; }

        [Column(TypeName = "INT")]
        [ForeignKey("Portrait")]
        public int? PortraitId { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string? FirstName { get; set; } = null!;

        [Column(TypeName = "VARCHAR(50)")]
        public string? LastName { get; set; } = null!;

        [Column(TypeName = "BIT")]
        public bool? IsFemale { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime? Birthday { get; set; }

        [Column(TypeName = "BIT")]
        public bool? PrefersMetric { get; set; }

        [Column(TypeName = "TEXT")]
        public string? ProfileDescription { get; set; } = null!;

        [Column(TypeName = "INT")]
        public int NumExperiencePoints { get; set; } = 0;

        [Column(TypeName = "SMALLINT")]
        public int? StreetNumber { get; set; } = null;

        [Column(TypeName = "VARCHAR(50)")]
        public string? StreetName { get; set; } = null;

        [Column(TypeName = "VARCHAR(25)")]
        public string? ApartmentNumber { get; set; } = null;

        [Column(TypeName = "VARCHAR(50)")]
        public string? CityName { get; set; } = null;

        [Column(TypeName = "VARCHAR(50)")]
        public string? SubdivisionName { get; set; } = null;

        [Column(TypeName = "VARCHAR(50)")]
        public string? CountryName { get; set; } = null;

        [Column(TypeName = "VARCHAR(25)")]
        public int? PostalCode { get; set; } = null;

        //Navigation

        //public string? Portrait_URI { get; set; } = null;

        public Portrait? Portrait { get; set; } = null;

        public ICollection<BiometricInformationSnapshot> BiometricInformation { get; set; } = new List<BiometricInformationSnapshot>();

        public ICollection<WorkoutProgram> WorkoutPrograms { get; set; } = new List<WorkoutProgram>();
    }
}