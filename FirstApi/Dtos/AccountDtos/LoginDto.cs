using FluentValidation;
using System.Linq;

namespace FirstApi.Dtos.AccountDtos
{
    public class LoginDto
    {
        public string Email { get; set; }   
        public string Password { get; set; }
    }
    public class LoginDtoValidator : AbstractValidator<LoginDto> 
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("can not be empty")
                .EmailAddress();
            RuleFor(x => x).Custom((x, context) =>
            {
                bool check = x.Password.Any(p => char.IsDigit(p))
                && x.Password.Any(p => char.IsLower(p))
                && x.Password.Any(p => char.IsUpper(p))
                && x.Password.Any(p => char.IsLetter(p))
                && x.Password.Length >= 8;

                if (!check)
                {
                    context.AddFailure("Password","Password wrong");
                }
            });
        }
    }
}
