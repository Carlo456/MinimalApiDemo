using FluentValidation;
using MinimalApiDemo.Models.DTO;

namespace MinimalApiDemo.Validations
{
    public class ProductCreateValidation : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateValidation() 
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Price).NotEmpty().GreaterThan(0);
        }
    }
}
