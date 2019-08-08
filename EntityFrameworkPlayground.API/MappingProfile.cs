﻿using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.Domain.Entitities;
using System.Collections.Generic;

namespace EntityFrameworkPlayground.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDTO>().ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.BookId)); ;
            CreateMap<Author, AuthorDTO>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(
                dest => dest.Books,
                opt => opt.MapFrom(src => src.Books));

        }
    }
}
