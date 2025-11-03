using Accounting.Application.JournalEntries.Lines.Responses;

namespace Accounting.Application.JournalEntries.Mapster;

/// <summary>
/// Mapster configuration for Journal Entry and Journal Entry Line mappings.
/// </summary>
public class JournalEntryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map JournalEntryLine to JournalEntryLineResponse with Account details
        config.NewConfig<JournalEntryLine, JournalEntryLineResponse>()
            .Map(dest => dest.AccountCode, src => src.Account != null ? src.Account.AccountCode : null)
            .Map(dest => dest.AccountName, src => src.Account != null ? src.Account.AccountName : null);
    }
}

