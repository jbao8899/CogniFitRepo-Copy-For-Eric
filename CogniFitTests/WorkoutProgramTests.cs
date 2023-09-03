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
    public class WorkoutProgramTests
    {
        //private readonly Mock<IWorkoutProgramRepository> _mockRepository = new Mock<IWorkoutProgramRepository>();
        //// https://stackoverflow.com/a/70467204
        //private readonly Mock<UserManager<ApplicationUser>> _mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        private ApplicationDbContext _context = null!; // will be set by SetUp()
        private IWorkoutProgramRepository _repository = null!; // will be set by SetUp()

        // store stuff here so it can be accessed in the tests:
        // It is set in StartUp()
        private Portrait _defaultPortrait = null!;
        
        private ApplicationUser _abcdefgUser = null!;
        private ApplicationUser _qwertyUser = null!;

        private ExerciseLevel _beginnerLevel = null!;
        private ExerciseLevel _intermediateLevel = null!;

        private ExerciseCategory _strengthCategory = null!;
        private ExerciseCategory _cardioCategory  = null!;

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

        // To get distinct values from ExerciseInstanceTests and ExerciseInstance_ExercisePropertyTests
        private int _numTestsRun = 100; 

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

            _repository = new WorkoutProgramRepository(_context);

            _numTestsRun++; // to ensure each a new in-memory database is created each time

            // Works
            //ExerciseRepository repo = new ExerciseRepository(_context);
            //IEnumerable<ExerciseDto> bodyOnlyExercises = repo.GetExercisesByMultipleAttributes("", "", "body only", "", "", "", "");
            //Console.WriteLine(bodyOnlyExercises.Count());
            //foreach (ExerciseDto exerciseDto in bodyOnlyExercises) // works!!!
            //{
            //    Console.WriteLine(exerciseDto.Name);
            //    Console.WriteLine(exerciseDto.Equipment); // not null????
            //}
        }

        // Works only when run independently -> sometimes? Says object (Portait) with same ID was already inserted???
        //[Test]
        //public void Test_NavigationPropertiesSet()
        //{
        //    ExerciseRepository repo = new ExerciseRepository(_context);
        //    IEnumerable<ExerciseDto> bodyOnlyExercises = repo.GetExercisesByMultipleAttributes("", "", "body only", "", "", "", "");
        //    Assert.That(bodyOnlyExercises.Count(), Is.EqualTo(2));
        //}

        //        public async Task GetWorkoutPrograms_HaveWorkoutPrograms_ReturnsWorkoutPrograms()
        [Test]
        public void GetWorkoutPrograms_HaveWorkoutPrograms_ReturnsWorkoutPrograms()
        {
            // Assemble:
            WorkoutProgramDto firstExpectedWorkoutProgram = _completeProgramForAbcdefg.ToDto();
            WorkoutProgramDto secondExpectedWorkoutProgram = _incompleteProgramForAbcdefg.ToDto();
            WorkoutProgramDto thirdExpectedWorkoutProgram = _incompleteProgramForQwerty.ToDto();

            // Act:
            IEnumerable<WorkoutProgramDto> workoutPrograms = _repository.GetWorkoutPrograms();

            // Assert:
            Assert.That(workoutPrograms.Count(), Is.EqualTo(3));

            // Need the OrderBy, as the order of the returns is apparently not guaranteed?
            List<WorkoutProgramDto> workoutProgramList = workoutPrograms.OrderBy(wp => wp.Id).ToList();

            // Cannot use Is.EqualTo, because that does not use the .Equals() methods I made.
            Assert.That(workoutProgramList[0].Equals(firstExpectedWorkoutProgram), Is.True);
            Assert.That(workoutProgramList[1].Equals(secondExpectedWorkoutProgram), Is.True);
            Assert.That(workoutProgramList[2].Equals(thirdExpectedWorkoutProgram), Is.True);

            //Console.WriteLine(workoutProgramList[0] == firstExpectedWorkoutProgram);
            //Console.WriteLine(workoutProgramList[0].Equals(firstExpectedWorkoutProgram));

            // The below stuff tests the endpoint with a mocked
            // repository
            // We don't use it any more, because the endpoint has little logic
            {
                //// Assemble
                //WorkoutProgramDto completeProgramForAbcdefg = new WorkoutProgramDto()
                //{
                //    Id = 1,
                //    UserId = "abcdefghijklmnopqrstuvwxyz",
                //    Name = "Complete program for abcdefg",
                //    StartDate = new DateTime(2023, 8, 22),
                //    EndDate = new DateTime(2023, 10, 22),
                //    Notes = "My first workout",
                //    IsComplete = true,
                //    WorkoutDtos = new List<WorkoutDto>()
                //    {
                //        new WorkoutDto()
                //        {
                //            Id = 1,
                //            WorkoutProgramId = 1,
                //            WorkoutDateTime = new DateTime(2023, 8, 24, 18, 0, 0),
                //            Notes = "Finish before dinner",
                //            IsComplete = true,
                //            ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                //            {
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 1,
                //                    ExerciseId = 370,
                //                    ExerciseName = "Jogging, Treadmill",
                //                    WorkoutId = 1,
                //                    WorkoutSequenceNumber = 1,
                //                    IsComplete = true,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 1,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 900f
                //                        },
                //                        new ExerciseInstance_ExercisePropertyDto() {
                //                            ExerciseInstanceId = 1,
                //                            ExercisePropertyId = 6,
                //                            Name = "Speed (kph)",
                //                            Amount = 8f
                //                        }
                //                    }
                //                },
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 2,
                //                    ExerciseId = 613,
                //                    ExerciseName = "Running, Treadmill",
                //                    WorkoutId = 1,
                //                    WorkoutSequenceNumber = 2,
                //                    IsComplete = true,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto { 
                //                            ExerciseInstanceId = 2,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 300f
                //                        },
                //                        new ExerciseInstance_ExercisePropertyDto { 
                //                            ExerciseInstanceId = 2,
                //                            ExercisePropertyId = 6,
                //                            Name = "Speed (kph)",
                //                            Amount = 12f
                //                        }
                //                    }
                //                }
                //            }
                //        },
                //        new WorkoutDto()
                //        {
                //            Id = 2,
                //            WorkoutProgramId = 1,
                //            WorkoutDateTime = new DateTime(2023, 8, 26, 15, 30, 0),
                //            IsComplete = true,
                //            ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                //            {
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 3,
                //                    ExerciseId = 370,
                //                    ExerciseName = "Jogging, Treadmill",
                //                    WorkoutSequenceNumber = 1,
                //                    IsComplete = true,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 3,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 900f
                //                        },
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 3,
                //                            ExercisePropertyId = 6,
                //                            Name = "Speed (kph)",
                //                            Amount = 8.5f
                //                        }
                //                    }
                //                },
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 4,
                //                    ExerciseId = 613,
                //                    ExerciseName = "Running, Treadmill",
                //                    WorkoutSequenceNumber = 2,
                //                    IsComplete = true,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 4,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 300f
                //                        },
                //                        new ExerciseInstance_ExercisePropertyDto() 
                //                        {
                //                            ExerciseInstanceId = 4,
                //                            ExercisePropertyId = 6,
                //                            Name = "Speed (kph)",
                //                            Amount = 13f
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //};
                //WorkoutProgramDto incompleteProgramForAbcdefg = new WorkoutProgramDto()
                //{
                //    Id = 2,
                //    UserId = "abcdefghijklmnopqrstuvwxyz",
                //    Name = "Incomplete program for abcdefg",
                //    StartDate = new DateTime(2024, 1, 1),
                //    EndDate = new DateTime(2024, 12, 31),
                //    Notes = "Future workout",
                //    IsComplete = false,
                //    WorkoutDtos = new List<WorkoutDto>()
                //    {
                //        new WorkoutDto()
                //        {
                //            Id = 3,
                //            WorkoutProgramId = 2,
                //            WorkoutDateTime = new DateTime(2024, 1, 1, 14, 15, 0),
                //            Notes = "New year workout",
                //            IsComplete = false,
                //            ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                //            {
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 5,
                //                    ExerciseId = 568,
                //                    ExerciseName = "Pushups",
                //                    WorkoutId = 3,
                //                    WorkoutSequenceNumber = 1,
                //                    IsComplete = false,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 5,
                //                            ExercisePropertyId = 1,
                //                            Name = "Reps",
                //                            Amount = 15f
                //                        }
                //                    }
                //                },
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 6,
                //                    ExerciseId = 539,
                //                    ExerciseName = "Plank",
                //                    WorkoutId = 3,
                //                    WorkoutSequenceNumber = 2,
                //                    IsComplete = false,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto {
                //                            ExerciseInstanceId = 6,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 60f
                //                        }
                //                    }
                //                }
                //            }
                //        },
                //        new WorkoutDto()
                //        {
                //            Id = 4,
                //            WorkoutProgramId = 2,
                //            WorkoutDateTime = new DateTime(2024, 2, 1, 20, 0, 0),
                //            IsComplete = false,
                //            ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                //            {
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 7,
                //                    ExerciseId = 568,
                //                    ExerciseName = "Pushups",
                //                    WorkoutId = 4,
                //                    WorkoutSequenceNumber = 1,
                //                    IsComplete = false,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 7,
                //                            ExercisePropertyId = 1,
                //                            Name = "Reps",
                //                            Amount = 18f
                //                        }
                //                    }
                //                },
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 8,
                //                    ExerciseId = 539,
                //                    ExerciseName = "Plank",
                //                    WorkoutId = 4,
                //                    WorkoutSequenceNumber = 2,
                //                    IsComplete = false,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto {
                //                            ExerciseInstanceId = 8,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 75f
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //};
                //WorkoutProgramDto incompleteProgramForQwerty = new WorkoutProgramDto()
                //{
                //    Id = 3,
                //    UserId = "qwertyuiopasdfghjklzxcvbnm",
                //    Name = "Incomplete program for qwerty",
                //    StartDate = new DateTime(2023, 8, 1),
                //    EndDate = new DateTime(2023, 9, 30),
                //    Notes = "QWERTY WORKOUT",
                //    IsComplete = false,
                //    WorkoutDtos = new List<WorkoutDto>()
                //    {
                //        new WorkoutDto()
                //        {
                //            Id = 5,
                //            WorkoutProgramId = 3,
                //            WorkoutDateTime = new DateTime(2023, 8, 15, 8, 15, 0),
                //            IsComplete = true,
                //            ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                //            {
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 9,
                //                    ExerciseId = 568,
                //                    ExerciseName = "Pushups",
                //                    WorkoutId = 5,
                //                    WorkoutSequenceNumber = 1,
                //                    IsComplete = true,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 9,
                //                            ExercisePropertyId = 1,
                //                            Name = "Reps",
                //                            Amount = 15f
                //                        }
                //                    }
                //                },
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 10,
                //                    ExerciseId = 539,
                //                    ExerciseName = "Plank",
                //                    WorkoutId = 5,
                //                    WorkoutSequenceNumber = 2,
                //                    IsComplete = true,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto {
                //                            ExerciseInstanceId = 10,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 60f
                //                        }
                //                    }
                //                }
                //            }
                //        },
                //        new WorkoutDto()
                //        {
                //            Id = 6,
                //            WorkoutProgramId = 3,
                //            WorkoutDateTime = new DateTime(2023, 9, 15, 13, 50, 0),
                //            IsComplete = false,
                //            ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                //            {
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 11,
                //                    ExerciseId = 370,
                //                    ExerciseName = "Jogging, Treadmill",
                //                    WorkoutSequenceNumber = 1,
                //                    IsComplete = false,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 11,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 900f
                //                        },
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 11,
                //                            ExercisePropertyId = 6,
                //                            Name = "Speed (kph)",
                //                            Amount = 8.5f
                //                        }
                //                    }
                //                },
                //                new ExerciseInstanceDto()
                //                {
                //                    Id = 12,
                //                    ExerciseId = 613,
                //                    ExerciseName = "Running, Treadmill",
                //                    WorkoutSequenceNumber = 2,
                //                    IsComplete = false,
                //                    ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                //                    {
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 12,
                //                            ExercisePropertyId = 4,
                //                            Name = "Time (s)",
                //                            Amount = 300f
                //                        },
                //                        new ExerciseInstance_ExercisePropertyDto()
                //                        {
                //                            ExerciseInstanceId = 12,
                //                            ExercisePropertyId = 6,
                //                            Name = "Speed (kph)",
                //                            Amount = 13f
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //};

                //IEnumerable<WorkoutProgramDto> expectedResults = new List<WorkoutProgramDto>()
                //{
                //    completeProgramForAbcdefg,
                //    incompleteProgramForAbcdefg,
                //    incompleteProgramForQwerty
                //};

                //_mockRepository.Setup(x => x.GetWorkoutPrograms()).Returns(expectedResults);

                //WorkoutProgramController controller = new WorkoutProgramController(
                //    _mockUserManager.Object,
                //    _mockRepository.Object
                //);

                //// Act
                //ActionResult<IEnumerable<WorkoutProgramDto>> response = await controller.GetWorkoutPrograms();

                //// Assert 
                ////Console.WriteLine("GOT HERE");
                //Assert.That(response.Result, Is.TypeOf(typeof(OkObjectResult)));
                //OkObjectResult result = (OkObjectResult)response.Result!;
                //Assert.That(result.StatusCode, Is.EqualTo(200));
                //Assert.That(result!.Value, Is.TypeOf(typeof(List<WorkoutProgramDto>)));
                //List<WorkoutProgramDto> resultList = (List<WorkoutProgramDto>)result.Value!;
                //Assert.That(resultList.Count, Is.EqualTo(3));

                //for (int i = 0; i < 3; i++)
                //{
                //    Assert.That(resultList[i].Id, Is.EqualTo((long)(i + 1)));
                //}
            }
        }

        [Test]
        public void GetWorkoutPrograms_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Assemble
            _context.ExerciseInstances = null!;

            // Act and Assert
            Assert.Throws<MissingMemberException>(() => _repository.GetWorkoutPrograms());
        }

        [Test]
        public void GetWorkoutProgram_InputValidId_ReturnsCorrectWorkoutProgram()
        {
            // Assemble
            WorkoutProgramDto secondExpectedWorkoutProgram = _incompleteProgramForAbcdefg.ToDto();

            // Act:
            WorkoutProgramDto workoutProgram = _repository.GetWorkoutProgram(2);

            // Assert:
            Assert.That(workoutProgram.Equals(secondExpectedWorkoutProgram), Is.True);
        }

        [Test]
        public void GetWorkoutProgram_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Assemble
            _context.ExerciseInstances = null!;

            // Act and Assert
            Assert.Throws<MissingMemberException>(() => _repository.GetWorkoutProgram(1));
        }

        [Test]
        public void GetWorkoutProgram_InputInvalidId_ThrowsKeyNotFoundException()
        {
            // Assemble
            // Nothing needed

            // Act and Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.GetWorkoutProgram(-1));
            Assert.Throws<KeyNotFoundException>(() => _repository.GetWorkoutProgram(0));
            Assert.Throws<KeyNotFoundException>(() => _repository.GetWorkoutProgram(4));
            Assert.Throws<KeyNotFoundException>(() => _repository.GetWorkoutProgram(100));
        }

        [Test]
        public void PutWorkoutProgram_InputADto_CorrectlyModifiesFields()
        {
            // Assemble
            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.StartDate = expectedResult.StartDate + TimeSpan.FromDays(1);
            expectedResult.Notes = "The quick brown fox jumped over the lazy dog";
            expectedResult.IsComplete = true;

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = _incompleteProgramForQwerty.Id,
                Name = _incompleteProgramForQwerty.Name,
                StartDate = expectedResult.StartDate,
                EndDate = _incompleteProgramForQwerty.EndDate,
                Notes = expectedResult.Notes,
                IsComplete = expectedResult.IsComplete,
            };

            // Act:
            _repository.PutWorkoutProgram(_incompleteProgramForQwerty.Id, input, _qwertyUser.Id);
            WorkoutProgramDto resultOfPut = _repository.GetWorkoutProgram(_incompleteProgramForQwerty.Id);

            // Assert:
            Assert.That(resultOfPut.Equals(expectedResult), Is.True);
        }

        [Test]
        public void PutWorkoutProgram_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Assemble
            _context.WorkoutPrograms = null!;

            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.Notes = "The quick brown fox jumped over the lazy dog";

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = _incompleteProgramForQwerty.Id,
                Name = _incompleteProgramForQwerty.Name,
                StartDate = _incompleteProgramForQwerty.StartDate,
                EndDate = _incompleteProgramForQwerty.EndDate,
                Notes = expectedResult.Notes,
                IsComplete = _incompleteProgramForQwerty.IsComplete,
            };

            // Act and Assert
            Assert.Throws<MissingMemberException>(() =>
                _repository.PutWorkoutProgram(_incompleteProgramForQwerty.Id, input, _qwertyUser.Id));
        }

        [Test]
        public void PutWorkoutProgram_InputIdDoesNotEqualInputDtoId_ThrowsArgumentException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.Notes = "The quick brown fox jumped over the lazy dog";

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = _incompleteProgramForQwerty.Id,
                Name = _incompleteProgramForQwerty.Name,
                StartDate = _incompleteProgramForQwerty.StartDate,
                EndDate = _incompleteProgramForQwerty.EndDate,
                Notes = expectedResult.Notes,
                IsComplete = _incompleteProgramForQwerty.IsComplete,
            };
            long incorrectId = 2;

            // Act and Assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PutWorkoutProgram(incorrectId, input, _qwertyUser.Id));
        }

        [Test]
        public void PutWorkoutProgram_StartDateAfterEndDate_ThrowsArgumentException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.StartDate = expectedResult.EndDate + TimeSpan.FromDays(1);

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = _incompleteProgramForQwerty.Id,
                Name = _incompleteProgramForQwerty.Name,
                StartDate = expectedResult.StartDate,
                EndDate = _incompleteProgramForQwerty.EndDate,
                Notes = _incompleteProgramForQwerty.Notes,
                IsComplete = _incompleteProgramForQwerty.IsComplete,
            };

            // Act and Assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PutWorkoutProgram(_incompleteProgramForQwerty.Id, input, _qwertyUser.Id));
        }

        [Test]
        public void PutWorkoutProgram_TryToUpdateNonexistantWorkoutProgram_ThrowsKeyNotFoundException()
        {
            long incorrectId = 4;
            // Assemble
            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.Id = incorrectId;
            expectedResult.StartDate = expectedResult.StartDate + TimeSpan.FromDays(1);
            expectedResult.Notes = "The quick brown fox jumped over the lazy dog";
            expectedResult.IsComplete = true;

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = incorrectId, // 4
                Name = _incompleteProgramForQwerty.Name,
                StartDate = expectedResult.StartDate,
                EndDate = _incompleteProgramForQwerty.EndDate,
                Notes = expectedResult.Notes,
                IsComplete = expectedResult.IsComplete,
            };

            // Act and Assert:
            Assert.Throws<KeyNotFoundException>(() =>
                _repository.PutWorkoutProgram(incorrectId, input, _qwertyUser.Id));
        }

        [Test]
        public void PutWorkoutProgram_TryToUpdateWorkoutProgramBelongingToOtherUser_ThrowsKeyNotFoundException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.Notes = "The quick brown fox jumped over the lazy dog";

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = _incompleteProgramForQwerty.Id, // 4
                Name = _incompleteProgramForQwerty.Name,
                StartDate = _incompleteProgramForQwerty.StartDate,
                EndDate = _incompleteProgramForQwerty.EndDate,
                Notes = expectedResult.Notes,
                IsComplete = _incompleteProgramForQwerty.IsComplete,
            };

            // Act and Assert:
            Assert.Throws<NotSupportedException>(() =>
                _repository.PutWorkoutProgram(_incompleteProgramForQwerty.Id, input, _abcdefgUser.Id));
        }

        [Test]
        public void PutWorkoutProgram_AWorkoutDateIsBeforeNewStart_ThrowsKeyNotFoundException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.StartDate = expectedResult.StartDate + TimeSpan.FromDays(15);

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = _incompleteProgramForQwerty.Id,
                Name = _incompleteProgramForQwerty.Name,
                StartDate = expectedResult.StartDate,
                EndDate = _incompleteProgramForQwerty.EndDate,
                Notes = _incompleteProgramForQwerty.Notes,
                IsComplete = _incompleteProgramForQwerty.IsComplete,
            };

            // Act and Assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PutWorkoutProgram(_incompleteProgramForQwerty.Id, input, _qwertyUser.Id));
        }

        [Test]
        public void PutWorkoutProgram_AWorkoutDateIsAfterNewEnd_ThrowsKeyNotFoundException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = _incompleteProgramForQwerty.ToDto();
            expectedResult.EndDate = expectedResult.EndDate - TimeSpan.FromDays(16);

            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Id = _incompleteProgramForQwerty.Id,
                Name = _incompleteProgramForQwerty.Name,
                StartDate = _incompleteProgramForQwerty.StartDate,
                EndDate = expectedResult.EndDate,
                Notes = _incompleteProgramForQwerty.Notes,
                IsComplete = _incompleteProgramForQwerty.IsComplete,
            };

            // Act and Assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PutWorkoutProgram(_incompleteProgramForQwerty.Id, input, _qwertyUser.Id));
        }

        // Created using CoPilot
        //Write a test case for PostWorkoutProgram() which inserts a new workout program that contains Workouts that contains ExerciseInstances which contain ExerciseInstance_ExerciseProperties and checks that the WorkoutProgramDto you get back equals the WorkoutProgramDto you put in
        [Test]
        public void PostWorkoutProgram_InputADto_CorrectlyInsertsWorkoutProgram()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2, // reversed order is fine
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1, // reversed order is fine
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 2, // reversed order is fine
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 1, // reversed order is fine
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act:
            _repository.PostWorkoutProgram(input, _qwertyUser.Id);
            WorkoutProgramDto resultOfPost = _repository.GetWorkoutProgram((long)expectedResult.Id);

            // Assert:
            Assert.That(resultOfPost.Equals(expectedResult), Is.True);
        }

        [Test]
        public void PostWorkoutProgram_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _context.Exercises = null!;

            // Act and assert:
            Assert.Throws<MissingMemberException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        [Test]
        public void PostWorkoutProgram_StartDateAfterEndDate_ThrowsArgumentException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 10, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 10, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act and assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        [Test]
        public void PostWorkoutProgram_AWorkoutIsBeforeStartDate_ThrowsArgumentException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 8, 31, 23, 59, 59),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 8, 31, 23, 59, 59),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act and assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        [Test]
        public void PostWorkoutProgram_AWorkoutIsAfterEndDate_ThrowsArgumentException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 10, 1, 0, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 10, 1, 0, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act and assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        [Test]
        public void PostWorkoutProgram_InputDtoContainsExerciseInstanceReferringToNonexistantExercise_ThrowsKeyNotFoundException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = 999, // does not exist
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = 999, // does not exist
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act and assert:
            Assert.Throws<KeyNotFoundException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        [Test]
        public void PostWorkoutProgram_InputDtoContainsReferenceToNonexistantExerciseProperty_ThrowsKeyNotFoundException()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = 10, // nonexistant property ID
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = 10, // nonexistant property ID
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act and assert:
            Assert.Throws<KeyNotFoundException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        [Test]
        public void PostWorkoutProgram_InputDtoContainsExerciseInstancesWhoseIndicesSkipAValue_CorrectlyInsertsWorkoutProgram()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 3, // skipped 2
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 3, // Skipped 2
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act and assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        [Test]
        public void PostWorkoutProgram_InputDtoContainsExerciseInstancesWhoseIndicesDoNotStartAt1_CorrectlyInsertsWorkoutProgram()
        {
            // Assemble
            WorkoutProgramDto expectedResult = new WorkoutProgramDto()
            {
                Id = 4,
                UserId = _qwertyUser.Id,
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        Id = 7,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 14,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 14,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 15,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 7,
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 15,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        Id = 8,
                        WorkoutProgramId = 4,
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                Id = 16,
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 0, // should be 1
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 16,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                Id = 17,
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutId = 8,
                                WorkoutSequenceNumber = 1, // should be 2
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExerciseInstanceId = 17,
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // same as expectedResult , but without IDs to other things being inserted.
            WorkoutProgramDto input = new WorkoutProgramDto()
            {
                Name = "New Workout Program",
                StartDate = new DateTime(2023, 9, 1),
                EndDate = new DateTime(2023, 9, 30),
                Notes = "Just put this in for testing",
                IsComplete = false,
                WorkoutDtos = new List<WorkoutDto>()
                {
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 5, 20, 30, 0),
                        Notes = "Some notes",
                        IsComplete = true,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 1,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 2,
                                IsComplete = true,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    },
                    new WorkoutDto()
                    {
                        WorkoutDateTime = new DateTime(2023, 9, 25, 6, 0, 0),
                        IsComplete = false,
                        ExerciseInstanceDtos = new List<ExerciseInstanceDto>()
                        {
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _joggingTreadmillExercise.Id,
                                ExerciseName = "Jogging, Treadmill",
                                WorkoutSequenceNumber = 0, // should be 1
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 10500f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 7f
                                    }
                                }
                            },
                            new ExerciseInstanceDto()
                            {
                                ExerciseId = _runningTreadmillExercise.Id,
                                ExerciseName = "Running, Treadmill",
                                WorkoutSequenceNumber = 1, // should be 2
                                IsComplete = false,
                                ExerciseInstance_ExercisePropertyDtos = new List<ExerciseInstance_ExercisePropertyDto>()
                                {
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _timeProperty.Id,
                                        Name = "Time (s)",
                                        Amount = 100f
                                    },
                                    new ExerciseInstance_ExercisePropertyDto()
                                    {
                                        ExercisePropertyId = _speedProperty.Id,
                                        Name = "Speed (kph)",
                                        Amount = 15f
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act and assert:
            Assert.Throws<ArgumentException>(() =>
                _repository.PostWorkoutProgram(input, _qwertyUser.Id));
        }

        // Creating using CoPilot
        // Write a test case for DeleteWorkoutProgram() which deletes the WorkoutProgram with an ID of _incompleteProgramForQwerty.Id and checks to see that the results of GetWorkoutPrograms() are _completeProgramForAbcdefg.ToDto() and _incompleteProgramForAbcdefg.ToDto()
        [Test]
        public void DeleteWorkoutProgram_InputAUserId_ThatProgramIsDeletedAndRestAreUntouched()
        {
            // Arrange:
            WorkoutProgramDto expectedResult1 = _completeProgramForAbcdefg.ToDto();
            WorkoutProgramDto expectedResult2 = _incompleteProgramForAbcdefg.ToDto();

            // Act:
            _repository.DeleteWorkoutProgram(_incompleteProgramForQwerty.Id, _qwertyUser.Id);
            List<WorkoutProgramDto> resultOfGet = _repository.GetWorkoutPrograms().ToList();

            // Assert:
            Assert.That(resultOfGet.Count(), Is.EqualTo(2));
            Assert.That(resultOfGet[0].Equals(expectedResult1), Is.True);
            Assert.That(resultOfGet[1].Equals(expectedResult2), Is.True);
        }

        [Test]
        public void DeleteWorkoutProgram_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange:
            _context.WorkoutPrograms = null!;

            // Act and assert:
            Assert.Throws<MissingMemberException>(() =>
                _repository.DeleteWorkoutProgram(_incompleteProgramForQwerty.Id, _qwertyUser.Id));
        }

        [Test]
        public void DeleteWorkoutProgram_InputIdOfProgramThatDoesNotExist_ThrowsMissingMemberException()
        {
            // Arrange:
            // None

            // Act and assert:
            Assert.Throws<KeyNotFoundException>(() =>
                _repository.DeleteWorkoutProgram(1000, _qwertyUser.Id));
        }

        [Test]
        public void DeleteWorkoutProgram_TryToDeleteWorkoutProgramOfOtherUser_ThrowsMissingMemberException()
        {
            // Arrange:
            // None

            // Act and assert:
            Assert.Throws<NotSupportedException>(() =>
                _repository.DeleteWorkoutProgram(_completeProgramForAbcdefg.Id, _qwertyUser.Id));
        }

        // Created using CoPilot
        // Write a test case checking that GetWorkoutProgramsForUser() returns _completeProgramForAbcdefg.ToDto() and
        // _incompleteProgramForAbcdefg.ToDto() when called with the ID of _abcdefgUser
        [Test]
        public void GetWorkoutProgramsForUser_InputAUserId_ReturnsDtosForProgramsOfThatUser()
        {
            // Arrange:
            WorkoutProgramDto expectedResult1 = _completeProgramForAbcdefg.ToDto();
            WorkoutProgramDto expectedResult2 = _incompleteProgramForAbcdefg.ToDto();

            // Act:
            List<WorkoutProgramDto> resultOfGet = _repository.GetWorkoutProgramsForUser(_abcdefgUser.Id).ToList();

            // Assert:
            Assert.That(resultOfGet.Count(), Is.EqualTo(2));
            Assert.That(resultOfGet[0].Equals(expectedResult1), Is.True);
            Assert.That(resultOfGet[1].Equals(expectedResult2), Is.True);
        }

        [Test]
        public void GetWorkoutProgramsForUser_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange:
            _context.ExerciseProperties = null!;

            // Act and assert:
            Assert.Throws<MissingMemberException>(() =>
                _repository.GetWorkoutProgramsForUser(_abcdefgUser.Id));
        }

        // Created using CoPilot
        // Write a test case checking that GetCompletedWorkoutProgramsForUser() returns a list containing
        // _completeProgramForAbcdefg.ToDto() when called with the ID of _abcdefgUser
        [Test]
        public void GetCompletedWorkoutProgramsForUser_InputAUserId_ReturnsDtosForCompletedProgramsOfThatUser()
        {
            // Arrange:
            WorkoutProgramDto expectedResult1 = _completeProgramForAbcdefg.ToDto();

            // Act:
            List<WorkoutProgramDto> resultOfGet = _repository.GetCompletedWorkoutProgramsForUser(_abcdefgUser.Id).ToList();

            // Assert:
            Assert.That(resultOfGet.Count(), Is.EqualTo(1));
            Assert.That(resultOfGet[0].Equals(expectedResult1), Is.True);
        }

        [Test]
        public void GetCompletedWorkoutProgramsForUser_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange:
            _context.ExerciseInstance_ExerciseProperties = null!;

            // Act and assert:
            Assert.Throws<MissingMemberException>(() =>
                _repository.GetCompletedWorkoutProgramsForUser(_abcdefgUser.Id));
        }

        // Created using CoPilot
        // Write a test case checking that GetIncompleteWorkoutProgramsForUser() returns a list containing
        // _incompleteProgramForAbcdefg.ToDto() when called with the ID of _abcdefgUser
        [Test]
        public void GetIncompleteWorkoutProgramsForUser_InputAUserId_ReturnsDtosForIncompleteProgramsOfThatUser()
        {
            // Arrange:
            WorkoutProgramDto expectedResult1 = _incompleteProgramForAbcdefg.ToDto();

            // Act:
            List<WorkoutProgramDto> resultOfGet = _repository.GetIncompleteWorkoutProgramsForUser(_abcdefgUser.Id).ToList();

            // Assert:
            Assert.That(resultOfGet.Count(), Is.EqualTo(1));
            Assert.That(resultOfGet[0].Equals(expectedResult1), Is.True);
        }

        [Test]
        public void GetIncompleteWorkoutProgramsForUser_ADbSetIsNull_ThrowsMissingMemberException()
        {
            // Arrange:
            _context.WorkoutPrograms = null!;

            // Act and assert:
            Assert.Throws<MissingMemberException>(() =>
                _repository.GetIncompleteWorkoutProgramsForUser(_abcdefgUser.Id));
        }
    }

}
