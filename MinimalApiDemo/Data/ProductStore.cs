using MinimalApiDemo.Models;

namespace MinimalApiDemo.Data
{
    public class ProductStore
    {
        public static List<Product> product_list = new List<Product>
        {
            new Product{ Id = 1, Name="modelo x", Description="Blue model", PhotoUrl= "https://www.google.com/"  },
            new Product{ Id = 2, Name="modelo z", Description="Red model", PhotoUrl= "https://www.youtube.com/"  }
        };
    }
}
