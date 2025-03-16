namespace FSH.Framework.Core.Extensions.Dto.Contracts;

public interface IBaseDto;

public interface IBaseDto<out TId> : IBaseDto
{
    TId Id { get; }
}
