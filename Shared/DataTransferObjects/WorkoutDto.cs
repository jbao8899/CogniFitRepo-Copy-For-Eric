namespace CogniFitRepo.Shared.DataTransferObjects
{
    public class WorkoutDto
    {
        public long? Id { get; set; }

        public long? WorkoutProgramId { get; set; }

        public DateTime WorkoutDateTime { get; set; }

        public string? Notes { get; set; } = null;

        public bool IsComplete { get; set; }

        public IEnumerable<ExerciseInstanceDto>? ExerciseInstanceDtos { get; set; } = null;

        public override bool Equals(object? obj)
        {
            //Console.WriteLine("WorkoutDto.Equals() called");
            if ((obj is null || !GetType().Equals(obj.GetType())))
            {
                //Console.WriteLine("Other object type differs");
                return false;
            }
            else
            {
                WorkoutDto otherAsDto = (WorkoutDto)obj;

                // Check that its own fields are the same
                if ((Id != otherAsDto.Id) ||
                    (WorkoutProgramId != otherAsDto.WorkoutProgramId) ||
                    (WorkoutDateTime != otherAsDto.WorkoutDateTime) ||
                    (Notes != otherAsDto.Notes) ||
                    (IsComplete != otherAsDto.IsComplete))
                {
                    //Console.WriteLine("Some field differs");
                    return false;
                }

                // check if stored exercise instances are the same
                if ((ExerciseInstanceDtos is null) &&
                    (otherAsDto.ExerciseInstanceDtos is null))
                {
                    // both have a null list of exercise instances
                    return true;
                }
                else if ((ExerciseInstanceDtos is not null) &&
                    (otherAsDto.ExerciseInstanceDtos is not null))
                {
                    // both have non-null list of exercise instances
                    if (ExerciseInstanceDtos.Count() !=
                        otherAsDto.ExerciseInstanceDtos.Count())
                    {
                        // Have different numbers of exercise instances
                        //Console.WriteLine("Number of exercise instances differs");
                        return false;
                    }
                    else
                    {
                        // Both have the same number of exercise instances
                        // Check each exercise instance to see if they are the same
                        List<ExerciseInstanceDto> ourExerciseInstanceList = ExerciseInstanceDtos.ToList();
                        List<ExerciseInstanceDto> otherExerciseInstanceList = otherAsDto.ExerciseInstanceDtos.ToList();
                        for (int i = 0; i < ourExerciseInstanceList.Count(); i++)
                        {
                            if (!ourExerciseInstanceList[i].Equals(otherExerciseInstanceList[i]))
                            {
                                // An exercise instance differed between the lists
                                //Console.WriteLine($"exercise instance differs at i == {i}"); // i == 0
                                return false;
                            }
                        }

                        // The two workouts have the same exercise instances
                        return true;
                    }
                }
                else
                {
                    // One workout has null for its list of exercise instances
                    // and the other does not
                    //Console.WriteLine("One object contains a null list and the other does not");
                    return false;
                }
            }
        }
    }
}
