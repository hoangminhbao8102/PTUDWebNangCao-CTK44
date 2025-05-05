using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class SubscriberValidator : AbstractValidator<SubscriberEditModel>
    {
        public SubscriberValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");

            RuleFor(x => x.Voluntary)
                .NotNull().WithMessage("Voluntary flag is required.");
        }
    }
}
