using FluentValidation;
using ProjectAPI.Model.DTO;

namespace ProjectAPI.Walidacja
{
    public class AktualizujProduktWalidacja : AbstractValidator<AktualizujProduktDTO>
    {
        public AktualizujProduktWalidacja()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
            RuleFor(model => model.Nazwa).NotEmpty();
            RuleFor(model => model.Cena).GreaterThan(0);
            RuleFor(model => model.Ilosc).GreaterThanOrEqualTo(0);
            RuleFor(model => model.Dostepny).NotEqual(true).When(model => model.Ilosc.Equals(0)).Equal(false).When(model => model.Ilosc.Equals(0));
        }
    }
}
