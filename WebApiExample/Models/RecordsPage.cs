namespace WebApiExample.Models;

public class RecordsPage
{
    public int TotalCount { get; set; }
    public Record[] Records { get; set; }
}