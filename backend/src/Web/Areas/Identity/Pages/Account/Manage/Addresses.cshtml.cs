using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Services.DataServices.Interfaces;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    public class AddressesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public UserAddress Address { get; set; }
        public IEnumerable<UserAddress> Addresses { get; set; }
        private readonly IUserAddressService _service;
        private readonly IUnitOfWork _unitOfWork;

        public AddressesModel(IUserAddressService service, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
            Addresses = _unitOfWork.UserAddress.GetAllAsync().Result;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Data not Valid";
            }
            else
            {
                await _unitOfWork.UserAddress.AddAsync(Address);
                await _unitOfWork.SaveAsync();
            }
            return Page();
        }
    }
}
