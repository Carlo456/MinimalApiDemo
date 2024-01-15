using MinimalApiDemo.Models;
using System.Text.Json;


namespace MinimalApiDemo.Data
{
    public class ProductStore
    {
        public static List<Product> product_list;
        static ProductStore()
        {
            LoadProductsFromJSON();
        }
        public static void LoadProductsFromJSON()
        {
            string json_file_path = "./guitarras.json";
            try
            {
                string jsonData = File.ReadAllText(json_file_path);
                product_list = JsonSerializer.Deserialize<List<Product>>(jsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading products from JSON: {e.Message}");
                product_list = new List<Product>();
            }
        }

    }
}
