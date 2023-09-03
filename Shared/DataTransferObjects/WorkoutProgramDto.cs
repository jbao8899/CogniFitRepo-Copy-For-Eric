using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniFitRepo.Shared.DataTransferObjects
{
    public class WorkoutProgramDto
    {
        public long? Id { get; set; }

        public string? UserId { get; set; } = null;

        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Notes { get; set; } = null;

        public bool IsComplete { get; set; }

        public IEnumerable<WorkoutDto>? WorkoutDtos { get; set; } = null;

        public override bool Equals(object? obj)
        {
            //Console.WriteLine("WorkoutProgramDto.Equals() called.");
            if ((obj is null || !GetType().Equals(obj.GetType())))
            {
                //Console.WriteLine("Other object type differs");
                return false;
            }
            else
            {
                WorkoutProgramDto otherAsDto = (WorkoutProgramDto)obj;

                // Check that its own fields are the same
                if ((Id != otherAsDto.Id) ||
                    (UserId != otherAsDto.UserId) ||
                    (Name != otherAsDto.Name) ||
                    (StartDate != otherAsDto.StartDate) ||
                    (EndDate != otherAsDto.EndDate) ||
                    (Notes != otherAsDto.Notes) ||
                    (IsComplete != otherAsDto.IsComplete))
                {
                    // Print all fields belonging to this object and otherAsDto
                    //Console.WriteLine($"Id: {Id} vs {otherAsDto.Id}");
                    //Console.WriteLine($"UserId: {UserId} vs {otherAsDto.UserId}");
                    //Console.WriteLine($"Name: {Name} vs {otherAsDto.Name}");
                    //Console.WriteLine($"StartDate: {StartDate} vs {otherAsDto.StartDate}");
                    //Console.WriteLine($"EndDate: {EndDate} vs {otherAsDto.EndDate}");
                    //Console.WriteLine($"Notes: {Notes} vs {otherAsDto.Notes}");
                    //Console.WriteLine($"IsComplete: {IsComplete} vs {otherAsDto.IsComplete}");
                    //Console.WriteLine("Some field differs");
                    return false;
                }

                // check if contained workouts are the same
                if ((WorkoutDtos is null) &&
                    (otherAsDto.WorkoutDtos is null))
                {
                    // both have a null list of workouts
                    return true;
                }
                else if ((WorkoutDtos is not null) &&
                    (otherAsDto.WorkoutDtos is not null))
                {
                    // both have non-null list of workouts
                    if (WorkoutDtos.Count() !=
                        otherAsDto.WorkoutDtos.Count())
                    {
                        // Have different numbers of workouts
                        //Console.WriteLine("Number of workouts differs");
                        return false;
                    }
                    else
                    {
                        // Both have the same number of workouts
                        // Check each workout to see if they are the same
                        List<WorkoutDto> ourWorkoutList = WorkoutDtos.ToList();
                        List<WorkoutDto> otherWorkoutList = otherAsDto.WorkoutDtos.ToList();
                        for (int i = 0; i < ourWorkoutList.Count(); i++)
                        {
                            if (!ourWorkoutList[i].Equals(otherWorkoutList[i]))
                            {
                                // An exercise instance differed between the lists
                                //Console.WriteLine($"workout differs at i == {i}"); // i == 0
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
                    Console.WriteLine("One object contains a null list and the other does not");
                    return false;
                }
            }
        }
    }
}
