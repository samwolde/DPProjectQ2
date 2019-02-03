using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_q2.models
{
    interface IProductData
    {
        List<Product> GetProducts();
    }

    class ProductData : IProductData, ICloneable
    {
        public List<Product> Products { get; set; }

        public ProductData()
        {
            Products = new List<Product>();
        }

        public void SeedData()
        {
            Products.Add(new Product { Name = "Samsung", Price = 5000, Quantity = 15 });
            Products.Add(new Product { Name = "Samsung 2", Price = 5000, Quantity = 15 });
            Products.Add(new Product { Name = "Motorola", Price = 5000, Quantity = 15 });
            Products.Add(new Product { Name = "Nokia", Price = 3000, Quantity = 15 });
            Products.Add(new Product { Name = "Panasonic", Price = 4000, Quantity = 15 });
            Products.Add(new Product { Name = "Sony", Price = 5000, Quantity = 15 });
            Products.Add(new Product { Name = "Samsung 4", Price = 2000, Quantity = 15 });
            Products.Add(new Product { Name = "Samsung 4", Price = 4500, Quantity = 15 });
            Products.Add(new Product { Name = "Samsung 4", Price = 5600, Quantity = 15 });
            Products.Add(new Product { Name = "Samsung 4", Price = 5000, Quantity = 15 });

            Products[0].Sales.Add(new Sale { Quantity = 4 });
            Products[0].Sales.Add(new Sale { Quantity = 4 });
            Products[3].Sales.Add(new Sale { Quantity = 4 });
            Products[3].Sales.Add(new Sale { Quantity = 4 });
        }

        public void AddProduct(String name, double price, int quantity)
        {
            Products.Add(new Product { Name = name, Price = price, Quantity = quantity });
        }

        public void PurchaseProduct(string name, int quantity) 
        {
            IEnumerator<Product> prodEnum = Products.GetEnumerator();

            while (prodEnum.MoveNext())
            {
                if (prodEnum.Current.Name.Equals(name))
                {
                    prodEnum.Current.Sales.Add(new Sale { Quantity = quantity });
                    break;
                }
            }
        }

        public List<Product> GetProducts()
        {
            List<Product> res = Products;

            return res;
        }

        public object Clone()
        {
            ProductData pd = new ProductData();
            List<Product> prod = new List<Product>();

            foreach(Product p in this.Products)
            {
                Product np = new Product { Name = p.Name, Price = p.Price, Quantity = p.Quantity };
                np.Sales.AddRange(p.Sales);
                prod.Add(np);
            }

            pd.Products = prod;
            return pd;
        }
    }

    abstract class ProductDecorator : IProductData
    {
        public IProductData ProductData { get; set; }

        public ProductDecorator(IProductData productData)
        {
            this.ProductData = productData;
        }

        abstract public List<Product> GetProducts();
    }

    class FilterProductSale : ProductDecorator
    {
        public FilterProductSale(IProductData productData) : base(productData) { }

        public override List<Product> GetProducts()
        {
            List<Product> res = new List<Product>();

            IEnumerator<Product> prodEnum = ProductData.GetProducts().GetEnumerator();

            while (prodEnum.MoveNext())
            {
                if (prodEnum.Current.Sales.Count > 0)
                {
                    res.Add(prodEnum.Current);
                }
            }

            return res;
        }
    }

    class FilterSignificantProduct : ProductDecorator
    {
        public FilterSignificantProduct(IProductData productData) : base(productData) { }
        public override List<Product> GetProducts()
        {
            int minVal = 3;

            List<Product> res = new List<Product>();

            IEnumerator<Product> prodEnum = ProductData.GetProducts().GetEnumerator();

            while (prodEnum.MoveNext())
            {
                if (prodEnum.Current.Quantity - prodEnum.Current.GetRemainingQty() > minVal)
                {
                    res.Add(prodEnum.Current);
                }
            }

            return res;
        }
    }

    class SortProduct : ProductDecorator
    {
        public SortProduct(IProductData productData):base(productData){ }

        public override List<Product> GetProducts()
        {
            List<Product> pd = this.ProductData.GetProducts();
            pd.Sort(compareByName);

            return pd; 
        }

        private int compareByName(Product prod1, Product prod2)
        {
            if (prod1 == null)
            {
                if (prod2 == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (prod2 == null)
                {
                    return 1;
                }
                else
                {
                    return prod1.Name.CompareTo(prod2.Name);
                }
            }
        }
    }
}
