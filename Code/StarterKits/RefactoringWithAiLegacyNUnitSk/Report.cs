namespace RefactoringWithAiLegacyNUnitSk;

public class Report
{
    public string type = "";
    public List<string> data = new();
    public bool sent;

    public string Generate()
    {
        string result = "";
        if (type == "html")
        {
            result = "<html><body>";
            foreach (var d in data)
                result += "<p>" + d + "</p>";
            result += "</body></html>";
        }
        else if (type == "csv")
        {
            foreach (var d in data)
                result += d + ",";
            result = result.TrimEnd(',');
        }
        else if (type == "json")
        {
            result = "[";
            foreach (var d in data)
                result += "\"" + d + "\",";
            result = result.TrimEnd(',') + "]";
        }
        sent = false;
        return result;
    }
}
