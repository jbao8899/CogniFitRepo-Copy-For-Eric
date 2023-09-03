using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.ExerciseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CogniFitRepo.Server.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ExercisePopulatorController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ExercisePopulatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ExercisePopulator")]
        public async Task<IActionResult> ExercisePopulator()
        {
            var json = System.IO.File.ReadAllText("Data/exercises.json");
            List<JObject> exerciseList = JsonConvert.DeserializeObject<List<JObject>>(json);
            foreach (JObject exercise in exerciseList)
            {
                //Create Category table
                string category = (string)exercise["category"];
                ExerciseCategory categoryEntity;
                if(_context.ExerciseCategories.Any(c => c.Name == category))
                {
                    categoryEntity = _context.ExerciseCategories.Where(c => c.Name == category).First();
                }
                else
                {
                    categoryEntity = new ExerciseCategory() { Name = category };
                    _context.Add(categoryEntity);
                    _context.SaveChanges();
                }

                //Create Exercise Level table
                string level = (string)exercise["level"];
                ExerciseLevel levelEntity;
                if (_context.ExerciseLevels.Any(l => l.Name == level))
                {
                    levelEntity = _context.ExerciseLevels.Where(l => l.Name == level).First();
                }
                else
                {
                    levelEntity = new ExerciseLevel() { Name = level };
                    _context.Add(levelEntity);
                    _context.SaveChanges();
                }

                //Create Exercise Force table
                string force = (string)exercise["force"]!=null ? (string)exercise["force"] : "None";
                Force forceEntity;
                if (_context.Forces.Any(f => f.Name == force))
                {
                    forceEntity = _context.Forces.Where(f => f.Name == force).First();
                }
                else
                {
                    forceEntity = new Force() { Name = force };
                    _context.Add(forceEntity);
                    _context.SaveChanges();
                }

                //Create Exercise Mechanic table
                string mechanic = (string)exercise["mechanic"] != null ? (string)exercise["mechanic"] : "None";
                Mechanic mechanicEntity;
                if (_context.Mechanics.Any(m => m.Name == mechanic))
                {
                    mechanicEntity = _context.Mechanics.Where(m => m.Name == mechanic).First();
                }
                else
                {
                    mechanicEntity = new Mechanic() { Name = mechanic };
                    _context.Add(mechanicEntity);
                    _context.SaveChanges();
                }

                //Create Exercise Equipment table
                string equipment = (string)exercise["equipment"] != null ? (string)exercise["equipment"] : "None";
                Equipment equipmentEntity;
                if (_context.Equipments.Any(e => e.Name == equipment))
                {
                    equipmentEntity = _context.Equipments.Where(e => e.Name == equipment).First();
                }
                else
                {
                    equipmentEntity = new Equipment() { Name = equipment };
                    _context.Add(equipmentEntity);
                    _context.SaveChanges();
                }

                //Create Exercise table
                string exerciseName = (string)exercise["name"];
                string instructions = "";
                foreach (string instruction in exercise["instructions"])
                {
                    if (instructions.Length == 0)
                        instructions = instruction;
                    else
                        instructions = $"{instructions} {instruction}";
                }
                Exercise exerciseEntity;
                if (_context.Exercises.Any(e => e.Name == exerciseName))
                {
                    exerciseEntity = _context.Exercises.Where(e => e.Name == exerciseName).First();
                }
                else
                {
                    exerciseEntity = new Exercise() { 
                        Name = exerciseName,
                        Instructions = instructions,
                        ExerciseCategory = categoryEntity,
                        Force = forceEntity,
                        Mechanic = mechanicEntity,
                        Equipment = equipmentEntity,
                        ExerciseLevel = levelEntity
                    };
                    _context.Add(exerciseEntity);
                    _context.SaveChanges();
                }

                //Create Muscle table primary and secondary
                Muscle tempMuscle;
                foreach(string muscle in exercise["primaryMuscles"])
                {
                    if (_context.Muscles.Any(m => m.Name == muscle))
                    {
                        tempMuscle = _context.Muscles.Where(m => m.Name == muscle).First();
                    }
                    else
                    {
                        tempMuscle = new Muscle() { Name = muscle };
                        _context.Add(tempMuscle);
                        _context.SaveChanges();
                    }

                    if (!_context.Exercise_Muscles.Any(em => em.Exercise == exerciseEntity && em.Muscle == tempMuscle))
                    {
                        _context.Add(new Exercise_Muscle
                        {
                            Exercise = exerciseEntity,
                            Muscle = tempMuscle,
                            IsPrimary = true
                        });
                        _context.SaveChanges();
                    }
                }
                foreach (string muscle in exercise["secondaryMuscles"])
                {
                    if (_context.Muscles.Any(m => m.Name == muscle))
                    {
                        tempMuscle = _context.Muscles.Where(m => m.Name == muscle).First();
                    }
                    else
                    {
                        tempMuscle = new Muscle() { Name = muscle };
                        _context.Add(tempMuscle);
                        _context.SaveChanges();
                    }

                    if (!_context.Exercise_Muscles.Any(em => em.Exercise == exerciseEntity && em.Muscle == tempMuscle))
                    {
                        _context.Add(new Exercise_Muscle
                        {
                            Exercise = exerciseEntity,
                            Muscle = tempMuscle,
                            IsPrimary = false
                        });
                        _context.SaveChanges();
                    }
                }

                //Create Exercise Images Table
                //Create Muscle table primary and secondary
                ExerciseImage tempImage;
                foreach (string image in exercise["images"])
                {
                    if (_context.ExerciseImages.Any(i => i.Path == image))
                    {
                        tempImage = _context.ExerciseImages.Where(i => i.Path == image).First();
                    }
                    else
                    {
                        tempImage = new ExerciseImage() { Path = image, Exercise = exerciseEntity };
                        _context.Add(tempImage);
                        _context.SaveChanges();
                    }
                }
            }
            return await Task.FromResult(Ok(exerciseList));
        }
    }
}
