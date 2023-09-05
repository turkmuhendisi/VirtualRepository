using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace VirtualRepository

{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommand cmd;

        void gridFill()
        {
            conn = new SqlConnection("server=.; Initial Catalog=virtualrepodb;Integrated Security=SSPI");
            adapter = new SqlDataAdapter("SELECT * FROM products", conn);
            ds = new DataSet();
            conn.Open();
            adapter.Fill(ds, "products");
            dataGridView1.DataSource = ds.Tables["products"];
            conn.Close();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gridFill();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string formattedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO products (p_name, quantity, price, date) VALUES (@p_name, @quantity, @price, @date)";
            cmd.Parameters.AddWithValue("@p_name", pNameTextBox.Text);
            cmd.Parameters.AddWithValue("@quantity", quantityTextBox.Text);
            cmd.Parameters.AddWithValue("@price", priceTextBox.Text);
            cmd.Parameters.AddWithValue("@date", formattedDate);

            cmd.ExecuteNonQuery();
            conn.Close();
            gridFill();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE products SET p_name = @p_name, quantity = @quantity, price = @price WHERE p_id = @p_id";
            cmd.Parameters.AddWithValue("@p_name", pNameTextBox.Text);
            cmd.Parameters.AddWithValue("@quantity", quantityTextBox.Text);
            cmd.Parameters.AddWithValue("@price", priceTextBox.Text);
            cmd.Parameters.AddWithValue("@p_id", idTextBox.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
            gridFill();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM products WHERE p_id = @p_id";
            cmd.Parameters.AddWithValue("@p_id", idTextBox.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
            gridFill();
        }

        private void updateBtn_MouseHover(object sender, EventArgs e)
        {
            idTextBox.Enabled = true;
        }

        private void addBtn_MouseHover(object sender, EventArgs e)
        {
            idTextBox.Enabled = false;
        }

        private void deleteBtn_MouseHover(object sender, EventArgs e)
        {
            idTextBox.Enabled = true;
        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT p_name, quantity, price FROM products WHERE p_id = @p_id";
            cmd.Parameters.AddWithValue("@p_id", idTextBox.Text);
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) 
            {
                pNameTextBox.Text = dataReader.GetValue(0).ToString();
                quantityTextBox.Text = dataReader.GetValue(1).ToString();
                priceTextBox.Text = dataReader.GetValue(2).ToString();
            }

            conn.Close();
        }
    }
}