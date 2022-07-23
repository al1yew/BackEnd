using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DTOs.BrandDTOs
{
    public class BrandPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandPutDtoValidator : AbstractValidator<BrandPutDto>
    {
        public BrandPutDtoValidator()
        {
            //brandpostdto-nu da goturub ishlede bilerik, prosto dla kajdogo metoda esli est svoy DTO i validator, nam leqche i chetche

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is not found");

            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Name must be at most 255 characters!")
                .NotEmpty().WithMessage("Name is required!");
        }
    }
}
