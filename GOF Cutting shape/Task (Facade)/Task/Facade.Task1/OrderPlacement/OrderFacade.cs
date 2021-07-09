using System;
using System.Collections.Generic;

namespace Facade.Task1.OrderPlacement
{
    public class OrderFacade
    {
        private InvoiceSystem _invoiceSystem;
        private PaymentSystem _paymentSystem;
        private ProductCatalog _productCatalog;

        public OrderFacade(InvoiceSystem invoiceSystem, PaymentSystem paymentSystem, ProductCatalog productCatalog )
        {
            _invoiceSystem = invoiceSystem;
            _paymentSystem = paymentSystem;
            _productCatalog = productCatalog;
        }

        public void PlaceOrder(string productId, int quantity, string email)
        {
            var productDetails = _productCatalog.GetProductDetails(productId);
            var payment = new Payment
            {
                ProductId = productId,
                ProductName = productDetails.Name,
                Quantity = quantity,
                TotalPrice = productDetails.Price
            };
            _paymentSystem.MakePayment(payment);

            var invoice = new Invoice
            {
                ProductId = productId,
                CustomerEmail = email,
                DueDate = DateTime.Now,
                InvoiceNumber = Guid.NewGuid(),
                ProductName = productDetails.Name,
                Quantity = quantity,
                SendDate = DateTime.Now,
                TotalPrice = productDetails.Price
            };
            _invoiceSystem.SendInvoice(invoice);
        }
    }
}
