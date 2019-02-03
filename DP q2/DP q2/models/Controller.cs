using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DP_q2.models
{
    class Controller
    {
        private View view;

        public Controller()
        {
            view = new CustViewProxy();
        }

        public void ChangeUserType(USER_TYPE userType)
        {
            if(userType == USER_TYPE.CUSTOMER)
            {
                view = new CustViewProxy();
            }
            else
            {
                view = new ExecViewProxy();
            }
        }

        public List<Product> GetProducts(USER_TYPE userType, Boolean isSignificant, Boolean isSales, Boolean isSorted)
        {
            List<Product> prods = view.GetData(userType, isSignificant, isSales, isSorted);
            return prods;
        }

        public Boolean AddProduct(USER_TYPE userType, string name, double price, int quantity)
        {
            view.AddProduct(userType, name, price, quantity);
            return true;
        }

        public Boolean PurchaseProduct(USER_TYPE userType, string name, int quantity)
        {
            view.PurchaseProduct(userType, name, quantity);
            return true;
        }
    }
}
