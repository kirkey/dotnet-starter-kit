using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Infrastructure.Exporting;
public interface IExportService
{
    Task<byte[]> ExportCsvAsync<T>(IEnumerable<T> rows);
}
public class ExportService : IExportService
{
    public Task<byte[]> ExportCsvAsync<T>(IEnumerable<T> rows)
    {
        var props = typeof(T).GetProperties();
        using var ms = new MemoryStream();
        using var sw = new StreamWriter(ms, System.Text.Encoding.UTF8, leaveOpen:true);
        sw.WriteLine(string.Join(',', props.Select(p=>Quote(p.Name))));
        foreach (var r in rows)
        {
            sw.WriteLine(string.Join(',', props.Select(p=>Quote(p.GetValue(r)?.ToString() ?? string.Empty))));
        }
        sw.Flush();
        return Task.FromResult(ms.ToArray());
    }
    private static string Quote(string s)
    {
        if (s.Contains(',') || s.Contains('"') || s.Contains('\n'))
        {
            var escaped = s.Replace("\"", "\"\"");
            return '"' + escaped + '"';
        }
        return s;
    }
}
