using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_q2.models
{
    public enum USER_TYPE{
        CUSTOMER,
        EXECUTIVE
    }

    abstract class View
    {
        public static ProductData productData;

        public View()
        {
            if (View.productData == null)
            {
                View.productData = new ProductData();
                View.productData.SeedData();
            }
        }

        public abstract List<Product> GetData(USER_TYPE userType, Boolean significant, Boolean sales, Boolean isSorted);
        public abstract Boolean AddProduct(USER_TYPE userType, string name, double price, int quantity);
        public abstract Boolean PurchaseProduct(USER_TYPE userType, string name, int quantity);
    }

    class CustViewProxy:View
    {
        public override List<Product> GetData(USER_TYPE userType, Boolean isSignificant, Boolean isSales, Boolean isSorted)
        {
            if(userType == USER_TYPE.CUSTOMER)
            {
                CustView custView = new CustView();
                
                return custView.GetData(userType, isSignificant, isSales, isSorted);
            }

            return null;
        }

        public override Boolean PurchaseProduct(USER_TYPE userType, string name, int quantity)
        {
            if (userType == USER_TYPE.CUSTOMER)
            {
                CustView custView = new CustView();
                return custView.PurchaseProduct(userType, name, quantity);
            }

            return false;
        }

        public override bool AddProduct(USER_TYPE userType, string name, double price, int quantity)
        {
            return true;
        }
    }

    class CustView:View
    {
        public override List<Product> GetData(USER_TYPE userType, Boolean isSignificant, Boolean isSales, Boolean isSorted)
        {
            IProductData prodData = (ProductData)productData.Clone();
            
            if (isSignificant)
            {
                prodData = new FilterSignificantProduct(prodData);
            }

            if (isSales)
            {
                prodData = new FilterProductSale(prodData);
            }

            if (isSorted)
            {
                prodData = new SortProduct(prodData);
            }

            
            return prodData.GetProducts();
        }

        public override Boolean PurchaseProduct(USER_TYPE userType, string name, int quantity)
        {
            productData.PurchaseProduct(name, quantity);

            return true;
        }

        public override bool AddProduct(USER_TYPE userType, string name, double price, int quantity)
        {
            return true;
        }
    }

    class ExecViewProxy : View
    {
        public override List<Product> GetData(USER_TYPE userType, Boolean isSignificant, Boolean isSales, Boolean isSorted)
        {
            if (userType == USER_TYPE.EXECUTIVE)
            {
                ExecView execView = new ExecView();
                return execView.GetData(userType, isSignificant, isSales, isSorted);
            }

            return null;
        }

        public override Boolean PurchaseProduct(USER_TYPE userType, string name, int quantity)
        {
            return false;
        }

        public override bool AddProduct(USER_TYPE userType, string name, double price, int quantity)
        {
            if (userType == USER_TYPE.EXECUTIVE)
            {
                ExecView execView = new ExecView();
                return execView.AddProduct(userType, name, price, quantity);
            }

            return false;
        }
    }

    class ExecView : View
    {
        public override List<Product> GetData(USER_TYPE userType, Boolean isSignificant, Boolean isSales, Boolean isSorted)
        {
            IProductData prodData = (ProductData)productData.Clone();

            prodData = new FilterProductSale(prodData);

            if (isSorted)
            {
                prodData = new SortProduct(prodData);
            }

            return prodData.GetProducts();
        }

        public override Boolean PurchaseProduct(USER_TYPE userType, string name, int quantity)
        {
            return false;
        }

        public override bool AddProduct(USER_TYPE userType, string name, double price, int quantity)
        {
            productData.AddProduct(name, price, quantity);
            return true;
        }
    }
}
