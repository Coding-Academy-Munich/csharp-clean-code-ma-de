namespace RefactoringWithAiNUnit;

public interface IReportFormatter
{
    string Format(List<string> data);
}

public class HtmlFormatter : IReportFormatter
{
    public string Format(List<string> data)
        => $"<html><body>{string.Join("", data.Select(d => $"<p>{d}</p>"))}</body></html>";
}

public class CsvFormatter : IReportFormatter
{
    public string Format(List<string> data)
        => string.Join(",", data);
}

public class JsonFormatter : IReportFormatter
{
    public string Format(List<string> data)
        => $"[{string.Join(",", data.Select(d => $"\"{d}\""))}]";
}

public class Report
{
    private readonly List<string> _data;
    private readonly IReportFormatter _formatter;
    public bool HasBeenSent { get; private set; }

    public Report(List<string> data, IReportFormatter formatter)
    {
        _data = data;
        _formatter = formatter;
        HasBeenSent = false;
    }

    public string Generate() => _formatter.Format(_data);
}
