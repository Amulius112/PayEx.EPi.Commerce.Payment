﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace PayEx.EPi.Commerce.Payment.Models
{
    public class OnlineInvoice
    {
        [XmlElement("OrderLine")]
        public List<OnlineInvoiceOrderLine> OrderLines { get; protected set; }

        public OnlineInvoice()
        {
            OrderLines = new List<OnlineInvoiceOrderLine>();
        }
    }

    public class OnlineInvoiceOrderLine
    {
        [XmlElement("Product")]
        public string ProductName { get; set; }

        [XmlElement("Qty")]
        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public int VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Amount { get; set; }
    }
}
