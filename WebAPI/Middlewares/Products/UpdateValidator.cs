using FluentValidation;
using WebAPI.DTOs.Products.Update;

namespace WebAPI.Middlewares.Products
{
    public class UpdateValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateValidator()
        {
            RuleFor(x => x.ProductName)
            .MaximumLength(100)
            .WithMessage("Tên sản phẩm tối đa 100 ký tự")
            .When(x => x.ProductName is not null);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId không hợp lệ")
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Giá phải lớn hơn 0")
                .LessThanOrEqualTo(100_000_000)
                .WithMessage("Giá không hợp lệ")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .WithMessage("URL ảnh tối đa 500 ký tự")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("URL ảnh không hợp lệ")
                .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Mô tả tối đa 1000 ký tự")
                .When(x => x.Description is not null);

        }
    }
}
