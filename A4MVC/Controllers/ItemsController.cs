/*
 * File: ItemsController.cs
 * Purpose: Controller to handle requests for the residence item catalog.
 * Author: [Your Group Names]
 * Date: 2026-04-07
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Assignment4MVC.Models;
namespace Assignment4MVC.Controllers
{
    /*
     * Class: ItemsController
     * Description: Manages the flow between the ResidenceItem model and the Views.
     */
    public class ItemsController : Controller
    {
        //declaration for configuration settings
        private readonly IConfiguration _configuration;
        public ItemsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /*
         * Method: Index
         * Description: Displays the catalog of items.
         */
        public IActionResult Index()
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (connectionString == null)
            {
                throw new Exception("Missing coneecting string");
            }
            
            List<CatalogItem> itemList = new List<CatalogItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "Select itemid, catalogid, itemname, itemtype, itemvalue From CatalogItem";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Map the SQL row to C# model
                            CatalogItem item = new CatalogItem();
                            item.ItemId = reader["itemid"].ToString() ?? "";
                            item.CatalogId = reader["catalogid"].ToString() ?? "";
                            item.ItemName = reader["itemname"].ToString() ?? "";
                            item.ItemType = reader["itemtype"].ToString() ?? "";
                            item.ItemValue = Convert.ToInt32(reader["itemvalue"]);

                            itemList.Add(item);
                        }
                    }
                }
            }
            IActionResult result = View(itemList);
            return result;
        }
    }
}