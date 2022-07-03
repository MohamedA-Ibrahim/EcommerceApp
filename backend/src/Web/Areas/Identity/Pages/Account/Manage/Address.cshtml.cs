using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Contracts.V1.Requests;
using Web.Services.DataServices.Interfaces;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    public class AddressModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public UserAddress Address { get; set; }
        public bool IsEdit { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserAddressService _userAddressService;

        public AddressModel(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IUserAddressService userAddressService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _userAddressService = userAddressService;
        }

        public async Task<IActionResult> OnGet()
        {
            Address = await _userAddressService.GetUserAddressAsync() ?? new UserAddress();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            StatusMessage = "";
            if (!ModelState.IsValid)
            {
                StatusMessage = "Not valid data";
                return Page();
            }
            Address = await _userAddressService.UpsertAsync(new UpsertUserAddressRequest()
            {
                City = Address.City,
                PhoneNumber = Address.PhoneNumber,
                RecieverName = Address.RecieverName,
                StreetAddress = Address.StreetAddress
            });
            TempData["success"] = "Address saved successfully";
            return Page();
        }
    }
}
