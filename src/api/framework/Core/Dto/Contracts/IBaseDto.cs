namespace FSH.Framework.Core.Dto.Contracts;

public interface IBaseDto;

public interface IBaseDto<out TId> : IBaseDto
{
    TId Id { get; }
}
