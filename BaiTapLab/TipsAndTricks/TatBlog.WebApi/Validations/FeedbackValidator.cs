using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class FeedbackValidator : AbstractValidator<FeedbackEditModel>
    {
        public FeedbackValidator()
        {
            RuleFor(f => f.FullName)
                .NotEmpty().WithMessage("Họ tên không được để trống")
                .MaximumLength(100);

            RuleFor(f => f.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");

            RuleFor(f => f.Subject)
                .NotEmpty().WithMessage("Tiêu đề không được để trống");

            RuleFor(f => f.Message)
                .NotEmpty().WithMessage("Nội dung góp ý không được để trống");
        }
    }
}
