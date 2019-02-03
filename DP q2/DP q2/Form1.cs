using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DP_q2.models;

namespace DP_q2
{
    public partial class Form1 : Form
    {
        private Boolean IsSorted { get; set; }
        private USER_TYPE UserType { get; set; }
        private List<Product> prods;

        private Controller controller;
        
        public Form1()
        {
            InitializeComponent();
            UserType = USER_TYPE.CUSTOMER;
            controller = new Controller();

            updateView();
        }

        private void updateView()
        {
            controller.ChangeUserType(UserType);

            if (UserType == USER_TYPE.CUSTOMER)
            {
                updateCustView();
            }
            else
            {
                updateExecView();
            }
        }

        private void updateExecView()
        {
            List<Product> products = controller.GetProducts(UserType, false, true, IsSorted);
           
            listView2.Items.Clear();

            if (products!=null && products.Count > 0)
            {
                foreach (Product product in products)
                {
                    int soldQty = 0;
                    foreach (Sale sale in product.Sales)
                    {
                        soldQty += sale.Quantity;
                    }

                    listView2.Items.Add(product.Name).SubItems.AddRange(new[] { product.Price.ToString(), soldQty.ToString(), (soldQty * product.Price).ToString() });
                }
            }
            
        }

        private void updateCustView()
        {
            List<Product> productList = controller.GetProducts(UserType, false, false, IsSorted);
            List<Product> significantList = controller.GetProducts(UserType, true, false, IsSorted);

            this.prods = productList;

            listView1.Items.Clear();
            listView3.Items.Clear();

            foreach (Product product in productList)
            {
                Console.WriteLine("Prod: " + product.Name);
                listView1.Items.Add(product.Name);   
            }

            foreach (Product product in significantList)
            {
                listView3.Items.Add(product.Name).SubItems.Add((product.Price.ToString()));
            }
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                UserType = USER_TYPE.CUSTOMER;
                panel3.BringToFront();
                updateView();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                UserType = USER_TYPE.EXECUTIVE;
                panel2.BringToFront();
                updateView();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            IsSorted = checkBox1.Checked;
            updateView();
        }

        // event handlers
        private void onNameChanged(Object sender, EventArgs args)
        {
            button1.Enabled = true;
        }

        private void onSelectedChanged(Object sender, EventArgs args)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textBox1.Text = listView1.SelectedItems[0].Text;
                
                numericUpDown1.Maximum = prods[listView1.SelectedIndices[0]].GetRemainingQty();
                numericUpDown1.Value = 0;

                if (Int32.Parse(numericUpDown1.Value.ToString()) > 0)
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox2.Text;
            double price;
            double.TryParse(textBox3.Text, out price);
            int quantity = Int32.Parse(numericUpDown2.Value.ToString());

            controller.AddProduct(UserType, name, price, quantity);
            updateView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            int quantity = (int)numericUpDown1.Value;

            controller.PurchaseProduct(UserType, name, quantity);
            updateView();
        }

        private void onQuantityChanged(object sender, EventArgs e)
        {
            if(Int32.Parse(numericUpDown1.Value.ToString()) > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
    }
}
