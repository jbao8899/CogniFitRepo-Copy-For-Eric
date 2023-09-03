// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CogniFitRepo.Server.Data;
using CogniFitRepo.Server.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CogniFitRepo.Server.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; } = string.Empty;

            [Display(Name = "IsFemale")]
            public bool IsFemale { get; set; } = false;

            [Display(Name = "Prefers Metric")]
            public bool PrefersMetric { get; set; } = true;

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Birth Day")]
            public DateTime? BirthDay { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Street Number")]
            public int? StreetNumber { get; set; }

            [Display(Name = "Street Name")]
            public string StreetName { get; set; }

            [Display(Name = "ApartmentNumber")]
            public string ApartmentNumber { get; set; }

            [Display(Name = "CityName")]
            public string CityName { get; set; }

            [Display(Name = "SubdivisionName")]
            public string SubdivisionName { get; set; }

            [Display(Name = "CountryName")]
            public string CountryName { get; set; }

            [DataType(DataType.PostalCode)]
            [Display(Name = "PostalCode")]
            public int? PostalCode { get; set; }

            [Display(Name = "Profile Description")]
            public string ProfileDescription { get; set; }

            [Display(Name = "Profile Picture")]
            public string PortraitUrl { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            string tempporturl;
            if (user.PortraitId == null)
            {
                tempporturl = "";
            }
            else
            {
                tempporturl = _context.Portraits.Find(user.PortraitId).Path;
            }

            Input = new InputModel
            {
                FirstName = user.FirstName??"",
                LastName = user.LastName,
                IsFemale = user.IsFemale??false,
                PrefersMetric = user.PrefersMetric??true,
                BirthDay = user.Birthday??DateTime.Today,
                PhoneNumber = user.PhoneNumber,
                StreetNumber = user.StreetNumber,
                StreetName = user.StreetName,
                ApartmentNumber = user.ApartmentNumber,
                CityName = user.CityName,
                SubdivisionName = user.SubdivisionName,
                CountryName = user.CountryName,
                PostalCode = user.PostalCode,
                ProfileDescription = user.ProfileDescription,
                PortraitUrl = tempporturl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.IsFemale = Input.IsFemale;
            user.PrefersMetric = Input.PrefersMetric;
            user.Birthday = Input.BirthDay;
            user.PhoneNumber = Input.PhoneNumber;
            user.StreetNumber = Input.StreetNumber;
            user.StreetName = Input.StreetName;
            user.ApartmentNumber = Input.ApartmentNumber;
            user.CityName = Input.CityName;
            user.SubdivisionName = Input.SubdivisionName;
            user.CountryName = Input.CountryName;
            user.PostalCode = Input.PostalCode;
            user.ProfileDescription = Input.ProfileDescription;

            if(_context.Portraits.Any(p=>p.Path == Input.PortraitUrl))
            {
                user.Portrait = _context.Portraits.First(p => p.Path == Input.PortraitUrl);
            }
            else
            {
                user.Portrait = new Portrait { Path = Input.PortraitUrl };
                _context.Portraits.Add(user.Portrait);
                _context.SaveChanges();
            }

            await _userManager.UpdateAsync(user);

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
