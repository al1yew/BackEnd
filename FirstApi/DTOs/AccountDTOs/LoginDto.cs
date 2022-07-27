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
    public class LoginDto
    {
        public string EmailUsername { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.EmailUsername).MaximumLength(255).WithMessage("Written must be max 255 characters!")
                .NotEmpty().WithMessage("Must be filled!");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");
        }
    }
}
