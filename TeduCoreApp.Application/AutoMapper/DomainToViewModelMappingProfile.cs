using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;

namespace TeduCoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}
