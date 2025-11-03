using Accounting.Application.JournalEntries.Lines.Responses;

namespace Accounting.Application.Common.Mapping;

/// <summary>
/// Mapster configuration for mapping JournalEntryLine entity to JournalEntryLineResponse.
/// Maps Account navigation property fields to response properties.
/// </summary>
public class JournalEntryLineMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<JournalEntryLine, JournalEntryLineResponse>()
            .Map(dest => dest.AccountCode, src => src.Account != null ? src.Account.AccountCode : null)
            .Map(dest => dest.AccountName, src => src.Account != null ? src.Account.AccountName : null);
    }
}
