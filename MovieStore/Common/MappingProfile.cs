using AutoMapper;
using MovieStore.Application.ActorOperations.Commands.CreateActor;
using MovieStore.Application.ActorOperations.Queries.GetActorById;
using MovieStore.Application.ActorOperations.Queries.GetActors;
using MovieStore.Application.ActorOperations.Queries.SharedViewModels;
using MovieStore.Application.CustomerOperations.Commands.CreateCustomer;
using MovieStore.Application.CustomerOperations.Queries.GetCustomerById;
using MovieStore.Application.CustomerOperations.Queries.GetCustomers;
using MovieStore.Application.CustomerOperations.Queries.SharedViewModels;
using MovieStore.Application.DirectorOperations.Commands.CreateDirector;
using MovieStore.Application.DirectorOperations.Queries.GetDirectorById;
using MovieStore.Application.DirectorOperations.Queries.GetDirectors;
using MovieStore.Application.DirectorOperations.Queries.SharedViewModels;
using MovieStore.Application.MovieOperations.Commands.CreateMovie;
using MovieStore.Application.MovieOperations.Queries.GetMovieById;
using MovieStore.Application.MovieOperations.Queries.GetMovies;
using MovieStore.Application.MovieOperations.Queries.SharedViewModels;
using MovieStore.Entities;

namespace MovieStore.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, MoviesViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Director,
                opt => opt.MapFrom(src => src.Director.FirstName + " " + src.Director.LastName));

        CreateMap<Movie, GetMovieByIdViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Director,
                opt => opt.MapFrom(src => src.Director.FirstName + " " + src.Director.LastName));

        CreateMap<CreateMovieModel, Movie>();
        CreateMap<Actor, ActorsViewModel>();
        CreateMap<Actor, GetActorByIdViewModel>();
        CreateMap<CreateActorModel, Actor>();
        CreateMap<Director, DirectorsViewModel>();
        CreateMap<Movie, DirectedMovieViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));

        CreateMap<Movie, ActedInMovieViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            .ForMember(dest => dest.Director,
                opt => opt.MapFrom(src => src.Director.FirstName + " " + src.Director.LastName));

        CreateMap<Director, GetDirectorByIdViewModel>();
        CreateMap<CreateDirectorModel, Director>();
        CreateMap<CreateCustomerModel, Customer>().ForMember(dest => dest.Password,
            opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
        CreateMap<Actor, ActorViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

        CreateMap<Customer, GetCustomerByIdViewModel>();
        CreateMap<Order, OrderViewModel>().ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src.Movie.Name));

        CreateMap<Genre, GenreViewModel>();
        CreateMap<Customer, CustomerViewModel>();
    }
}