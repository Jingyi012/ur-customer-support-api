using Application.DTOs.Enquiry;
using Application.DTOs.News;
using Application.DTOs.Product;
using Application.DTOs.Project;
using Application.Features.Enquiry.Commands;
using Application.Features.Enquiry.Queries;
using Application.Features.News.Commands;
using Application.Features.News.Queries;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.Projects.Commands;
using Application.Features.Projects.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Settings;
using System.Linq;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile(string prefix)
        {
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(img => prefix + img.ImageUrl).ToList()));

            CreateMap<CreateProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();

            CreateMap<ProductCategory, ProductCategoryResponseDto>();

            CreateMap<Project, ProjectResponseDto>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(img => prefix + img.ImageUrl).ToList()));
            CreateMap<CreateProjectCommand, Project>();
            CreateMap<UpdateProjectCommand, Project>();
            CreateMap<GetAllProjectsQuery, GetAllProjectsParameter>();

            CreateMap<News, NewsResponseDto>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(img => prefix + img.ImageUrl).ToList()));
            CreateMap<CreateNewsCommand, News>();
            CreateMap<UpdateNewsCommand, News>();
            CreateMap<GetAllNewsQuery, GetAllNewsParameter>();

            CreateMap<Enquiry, EnquiryResponseDto>();
            CreateMap<CreateEnquiryCommand,  Enquiry>();
            CreateMap<UpdateEnquiryCommand, Enquiry>();
            CreateMap<GetAllEnquiryQuery, GetAllEnquiryParameter>();

            CreateMap<EnquiryHistory, EnquiryHistoryResponseDto>();
        }
    }
}
