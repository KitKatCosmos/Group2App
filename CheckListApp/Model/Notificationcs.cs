using SQLite;

namespace CheckListApp.Model;


public class Notification
{
    [PrimaryKey, AutoIncrement]
    public int NotificationID { get; set; }
    public int UserID { get; set; }
    public int? TaskID { get; set; }
    public string? Message { get; set; }
    public DateTime NotificationDate { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
}
