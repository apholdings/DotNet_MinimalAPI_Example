using DotNet_MinimalAPI_Example.Models.DTO;
using FluentValidation;

namespace DotNet_MinimalAPI_Example.Validations
{
    public class ProductCreateValidation : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateValidation() 
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Price).InclusiveBetween(1,10000);
        }
    }
}
