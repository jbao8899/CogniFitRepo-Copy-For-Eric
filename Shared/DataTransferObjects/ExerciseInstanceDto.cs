using System.Collections.Generic;
using System.Xml.Linq;

namespace CogniFitRepo.Shared.DataTransferObjects
{
    public class ExerciseInstanceDto
    {
        public long? Id { get; set; }

        // perhaps to be used to get more information about
        // some exercise instance?
        public int ExerciseId { get; set; }

        public string? ExerciseName { get; set; } = null;

        public long? WorkoutId { get; set; }

        // I don't think the workout id is needed here????

        public int WorkoutSequenceNumber { get; set; }

        public bool IsComplete { get; set; }

        //public ExerciseDto? ExerciseDto { get; set; } = null;

        // to store all of the properties
        public IEnumerable<ExerciseInstance_ExercisePropertyDto>? ExerciseInstance_ExercisePropertyDtos { get; set; } = null;

        public ExerciseInstanceDto() { }

        public override bool Equals(object? obj)
        {
            //Console.WriteLine("ExerciseInstance.Equals() called");
            if ((obj is null || !GetType().Equals(obj.GetType())))
            {
                return false;
            }
            else
            {
                ExerciseInstanceDto otherAsDto = (ExerciseInstanceDto)obj;

                // Check that its own fields are the same
                if ((Id != otherAsDto.Id) ||
                    (ExerciseId != otherAsDto.ExerciseId) ||
                    (ExerciseName != otherAsDto.ExerciseName) ||
                    (WorkoutId != otherAsDto.WorkoutId) ||
                    (WorkoutSequenceNumber != otherAsDto.WorkoutSequenceNumber) ||
                    (IsComplete != otherAsDto.IsComplete))
                {
                    return false;

                }

                // check if properties are the same
                if ((ExerciseInstance_ExercisePropertyDtos is null) &&
                    (otherAsDto.ExerciseInstance_ExercisePropertyDtos is null)) {
                    // both have a null list of joining table entries to properties
                    return true;
                }
                else if ((ExerciseInstance_ExercisePropertyDtos is not null) &&
                    (otherAsDto.ExerciseInstance_ExercisePropertyDtos is not null))
                {
                    // both have non-null list of properties
                    if (ExerciseInstance_ExercisePropertyDtos.Count() !=
                        otherAsDto.ExerciseInstance_ExercisePropertyDtos.Count())
                    {
                        // Have different numbers of properties
                        return false;
                    }
                    else
                    {
                        // Both have the same number of properties
                        // Check each property to see if they are the same
                        List<ExerciseInstance_ExercisePropertyDto> ourPropList = ExerciseInstance_ExercisePropertyDtos.ToList();
                        List<ExerciseInstance_ExercisePropertyDto> otherPropList = otherAsDto.ExerciseInstance_ExercisePropertyDtos.ToList();
                        for (int i = 0; i < ourPropList.Count(); i++)
                        {
                            if (!ourPropList[i].Equals(otherPropList[i]))
                            {
                                // A property differed between the lists
                                //Console.WriteLine("Some property differs");
                                return false;
                            }
                        }

                        // The two exercise instances have the same connections to the same properties
                        return true;
                    }
                }
                else
                {
                    // One exercise instance has null for its list of properties
                    // and the other does not
                    return false;
                }
            }
        }
    }
}
