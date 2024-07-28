namespace WebApi_SGI_T.Models.Commons.Response
{
    public class BaseEntityResponse<T>
    {
        public int? TotalRecords { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}
