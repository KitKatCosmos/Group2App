using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckListApp.Model;
public class Comment
{
   
    [PrimaryKey, AutoIncrement]
    public int CommentID { get; set; }
    public int TaskId { get; set; }
    public int UserID { get; set; }
    public string? CommentText { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}