namespace CogniFitRepo.Shared.DataTransferObjects
{
    public class ExerciseDto
    {
        public int Id { get; set; }

        public string? ExerciseLevel { get; set; } = null;

        public string? ExerciseCategory { get; set; } = null;

        public string? Force { get; set; } = null;

        public string? Mechanic { get; set; } = null;

        public string? Equipment { get; set; } = null;

        public string? Name { get; set; } = null;

        public string? Instructions { get; set; } = null;

        public IEnumerable<string>? PrimaryMuscles { get; set; } = null;

        public IEnumerable<string>? SecondaryMuscles { get; set; } = null;

        public IEnumerable<string>? ImagePaths { get; set; } = null;
    }
}
