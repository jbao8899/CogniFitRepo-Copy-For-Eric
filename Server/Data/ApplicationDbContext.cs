﻿using CogniFitRepo.Server.Models;
using CogniFitRepo.Server.Models.ExerciseModels;
using CogniFitRepo.Server.Models.UserModels;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace CogniFitRepo.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
             * Setting up PrimaryMuscle
             */
            modelBuilder.Entity<Exercise_Muscle>()
                //.HasNoKey();
                .HasKey(o => new { o.MuscleId, o.ExerciseId, o.IsPrimary });

            // Making the connector table ExerciseInstance_ExerciseProperty use
            // a compound key
            modelBuilder.Entity<ExerciseInstance_ExerciseProperty>()
                .HasKey(o => new { o.ExerciseInstanceId, o.ExercisePropertyId });

            // Cascade Deletes???
            modelBuilder.Entity<ExerciseInstance>()
                .HasMany(e => e.ExerciseInstance_ExerciseProperties)
                .WithOne(e => e.ExerciseInstance)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Workout>()
                .HasMany(e => e.ExerciseInstances)
                .WithOne(e => e.Workout)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<WorkoutProgram>()
                .HasMany(e => e.Workouts)
                .WithOne(e => e.WorkoutProgram)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<ExerciseProperty>().HasData(new ExerciseProperty
            {
                Id = 1,
                Name = "Reps",
            });
            modelBuilder.Entity<ExerciseProperty>().HasData(new ExerciseProperty
            {
                Id = 2,
                Name = "Sets",
            });
            modelBuilder.Entity<ExerciseProperty>().HasData(new ExerciseProperty
            {
                Id = 3,
                Name = "Weight (kg)",
            });
            modelBuilder.Entity<ExerciseProperty>().HasData(new ExerciseProperty
            {
                Id = 4,
                Name = "Time (s)",
            });
            modelBuilder.Entity<ExerciseProperty>().HasData(new ExerciseProperty
            {
                Id = 5,
                Name = "Distance (m)",
            });
            modelBuilder.Entity<ExerciseProperty>().HasData(new ExerciseProperty
            {
                Id = 6,
                Name = "Speed (kph)",
            });

            // Add admin role and user: https://stackoverflow.com/a/59350189 
            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string ROLE_ID = "ad376a8f-9eab-4bb9-9fca-30b01540f445";

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true, // was false by default, does not work
                PasswordHash = hasher.HashPassword(null, "Admin123#"),
                SecurityStamp = Guid.NewGuid().ToString(), // Not string.Empty???
                LockoutEnabled = true // This was not specified in the source, and must be set to true, or you won't be allowed to log in
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

        }
        /*
         * Come back to verify that none of these are null or can be null.
         * The ?'s are only hiding the warning.
         */
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<Exercise_Muscle> Exercise_Muscles { get; set; }
        public DbSet<ExerciseLevel> ExerciseLevels { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutProgram> WorkoutPrograms { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Force> Forces { get; set; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; set; }
        public DbSet<ExerciseImage> ExerciseImages { get; set; }
        public DbSet<ExerciseInstance> ExerciseInstances { get; set; }
        public DbSet<ExerciseInstance_ExerciseProperty> ExerciseInstance_ExerciseProperties { get; set; }
        public DbSet<ExerciseProperty> ExerciseProperties { get; set; }
        public DbSet<BiometricInformationSnapshot> BiometricInformationSnapshots { get; set; }
        public DbSet<Portrait> Portraits { get; set; }
    }
}