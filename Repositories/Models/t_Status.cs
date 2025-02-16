namespace Repositories.Models
{
    public class t_Status
    {
        public int? c_StatusId { get; set; }
        public string? c_StatusName { get; set;}

        public t_Status()
        {
            c_StatusId = 0;
            c_StatusName = "";
        }
    }
}