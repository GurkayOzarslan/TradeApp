using FluentValidation;

namespace TradeAppApplication.Commands.User.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ad alanı boş bırakılamaz.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
        }
    }
}

