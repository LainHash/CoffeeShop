using FluentValidation;
using WebAPI.DTOs.Products.Create;

namespace WebAPI.Middlewares.Products
{
    public class CreateValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
                .MaximumLength(100).WithMessage("Tên sản phẩm tối đa 100 ký tự");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId không hợp lệ");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0")
                .LessThanOrEqualTo(100_000_000).WithMessage("Giá không hợp lệ");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("URL ảnh tối đa 500 ký tự")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.ImageUrl))
                .WithMessage("ImageUrl không đúng định dạng URL");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Mô tả tối đa 1000 ký tự");
        }
    }
}
