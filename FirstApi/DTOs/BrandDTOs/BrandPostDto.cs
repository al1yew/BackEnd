using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DTOs.BrandDTOs
{
    public class BrandPostDto
    {
        public string Name { get; set; }
    }

    public class BrandPostDtoValidator : AbstractValidator<BrandPostDto>
    {
        public BrandPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Name must be at most 255 characters!")
                .NotEmpty().WithMessage("Name is required!");
        }
    }
}
