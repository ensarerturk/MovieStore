using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Application.DirectorOperations.Commands.CreateDirector;
using MovieStore.Application.DirectorOperations.Commands.DeleteDirector;
using MovieStore.Application.DirectorOperations.Commands.UpdateDirector;
using MovieStore.Application.DirectorOperations.Queries.GetDirectorById;
using MovieStore.Application.DirectorOperations.Queries.GetDirectors;
using MovieStore.DBOperations;

namespace MovieStore.Controllers;

[ApiController]
[Route("[Controller]s")]
public class DirectorController : ControllerBase
{
    private readonly IMovieStoreDbContext _context;
    private readonly IMapper _mapper;

    public DirectorController(IMovieStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetDirectors()
    {
        GetDirectorsQuery query = new GetDirectorsQuery(_context, _mapper);
        List<DirectorsViewModel> result = query.Handle();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetDirectorById(int id)
    {
        GetDirectorByIdQuery query = new GetDirectorByIdQuery(_context, _mapper);
        query.Id = id;

        GetDirectorByIdQueryValidator validator = new GetDirectorByIdQueryValidator();
        validator.ValidateAndThrow(query);

        GetDirectorByIdViewModel result = query.Handle();

        return Ok(result);
    }

    [HttpPost]
    public IActionResult CreateDirector([FromBody] CreateDirectorModel newDirector)
    {
        CreateDirectorCommand command = new CreateDirectorCommand(_context, _mapper);
        command.Model = newDirector;

        CreateDirectorCommandValidator validator = new CreateDirectorCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateDirector(int id, [FromBody] UpdateDirectorModel updatedDirector)
    {
        UpdateDirectorCommand command = new UpdateDirectorCommand(_context);

        command.Id = id;
        command.Model = updatedDirector;
        UpdateDirectorCommandValidator validator = new UpdateDirectorCommandValidator();
        validator.ValidateAndThrow(command);
        command.Handle();

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDirector(int id)
    {
        DeleteDirectorCommand command = new DeleteDirectorCommand(_context);

        command.Id = id;
        DeleteDirectorCommandValidator validator = new DeleteDirectorCommandValidator();
        validator.ValidateAndThrow(command);
        command.Handle();

        return Ok();
    }
}