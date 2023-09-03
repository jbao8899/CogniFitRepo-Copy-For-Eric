using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Shared.DataTransferObjects
{
    public class ApplicationUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int? UserAddressId { get; set; }

        public string? FirstName { get; set; } = null!;

        public string? LastName { get; set; } = null!;

        public bool? IsFemale { get; set; }

        public DateTime? Birthday { get; set; }

        public bool? PrefersMetric { get; set; }

        public string? ProfileDescription { get; set; } = null!;

        public int NumExperiencePoints { get; set; } = 0;

        public int? StreetNumber { get; set; } = null;

        public string? StreetName { get; set; } = null;

        public string? ApartmentNumber { get; set; } = null;

        public string? CityName { get; set; } = null;

        public string? SubdivisionName { get; set; } = null;

        public string? CountryName { get; set; } = null;

        public int? PostalCode { get; set; } = null;

        public string? PortraitPath { get; set; } = null;

        public int? PortraitId { get; set; }
    }
}
