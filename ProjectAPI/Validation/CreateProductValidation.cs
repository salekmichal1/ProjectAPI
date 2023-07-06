using FluentValidation;
using FluentValidation.Validators;
using ProjectAPI.Model.DTO;

namespace ProjectAPI.Walidacja
{
    /// <summary>
    /// Klasa tworząca walidacje wprowadancych danych podczas tworzenia produktu
    /// </summary>
    public class CreateProductValidation : AbstractValidator<CreateProductDTO>
    {
        public CreateProductValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Price).GreaterThan(0);
            RuleFor(model => model.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(model => model.Available).NotEqual(true).When(model => model.Quantity.Equals(0)).Equal(false).When(model => model.Quantity.Equals(0));
        }
    }
}
