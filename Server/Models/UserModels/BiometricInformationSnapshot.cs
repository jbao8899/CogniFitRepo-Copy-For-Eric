using CogniFitRepo.Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Server.Models.UserModels
{
    public class BiometricInformationSnapshot
    {
        [Key]
        [Column(TypeName = "BIGINT")]
        public long Id { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column(TypeName = "NVARCHAR(450)")]
        public string? UserId { get; set; } = null!;

        [Column(TypeName = "DATETIME")]
        public DateTime MeasurementDateTime { get; set; }

        [Column(TypeName = "FLOAT")]
        public float? HeightCm { get; set; }

        [Column(TypeName = "FLOAT")]
        public float? WeightKg { get; set; }

        [Column(TypeName = "FLOAT")]
        public float? BodyFatPercentage { get; set; }

        // Navigation Properties
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public BiometricInformationSnapshotDto ToDto()
        {
            return new BiometricInformationSnapshotDto
            {
                Id = Id,
                UserId = UserId,
                MeasurementDateTime = MeasurementDateTime,
                HeightCm = HeightCm,
                WeightKg = WeightKg,
                BodyFatPercentage = BodyFatPercentage,
            };
        }

        public BiometricInformationSnapshot() { }

        // Does not set the ID
        // No error checking is done
        // Sets the MeasurementDateTime to the current time if none is provided in the DTO
        public BiometricInformationSnapshot(BiometricInformationSnapshotDto biometricInformationSnapshotDto)
        {
            UserId = biometricInformationSnapshotDto.UserId;

            if (biometricInformationSnapshotDto.MeasurementDateTime is not null)
            {
                MeasurementDateTime = (DateTime)biometricInformationSnapshotDto.MeasurementDateTime;
            }
            else
            {
                MeasurementDateTime = DateTime.Now;
            }
            HeightCm = biometricInformationSnapshotDto.HeightCm;
            WeightKg = biometricInformationSnapshotDto.WeightKg;
            BodyFatPercentage = biometricInformationSnapshotDto.BodyFatPercentage;
        }
    }

}
