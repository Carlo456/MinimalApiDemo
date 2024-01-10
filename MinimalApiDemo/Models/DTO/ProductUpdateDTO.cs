namespace MinimalApiDemo.Models.DTO
{
    public class ProductUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public float Price { get; set; }
    }
}
