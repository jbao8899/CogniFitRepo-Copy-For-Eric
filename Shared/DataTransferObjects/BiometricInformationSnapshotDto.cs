using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniFitRepo.Shared.DataTransferObjects
{
    /* Make a Biometric DTO  

    Get User Biometrics(Current User) 

    Post User Biometric Snapshot(Current User) 

    Delete Biometric(Id) 

    Put User Biometric Snapshot(Current User)(Id)  */

    public class BiometricInformationSnapshotDto
    {
        public long? Id { get; set; }

        public string? UserId { get; set; } = null!;

        public DateTime? MeasurementDateTime { get; set; }

        public float? HeightCm { get; set; }

        public float? WeightKg { get; set; }

        public float? BodyFatPercentage { get; set; }
    }
}
