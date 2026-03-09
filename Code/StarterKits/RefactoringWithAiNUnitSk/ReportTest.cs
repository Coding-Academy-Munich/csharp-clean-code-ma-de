namespace RefactoringWithAiNUnitSk;

[TestFixture]
public class ReportTest
{
    [Test]
    public void Generate_HtmlFormat_ProducesCorrectOutput()
    {
        var report = new Report { type = "html", data = new List<string> { "A", "B" } };

        Assert.That(report.Generate(), Is.EqualTo("<html><body><p>A</p><p>B</p></body></html>"));
    }

    [Test]
    public void Generate_CsvFormat_ProducesCorrectOutput()
    {
        var report = new Report { type = "csv", data = new List<string> { "A", "B", "C" } };

        Assert.That(report.Generate(), Is.EqualTo("A,B,C"));
    }

    [Test]
    public void Generate_JsonFormat_ProducesCorrectOutput()
    {
        var report = new Report { type = "json", data = new List<string> { "A", "B" } };

        Assert.That(report.Generate(), Is.EqualTo("[\"A\",\"B\"]"));
    }

    [Test]
    public void Generate_EmptyData_HtmlFormat()
    {
        var report = new Report { type = "html", data = new List<string>() };

        Assert.That(report.Generate(), Is.EqualTo("<html><body></body></html>"));
    }

    [Test]
    public void Generate_EmptyData_CsvFormat()
    {
        var report = new Report { type = "csv", data = new List<string>() };

        Assert.That(report.Generate(), Is.EqualTo(""));
    }

    [Test]
    public void Generate_EmptyData_JsonFormat()
    {
        var report = new Report { type = "json", data = new List<string>() };

        Assert.That(report.Generate(), Is.EqualTo("[]"));
    }

    [Test]
    public void Generate_SingleItem_CsvFormat()
    {
        var report = new Report { type = "csv", data = new List<string> { "Only" } };

        Assert.That(report.Generate(), Is.EqualTo("Only"));
    }
}
