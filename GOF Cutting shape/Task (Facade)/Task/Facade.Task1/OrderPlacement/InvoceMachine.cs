using System;


namespace Facade.Task1.OrderPlacement
{
    public class InvoceMachine : InvoiceSystem
    {
        public void SendInvoice(Invoice invoice)
        {
            Console.WriteLine("Done");
        }
    }
}
