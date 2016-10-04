using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host_Order_Copy
{
    class Program
    {
        static void Main(string[] args)
        {
            
            HostOrderDetail Detail = new HostOrderDetail();

            HostOrder Order = new HostOrder("1000341100");

            
            Order.UpdateStatus("W");

            




        }
    }
}
