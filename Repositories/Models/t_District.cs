namespace Repositories.Models
{
    public class t_District
    {
        public int? c_DistrictId { get; set; }
        public string? c_DistrictName { get; set; }

        public t_District()
        {
            c_DistrictId = 0;
            c_DistrictName = "";
        }
    }
}