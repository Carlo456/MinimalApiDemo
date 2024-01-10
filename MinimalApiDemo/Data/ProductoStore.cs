using MinimalApiDemo.Models;

namespace MinimalApiDemo.Data
{
    public class ProductoStore
    {
        public static List<Producto> product_list = new List<Producto>
        {
            new Producto{ Id = 1, Name="modelo x", Description="Blue model", PhotoUrl= "https://www.google.com/"  },
            new Producto{ Id = 2, Name="modelo z", Description="Red model", PhotoUrl= "https://www.youtube.com/"  }
        };
    }
}
