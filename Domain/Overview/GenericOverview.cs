namespace Domain.Overview
{
    public class GenericOverview<T> where T : class
    {
        public long Total { get; set; }

        public List<T> Data { get; set; }
    }
}
