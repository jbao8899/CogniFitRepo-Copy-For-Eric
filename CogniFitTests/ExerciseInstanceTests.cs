using NUnit.Framework;
using System;
using System.Collections.Generic;
using CogniFitRepo.Server.Models.UserModels;
using CogniFitRepo.Server.Repositories;
using CogniFitRepo.Shared.DataTransferObjects;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CogniFitRepo.Server.Data;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models;

namespace CogniFitRepo.Server.Test
{
    [TestFixture]
    public class ExerciseInstanceTests
    {
        private ApplicationDbContext _context = null!; // will be set by SetUp()
        private IExerciseInstanceRepository _repository = null!; // will be set by SetUp()

        // store stuff here so it can be accessed in the tests:
        // It is set in StartUp()
        private Portrait _defaultPortrait = null!;

        private ApplicationUser _abcdefgUser = null!;
        private ApplicationUser _qwertyUser = null!;

        private ExerciseLevel _beginnerLevel = null!;
        private ExerciseLevel _intermediateLevel = null!;

        private ExerciseCategory _strengthCategory = null!;
        private ExerciseCategory _cardioCategory = null!;

        private Force _pushForce = null!;
        private Force _staticForce = null!;
        private Force _noneForce = null!;

        private Mechanic _compoundMechanic = null!;
        private Mechanic _noneMechanic = null!;
        private Mechanic _isolationMechanic = null!;

        private Equipment _bodyOnlyEquipment = null!;
        private Equipment _machineEquipment = null!;

        private Exercise _joggingTreadmillExercise = null!;
        private Exercise _plankExercise = null!;
        private Exercise _pushupsExercise = null!;
        private Exercise _runningTreadmillExercise = null!;
        private Exercise _stairmasterExercise = null!;

        private ExerciseProperty _repsProperty = null!;
        private ExerciseProperty _timeProperty = null!;
        private ExerciseProperty _speedProperty = null!;

        private WorkoutProgram _completeProgramForAbcdefg = null!;
        private Workout _abcdefgCompleteProgramFirstWorkout = null!;
        private ExerciseInstance _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance = null!;
        private ExerciseInstance _abcdefgCompleteProgramFirstWorkoutSecondExerciseInstance = null!;
        private Workout _abcdefgCompleteProgramSecondWorkout = null!;
        private ExerciseInstance _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance = null!;
        private ExerciseInstance _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance = null!;

        private WorkoutProgram _incompleteProgramForAbcdefg = null!;
        private Workout _abcdefgIncompleteProgramFirstWorkout = null!;
        private ExerciseInstance _abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance = null!;
        private ExerciseInstance _abcdefgIncompleteProgramFirstWorkoutSecondExerciseInstance = null!;
        private Workout _abcdefgIncompleteProgramSecondWorkout = null!;
        private ExerciseInstance _abcdefgIncompleteProgramSecondWorkoutFirstExerciseInstance = null!;
        private ExerciseInstance _abcdefgIncompleteProgramSecondWorkoutSecondExerciseInstance = null!;

        private WorkoutProgram _incompleteProgramForQwerty = null!;
        private Workout _qwertyIncompleteProgramFirstWorkout = null!;
        private ExerciseInstance _qwertyIncompleteProgramFirstWorkoutFirstExerciseInstance = null!;
        private ExerciseInstance _qwertyIncompleteProgramFirstWorkoutSecondExerciseInstance = null!;
        private Workout _qwertyIncompleteProgramSecondWorkout = null!;
        private ExerciseInstance _qwertyIncompleteProgramSecondWorkoutFirstExerciseInstance = null!;
        private ExerciseInstance _qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance = null!;
        private ExerciseInstance _qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance = null!;

        private int _numTestsRun = 0;

        // The code below seems to allow us to create an in-memory database
        // to test on, but creating all of the neccesary data
        // for testing in the short time we have left seems infeasible
        // Eric indicated that it would be acceptable to just test the controller
        // endpoints.
        [SetUp]
        public void SetUp()
        {
            // https://stackoverflow.com/questions/39481353/how-do-i-moq-the-applicationdbcontext-in-net-core
            // https://stackoverflow.com/a/48062124
            // https://stackoverflow.com/a/65004216
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "database " + _numTestsRun.ToString()) // + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.ff tt"))
                .Options;
            var operationalStoreOptions = new OperationalStoreOptions(); // ???? How do you do this????
            var wrappedOperationalStoreOptions = Options.Create(operationalStoreOptions);
            _context = new ApplicationDbContext(options, wrappedOperationalStoreOptions);

            _defaultPortrait = new Portrait()
            {
                Id = 1,
                Path = "./wwwroot/default-portrait.png"
            };
            _context.Portraits.Add(_defaultPortrait);
            _context.SaveChanges();

            _abcdefgUser = new ApplicationUser()
            {
                Id = "abcdefghijklmnopqrstuvwxyz",
                PortraitId = _defaultPortrait.Id,
                FirstName = "John",
                LastName = "Smith",
                IsFemale = false,
                Birthday = new DateTime(1999, 4, 15),
                PrefersMetric = true,
                ProfileDescription = "My name is John Smith.",
                StreetNumber = 123,
                StreetName = "Main St.",
                CityName = "Anytown",
                SubdivisionName = "Colorado",
                CountryName = "United States",
                PostalCode = 12345,
                UserName = "password-is-Abcdefg-123@gmail.com",
                Email = "password-is-Abcdefg-123@gmail.com"
            };
            _context.Users.Add(_abcdefgUser);
            _qwertyUser = new ApplicationUser()
            {
                Id = "qwertyuiopasdfghjklzxcvbnm",
                PortraitId = _defaultPortrait.Id,
                FirstName = "Nancy",
                LastName = "Johnson",
                IsFemale = true,
                Birthday = new DateTime(1995, 11, 22),
                PrefersMetric = false,
                ProfileDescription = "Blah blah blah.",
                StreetNumber = 456,
                StreetName = "Side Blvd.",
                CityName = "Springfield",
                SubdivisionName = "Illinois",
                CountryName = "United States",
                PostalCode = 67890,
                UserName = "password-is-Qwerty-98765@gmail.com",
                Email = "password-is-Qwerty-98765@gmail.com"
            };
            _context.Users.Add(_qwertyUser);
            _context.SaveChanges();

            _beginnerLevel = new ExerciseLevel()
            {
                Id = 1,
                Name = "beginner"
            };
            _context.ExerciseLevels.Add(_beginnerLevel);
            _intermediateLevel = new ExerciseLevel()
            {
                Id = 2,
                Name = "intermediate"
            };
            _context.ExerciseLevels.Add(_intermediateLevel);
            _context.SaveChanges();

            _strengthCategory = new ExerciseCategory()
            {
                Id = 1,
                Name = "strength"
            };
            _context.ExerciseCategories.Add(_strengthCategory);
            _cardioCategory = new ExerciseCategory()
            {
                Id = 6,
                Name = "cardio"
            };
            _context.ExerciseCategories.Add(_cardioCategory);
            _context.SaveChanges();

            _pushForce = new Force()
            {
                Id = 2,
                Name = "push"
            };
            _context.Forces.Add(_pushForce);
            _staticForce = new Force()
            {
                Id = 3,
                Name = "static"
            };
            _context.Forces.Add(_staticForce);
            _noneForce = new Force()
            {
                Id = 4,
                Name = "none"
            };
            _context.Forces.Add(_noneForce);
            _context.SaveChanges();

            _compoundMechanic = new Mechanic()
            {
                Id = 1,
                Name = "compound"
            };
            _context.Mechanics.Add(_compoundMechanic);
            _noneMechanic = new Mechanic()
            {
                Id = 2,
                Name = "None"
            };
            _context.Mechanics.Add(_noneMechanic);
            _isolationMechanic = new Mechanic()
            {
                Id = 3,
                Name = "isolation"
            };
            _context.Mechanics.Add(_isolationMechanic);
            _context.SaveChanges();

            _bodyOnlyEquipment = new Equipment()
            {
                Id = 1,
                Name = "body only"
            };
            _context.Equipments.Add(_bodyOnlyEquipment);
            _machineEquipment = new Equipment()
            {
                Id = 2,
                Name = "machine"
            };
            _context.Equipments.Add(_machineEquipment);
            _context.SaveChanges();

            _joggingTreadmillExercise = new Exercise()
            {
                Id = 370,
                ExerciseLevelId = _beginnerLevel.Id,
                ExerciseCategoryId = _cardioCategory.Id,
                ForceId = _noneForce.Id,
                MechanicId = _noneMechanic.Id,
                EquipmentId = _machineEquipment.Id,
                Name = "Jogging, Treadmill",
                Instructions = "To begin, step onto the treadmill and select the desired option from the menu. Most treadmills have a manual setting, or you can select a program to run. Typically, you can enter your age and weight to estimate the amount of calories burned during exercise. Elevation can be adjusted to change the intensity of the workout. Treadmills offer convenience, cardiovascular benefits, and usually have less impact than jogging outside. A 150 lb person will burn almost 250 calories jogging for 30 minutes, compared to more than 450 calories running. Maintain proper posture as you jog, and only hold onto the handles when necessary, such as when dismounting or checking your heart rate."
            };
            _context.Exercises.Add(_joggingTreadmillExercise);
            _plankExercise = new Exercise()
            {
                Id = 539,
                ExerciseLevelId = _beginnerLevel.Id,
                ExerciseCategoryId = _strengthCategory.Id,
                ForceId = _staticForce.Id,
                MechanicId = _isolationMechanic.Id,
                EquipmentId = _bodyOnlyEquipment.Id,
                Name = "Plank",
                Instructions = "Get into a prone position on the floor, supporting your weight on your toes and your forearms. Your arms are bent and directly below the shoulder. Keep your body straight at all times, and hold this position as long as possible. To increase difficulty, an arm or leg can be raised."
            };
            _context.Exercises.Add(_plankExercise);
            _pushupsExercise = new Exercise()
            {
                Id = 568,
                ExerciseLevelId = _beginnerLevel.Id,
                ExerciseCategoryId = _strengthCategory.Id,
                ForceId = _pushForce.Id,
                MechanicId = _compoundMechanic.Id,
                EquipmentId = _bodyOnlyEquipment.Id,
                Name = "Pushups",
                Instructions = "Lie on the floor face down and place your hands about 36 inches apart while holding your torso up at arms length. Next, lower yourself downward until your chest almost touches the floor as you inhale. Now breathe out and press your upper body back up to the starting position while squeezing your chest. After a brief pause at the top contracted position, you can begin to lower yourself downward again for as many repetitions as needed."
            };
            _context.Exercises.Add(_pushupsExercise);
            _runningTreadmillExercise = new Exercise()
            {
                Id = 613,
                ExerciseLevelId = _beginnerLevel.Id,
                ExerciseCategoryId = _cardioCategory.Id,
                ForceId = _noneForce.Id,
                MechanicId = _noneMechanic.Id,
                EquipmentId = _machineEquipment.Id,
                Name = "Running, Treadmill",
                Instructions = "To begin, step onto the treadmill and select the desired option from the menu. Most treadmills have a manual setting, or you can select a program to run. Typically, you can enter your age and weight to estimate the amount of calories burned during exercise. Elevation can be adjusted to change the intensity of the workout. Treadmills offer convenience, cardiovascular benefits, and usually have less impact than running outside. A 150 lb person will burn over 450 calories running 8 miles per hour for 30 minutes. Maintain proper posture as you run, and only hold onto the handles when necessary, such as when dismounting or checking your heart rate."
            };
            _context.Exercises.Add(_runningTreadmillExercise);
            _stairmasterExercise = new Exercise()
            {
                Id = 740,
                ExerciseLevelId = _intermediateLevel.Id,
                ExerciseCategoryId = _cardioCategory.Id,
                ForceId = _noneForce.Id,
                MechanicId = _noneMechanic.Id,
                EquipmentId = _machineEquipment.Id,
                Name = "Stairmaster",
                Instructions = "To begin, step onto the stairmaster and select the desired option from the menu. You can choose a manual setting, or you can select a program to run. Typically, you can enter your age and weight to estimate the amount of calories burned during exercise. Pump your legs up and down in an established rhythm, driving the pedals down but not all the way to the floor. It is recommended that you maintain your grip on the handles so that you donâ€™t fall. The handles can be used to monitor your heart rate to help you stay at an appropriate intensity. Stairmasters offer convenience, cardiovascular benefits, and usually have less impact than running outside. They are typically much harder than other cardio equipment. A 150 lb person will typically burn over 300 calories in 30 minutes, compared to about 175 calories walking."
            };
            _context.Exercises.Add(_stairmasterExercise);
            _context.SaveChanges();

            _repsProperty = new ExerciseProperty()
            {
                Id = 1,
                Name = "Reps"
            };
            _context.ExerciseProperties.Add(_repsProperty);
            _timeProperty = new ExerciseProperty()
            {
                Id = 4,
                Name = "Time (s)"
            };
            _context.ExerciseProperties.Add(_timeProperty);
            _speedProperty = new ExerciseProperty
            {
                Id = 6,
                Name = "Speed (kph)",
            };
            _context.ExerciseProperties.Add(_speedProperty);
            _context.SaveChanges();

            _completeProgramForAbcdefg = new WorkoutProgram()
            {
                Id = 1,
                UserId = _abcdefgUser.Id,
                Name = "Complete program for abcdefg",
                StartDate = new DateTime(2023, 8, 22),
                EndDate = new DateTime(2023, 10, 22),
                Notes = "My first workout",
                IsComplete = true
            };
            _context.WorkoutPrograms.Add(_completeProgramForAbcdefg);
            _abcdefgCompleteProgramFirstWorkout = new Workout()
            {
                Id = 1,
                WorkoutProgramId = _completeProgramForAbcdefg.Id,
                WorkoutDateTime = new DateTime(2023, 8, 24, 18, 0, 0),
                Notes = "Finish before dinner",
                IsComplete = true
            };
            _context.Workouts.Add(_abcdefgCompleteProgramFirstWorkout);
            _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance = new ExerciseInstance()
            {
                Id = 1,
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutId = _abcdefgCompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 1,
                IsComplete = true
            };
            _context.ExerciseInstances.Add(_abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 900f
            });
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _speedProperty.Id,
                Amount = 8f
            });
            _abcdefgCompleteProgramFirstWorkoutSecondExerciseInstance = new ExerciseInstance()
            {
                Id = 2,
                ExerciseId = _runningTreadmillExercise.Id,
                WorkoutId = _abcdefgCompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 2,
                IsComplete = true
            };
            _context.ExerciseInstances.Add(_abcdefgCompleteProgramFirstWorkoutSecondExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramFirstWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 300f
            });
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramFirstWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _speedProperty.Id,
                Amount = 12f
            });
            _abcdefgCompleteProgramSecondWorkout = new Workout()
            {
                Id = 2,
                WorkoutProgramId = _completeProgramForAbcdefg.Id,
                WorkoutDateTime = new DateTime(2023, 8, 26, 15, 30, 0),
                IsComplete = true
            };
            _context.Workouts.Add(_abcdefgCompleteProgramSecondWorkout);
            _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance = new ExerciseInstance()
            {
                Id = 3,
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutId = _abcdefgCompleteProgramSecondWorkout.Id,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
            };
            _context.ExerciseInstances.Add(_abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 900f
            });
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _speedProperty.Id,
                Amount = 8.5f
            });
            _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance = new ExerciseInstance()
            {
                Id = 4,
                ExerciseId = _runningTreadmillExercise.Id,
                WorkoutId = _abcdefgCompleteProgramSecondWorkout.Id,
                WorkoutSequenceNumber = 2,
                IsComplete = true,
            };
            _context.ExerciseInstances.Add(_abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 300f
            });
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _speedProperty.Id,
                Amount = 13f
            });
            _context.SaveChanges();

            _incompleteProgramForAbcdefg = new WorkoutProgram()
            {
                Id = 2,
                UserId = _abcdefgUser.Id,
                Name = "Incomplete program for abcdefg",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 12, 31),
                Notes = "Future workout",
                IsComplete = false,
            };
            _context.WorkoutPrograms.Add(_incompleteProgramForAbcdefg);
            _abcdefgIncompleteProgramFirstWorkout = new Workout()
            {
                Id = 3,
                WorkoutProgramId = _incompleteProgramForAbcdefg.Id,
                WorkoutDateTime = new DateTime(2024, 1, 1, 14, 15, 0),
                Notes = "New year workout",
                IsComplete = false,
            };
            _context.Workouts.Add(_abcdefgIncompleteProgramFirstWorkout);
            _abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance = new ExerciseInstance()
            {
                Id = 5,
                ExerciseId = _pushupsExercise.Id,
                WorkoutId = _abcdefgIncompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 1,
                IsComplete = false,
            };
            _context.ExerciseInstances.Add(_abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _repsProperty.Id,
                Amount = 15f
            });
            _abcdefgIncompleteProgramFirstWorkoutSecondExerciseInstance = new ExerciseInstance()
            {
                Id = 6,
                ExerciseId = _plankExercise.Id,
                WorkoutId = _abcdefgIncompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 2,
                IsComplete = false,
            };
            _context.ExerciseInstances.Add(_abcdefgIncompleteProgramFirstWorkoutSecondExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgIncompleteProgramFirstWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 60f
            });
            _abcdefgIncompleteProgramSecondWorkout = new Workout()
            {
                Id = 4,
                WorkoutProgramId = _incompleteProgramForAbcdefg.Id,
                WorkoutDateTime = new DateTime(2024, 2, 1, 20, 0, 0),
                IsComplete = false,
            };
            _context.Workouts.Add(_abcdefgIncompleteProgramSecondWorkout);
            _abcdefgIncompleteProgramSecondWorkoutFirstExerciseInstance = new ExerciseInstance()
            {
                Id = 7,
                ExerciseId = _pushupsExercise.Id,
                WorkoutId = _abcdefgIncompleteProgramSecondWorkout.Id,
                WorkoutSequenceNumber = 1,
                IsComplete = false,
            };
            _context.ExerciseInstances.Add(_abcdefgIncompleteProgramSecondWorkoutFirstExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgIncompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _repsProperty.Id,
                Amount = 18f
            });
            _abcdefgIncompleteProgramSecondWorkoutSecondExerciseInstance = new ExerciseInstance()
            {
                Id = 8,
                ExerciseId = _plankExercise.Id,
                WorkoutId = _abcdefgIncompleteProgramSecondWorkout.Id,
                WorkoutSequenceNumber = 2,
                IsComplete = false,
            };
            _context.ExerciseInstances.Add(_abcdefgIncompleteProgramSecondWorkoutSecondExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _abcdefgIncompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 75f
            });
            _context.SaveChanges();

            _incompleteProgramForQwerty = new WorkoutProgram()
            {
                Id = 3,
                UserId = _qwertyUser.Id,
                Name = "Incomplete program for qwerty",
                StartDate = new DateTime(2023, 8, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "QWERTY WORKOUT PROGRAM",
                IsComplete = false,
            };
            _context.WorkoutPrograms.Add(_incompleteProgramForQwerty);
            _qwertyIncompleteProgramFirstWorkout = new Workout()
            {
                Id = 5,
                WorkoutProgramId = _incompleteProgramForQwerty.Id,
                WorkoutDateTime = new DateTime(2023, 8, 15, 8, 15, 0),
                IsComplete = true
            };
            _context.Workouts.Add(_qwertyIncompleteProgramFirstWorkout);
            _qwertyIncompleteProgramFirstWorkoutFirstExerciseInstance = new ExerciseInstance()
            {
                Id = 9,
                ExerciseId = _pushupsExercise.Id,
                WorkoutId = _qwertyIncompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 1,
                IsComplete = true,
            };
            _context.ExerciseInstances.Add(_qwertyIncompleteProgramFirstWorkoutFirstExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _repsProperty.Id,
                Amount = 15f
            });
            _qwertyIncompleteProgramFirstWorkoutSecondExerciseInstance = new ExerciseInstance()
            {
                Id = 10,
                ExerciseId = _plankExercise.Id,
                WorkoutId = _qwertyIncompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 2,
                IsComplete = true
            };
            _context.ExerciseInstances.Add(_qwertyIncompleteProgramFirstWorkoutSecondExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramFirstWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 60f
            });
            _qwertyIncompleteProgramSecondWorkout = new Workout()
            {
                Id = 6,
                WorkoutProgramId = _incompleteProgramForQwerty.Id,
                WorkoutDateTime = new DateTime(2023, 9, 15, 13, 50, 0),
                IsComplete = false,
            };
            _context.Workouts.Add(_qwertyIncompleteProgramSecondWorkout);
            _qwertyIncompleteProgramSecondWorkoutFirstExerciseInstance = new ExerciseInstance()
            {
                Id = 11,
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutSequenceNumber = 1,
                IsComplete = false
            };
            _context.ExerciseInstances.Add(_qwertyIncompleteProgramSecondWorkoutFirstExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 900f
            });
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                ExercisePropertyId = _speedProperty.Id,
                Amount = 8.5f
            });
            _qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance = new ExerciseInstance()
            {
                Id = 12,
                ExerciseId = _runningTreadmillExercise.Id,
                WorkoutSequenceNumber = 2,
                IsComplete = false,
            };
            _context.ExerciseInstances.Add(_qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 300f
            });
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                ExercisePropertyId = _speedProperty.Id,
                Amount = 12f
            });
            _qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance = new ExerciseInstance()
            {
                Id = 13,
                ExerciseId = _stairmasterExercise.Id,
                WorkoutSequenceNumber = 3,
                IsComplete = false,
            };
            _context.ExerciseInstances.Add(_qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance);
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance.Id,
                ExercisePropertyId = _timeProperty.Id,
                Amount = 150f
            });
            _context.ExerciseInstance_ExerciseProperties.Add(new ExerciseInstance_ExerciseProperty()
            {
                ExerciseInstanceId = _qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance.Id,
                ExercisePropertyId = _speedProperty.Id,
                Amount = 2.5f
            });
            _context.SaveChanges();

            _repository = new ExerciseInstanceRepository(_context);

            _numTestsRun++; // to ensure each a new in-memory database is created each time
        }

        [Test]
        public void GetAllExerciseInstances_HaveExerciseInstances_ReturnsAllExerciseInstances()
        {
            // Arrange
            // Done in setup

            // Act
            // We need the OrderBy(), as things may not be returned in the same order they were added
            List<ExerciseInstanceDto> results = _repository.GetAll().OrderBy(ei => ei.Id).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(13));

            // We cannot use Is.EqualTo(), as that uses the default == instead of our .Equals()
            Assert.That(results[0].Equals(_abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[1].Equals(_abcdefgCompleteProgramFirstWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[2].Equals(_abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[3].Equals(_abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[4].Equals(_abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[5].Equals(_abcdefgIncompleteProgramFirstWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[6].Equals(_abcdefgIncompleteProgramSecondWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[7].Equals(_abcdefgIncompleteProgramSecondWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[8].Equals(_qwertyIncompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[9].Equals(_qwertyIncompleteProgramFirstWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[10].Equals(_qwertyIncompleteProgramSecondWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[11].Equals(_qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[12].Equals(_qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance.ToDto()), Is.True);
        }

        [Test]
        public void GetAllExerciseInstances_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange
            _context.ExerciseProperties = null!;

            // Act and Assert
            Assert.Throws<MissingMemberException>(() => _repository.GetAll());
        }

        // Made with Copilot
        // Write a test case to check that GetById(5) returns an ExerciseInstanceDto
        // that is equal to _abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()
        [Test]
        public void GetById_ExerciseInstanceExists_ReturnsExerciseInstanceDto()
        {
            // Arrange
            // Done in setup

            // Act
            ExerciseInstanceDto result = _repository.GetById(5);

            // Assert
            Assert.That(result.Equals(_abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()), Is.True);
        }

        [Test]
        public void GetById_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange
            _context.ExerciseInstance_ExerciseProperties = null!;

            // Act and Assert
            Assert.Throws<MissingMemberException>(() => _repository.GetById(5));
        }

        [Test]
        public void GetById_InputNonexistantId_ThrowsKeyNotFoundException()
        {
            // Arrange
            // Nothing required

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.GetById(14));
        }

        // Created using Github Copilot
        [Test]
        public void PostToWorkout_ValidInput_CorrectlyPosted()
        {
            // Arrange
            ExerciseInstanceDto expectToAddThis = new ExerciseInstanceDto()
            {
                Id = 14,
                ExerciseId = _joggingTreadmillExercise.Id,
                ExerciseName = "Jogging, Treadmill",
                WorkoutId = _qwertyIncompleteProgramSecondWorkout.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _timeProperty.Id,
                        Name = "Time (s)",
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _speedProperty.Id,
                        Name = "Speed (kph)",
                        Amount = 5f
                    }
                }
            };

            ExerciseInstanceDto toAdd = new ExerciseInstanceDto()
            {
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _timeProperty.Id,
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _speedProperty.Id,
                        Amount = 5f
                    }
                }
            };

            // Act
            _repository.PostToWorkout(_qwertyIncompleteProgramSecondWorkout.Id, toAdd, _qwertyUser.Id);

            // Assert
            ExerciseInstanceDto postedExerciseInstanceDto = _repository.GetById(14);
            Assert.That(postedExerciseInstanceDto.Equals(expectToAddThis), Is.True);
        }

        [Test]
        public void PostToWorkout_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange
            _context.Exercises = null!;

            ExerciseInstanceDto expectToAddThis = new ExerciseInstanceDto()
            {
                Id = 14,
                ExerciseId = _joggingTreadmillExercise.Id,
                ExerciseName = "Jogging, Treadmill",
                WorkoutId = _qwertyIncompleteProgramSecondWorkout.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _timeProperty.Id,
                        Name = "Time (s)",
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _speedProperty.Id,
                        Name = "Speed (kph)",
                        Amount = 5f
                    }
                }
            };

            ExerciseInstanceDto toAdd = new ExerciseInstanceDto()
            {
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _timeProperty.Id,
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _speedProperty.Id,
                        Amount = 5f
                    }
                }
            };

            // Act and Assert
            Assert.Throws<MissingMemberException>(() => _repository.PostToWorkout(_qwertyIncompleteProgramSecondWorkout.Id, toAdd, _qwertyUser.Id));
        }

        [Test]
        public void PostToWorkout_TryPostToNonexistantWorkout_ThrowsKeyNotFoundException()
        {
            // Arrange
            int badWorkoutId = 7;

            ExerciseInstanceDto expectToAddThis = new ExerciseInstanceDto()
            {
                Id = 14,
                ExerciseId = _joggingTreadmillExercise.Id,
                ExerciseName = "Jogging, Treadmill",
                WorkoutId = badWorkoutId,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _timeProperty.Id,
                        Name = "Time (s)",
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _speedProperty.Id,
                        Name = "Speed (kph)",
                        Amount = 5f
                    }
                }
            };

            ExerciseInstanceDto toAdd = new ExerciseInstanceDto()
            {
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _timeProperty.Id,
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _speedProperty.Id,
                        Amount = 5f
                    }
                }
            };

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.PostToWorkout(badWorkoutId, toAdd, _qwertyUser.Id));
        }

        [Test]
        public void PostToWorkout_TryPostToWorkoutOfDifferentUser_ThrowsNotSupportedException()
        {
            ExerciseInstanceDto expectToAddThis = new ExerciseInstanceDto()
            {
                Id = 14,
                ExerciseId = _joggingTreadmillExercise.Id,
                ExerciseName = "Jogging, Treadmill",
                WorkoutId = _abcdefgCompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _timeProperty.Id,
                        Name = "Time (s)",
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _speedProperty.Id,
                        Name = "Speed (kph)",
                        Amount = 5f
                    }
                }
            };

            ExerciseInstanceDto toAdd = new ExerciseInstanceDto()
            {
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _timeProperty.Id,
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _speedProperty.Id,
                        Amount = 5f
                    }
                }
            };

            // Act and Assert
            Assert.Throws<NotSupportedException>(() => _repository.PostToWorkout(_abcdefgCompleteProgramFirstWorkout.Id, toAdd, _qwertyUser.Id));
        }

        [Test]
        public void PostToWorkout_TryPostExerciseInstanceReferringToNonexistantExercise_ThrowsKeyNotFoundException()
        {
            int badExerciseId = 999;
            ExerciseInstanceDto expectToAddThis = new ExerciseInstanceDto()
            {
                Id = 14,
                ExerciseId = badExerciseId,
                ExerciseName = "Jogging, Treadmill",
                WorkoutId = _abcdefgCompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _timeProperty.Id,
                        Name = "Time (s)",
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _speedProperty.Id,
                        Name = "Speed (kph)",
                        Amount = 5f
                    }
                }
            };

            ExerciseInstanceDto toAdd = new ExerciseInstanceDto()
            {
                ExerciseId = badExerciseId,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _timeProperty.Id,
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _speedProperty.Id,
                        Amount = 5f
                    }
                }
            };

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.PostToWorkout(_qwertyIncompleteProgramSecondWorkout.Id, toAdd, _qwertyUser.Id));
        }

        [Test]
        public void PostToWorkout_TryPostExerciseInstanceReferringToNonexistantProperty_ThrowsKeyNotFoundException()
        {
            int badPropertyId = 999;
            ExerciseInstanceDto expectToAddThis = new ExerciseInstanceDto()
            {
                Id = 14,
                ExerciseId = _joggingTreadmillExercise.Id,
                ExerciseName = "Jogging, Treadmill",
                WorkoutId = _abcdefgCompleteProgramFirstWorkout.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = badPropertyId ,
                        Name = "Time (s)",
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExerciseInstanceId = 14,
                        ExercisePropertyId = _speedProperty.Id,
                        Name = "Speed (kph)",
                        Amount = 5f
                    }
                }
            };

            ExerciseInstanceDto toAdd = new ExerciseInstanceDto()
            {
                ExerciseId = _joggingTreadmillExercise.Id,
                WorkoutSequenceNumber = 4,
                IsComplete = false,
                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                {
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = badPropertyId,
                        Amount = 900f
                    },
                    new ExerciseInstance_ExercisePropertyDto()
                    {
                        ExercisePropertyId = _speedProperty.Id,
                        Amount = 5f
                    }
                }
            };

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.PostToWorkout(_qwertyIncompleteProgramSecondWorkout.Id, toAdd, _qwertyUser.Id));
        }

        [Test]
        public void Delete_Valid_CorrectExerciseInstanceDeletedAndOthersUntouched()
        {
            // Arrange
            // Done in setup

            // Act
            _repository.Delete(6, _abcdefgUser.Id);

            // We need the OrderBy(), as things may not be returned in the same order they were added
            List<ExerciseInstanceDto> results = _repository.GetAll().OrderBy(ei => ei.Id).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(12));

            // We cannot use Is.EqualTo(), as that uses the default == instead of our .Equals()
            Assert.That(results[0].Equals(_abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[1].Equals(_abcdefgCompleteProgramFirstWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[2].Equals(_abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[3].Equals(_abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[4].Equals(_abcdefgIncompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[5].Equals(_abcdefgIncompleteProgramSecondWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[6].Equals(_abcdefgIncompleteProgramSecondWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[7].Equals(_qwertyIncompleteProgramFirstWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[8].Equals(_qwertyIncompleteProgramFirstWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[9].Equals(_qwertyIncompleteProgramSecondWorkoutFirstExerciseInstance.ToDto()), Is.True);
            Assert.That(results[10].Equals(_qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance.ToDto()), Is.True);
            Assert.That(results[11].Equals(_qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance.ToDto()), Is.True);
        }

        [Test]
        public void Delete_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange
            _context.ExerciseInstances = null!;

            // Act and assert
            Assert.Throws<MissingMemberException>(() => _repository.Delete(6, _abcdefgUser.Id));
        }

        [Test]
        public void Delete_TryToDeleteNonexistantExerciseInstance_ThrowsKeyNotFoundException()
        {
            // Arrange
            // Not needed

            // Act and assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Delete(0, _abcdefgUser.Id));
        }

        [Test]
        public void Delete_TryToDeleteExerciseInstanceWhichBelongsToOtherUser_ThrowsKeyNotFoundException()
        {
            // Arrange
            // Not needed

            // Act and assert
            Assert.Throws<NotSupportedException>(() => _repository.Delete(6, _qwertyUser.Id));
        }

        [Test]
        public void GetByWorkoutId_Valid_ReturnsListContainingCorrectExerciseInstances()
        {
            // Arrange
            ExerciseInstanceDto firstExpected = _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.ToDto();
            ExerciseInstanceDto secondExpected = _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.ToDto();

            // Act
            // We need the OrderBy(), as things may not be returned in the same order they were added
            List<ExerciseInstanceDto> results = _repository.GetByWorkoutId(2).OrderBy(ei => ei.Id).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(2));

            // We cannot use Is.EqualTo(), as that uses the default == instead of our .Equals()
            Assert.That(results[0].Equals(firstExpected), Is.True);
            Assert.That(results[1].Equals(secondExpected), Is.True);
        }

        [Test]
        public void GetByWorkoutId_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange
            _context.ExerciseInstances = null!;

            // Act and Assert
            // We need the OrderBy(), as things may not be returned in the same order they were added
            Assert.Throws<MissingMemberException>(() => _repository.GetByWorkoutId(2));
        }

        [Test]
        public void GetByWorkoutId_NoWorkoutHasIdPassedIntoFunction_ThrowsKeyNotFoundException()
        {
            // Arrange
            // Nothing needed

            // Act and Assert
            // We need the OrderBy(), as things may not be returned in the same order they were added
            Assert.Throws<KeyNotFoundException>(() => _repository.GetByWorkoutId(7));
        }

        [Test]
        public void ChangeExercise_ValidInputs_CorrectlyChangesWhichExerciseAnExerciseInstancePointsTo()
        {
            // Arrange
            ExerciseInstanceDto expected = _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.ToDto();
            expected.ExerciseId = _runningTreadmillExercise.Id;
            expected.ExerciseName = _runningTreadmillExercise.Name;

            // Act
            _repository.ChangeExercise(_abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                _runningTreadmillExercise.Id,
                _abcdefgUser.Id);

            ExerciseInstanceDto resultOfPut = _repository.GetById(_abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id);

            // Assert
            Assert.That(resultOfPut.Equals(expected), Is.True);
        }

        [Test]
        public void ChangeExercise_DbSetIsNull_ThrowMissingMemberException()
        {
            // Arrange
            _context.Exercises = null!;

            // Act and assert
            Assert.Throws<MissingMemberException>(() => _repository.ChangeExercise(
                _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                _runningTreadmillExercise.Id,
                _abcdefgUser.Id));
        }

        [Test]
        public void ChangeExercise_TryChangeNonexistantExerciseInstance_ThrowKeyNotFoundException()
        {
            // Arrange
            // Not needed

            // Act and assert
            Assert.Throws<KeyNotFoundException>(() => _repository.ChangeExercise(
                9000,
                _runningTreadmillExercise.Id,
                _abcdefgUser.Id));
        }


        [Test]
        public void ChangeExercise_TryChangeExerciseInstanceBelongingToOtherUser_ThrowKeyNotFoundException()
        {
            // Arrange
            // Not needed

            // Act and assert
            Assert.Throws<NotSupportedException>(() => _repository.ChangeExercise(
                _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                _runningTreadmillExercise.Id,
                _qwertyUser.Id));
        }

        [Test]
        public void ChangeExercise_TryChangeToNonexistantExercise_ThrowKeyNotFoundException()
        {
            // Arrange
            // Not needed

            // Act and assert
            Assert.Throws<KeyNotFoundException>(() => _repository.ChangeExercise(
                _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                999,
                _abcdefgUser.Id));
        }

        [Test]
        public void SwapPositions_TwoExerciseInstancesBelongingToSameWorkout_CorrectlySwapPositionsWithinWorkout()
        {
            // Arrange
            ExerciseInstanceDto firstExpected = _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.ToDto();
            int temp = firstExpected.WorkoutSequenceNumber;
            firstExpected.WorkoutSequenceNumber = _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.WorkoutSequenceNumber;
            ExerciseInstanceDto secondExpected = _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.ToDto();
            secondExpected.WorkoutSequenceNumber = temp;


            // Act
            _repository.SwapPositions(
                _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                _abcdefgUser.Id);

            ExerciseInstanceDto firstPutResult = _repository.GetById(_abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id);
            ExerciseInstanceDto secondPutResult = _repository.GetById(_abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.Id);

            // Assert
            Assert.That(firstPutResult.Equals(firstExpected), Is.True);
            Assert.That(secondPutResult.Equals(secondExpected), Is.True);
        }

        [Test]
        public void SwapPositions_ADbSetIsNull_ThrowMissingMemberException()
        {
            // Arrange
            _context.ExerciseInstances = null!;

            // Act and Assert
            Assert.Throws<MissingMemberException>(() =>
            _repository.SwapPositions(
                _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                _abcdefgUser.Id));
        }

        [Test]
        public void SwapPositions_FirstIdDoesNotCorrespondToAnExerciseInstance_ThrowKeyNotFoundException()
        {
            // Arrange
            // Nothing needed

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() =>
            _repository.SwapPositions(
                0,
                _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                _abcdefgUser.Id));
        }

        [Test]
        public void SwapPositions_SecondIdDoesNotCorrespondToAnExerciseInstance_ThrowKeyNotFoundException()
        {
            // Arrange
            // Nothing needed

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() =>
            _repository.SwapPositions(
                _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                14,
                _abcdefgUser.Id));
        }

        [Test]
        public void SwapPositions_TwoExercisesDoNotBelongToSameWorkout_ThrowArgumentException()
        {
            // Arrange

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
            _repository.SwapPositions(
                _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                _abcdefgIncompleteProgramFirstWorkoutSecondExerciseInstance.Id,
                _abcdefgUser.Id));
        }

        [Test]
        public void SwapPositions_TwoExercisesDoNotBelongToSameUser_ThrowArgumentException()
        {
            // Arrange

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
            _repository.SwapPositions(
                _abcdefgIncompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                _qwertyIncompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                _abcdefgUser.Id));
        }

        [Test]
        public void SwapPositions_BothExerciseInstancesBelongToOtherUser_ThrowNotSupportedException()
        {
            // Arrange

            // Act and Assert
            Assert.Throws<NotSupportedException>(() =>
            _repository.SwapPositions(
                _abcdefgCompleteProgramSecondWorkoutFirstExerciseInstance.Id,
                _abcdefgCompleteProgramSecondWorkoutSecondExerciseInstance.Id,
                _qwertyUser.Id));

            // if one of these belongs to qwertyUser, then this will be caught
            // by the workout IDs differing and throw an ArgumentException instead
        }

        [Test]
        public void ToggleCompletion_ExerciseInstanceIsIncomplete_MakesItComplete()
        {
            // Arrange
            ExerciseInstanceDto expected = _qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance.ToDto();
            expected.IsComplete = true;

            // Act
            _repository.ToggleCompletion(
                _qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance.Id,
                _qwertyUser.Id
            );

            ExerciseInstanceDto resultOfPut = _repository.GetById(_qwertyIncompleteProgramSecondWorkoutThirdExerciseInstance.Id);

            // Assert
            Assert.That(resultOfPut.Equals(expected), Is.True);
        }

        [Test]
        public void ToggleCompletion_ExerciseInstanceIsComplete_MakesItIncomplete()
        {
            // Arrange
            ExerciseInstanceDto expected = _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.ToDto();
            expected.IsComplete = false;

            // Act
            _repository.ToggleCompletion(
                _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                _abcdefgUser.Id
            );

            ExerciseInstanceDto resultOfPut = _repository.GetById(_abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id);

            // Assert
            Assert.That(resultOfPut.Equals(expected), Is.True);
        }

        [Test]
        public void ToggleCompletion_ADbSetIsNull_ThrowMissingMemberException()
        {
            // Arrange
            _context.ExerciseInstances = null!;

            // Act and Assert
            Assert.Throws<MissingMemberException>(() =>
                _repository.ToggleCompletion(
                    _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                    _abcdefgUser.Id
                )
            );
        }

        [Test]
        public void ToggleCompletion_ExerciseInstanceSpecifiedDoesNotExist_ThrowKeyNotFoundException()
        {
            // Arrange
            // Nothing needed

            // Act and Assert
            Assert.Throws<KeyNotFoundException>( () =>
                _repository.ToggleCompletion(
                    14,
                    _abcdefgUser.Id
                )
            );
        }

        [Test]
        public void ToggleCompletion_ExerciseInstanceBelongsToOtherUser_ThrowNotSupportedException()
        {
            // Arrange
            // Nothing needed

            // Act and Assert
            Assert.Throws<NotSupportedException>(() =>
                _repository.ToggleCompletion(
                    _abcdefgCompleteProgramFirstWorkoutFirstExerciseInstance.Id,
                    _qwertyUser.Id
                )
            );
        }

    }
}
