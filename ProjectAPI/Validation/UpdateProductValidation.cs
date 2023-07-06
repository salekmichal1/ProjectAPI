using FluentValidation;
using ProjectAPI.Model.DTO;

namespace ProjectAPI.Walidacja
{
    /// <summary>
    /// Kala tworząca walidacje wprowadancych danych podczas aktualiacji produktu
    /// </summary>
    public class UpdateProductValidation : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductValidation()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Price).GreaterThan(0);
            RuleFor(model => model.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(model => model.Available).NotEqual(true).When(model => model.Quantity.Equals(0)).Equal(false).When(model => model.Quantity.Equals(0));
        }
    }
}
