namespace CogniFitRepo.Shared.DataTransferObjects
{
    public class ExerciseInstance_ExercisePropertyDto
    {
        public long? ExerciseInstanceId { get; set; }

        public int ExercisePropertyId { get; set; }

        public string? Name { get; set; } = null;

        public float Amount { get; set; }

        public override bool Equals(object? obj)
        {
            if ((obj is null || !GetType().Equals(obj.GetType())))
            {
                return false;
            }
            else
            {
                ExerciseInstance_ExercisePropertyDto otherAsDto = (ExerciseInstance_ExercisePropertyDto)obj;
                return (ExerciseInstanceId == otherAsDto.ExerciseInstanceId) &&
                       (ExercisePropertyId == otherAsDto.ExercisePropertyId) &&
                       (Name == otherAsDto.Name) &&
                       (Amount == otherAsDto.Amount);
            }
        }
    }
}
