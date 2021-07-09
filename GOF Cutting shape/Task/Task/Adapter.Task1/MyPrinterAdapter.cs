using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adapter.Task1
{
    public class MyPrinterAdapter : IMyPrinter
    {
        private readonly Printer printer;

        public MyPrinterAdapter(Printer printer)
        {
            this.printer = printer;
        }

        public void Print<T>(IElements<T> elements)
        {
            var conteiner = new MyContainer<T> 
            { 
                Items = elements.GetElements(),
                Count = elements.GetElements().Count()
            };

            printer.Print(conteiner);
        }
    }
}
