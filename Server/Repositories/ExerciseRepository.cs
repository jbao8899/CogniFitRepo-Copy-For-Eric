using CogniFitRepo.Server.Data;
using CogniFitRepo.Shared.DataTransferObjects;
using CogniFitRepo.Server.Models.ExerciseModels;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CogniFitRepo.Server.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExerciseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ExerciseDto> GetExercises()
        {
            var exercises = _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .ToList();

            var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

            return exerciseDtos;
        }

        public ExerciseDto? GetExerciseById(int id)
        {
            var exercise = (from exerciseLinq in _context.Exercises
                            where exerciseLinq.Id == id
                            select new ExerciseDto()
                            {
                                Id = exerciseLinq.Id,
                                ExerciseLevel = exerciseLinq.ExerciseLevel.Name,
                                ExerciseCategory = exerciseLinq.ExerciseCategory.Name,
                                Force = exerciseLinq.Force.Name,
                                Mechanic = exerciseLinq.Mechanic.Name,
                                Equipment = exerciseLinq.Equipment.Name,
                                Name = exerciseLinq.Name,
                                Instructions = exerciseLinq.Instructions,
                                PrimaryMuscles = (from exercise_muscle in exerciseLinq.Exercise_Muscles
                                                  where exercise_muscle.IsPrimary
                                                  select exercise_muscle.Muscle.Name),
                                SecondaryMuscles = (from exercise_muscle in exerciseLinq.Exercise_Muscles
                                                    where !exercise_muscle.IsPrimary
                                                    select exercise_muscle.Muscle.Name),
                                ImagePaths = (from image in exerciseLinq.ExerciseImages
                                              select image.Path)
                            }).FirstOrDefault();

            return exercise;
        }

        //returns an empty list if primaryMuscleName is null, empty, or if query finds no results.
        public List<ExerciseDto> GetExerciseByPrimaryMuscle(string primaryMuscleName)
        {
            if (!primaryMuscleName.IsNullOrEmpty())
            {

                var exercises =
                    _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .Where(e => e.Exercise_Muscles.Any(m => m.Muscle.Name.ToUpper() == primaryMuscleName.ToUpper()
                    && m.IsPrimary == true)).ToList();

                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        public List<ExerciseDto> GetExercisesByAnyMuscle(string muscleName)
        {
            if (!muscleName.IsNullOrEmpty())
            {
                var exercises =
                    _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .Where(e => e.Exercise_Muscles.Any(m => m.Muscle.Name.ToUpper() == muscleName.ToUpper())).ToList();

                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        public List<ExerciseDto> GetExercisesByEquipment(string equipmentName)
        {
            if (!equipmentName.IsNullOrEmpty())
            {
                var exercises =
                   _context.Exercises
                   .Include(e => e.ExerciseLevel)
                   .Include(e => e.ExerciseCategory)
                   .Include(e => e.Force)
                   .Include(e => e.Mechanic)
                   .Include(e => e.Equipment)
                   .Include(e => e.Exercise_Muscles)
                       .ThenInclude(ex => ex.Muscle)
                   .Include(e => e.ExerciseImages)
                   .Where(e => e.Equipment.Name.ToUpper() == equipmentName.ToUpper()).ToList();

                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        public List<ExerciseDto> GetExercisesByLevel(string levelName)
        {
            if (!levelName.IsNullOrEmpty())
            {
                var exercises =
                    _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .Where(e => e.ExerciseLevel.Name.ToUpper() == levelName.ToUpper()).ToList();

                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        public List<ExerciseDto> GetExercisesByCategory(string categoryName)
        {
            if (!categoryName.IsNullOrEmpty())
            {
                var exercises = _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .Where(e => e.ExerciseCategory.Name.ToUpper() == categoryName.ToUpper()).ToList();

                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        public List<ExerciseDto> GetExercisesByForce(string forceName)
        {
            if (!forceName.IsNullOrEmpty())
            {
                var exercises = _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .Where(e => e.Force.Name.ToUpper() == forceName.ToUpper()).ToList();

                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        public List<ExerciseDto> GetExercisesByMechanic(string mechanicName)
        {
            if (!mechanicName.IsNullOrEmpty())
            {
                var exercises = _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .Where(e => e.Mechanic.Name.ToUpper() == mechanicName.ToUpper()).ToList();

                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        // probably unneccesary
        public List<ExerciseImage> GetExerciseImages(int id)
        {
            return (from exercise_image in _context.ExerciseImages
                    join exercise in _context.Exercises
                    on exercise_image.ExerciseId equals exercise.Id
                    where exercise.Id == id
                    select exercise_image).ToList();
        }

        public List<ExerciseDto> GetExercisesByMultipleAttributes(string primaryMuscle, string anyMuscle,
             string equipment, string force, string level, string category, string mechanic)
        {
            var exercises = _context.Exercises
                    .Include(e => e.ExerciseLevel)
                    .Include(e => e.ExerciseCategory)
                    .Include(e => e.Force)
                    .Include(e => e.Mechanic)
                    .Include(e => e.Equipment)
                    .Include(e => e.Exercise_Muscles)
                        .ThenInclude(ex => ex.Muscle)
                    .Include(e => e.ExerciseImages)
                    .Where(e => primaryMuscle.IsNullOrEmpty() || e.Exercise_Muscles.Any(m => m.Muscle.Name.ToUpper() == primaryMuscle.ToUpper()))
                    .Where(e => anyMuscle.IsNullOrEmpty() || e.Exercise_Muscles.Any(m => m.Muscle.Name.ToUpper() == anyMuscle.ToUpper()))
                    .Where(e => equipment.IsNullOrEmpty() || e.Equipment.Name.ToUpper() == equipment.ToUpper())
                    .Where(e => force.IsNullOrEmpty() || e.Force.Name.ToUpper() == force.ToUpper())
                    .Where(e => level.IsNullOrEmpty() || e.ExerciseLevel.Name.ToUpper() == level.ToUpper())
                    .Where(e => category.IsNullOrEmpty() || e.ExerciseCategory.Name.ToUpper() == category.ToUpper())
                    .Where(e => mechanic.IsNullOrEmpty() || e.Mechanic.Name.ToUpper() == mechanic.ToUpper())
                    .ToList();
            if (!exercises.IsNullOrEmpty())
            {
                var exerciseDtos = exercises.Select(GetExerciseDtoFromExercise).ToList();

                return exerciseDtos;
            }
            return new List<ExerciseDto>();
        }

        private ExerciseDto GetExerciseDtoFromExercise(Exercise exercise)
        {
            return new ExerciseDto()
            {
                Id = exercise.Id,
                ExerciseLevel = exercise.ExerciseLevel.Name,
                ExerciseCategory = exercise.ExerciseCategory.Name,
                Force = exercise.Force.Name,
                Mechanic = exercise.Mechanic.Name,
                Equipment = exercise.Equipment.Name,
                Name = exercise.Name,
                Instructions = exercise.Instructions,
                PrimaryMuscles = exercise.Exercise_Muscles
                                        .Where(ex => ex.IsPrimary)
                                        .Select(ex => ex.Muscle.Name),
                SecondaryMuscles = exercise.Exercise_Muscles
                                        .Where(ex => !ex.IsPrimary)
                                        .Select(ex => ex.Muscle.Name)
                                        .ToList(),
                ImagePaths = exercise.ExerciseImages.Select(exi => exi.Path).ToList()
            };

        }
    }
}
