using CogniFitRepo.Server.Data;
using CogniFitRepo.Shared.DataTransferObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CogniFitRepo.Server.Models.ExerciseModels
{
    public class ExerciseInstance_ExerciseProperty
    {
        [Column(TypeName = "BIGINT")]
        [ForeignKey("ExerciseInstance")]
        public long ExerciseInstanceId { get; set; }

        [Column(TypeName = "TINYINT")]
        [ForeignKey("ExerciseProperty")]
        public int ExercisePropertyId { get; set; }

        [Column(TypeName = "FLOAT")]
        public float Amount { get; set; }

        public ExerciseInstance ExerciseInstance { get; set; } = null!;
        public ExerciseProperty ExerciseProperty { get; set; } = null!;

        public ExerciseInstance_ExercisePropertyDto ToDto()
        {
            ExerciseInstance_ExercisePropertyDto retVal = new ExerciseInstance_ExercisePropertyDto()
            {
                ExerciseInstanceId = ExerciseInstanceId,
                ExercisePropertyId = ExercisePropertyId,
                Name = ExerciseProperty.Name,
                Amount = Amount
            };

            return retVal;
        }

        public ExerciseInstance_ExerciseProperty() { }

        // Construct from DTO
        // No error checking is done
        // Ignore exerciseInstance_ExercisePropertyDto.ExerciseInstanceId, 
        // as that is not known ahead of time by the front end
        // Name is not needed either
        public ExerciseInstance_ExerciseProperty(ExerciseInstance_ExercisePropertyDto exerciseInstance_ExercisePropertyDto)
        {
            ExercisePropertyId = exerciseInstance_ExercisePropertyDto.ExercisePropertyId;
            Amount = exerciseInstance_ExercisePropertyDto.Amount;
        }
    }
}
