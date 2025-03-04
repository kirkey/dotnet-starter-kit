using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Todo.Features.Create.v1;
public record CreateTodoCommand(
    [property: DefaultValue("Hello World!")] string Name,
    [property: DefaultValue("This is desciption.")] string Description,
    [property: DefaultValue("Important Note.")] string Notes) : IRequest<CreateTodoResponse>;



