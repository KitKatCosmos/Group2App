using SQLite;
using System;
using System.Threading.Tasks;

namespace CheckListApp.Model
{
    [Table("UserTasks")]
    public class UserTask
    {
        [PrimaryKey, AutoIncrement]
        public int TaskID { get; set; }

        public int UserId { get; set; }

        public string? Title { get; set; }

        public string? CreatedTask { get; set; }

        public string? Description { get; set; }

        public int PriorityLevel { get; set; }

        public string? Category { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsRepeating { get; set; }

        public string? RepeatInterval { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public static async Task Delay(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }



    }
}