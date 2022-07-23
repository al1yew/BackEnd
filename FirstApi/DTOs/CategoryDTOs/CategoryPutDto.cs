using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.DTOs.CategoryDTOs
{
    public class CategoryPutDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsMain { get; set; }
        public Nullable<int> ParentId { get; set; }
    }

    public class CategoryPutDtoValidator : AbstractValidator<CategoryPutDto>
    {
        public CategoryPutDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is not found");

            RuleFor(x => x.Name).MaximumLength(255).WithMessage("Name must be max 255 characters!")
                .NotEmpty().WithMessage("Must be filled!");

            RuleFor(x => x.Image).MaximumLength(500).WithMessage("Image must be max 500 characters!");


            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.IsMain)
                {
                    if (x.Image == null)
                    {
                        context.AddFailure("Image is required for main category!");
                    }
                }
                else
                {
                    if (x.ParentId == null)
                    {
                        context.AddFailure("Main category is required");
                    }

                    if (x.Id == x.ParentId)
                    {
                        context.AddFailure("id is same as id of main category");
                    }
                }
            });
        }
    }
}
