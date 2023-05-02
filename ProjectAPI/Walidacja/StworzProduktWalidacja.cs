using FluentValidation;
using FluentValidation.Validators;
using ProjectAPI.Model.DTO;

namespace ProjectAPI.Walidacja
{
    /// <summary>
    /// Kala impelentująca walidacje wprowadancych danych podczas tworzenia produktu
    /// </summary>
    public class StworzProduktWalidacja : AbstractValidator<StworzProduktDTO>
    {
        public StworzProduktWalidacja()
        {
            RuleFor(model => model.Nazwa).NotEmpty();
            RuleFor(model => model.Cena).GreaterThan(0);
            RuleFor(model => model.Ilosc).GreaterThanOrEqualTo(0);
            RuleFor(model => model.Dostepny).NotEqual(true).When(model => model.Ilosc.Equals(0)).Equal(false).When(model => model.Ilosc.Equals(0));
        }
    }
}
