using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


/*
 * Program name:Warehouse Transfer
 * Known Bugs: None
 * Written by: Tim Turner 
 * Date Last Updated: 7/2/20015
 * Low Level Explnation: Read in orders from the host order table then breaks the order down and assigns cases and piece picks to the order,
 *      This Program will only handel the WHT( Warehouse Transfer) orders. we want this because with warehouse transfers we do not want the orders to consolidate into one box,
 *      we want every piece pick product to be in its own box so it is easier to put away
 * High Level Explnation:
 * 
 * to do:fix the log so only relivant information is printed, 
 *          add email, 
 *          sort orders based on time the go out to the time it is right now, 
 *          cartonize chairs
 *          INVENTORY last resort( look around the ware house to fill a case from anything) 
 *          WHT PPCartonize( dont consolidate orders into the same box, every product get's it's own box)
 *          PPCartonize( new cartonize, like products in same box, need the rest of the requirements)
 *          add try catch to everything, and if we hit an error set host order status or E or R( not sure which) and purge the order from outbound shipments, OBSD, Move queue, and inventory allocation
 *          Purchase_order in OutboundSHipments needs to not be null. this is what caused the printing error 

*/
namespace WarehouseTransfer
{
    class Program : Cartonize
    {

        static void Main(string[] args)
        {
          
                Cartonize cartonize = new Cartonize();
                Order[] Orders = new Order[200];
                DebugLog log = new DebugLog();
                Stopwatch TimePass = Stopwatch.StartNew();

                int PID, MaxCasePerLine;
                int OrderCnt = 0;
                string Status;
                string OrderType = "WHT";

                if (cartonize.RunProgram() == true)
                {
                    
                        /*******************************************
                         * gets a unique process ID so if we get an error we know what order to look at in the log
                         * all finished with try/catch update
                         *****************************************/
                        PID = cartonize.GetUniqueProcessID();
                        if (PID == -1)
                        {
                            System.Environment.Exit(0);
                        }

                        /*******************************************
                        * get the max amount of cases per order line( default is 100)(also not sure if we really need it)
                        *****************************************/
                        //MaxCasePerLine = cartonize.GetMaxCasesPerLine();

                        /*******************************************
                        * sets the Carton Info Object inside Cartononize( this can probably be moved into cartonize)
                        ********************************************/
                        cartonize.SetBoxes();

                        /*******************************************
                        * get the WHT orders from Host_Orders and store each order in an index in the Order Object
                        *****************************************/
                        Orders = cartonize.GetOrders(OrderType);
                        if (Orders == null)
                        {
                            System.Environment.Exit(0);
                        }

                        /*******************************************
                       * get the total number of orders that were read in from the database that need to be cartonized
                       *****************************************/
                        OrderCnt = Orders[0].OrderCount;

                        /*******************************************
                      * begin looping through each order one at a time 
                      *****************************************/
                        for (int i = 0; i < OrderCnt; i++)
                        {
                            // Orders[i].printOrderInfo();
                            /*******************************************
                             * Sets the order's status to I then updates the database
                             *****************************************/
                            cartonize.SetOrderToProcessing(Orders[i], PID);
                            int Stat=Orders[i].updateHostOrdersTable();
                            if (Stat == -1)
                            {
                                log.Write("Update Failed Exiting");
                                System.Environment.Exit(0);
                            }
                            /*******************************************
                             * call CartonizeOrder so we can break this order down into pieces and cases
                             *****************************************/
                            Status = cartonize.CartonizeOrder(Orders[i], OrderType);
                            if (Status == null)
                            {
                                System.Environment.Exit(0);
                            }
                            /*******************************************
                             * Now that the order is broken down we will 
                             * look in our inventory and try to fill the order 
                             *****************************************/
                            cartonize.CreatePicks(Orders[i], OrderType);
                           
                            
                            

                        }
                        TimePass.Stop();
                        cartonize.UpdateMaxThreads();
                        TimePass.Stop();
                        TimeSpan ts = TimePass.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                             ts.Hours, ts.Minutes, ts.Seconds,
                             ts.Milliseconds / 10);
                        log.Write("RunTime " + elapsedTime);
                        cartonize.ResetThread();
                   
                }
                else
                {
                    log.Write("Cartonize is currently running, exiting program to avoid errors");
                    TimePass.Stop();
                    TimeSpan ts = TimePass.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                         ts.Hours, ts.Minutes, ts.Seconds,
                         ts.Milliseconds / 10);
                    log.Write("RunTime " + elapsedTime);
                   // log.Write("Program Run Time: "+TimePass.ElapsedMilliseconds.ToString());
                }

                // Console.ReadLine();

            
           
        }
        
    
    }
    
}
