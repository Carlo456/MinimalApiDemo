using FluentValidation;
using MinimalApiDemo.Models.DTO;

namespace MinimalApiDemo.Validations
{
    public class ProductUpdateValidation : AbstractValidator<ProductUpdateDTO>
    {
        public ProductUpdateValidation() 
        {
            RuleFor(p => p.Id).NotEmpty().GreaterThan(0);
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Price).GreaterThan(0).NotEmpty();
        }
    }
}
