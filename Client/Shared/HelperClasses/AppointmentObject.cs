using CogniFitRepo.Shared.DataTransferObjects;

namespace CogniFitRepo.Client.Shared.HelperClasses
{
    public class AppointmentObject
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        public string WorkoutProgramName { get; set; }
        public WorkoutDto Workout { get; set; }
    }
}
