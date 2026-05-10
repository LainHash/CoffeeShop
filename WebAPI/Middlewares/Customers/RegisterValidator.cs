using FluentValidation;
using WebAPI.DTOs.Auths.Customers;

namespace WebAPI.Middlewares.Customers
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không đúng định dạng")
                .MaximumLength(100).WithMessage("Email tối đa 100 ký tự");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username không được để trống")
                .MinimumLength(3).WithMessage("Username tối thiểu 3 ký tự")
                .MaximumLength(50).WithMessage("Username tối đa 50 ký tự")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username chỉ được chứa chữ cái, số và dấu gạch dưới");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ tên không được để trống")
                .MaximumLength(100).WithMessage("Họ tên tối đa 100 ký tự");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Số điện thoại không được để trống")
                .Matches(@"^(0|\+84)[3-9]\d{8}$").WithMessage("Số điện thoại không hợp lệ");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu tối thiểu 6 ký tự")
                .MaximumLength(100).WithMessage("Mật khẩu tối đa 100 ký tự")
                .Matches(@"[A-Z]").WithMessage("Mật khẩu phải có ít nhất 1 chữ hoa")
                .Matches(@"[0-9]").WithMessage("Mật khẩu phải có ít nhất 1 chữ số");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Xác nhận mật khẩu không được để trống")
                .Equal(x => x.Password).WithMessage("Xác nhận mật khẩu không khớp");
        }
    }
}
