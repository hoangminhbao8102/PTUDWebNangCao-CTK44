using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class CommentEditModelValidator : AbstractValidator<CommentEditModel>
    {
        public CommentEditModelValidator()
        {
            RuleFor(c => c.PostId)
                .GreaterThan(0).WithMessage("Phải chọn bài viết để bình luận.");

            RuleFor(c => c.AuthorName)
                .NotEmpty().WithMessage("Tên người bình luận không được để trống.");

            RuleFor(c => c.Email)
                .NotEmpty().EmailAddress().WithMessage("Email không hợp lệ.");

            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Nội dung bình luận không được để trống.");
        }
    }
}
