using FluentValidation;
using System.Linq;

namespace FirstApi.Dtos.AccountDtos
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CheckPassword { get; set; }
    }
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Can not be empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Can not be empty")
                .EmailAddress().WithMessage("only mail address");

            RuleFor(x => x).Custom((x, context) =>
            {
                bool check = x.Password.Any(p => char.IsDigit(p))
                && x.Password.Any(p => char.IsLower(p)) 
                && x.Password.Any(p=>char.IsUpper(p))
                && x.Password.Any(p=>char.IsLetter(p))
                && x.Password.Length>=8;

                if (!check)
                {
                    context.AddFailure("Password",
                        "Password must be at least " +
                        "8 characters," +
                        "1 uppercase," +
                        "1 lowercase," +
                        "1 numeric," +
                        " and 1 special characters. Example: Tural12@");
                }
            });

            RuleFor(x => x.CheckPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");

        }
    }
}
