/*
 * File: ResidenceItem.cs
 * Purpose: Model class representing a household item for insurance cataloging.
 * Author: [Your Group Names]
 * Date: 2026-04-07
 */

namespace Assignment4MVC.Models
{
    /*
     * Class: ResidenceItem
     * Description: To hold data related to a specific item within a residence.
     */
    public class CatalogItem
    {
        // Explicitly declared properties
        public string ItemId { get; set; } = string.Empty;
        public string CatalogId { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public int ItemValue { get; set; }

        /*
         * Method: GetFormattedValue
         * Description: Returns the value as a formatted currency string.
         */
        public string GetFormattedValue()
        {
            string formattedValue = "$" + ItemValue.ToString("N2");
            return formattedValue;
        }
    }
}
