using CRUDProduct.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace CRUDProduct.Controllers
{
    public class ProductController : Controller
    {
        private readonly database db = new database();

        public IActionResult Index()
        {
            List<Product> products = new List<Product>();
            using (var conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM products";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        product_id = reader.GetInt32("product_id"),
                        product_name = reader.GetString("product_name"),
                        product_price = reader.GetInt32("product_price"),
                        product_stok = reader.GetInt32("product_stok")
                    });
                }
            }
            return View(products);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO products (product_name, product_price, product_stok) VALUES (@name, @price, @stok)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", product.product_name);
                cmd.Parameters.AddWithValue("@price", product.product_price);
                cmd.Parameters.AddWithValue("@stok", product.product_stok);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int id)
        {
            Product product = null;

            using (var conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM products WHERE product_id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            product_id = reader.GetInt32("product_id"),
                            product_name = reader.GetString("product_name"),
                            product_price = reader.GetInt32("product_price"),
                            product_stok = reader.GetInt32("product_stok")
                        };
                    }
                }
            }

            if (product == null)
            {
                return NotFound(); // Jika produk tidak ditemukan
            }

            return View(product); // Kembalikan view dengan produk untuk diedit
        }

        // POST: Products/Edit/5
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid) // Memastikan model valid
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE products SET product_name = @name, product_price = @price, product_stok = @stok WHERE product_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", product.product_id);
                    cmd.Parameters.AddWithValue("@name", product.product_name);
                    cmd.Parameters.AddWithValue("@price", product.product_price);
                    cmd.Parameters.AddWithValue("@stok", product.product_stok);
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index"); // Arahkan setelah berhasil
            }

            return View(product); // Kembalikan ke view jika model tidak valid
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int id)
        {
            Product product = null;

            using (var conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM products WHERE product_id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            product_id = reader.GetInt32("product_id"),
                            product_name = reader.GetString("product_name"),
                            product_price = reader.GetInt32("product_price"),
                            product_stok = reader.GetInt32("product_stok")
                        };
                    }
                }
            }

            if (product == null)
            {
                return NotFound(); // Jika produk tidak ditemukan
            }

            return View(product); // Kembalikan view konfirmasi penghapusan
        }

        // POST: Products/Delete/5
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM products WHERE product_id = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index"); // Arahkan setelah berhasil
        }


    }
}
