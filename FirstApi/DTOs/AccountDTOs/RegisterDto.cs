using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DTOs.AccountDTOs
{
    /// <summary>
    /// to register
    /// </summary>
    public class RegisterDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email).EmailAddress().MaximumLength(255).WithMessage("Email must be max 255 characters!")
                .NotEmpty().WithMessage("Must be filled!");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.SurName).NotEmpty().WithMessage("Surname is required.");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name is required.");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");
        }
    }
}
