using EntityLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class ProductValidation : AbstractValidator<Products>
    {
        public ProductValidation()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün Adı Boş Bırakılamaz");
            RuleFor(x => x.UpdatedUser).NotEmpty().WithMessage("Güncelleyen kullanıcının kullanıcı adını giriniz");
            RuleFor(x => x.CreatedUser).NotEmpty().WithMessage("Oluşturan kullanıcının kullanıcı adını giriniz");
        }
    }
}
