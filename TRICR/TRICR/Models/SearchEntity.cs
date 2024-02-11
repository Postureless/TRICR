using SQLite;

namespace TRICR.Models;

public class SearchEntity
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string From { get; set; }
    public string Where { get; set; }
    public string DepartureDate { get; set; }
    public string ReturnDate { get; set; }
    public string Passengers { get; set; }
}