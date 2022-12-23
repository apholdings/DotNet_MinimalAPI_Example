using DotNet_MinimalAPI_Example.Models.DTO;
using FluentValidation;

namespace DotNet_MinimalAPI_Example.Validations
{
    public class ProductUpdateValidation : AbstractValidator<ProductUpdateDTO>
    {
        public ProductUpdateValidation() 
        {
            RuleFor(p => p.ProductId).NotEmpty().GreaterThan(0);
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Price).InclusiveBetween(1,10000);
        }
    }
}
