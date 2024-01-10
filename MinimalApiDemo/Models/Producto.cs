namespace MinimalApiDemo.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }

        public string ProductoInfo(Producto prod)
        {
            return $"Id: {prod.Id}, Name: {prod.Name}, Description: {prod.Description}, Photo url: {prod.PhotoUrl}";
        }

    }
}
