using AutoMapper;
using ProductService.Contracts.DTOs;
using ProductService.DAL.Entities;

namespace ProductService.BLL
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();

            CreateMap<CreateProductDTO, Product>();

            CreateMap<UpdateProductDTO, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); ;
        }

    }
}
