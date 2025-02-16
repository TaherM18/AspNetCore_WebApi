namespace Repositories.Models
{
    public class t_State
    {
        public int? c_StateId { get; set;}
        public string? c_StateName { get; set;}

        public t_State()
        {
            c_StateId = 0;
            c_StateName = "";
        }
    }
}