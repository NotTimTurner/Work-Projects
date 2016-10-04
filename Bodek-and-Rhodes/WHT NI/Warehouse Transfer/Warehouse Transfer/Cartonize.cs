using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading;

namespace WarehouseTransfer
{

    public class Cartonize
    {
        int MaxThreads = -1;
        int currentThreadCount = 0;
        DebugLog Log = new DebugLog();
        CartonInfo[] boxes = new CartonInfo[10];
        /**************************************************************************************
         * Function name:                   GetUnigureProcessID
         * What this Function Does:         This function will try and create a unigure process id by randomly generating it        
         * Variables passed in:             None
         * Variables Declared :             PID- The Process ID 
         * returns :                        PID
         * **************************************************************************************/
        public int GetUniqueProcessID()
        {
            try
            {
                int PID = 0;
                Random rnd = new Random();
                PID = ((999999 - 999 + 1) * rnd.Next(0, 1000) + 999);


                Log.Write("Process ID: " + PID);
                Log.newLine();

                return (PID);
            }
            catch (Exception e)
            {
                if (e.Source != null)
                    Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                UpdateThreads(0);
                return -1;
            }

        }

        /**************************************************************************************
     * Function name:                   GetMaxCasesPerLine
     * What this Function Does:         Gets the Max cases per line from the database, if it cant find it then it returns the default(100)        
     * Variables passed in:             None
     * Variables Declared :             MAXCPL-  The max number of cases allowed per order line
     * returns :                        MAXCPL
     * **************************************************************************************/
        public int GetMaxCasesPerLine()
        {
            try
            {
                int MAXCPL = 100;
                using (WMSEntities wms = new WMSEntities())
                {
                    var sys = from system in wms.SYSTEMs where system.NAME == "MAXCASESPERLINE" select system;

                    foreach (var item in sys)
                    {
                        if (item.VALUE == "")
                            return (MAXCPL);

                        else
                            return (Convert.ToInt32(item.VALUE));
                    }
                }

                return (0);
            }


            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                    }
                }
                return (0);
            }
        }

        /**************************************************************************************
         * Function name:                   GetCartonMast
         * What this Function Does:         Populates the carton info objuct with the approate info
          * update needed:                  add part of this to the cartonInfo object
         * Variables passed in:             None
         * Variables Declared :             Boxcnt-number of different type of boxes
          *                                 boxes- the carton info object
         * returns :                        boxes
         * **************************************************************************************/
        public CartonInfo[] GetCartonMast()
        {
            try
            {
                int BoxCnt = 0;


                CartonInfo[] boxes = new CartonInfo[10];


                using (WMSEntities wms = new WMSEntities())
                {
                    var data = from CartonMaster in wms.CARTON_MASTER select CartonMaster;
                    BoxCnt = (from CartonMaster in wms.CARTON_MASTER select CartonMaster).Count();
                    int i = 0;
                    // Log.Write("Number of box types: " + BoxCnt);
                    // Log.Write("\n");
                    foreach (var box in data)
                    {
                        boxes[i] = new CartonInfo(box.CARTON_TYPE, box.DESCRIPTION, box.LENGTH, box.WIDTH, box.HEIGHT, box.UCB_PERCENT, box.LCB_PERCENT, box.MAX_WEIGHT,
                          box.DUNNAGE_WEIGHT, box.CONVEYABLE_IND, BoxCnt);
                        i++;
                    }
                }
                boxes = SortCartonArray(boxes, BoxCnt);
                return boxes;
            }
            catch (Exception e)
            {
                if (e.Source != null)
                    Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                UpdateThreads(0);
                return null;
            }

        }

        /**************************************************************************************
        * Function name:                   SortCartonArray
         * 
        * What this Function Does:         Sorts the CartonInfo object by size, in ascending order
         * 
         * update needed:                  none
         * 
        * Variables passed in:             Boxes- the carton info object array
         *                                 BoxCnt- the number of boxes, used to loop through the array
         *                                 
        * Variables Declared :            temp: a temp carton info object to hold the info while we sort
         * 
        * returns :                        boxes
        * **************************************************************************************/
        public CartonInfo[] SortCartonArray(CartonInfo[] Boxes, int BoxCnt)
        {

            CartonInfo temp = new CartonInfo();

            for (int i = 0; i < Boxes[1].BoxCnt; i++)
            {
                // Console.WriteLine(Boxes[i].GetName());
                //  Console.WriteLine(Boxes[i].GetMaxVolume());
                for (int j = 0; j < i; j++)
                {
                    if (Boxes[i].GetMaxVolume() > Boxes[j].GetMaxVolume())
                    {
                        temp = Boxes[j];
                        Boxes[j] = Boxes[i];
                        Boxes[i] = temp;
                    }
                }
            }
            return (Boxes);
        }

        /**************************************************************************************
        * Function name:                   CheckTruck
         * 
        * What this Function Does:         Checks to see if the order is ship mode 7(Truck) and of terms 1 or 2, if it is we change the order status to E 
         * 
         * update needed:                  none
         * 
        * Variables passed in:             None
         *                                 
        * Variables Declared :            None
         * 
        * returns :                        Nothing
        * **************************************************************************************/
        public void CheckTruck()
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<HOST_ORDERS> items = from H in wms.HOST_ORDERS
                                                where
                                                    ((H.TERMS == 1 && H.ORIGINAL_SHIP_MODE == "7") || (H.TERMS == 2 && H.ORIGINAL_SHIP_MODE == "7")) && H.STATUS == "N"
                                                select H;

                foreach (HOST_ORDERS x in items)
                {

                    x.STATUS = "E";
                    Log.Write("Status Changed from N to E in CheckTruck()\n");
                }

                wms.SaveChanges();
            }
        }

        /**************************************************************************************
        * Function name:                   SetBoxes
         * 
        * What this Function Does:         Calls GetCartonMast to set the boxes object, and sets boxcnt to print the boxes
         * 
         * update needed:                  changing how the constructor works so we propably wont need this 
         * 
        * Variables passed in:             None
         *                                 
        * Variables Declared :            boxcnt-number of box
         * 
        * returns :                        Nothing
        * **************************************************************************************/
        public void SetBoxes()
        {
            try
            {
                int boxcnt;
                boxes = GetCartonMast();
                boxcnt = boxes[0].BoxCnt;


                //prints info about boxes to screen used in testing 
                for (int i = 0; i < boxcnt; i++)
                {
                    // boxes[i].PrintBoxInfo();

                }
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                UpdateThreads(0);
                System.Environment.Exit(0);
            }


        }

        /**************************************************************************************
        * Function name:                   GetOrders
         * 
        * What this Function Does:         gets all new  orders from Host orders
         * 
         * update needed:                  Populate Host Orders Object here
         * 
        * Variables passed in:             OrderType- They type of orders we are searching for
         *                                 
        * Variables Declared :             i- counter for the order array
         *                                  orderCnt- number of orders we are reading in
         * 
        * returns :                        Orders
        * **************************************************************************************/
        public Order[] GetOrders(string OrderType)
        {
            try
            {
                int i = 0;
                int orderCnt = 0;
                Order[] orders = new Order[100];

                using (WMSEntities wms = new WMSEntities())
                {

                   

                   ;
                    CheckTruck();


                    //add in the cust ID for the new transfer here TIm
                    var data = (from D in wms.HOST_ORDERS where D.STATUS == "N" &&  (D.TYPE == "WHT" || D.CUSTOMER_ID == "101210") orderby D.EIS_ORDER_ID select D);
                    orderCnt = (from D in wms.HOST_ORDERS where D.STATUS == "N" && (D.TYPE == "WHT" || D.CUSTOMER_ID == "101210") select D).Count();


                    //else
                    //{
                    //    data = from D in wms.HOST_ORDERS where D.STATUS == "N" && !cancelOrder.Contains(D.EIS_ORDER_ID) && D.TYPE != "WHT" orderby D.EIS_ORDER_ID select D;
                    //    orderCnt = (from D in wms.HOST_ORDERS where D.STATUS == "N" && !cancelOrder.Contains(D.EIS_ORDER_ID) && D.TYPE != "WHT" select D).Count();
                    //}
                    Log.Write("Number of Orders: " + orderCnt);
                    Log.newLine();

                    if (orderCnt == 0)
                    {
                        Log.Write("No Orders Found Exiting Program");
                        UpdateThreads(0);
                        UpdateMaxThreads();
                        System.Environment.Exit(1);
                    }
                    foreach (HOST_ORDERS item in data)
                    {
                        DateTime NewDate;
                        DateTime? dateornull = item.C_LAST_UPDATED_;
                        if (dateornull != null)
                        {
                            NewDate = item.C_LAST_UPDATED_.Value;
                        }
                        else
                        {
                            NewDate = DateTime.Now;
                        }
                        orders[i] = new Order(item.EIS_ORDER_ID, item.STATUS, item.EXPEDITE_IND, item.CARRIER_ID, DateTime.Now, item.C_LAST_USER_, item.FREIGHT, item.DEPOSIT_AMOUNT, item.TERMS, orderCnt, item.C_LAST_MODULE_, item.QC_REQUIRED, item.QC_CODES, OrderType);
                        i++;
                    }
                }
                return orders;
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                UpdateThreads(0);
                return null;
            }
        }

        /**************************************************************************************
        * Function name:                   SetOrderToProcessing
         * 
        * What this Function Does:         Sets the order Status to I and the last module to Cartonize
         * 
         * update needed:                  Have it update the database
         * 
        * Variables passed in:             order-the order object we are working with
         *                                  PID- the processID( THIS NEED'S TO CHANGE)
         *                                 
        * Variables Declared :             None
         * 
        * returns :                        None
        * **************************************************************************************/
        public void SetOrderToProcessing(Order order, int PID)
        {
            order.Status = "I";
            order.LastModule = PID.ToString();


            // order.printOrderInfo();
        }
        /**************************************************************************************
        * Function name:                   CartonizeOrder
         * 
        * What this Function Does:         Calls the various function in the Order Class to begin cartonizing the order
         * 
         * update needed:                  if an error occures somewhere the status needs to change
         * 
        * Variables passed in:             order- the order object
         *                                  orderType- the type of order we are working with
         *                                 
        * Variables Declared :             Status- the status of the order currently
         * 
        * returns :                        Status
        * **************************************************************************************/
        public String CartonizeOrder(Order order, string orderType)
        {
            try
            {
                string Status = "C";
                int Stat = 0;
                Stat = order.getOrderLines();
                if (Stat == -1)
                    return null;

                Stat = order.CheckDeposit();
                if (Stat == -1 || Stat == 2)
                    return null;


                Stat = order.SetPieces();
                if (Stat == -1)
                    return null;

                CartonizePieces(order, orderType);
                Status = CreateOutBoundShipmentEntry(order);


                return (Status);
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                UpdateThreads(0);
                return null;
            }

        }
        /**************************************************************************************
        * Function name:                   CreateOutboundShipmentEntry
         * 
        * What this Function Does:         inserts the cartonized order into the outbound shipments and outbound shipment details table in the database
         * 
         * update needed:                  if error Status is set to R or E
         * 
        * Variables passed in:             OrderType- They type of orders we are searching for
         *                                 
        * Variables Declared :             Status: status of the order(C= compleate E & R=error)
         *                                  OrderNumber-Not sure if needed, probably can be deleted
         *                                  
        * returns :                        Status
        * **************************************************************************************/
        private string CreateOutBoundShipmentEntry(Order order)
        {
           // try
           // {
                string status = "C";
                int OrderNumber = 0;

                OrderNumber = order.InsertPiecesIntoOutboudShipments(boxes);
                order.InsertCasesIntoOutboundShipments(OrderNumber);
                order.ResortOBS();

                return status;
           // }
            //catch (Exception e)
            //{

            //    Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            //    Log.Write("Error Message: " + e.Message);
            //    Log.Write("Stack Trace: " + e.StackTrace);
            //    Log.Write("Target Site: " + e.TargetSite);
            //    Log.Write("Exiting Program");
            //    UpdateThreads(0);
            //    System.Environment.Exit(0);
            //    return null;
            //}

        }

        //got board one friday and decided to look up the number of shirts and the number to unique products that are in philly.
        //no scope for this project, but might be useful in the future
        public void GetNumberOfItemsInPhilly()
        {
            long items = 0;
            long Products = 0;
            String strConnString = "Data Source=192.168.1.2;Initial Catalog=WMS;User ID=bodek;";
            //multiple connections because each reader needs it's own connection
            SqlConnection conn = new SqlConnection(strConnString);
            SqlDataReader rdr = null;
            string sql = "select Quantity from INVENTORY where Quantity <> 0 and Quantity is not null";

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string temp = rdr["Quantity"].ToString();
                //Console.WriteLine(temp);
                items += Convert.ToInt64(temp);
            }
            conn.Close();
            Console.WriteLine("Number of pieces in the Philly Warehouse: " + items);

            string sql2 = "select distinct product_id from INVENTORY where Quantity <> 0 and Quantity is not null";
            SqlCommand cmd2 = new SqlCommand(sql2, conn);
            conn.Open();
            rdr = cmd2.ExecuteReader();

            while (rdr.Read())
            {

                Products++;
            }
            Console.WriteLine("Number of Different Products in the Philly Warehouse: " + Products);
            conn.Close();
            Console.ReadLine();


        }
        /**************************************************************************************
        * Function name:                   CartonizePieces
         * 
        * What this Function Does:         break the order down into pieces and cases
         * 
         * update needed:                  none
         * 
        * Variables passed in:             order- the order object
         *                                 orderType- the type of order we are processing 
         *                                 
        * Variables Declared :             i- counter for the order array
         *                                  orderCnt- number of orders we are reading in
         * 
        * returns :                        Orders
        * **************************************************************************************/
        public void CartonizePieces(Order order, string orderType)
        {
            try
            {
                int boxCount = boxes[0].BoxCnt;
                // boxes[0] should be XLarge box
                double MaxVolume = Convert.ToDouble(boxes[0].GetMaxVolume());
                int totalPieces;
                double totalVolumeRemaining = 0;
                // bool chairFound = false;
                int chairsOrdered = 0;

                bool DoTheyFit;

                totalPieces = order.CountTotalPieces();


                DoTheyFit = order.WillOneItemFitInLargestCarton(MaxVolume);
                if (DoTheyFit == false)
                {
                    Log.Write("Something will not fit in on of our boxes, this is bad and should not happen");
                    Environment.Exit(1);
                }

                chairsOrdered = order.FindChairs();
                if (chairsOrdered > 0)
                {
                    Log.Write("someone ordered" + chairsOrdered + "Chairs");
                    //chairFound = true;
                }
                else
                {
                    Log.newLine();
                    Log.Write("No Chairs Found");
                    Log.newLine();

                }


                totalVolumeRemaining = order.GetTotalVolumeOfPiecesRemaining();

                do
                {
                    totalVolumeRemaining = order.AssignPiecesToCarton(totalVolumeRemaining, boxes, orderType);
                    if (totalVolumeRemaining == -1)
                    {
                        UpdateThreads(0);
                        System.Environment.Exit(0);
                    }




                } while (totalVolumeRemaining > 0);
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                UpdateThreads(0);
                System.Environment.Exit(0);
            }


        }

        /**************************************************************************************
        * Function name:                   CreatePicks
         * 
        * What this Function Does:         once the order is broken down, this function will look for the inventory to try and fill the order
         * 
         * update needed:                  error handeling
         * 
        * Variables passed in:             order- the order object
         *                                 orderType- the type of order we are processing 
         *                                 
        * Variables Declared :             AllocationStatus: the status of the order being allocated(   C:compleate
         *                                                                                              E:Error
         *                                                                                              R:Error
         *                                                                                              S:short)
         *                                 OrderCancelRequest- bool value to see if the order was canceled;
         * 
        * returns :                        Nothing
        * **************************************************************************************/
        internal void CreatePicks(Order Orders, string orderType)
        {
            string allocationstatus;
            bool OrderCancelRequest = false;
            allocationstatus = Orders.PiecePickCreation();

            Log.Write("ALLOCATION STATUS: " + allocationstatus);

            if ((allocationstatus == "E" || allocationstatus == "R") || (allocationstatus == "S"))
            {
                Log.Write("*********************************************************************************");
                Log.Write("COULD NOT ALLOCATE INVENTORY FOR ORDER " + Orders.OrderID + ".....ROLLING BACK");
                Log.Write("*********************************************************************************");

                Orders.RemoveAllocation(allocationstatus, Orders.OrderID);
                Orders.UpdateHostOrderStatus(allocationstatus, OrderCancelRequest, orderType);
            }
            else
            {
                using (WMSEntities wms = new WMSEntities())
                {
                    
                        Log.Write("Sucessful Allocation");
                        if ((allocationstatus == "C"))
                        {

                            Orders.UpdateStatuses("N");
                            Orders.UpdateHostOrderStatus(allocationstatus, OrderCancelRequest, orderType);
                        }
                    
                }
            }
        }




        /**************************************************************************************
       * Function name:                   RunProgram
        * 
       * What this Function Does:         checks to see if the number of cartonize threads is less than the max threds alowed, 
         * if so it increments the threads and runs the program, if not if sleeps and tires again later. it will eventually quit.
        * 
        * update needed:                  error handeling
        * 
       * Variables passed in:            none 
        *                                 
       * Variables Declared :            
        * 
       * returns :                        true if open threads, false if no open threads
       * **************************************************************************************/
        internal bool RunProgram()
        {
            int threads = -1;
            int count = 0;
            while (count < 3)
            {
                threads = GetThreads();
                if (threads == 0)
                {
                    threads++;
                    MaxThreads = GetMaxThreads();
                    UpdateThreads(threads);
                    UpdateMaxThreads();
                    return true;
                }
                else
                {
                    Log.Write("Cartonize is Running, trying again in 30 seconds " + (3 - count) + " tries left");
                    Thread.Sleep(30000);

                    count++;
                }


            }
            return false;
        }

        internal void UpdateMaxThreads()
        {
            //if max threads was not 1, set it to 1
            Log.Write("threads allowed: " + MaxThreads);
            if (MaxThreads > 0 && MaxThreads != 1)
            {
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<SYSTEM> Data = from S in wms.SYSTEMs
                                              where
                                                   S.NAME == "Cartonize_threads_allowed"
                                              select S;

                    foreach (SYSTEM sysData in Data)
                    {
                        sysData.VALUE = "1";
                        sysData.C_LAST_UPDATED_ = DateTime.Now;
                        sysData.C_LAST_USER_ = "ATS";
                        sysData.C_LAST_MODULE_ = "WHT";
                    }
                    wms.SaveChanges();
                    currentThreadCount = 1;
                }
            }



            if (currentThreadCount == 1 && currentThreadCount != MaxThreads)
            {
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<SYSTEM> Data = from S in wms.SYSTEMs
                                              where
                                                   S.NAME == "Cartonize_threads_allowed"
                                              select S;

                    foreach (SYSTEM sysData in Data)
                    {
                        sysData.VALUE = MaxThreads.ToString();
                        sysData.C_LAST_UPDATED_ = DateTime.Now;
                        sysData.C_LAST_USER_ = "ATS";
                        sysData.C_LAST_MODULE_ = "WHT";
                    }
                    wms.SaveChanges();
                    currentThreadCount = 0;
                }
            }
        }


        private int GetMaxThreads()
        {
            int threads = -1;
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<SYSTEM> Data = from s in wms.SYSTEMs where s.NAME == "Cartonize_threads_allowed" select s;

                foreach (SYSTEM x in Data)
                {
                    threads = Convert.ToInt32(x.VALUE);
                }
            }
            return threads;
        }

        private int GetThreads()
        {
            try
            {


                int threads = -1;
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<SYSTEM> Data = from s in wms.SYSTEMs where s.NAME == "Cartonize_threads" select s;

                    foreach (SYSTEM x in Data)
                    {
                        threads = Convert.ToInt32(x.VALUE);
                    }
                }
                return threads;
            }
            catch(Exception e)
            {
                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                return -1;
            }
        }

        private void UpdateThreads(int threadValue)
        {

            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<SYSTEM> Data = from S in wms.SYSTEMs
                                          where
                                               S.NAME == "Cartonize_threads"
                                          select S;

                foreach (SYSTEM sysData in Data)
                {
                    sysData.VALUE = threadValue.ToString();
                    sysData.C_LAST_UPDATED_ = DateTime.Now;
                    sysData.C_LAST_USER_ = "ATS";
                    sysData.C_LAST_MODULE_ = "WHT";
                }
                wms.SaveChanges();
            }
        }

        internal void ResetThread()
        {
            int threads = GetThreads();
            if (threads > 0)
            {
                threads--;
            }
            UpdateThreads(threads);
        }
    }
}
