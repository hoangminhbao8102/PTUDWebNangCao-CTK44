using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class TagValidator : AbstractValidator<TagEditModel>
    {
        public TagValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên thẻ không được để trống")
                .MaximumLength(100);

            RuleFor(x => x.UrlSlug)
                .NotEmpty()
                .WithMessage("Slug không được để trống")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
