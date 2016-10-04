using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;

namespace WarehouseTransfer
{
    public class Order
    {
        DebugLog Log = new DebugLog();
        public string OrderID { get; set; }
        public string TypeOfOrder { get; set; }
        public string Status { get; set; }
        public string ExpectedInd { get; set; }
        public string CarrierID { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; }
        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }

        }
        public decimal? Freight { get; set; }
        public float? DepositAmount { get; set; }
        public short? Terms { get; set; }
        public string LastModule { get; set; }
        public string QCRequired { get; set; }
        public string QCCodes { get; set; }
        public int OrderCount { get; set; }
        public double GoodsCost { get; set; }
        public OrderLine[] OrderLines = new OrderLine[9999];
        public Piece[] Pieces = new Piece[9999];
        private int PieceCount = 0;
        private int OBSDCount = 0;
        private int OBSCount = 0;
        public int CartonID = 0;
        private int InvAllCount = 0;
        private struct InvAvail
        {
            public int InventoryRID { get; set; }
            public int MoveQRID { get; set; }
            public string FromLocation { get; set; }
            public string MoveQStatus { get; set; }
            public char zone { get; set; }
            public string Aisle { get; set; }
            public string Bin { get; set; }
            public char Level { get; set; }
        }

        public OutboundShipments[] OBS = new OutboundShipments[9999];
        public OutboundShipmentDetails[] OBSD = new OutboundShipmentDetails[9999];
        public InventoryAllocation[] InvAllocat = new InventoryAllocation[9999];
        public HostOrder Host = new HostOrder();



        //basic constructor
        public Order()
        {
            OrderID = "";
            Status = "";
            ExpectedInd = "";
            CarrierID = "";
            LastUpdate = DateTime.Now;
            LastUser = "";

            Freight = 0;
            DepositAmount = 0;
            Terms = 0;

        }
        //this constructor takes in information from host order
        public Order(string InOrderID, string InStatus, string inExpectedInd, string InCarrierID, DateTime InLastUpdate,
            string InLastUser, decimal? InFreight, float? InDepositAmount, short? InTerms, int InOrderCnt,
            string InLastModule, string InQCRequired, string InQCCodes, string InOrderType)
        {
            OrderID = InOrderID;
            Status = InStatus;
            ExpectedInd = inExpectedInd;
            CarrierID = InCarrierID;
            LastUpdate = InLastUpdate;
            LastUser = InLastUser;
            Freight = InFreight;
            DepositAmount = InDepositAmount;
            Terms = InTerms;
            OrderCount = InOrderCnt;
            QCRequired = InQCRequired;
            QCCodes = InQCCodes;
            TypeOfOrder = InOrderType;
            Host = new HostOrder(InOrderID);




        }

        /**************************************************************************************
        * Function name:                  UodateHostOrdersTable     
         * 
        * What this Function Does:         Updates the Host order table to begin cartonizing    
         * 
         * update needed:                  This needs to be moved to the HostOrders Object
         * 
        * Variables passed in:             None
         *                                 
        * Variables Declared :             none
         * 
        * returns :                        Nothing
        * **************************************************************************************/
        public int updateHostOrdersTable()
        {
            try
            {
                using (WMSEntities wms = new WMSEntities())
                {
                    IQueryable<HOST_ORDERS> Data = from H in wms.HOST_ORDERS where H.EIS_ORDER_ID == OrderID select H;

                    foreach (HOST_ORDERS x in Data)
                    {
                        x.STATUS = Status;
                        x.EXPEDITE_IND = ExpectedInd;
                        x.CARRIER_ID = CarrierID;
                        x.C_LAST_UPDATED_ = LastUpdate;
                        x.C_LAST_USER_ = Environment.MachineName;
                        x.FREIGHT = Freight;
                        x.DEPOSIT_AMOUNT = DepositAmount;
                        x.TERMS = Terms;
                    }

                    wms.SaveChanges();
                    return 0;
                }
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                return -1;

            }
        }
        /**************************************************************************************
        * Function name:                  PrintOrderInfo       
         * 
        * What this Function Does:         Updates the Host order table to begin cartonizing    
         * 
         * update needed:                  This needs to be moved to the HostOrders Object
         * 
        * Variables passed in:             None
         *                                 
        * Variables Declared :             none
         * 
        * returns :                        Nothing
        * **************************************************************************************/
        //prints all the information in this object to the debug log.( needs update for the arrays)
        public void printOrderInfo()
        {
            Log.Write("Order ID: " + OrderID);
            Log.Write("Order Status: " + Status);
            Log.Write("Expected Ind: " + ExpectedInd);
            Log.Write("Carrier ID: " + CarrierID);
            Log.Write("Last Updated: " + LastUpdate);
            Log.Write("Last User: " + LastUser);
            Log.Write("Current Date: " + CurrentDate);
            Log.Write("Freight: " + Freight);
            Log.Write("Deposit Amount: " + DepositAmount);
            Log.Write("Terms: " + Terms);
            Log.Write("Last Module: " + LastModule);
            Log.Write("Order Count : " + OrderCount);
            Log.newLine();
            Log.newLine();

        }
        //gets the information from host order details and puts it into the orderline object
        public int getOrderLines()
        {
            try
            {
                int orderLineCnt;
                using (WMSEntities wms = new WMSEntities())
                {
                    var data = from H in wms.HOST_ORDER_DETAIL where H.EIS_ORDER_ID == OrderID select H;
                    orderLineCnt = (from H in wms.HOST_ORDER_DETAIL where H.EIS_ORDER_ID == OrderID select H).Count();
                    int i = 0;
                    Log.Write("Number of OrderLines: " + orderLineCnt);
                    Log.Write("\n");
                    foreach (var Line in data)
                    {

                        OrderLines[i] = new OrderLine(Line.EIS_ORDER_ID, Line.EIS_ORDER_LINE, Line.TYPE, Line.PRODUCT_ID,
                            Line.ORDERED_UNITS, Line.UOM, Line.SCHEDULED_DATE, Line.NOTES, Line.EIS_ORDLINE_PRICE, Line.EIS_UNIT_PRICE, orderLineCnt);
                        // 
                        i++;

                    }
                    GetGoodsCost();
                    return 1;
                }
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");

                return -1;
            }

        }
        //sets the total price of all the goods
        public void GetGoodsCost()
        {
            for (int i = 0; i < OrderLines[0].OrderLineCount; i++)
            {
                GoodsCost += OrderLines[i].GetLinePrice();
                //OrderLines[i].printOrderLineInfo();
            }

        }
        //checks to make sure the deposit amount is not more than the order
        public int CheckDeposit()
        {
            try
            {
                double CODAmount;
                if (Terms == 1 || Terms == 2)
                {
                    CODAmount = Math.Round((GoodsCost + Convert.ToDouble(Freight)), 2);
                    if (Convert.ToDouble(DepositAmount) > CODAmount)
                    {
                        Log.Write("The Deposit was more than the Amount of the order Exiting Program");
                        return 2;
                    }

                }
                return 0;
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                return -1;

            }


        }
        //sets the pieces array. we will use this to assing left over pieces to cartons 
        public int SetPieces()
        {
            try
            {
                for (int i = 0; i < OrderLines[0].OrderLineCount; i++)
                {
                    if (OrderLines[i].LineHasPieces() == true)
                    {
                        Pieces[PieceCount] = new Piece();

                        Pieces[PieceCount].ProductID = OrderLines[i].ProductID;
                        Pieces[PieceCount].TotalVolume = OrderLines[i].GetTotalVolumeOfPieces();
                        Pieces[PieceCount].PiecesToCartonize = OrderLines[i].GetPiecesToCartonize();
                        Pieces[PieceCount].PiecePickLocation = OrderLines[i].PiecePickLocation;
                        Pieces[PieceCount].BreakApartInformation();
                        Pieces[PieceCount].PrintPieceInfo();
                        Pieces[PieceCount].CartonID = -1;
                        PieceCount++;

                    }
                }
                Log.Write("Piece Count: " + PieceCount);
                return 0;
            }
            catch (Exception e)
            {
                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                return -1;
            }

        }
        //sorts the array by product ID then sorts it by zone, then aisle, then bin, then level
        public void SortPieces()
        {
            Piece tempPiece = new Piece();
            //sorts by product ID;
            for (int i = 0; i < PieceCount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (Convert.ToInt64(Pieces[j].ProductID) > Convert.ToInt64(Pieces[i].ProductID))
                    {
                        tempPiece = Pieces[i];
                        Pieces[i] = Pieces[j];
                        Pieces[j] = tempPiece;
                    }
                }
            }

            //for (int i = 0; i < PieceCount; i++)
            //{
            //    for (int j = 0; j < i; j++)
            //    {
            //        if ((char.ToUpper(Pieces[j].zone) - 64) > (char.ToUpper(Pieces[i].zone) - 64))
            //        {
            //            tempPiece = Pieces[i];
            //            Pieces[i] = Pieces[j];
            //            Pieces[j] = tempPiece;
            //        }
            //        else if ((char.ToUpper(Pieces[j].zone) - 64) == (char.ToUpper(Pieces[i].zone) - 64))
            //        {
            //            if (Convert.ToInt16(Pieces[j].Aisle) > Convert.ToInt16(Pieces[i].Aisle))
            //            {
            //                tempPiece = Pieces[i];
            //                Pieces[i] = Pieces[j];
            //                Pieces[j] = tempPiece;
            //            }
            //            else if (Convert.ToInt16(Pieces[j].Aisle) == Convert.ToInt16(Pieces[i].Aisle))
            //            {
            //                if (Convert.ToInt16(Pieces[j].Bin) > Convert.ToInt16(Pieces[i].Bin))
            //                {
            //                    tempPiece = Pieces[i];
            //                    Pieces[i] = Pieces[j];
            //                    Pieces[j] = tempPiece;
            //                }
            //                else if (Convert.ToInt16(Pieces[j].Bin) == Convert.ToInt16(Pieces[i].Bin))
            //                {
            //                    if ((char.ToUpper(Pieces[j].Level) - 64) > (char.ToUpper(Pieces[i].Level) - 64))
            //                    {
            //                        tempPiece = Pieces[i];
            //                        Pieces[i] = Pieces[j];
            //                        Pieces[j] = tempPiece;
            //                    }

            //                }

            //            }

            //        }
            //    }
            //}


            //for (int i = 0; i < PieceCount; i++)
            //{
            //    Pieces[i].PrintPieceInfo();
            //}

        }
        //gets the total number of pieces we need to cartonize
        public int CountTotalPieces()
        {
            int totalPieces = 0;
            for (int i = 0; i < PieceCount; i++)
            {
                totalPieces += Pieces[i].PiecesToCartonize;
            }


            return totalPieces;
        }
        //check to see if one of each of the pieces will fit in our largest box
        public bool WillOneItemFitInLargestCarton(double MaxVolume)
        {
            bool DoesItFit;
            for (int i = 0; i < PieceCount; i++)
            {
                DoesItFit = Pieces[i].WillItFit(MaxVolume);


                if (DoesItFit == false)
                {
                    return false;
                }
            }

            return true;

        }
        //checks to see if the order has any chairs in it 
        public int FindChairs()
        {
            int NumberOfChairs = 0;

            for (int i = 0; i < PieceCount; i++)
            {
                if (Pieces[i].IsThisProductAChair() == true)
                {
                    NumberOfChairs += Pieces[i].PiecesToCartonize;
                }
            }


            return NumberOfChairs;
        }
        //takes the volume of all the pieces we need to cartonize and returns that number
        public double GetTotalVolumeOfPiecesRemaining()
        {
            double VolumeRemaining = 0;

            for (int i = 0; i < PieceCount; i++)
            {
                if (Pieces[i].IsThisProductAssignedToACarton() == false)
                {
                    VolumeRemaining += Pieces[i].TotalVolume;
                }
            }

            Log.Write("the total volume to cartonize is " + VolumeRemaining + '\n');

            return VolumeRemaining;

        }
        //this fuction takes all the pieces in the order and will try and assign them to boxes
        public double AssignPiecesToCarton(double TotalVolumeRemaining, CartonInfo[] boxes, string orderType)
        {
            try
            {
                int count = 0;
                int CartonIDX = 0;
                int PreviousCarton = -1;
                int PreviousCartonID = -1;
                double PreviousCartonVolumeRemaining = 0;
                int PiecesToConsolidate = 0;
                double VolumeToConsolidate = 0;
                double eachVol = 0;
                double VolumeRemaining = 0;

                while (Pieces[count] != null)
                {
                    count++;

                }
                for (int currentProduct = 0; currentProduct < count; currentProduct++)
                {

                    Log.Write("Cartonizing Array Line" + currentProduct + "Product :" + Pieces[currentProduct].ProductID);
                    if (orderType == "WHT")
                    {

                        if (Pieces[currentProduct].IsThisProductAChair() == true)
                        {
                            //call cartonize chairs
                        }

                        if (DoWeTryNewCartonize() == true)
                        {
                            //work on after old cartonize works
                            // NewCartonize();
                        }
                        if (SplitProduct(currentProduct, boxes) == false)
                        {

                            //split not nesisary so lets put this shit in a box 
                            CartonID++;
                            Pieces[currentProduct].CartonTypeIndex = GetRightSizeCarton((Pieces[currentProduct].VolumeOfAPiece * Pieces[currentProduct].PiecesToCartonize), boxes);

                        }
                    }
                    //Tim this needs to be fixed 
                    else
                    {
                        if (currentProduct > 0)
                        {
                            PreviousCarton = currentProduct - 1;
                            PreviousCartonID = Pieces[PreviousCarton].CartonID;
                            for (int i = 0; i >= PreviousCarton; i++)
                            {
                                //we are doing this bacause we are only adusting the volume in the first orderline. i want to change this is the near future- Tim 6/17/2015
                                if (Pieces[i].CartonID == PreviousCartonID)
                                {
                                    PreviousCarton = i;
                                    break;
                                }
                            }
                            PreviousCartonVolumeRemaining = Pieces[PreviousCarton].VolumeLeftInBox;
                            if (Pieces[PreviousCarton].VolumeOfAPiece > PreviousCartonVolumeRemaining)
                            {
                                //attempt to fit all of it in one carton
                                CartonIDX = TotalVolumeRemainingToCarton(TotalVolumeRemaining, boxes, Pieces);
                                if (CartonIDX == -1)
                                    Log.Write("total volume remaining will not fit in one carton");
                                else //a piece will fit, but we dont want to assign it to a new carton which is what TotalVolumeRemainingToCarton does 
                                    CartonIDX = -1;
                            }
                        }
                        else //currentProduct=0
                        {
                            CartonIDX = TotalVolumeRemainingToCarton(TotalVolumeRemaining, boxes, Pieces);
                        }

                        if (CartonIDX == -1)
                        {
                            Log.Write("Total Volume won't fit in one carton");
                            if (currentProduct == 0)
                            {
                                if (SplitProduct(currentProduct, boxes) == false)
                                {
                                    AssignProductToLargestCarton(Pieces, boxes);
                                    CartonID++;
                                }
                            }
                            //if this was not the first product then we will try and consolidate 
                            else
                            {
                                PreviousCarton = currentProduct - 1;
                                PreviousCarton = Pieces[PreviousCarton].CartonID;

                                for (int i = 0; i < PreviousCarton; i++)
                                {
                                    if (Pieces[i].CartonID == PreviousCartonID)
                                        PreviousCarton = i;
                                }
                                PreviousCartonVolumeRemaining = Pieces[PreviousCarton].VolumeLeftInBox;
                                eachVol = Pieces[currentProduct].TotalVolume / Pieces[currentProduct].PiecesToCartonize;
                                PiecesToConsolidate = Convert.ToInt16(PreviousCartonVolumeRemaining / eachVol);
                                VolumeToConsolidate = eachVol * PiecesToConsolidate;

                                Log.Write("Previous Carton:" + PreviousCarton);
                                Log.Write("Prevvious Carton Volume Remaining: " + PreviousCartonVolumeRemaining);
                                Log.Write("EachVol: " + eachVol);

                                if (PiecesToConsolidate == 0)
                                {
                                    if (SplitProduct(currentProduct, boxes) == false)
                                    {
                                        AssignProductToLargestCarton(Pieces, boxes);
                                        CartonID++;
                                    }
                                }
                                else
                                {
                                    Log.Write("Pieces To Consolidate:" + PiecesToConsolidate);
                                    Log.Write("Pieces To Cartonize:" + Pieces[currentProduct].PiecesToCartonize);
                                    //this is a perfect fit. no need to split this product
                                    if (PiecesToConsolidate == Pieces[currentProduct].PiecesToCartonize)
                                    {
                                        Pieces[PreviousCarton].VolumeLeftInBox = Pieces[PreviousCarton].VolumeLeftInBox - VolumeToConsolidate;
                                        //  Pieces[currentProduct].VolumeLeftInBox = Pieces[PreviousCarton].VolumeLeftInBox;
                                        Pieces[currentProduct].CartonTypeIndex = Pieces[PreviousCarton].CartonTypeIndex;
                                        Pieces[currentProduct].CartonID = Pieces[PreviousCarton].CartonID;

                                    }
                                    //More Product Then will fit. we need to create a new OrderLine
                                    //to do so we increment
                                    else if (Pieces[currentProduct].PiecesToCartonize > PiecesToConsolidate)
                                    {
                                        Pieces[PieceCount].ProductID = Pieces[currentProduct].ProductID;
                                        Pieces[PieceCount].TotalVolume = Pieces[currentProduct].TotalVolume - VolumeToConsolidate;
                                        Pieces[PieceCount].PiecesToCartonize = Pieces[currentProduct].PiecesToCartonize - PiecesToConsolidate;
                                        Pieces[PieceCount].PiecePickLocation = Pieces[currentProduct].PiecePickLocation;
                                        Pieces[PieceCount].BreakApartInformation();
                                        PieceCount++;
                                        SortPieces();

                                    }
                                    //there is still more room in the box after we consolidate
                                    else
                                    {
                                        Log.Write("Pieces[currentProduct].PiecesToCartonize < PiecesToConsolidate)");
                                        Pieces[PreviousCarton].VolumeLeftInBox = Pieces[PreviousCarton].VolumeLeftInBox - Pieces[currentProduct].TotalVolume;
                                        //Pieces[currentProduct].VolumeLeftInBox = Pieces[PreviousCarton].VolumeLeftInBox;
                                        Pieces[currentProduct].CartonTypeIndex = Pieces[PreviousCarton].CartonTypeIndex;
                                        Pieces[currentProduct].CartonID = Pieces[PreviousCarton].CartonID;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Log.Write("total volume will fit in a box");

                            CartonID++;
                            CartonIDX = GetRightSizeCarton(TotalVolumeRemaining, boxes);
                            Log.Write("carton ID: " + CartonID);
                            Log.Write("carton IDX: " + CartonIDX);
                            if (CartonIDX == -1)
                            {
                                //smallest boxwas too small but use it anyway because this is the last piece to cartonize
                                CartonIDX = 0;
                            }
                            Pieces[currentProduct].VolumeLeftInBox = boxes[CartonIDX].GetMaxVolume() - TotalVolumeRemaining;
                            for (int i = currentProduct; i < PieceCount; i++)
                            {
                                Pieces[i].CartonTypeIndex = CartonIDX;
                                Pieces[i].CartonID = CartonID;
                            }

                        }
                        for (int i = 0; i < PieceCount; i++)
                        {
                            if (Pieces[i].CartonTypeIndex == -1)
                            {
                                VolumeRemaining += Pieces[i].TotalVolume;
                            }
                        }
                    }
                }
                return VolumeRemaining;
            }
            catch (Exception e)
            {

                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                return -1;
            }


        }




        private void AssignProductToLargestCarton(Piece[] Pieces, CartonInfo[] boxes)
        {
            int i = 0;
            int CurrentProd = -1;
            int CartonID;
            int LargestCartonIndex = boxes[0].BoxCnt - 1;
            while (CurrentProd == -1)
            {
                if (Pieces[i].CartonTypeIndex == -1)
                {
                    CurrentProd = i;
                }
                i++;
            }
            if (CurrentProd == 0)
            {
                CartonID = 1;
            }
            else
            {
                i = CurrentProd - 1;
                CartonID = Pieces[i].CartonID;
                CartonID++;
            }
            Pieces[CurrentProd].CartonTypeIndex = LargestCartonIndex;
            Pieces[CurrentProd].VolumeLeftInBox = boxes[LargestCartonIndex].GetMaxVolume() - Pieces[CurrentProd].TotalVolume;
            Pieces[CurrentProd].CartonID = CartonID;

        }
        /*******************************************
        *returns true is multiple product in this order are of the same model and color but not the aame size
        *I.E. if a order has 2 large red and two medium red then we reeturn true
         *******************************************/
        public bool DoWeTryNewCartonize()
        {
            for (int i = 0; i < PieceCount; i++)
            {
                Pieces[i].BreakApartProductID();
            }

            for (int i = 0; i < PieceCount; i++)
            {
                for (int j = 0; j < PieceCount; j++)
                {
                    if (Pieces[i].ShortProductID == Pieces[j].ShortProductID && Pieces[i].PiecesToCartonize > 1 && Pieces[i].PiecesToCartonize > 1)
                    {
                        Log.Write("GOING TO USE NEW CARTONIZE");
                        return true;
                        //maybe put call for new cartonize here
                    }
                }
            }
            Log.Write("Not using new cartonize");
            return false;
        }

        public void NewCartonize()
        {
            //add like products to the same box then add everything else ontop
        }
        //we have too many of a product to fit in a single box. we are going to split it up
        public bool SplitProduct(int CurrentProduct, CartonInfo[] Boxes)
        {
            Log.Write("calling SplitProduct");
            int MaxPiecesPerCarton = 0;
            double MaxPiecesPerCartonVolume = 0;
            int fullCartons = 0;
            int largestCartonIndex = 0;//we sort the carton array so the largest is in index 0

            MaxPiecesPerCarton = Convert.ToInt16(Boxes[0].GetMaxVolume() / Pieces[CurrentProduct].VolumeOfAPiece);
            MaxPiecesPerCartonVolume = MaxPiecesPerCarton / Pieces[CurrentProduct].VolumeOfAPiece;
            Log.Write("Pieces to cartonize: " + Convert.ToInt16(Pieces[CurrentProduct].PiecesToCartonize));
            fullCartons = Convert.ToInt16(Pieces[CurrentProduct].PiecesToCartonize / MaxPiecesPerCarton);

            if (fullCartons == Pieces[CurrentProduct].PiecesToCartonize / MaxPiecesPerCarton)
            {

                fullCartons = fullCartons - 1;
            }

            for (int i = 1; i < fullCartons; i++)
            {

                CartonID++;
                Pieces[PieceCount] = new Piece();
                Pieces[PieceCount].ProductID = Pieces[CurrentProduct].ProductID;
                Pieces[PieceCount].TotalVolume = MaxPiecesPerCartonVolume;
                Pieces[PieceCount].PiecesToCartonize = MaxPiecesPerCarton;
                Pieces[PieceCount].CartonTypeIndex = largestCartonIndex;
                Pieces[PieceCount].VolumeLeftInBox = Boxes[largestCartonIndex].GetMaxVolume() - Pieces[PieceCount].TotalVolume;
                Pieces[PieceCount].CartonID = CartonID;
                Pieces[PieceCount].PiecePickLocation = Pieces[CurrentProduct].PiecePickLocation;
                Pieces[PieceCount].BreakApartInformation();
                PieceCount++;

                Pieces[CurrentProduct].TotalVolume = Pieces[CurrentProduct].TotalVolume - MaxPiecesPerCartonVolume;
                Pieces[CurrentProduct].PiecesToCartonize = Pieces[CurrentProduct].PiecesToCartonize - MaxPiecesPerCarton;

            }
            Log.Write("Max Pieces Per Carton: " + MaxPiecesPerCarton);
            Log.Write("Max Pieces per Carton Volume: " + MaxPiecesPerCartonVolume);
            Log.Write("full Cartons: " + fullCartons);
            if (fullCartons > 0)
            {
                Log.Write("Split Sucessful");
                SortPieces();
                return true;
            }
            else
            {
                Log.Write("No Need TO SPLIT");
                return false;
            }
        }
        //gets the right size carton based on the volume
        private int GetRightSizeCarton(double TotalVolume, CartonInfo[] Boxes)
        {
            int i = 0;


            while (i <= Boxes[0].BoxCnt)
            {

                if (Boxes[i] != null && TotalVolume >= Boxes[i].GetMaxVolume())
                {
                    Log.Write("The right size carton is: " + Boxes[i].Name);
                    return i;
                }
                else if (Boxes[i] == null)
                {
                    return i - 1;
                }
                else
                {
                    i++;
                    if (i > Boxes[0].BoxCnt)
                    {
                        return Boxes[0].BoxCnt;
                    }
                }
            }

            Log.Write("The Smallest box was too small. using carton type: " + Boxes[i - 1].Name);

            return i - 1;
        }
        //takes the volume remaining and tries to add it all into one carton
        private int TotalVolumeRemainingToCarton(double VolumeRemaining, CartonInfo[] Boxes, Piece[] Pieces)
        {
            bool MaxFit = false;
            int CartonIDX;
            int i = 0;
            int boxCount = Boxes[0].BoxCnt;

            while (i < boxCount && VolumeRemaining <= Boxes[i].GetMaxVolume())
            {
                MaxFit = true;
                if (VolumeRemaining >= Boxes[i].GetMaxVolume())
                    return i;
                else
                {
                    i++;
                    if (i > boxCount)
                    {
                        CartonIDX = Consolidate(VolumeRemaining, Pieces);
                        if (CartonIDX == -1)
                            return i - 1;

                        else
                            return CartonIDX;
                    }
                }
            }
            if (MaxFit == true)
                return i--;
            else
                return -1;

        }
        //this function will try and find a box to consolidate pieces into
        private int Consolidate(double Volume, Piece[] Pieces)
        {

            for (int i = 0; i < Pieces[0].PiecesToCartonize; i++)
            {
                if (Pieces[i].VolumeLeftInBox >= Volume)
                {
                    return i;
                }
            }
            return -1;
        }
        //this function will re sort the Pieces array having pieces in the same carton next to each other 
        public void SortPiecesByCartonID()
        {

            Piece temp = new Piece();

            for (int i = 0; i < PieceCount; i++)
            {
                // Console.WriteLine(Boxes[i].GetName());
                //  Console.WriteLine(Boxes[i].GetMaxVolume());
                for (int j = 0; j < i; j++)
                {
                    if (Pieces[i].CartonID > Pieces[j].CartonID)
                    {
                        temp = Pieces[j];
                        Pieces[j] = Pieces[i];
                        Pieces[i] = temp;
                    }
                }
            }
        }
        //populates the outbound shipment object for pieces( this should be in the outboundshipment object, fix this later)
        internal int InsertPiecesIntoOutboudShipments(CartonInfo[] boxes)
        {
            int OrderNumberToContinueAt = 0;

            using (WMSEntities wms = new WMSEntities())
            {

                for (int i = 0; i < CartonID; i++)
                {
                    IQueryable<HOST_ORDERS> Item = from HO in wms.HOST_ORDERS where HO.EIS_ORDER_ID == OrderID select HO;
                    foreach (HOST_ORDERS h in Item)
                    {
                        OBS[OBSCount] = new OutboundShipments();
                        OBS[OBSCount].ShipmentID = h.EIS_ORDER_ID;
                        OBS[OBSCount].Type = h.TYPE;
                        OBS[OBSCount].Customer_ID = h.CUSTOMER_ID;
                        OBS[OBSCount].Name = h.NAME;
                        OBS[OBSCount].Address1 = h.ADDRESS_1;
                        OBS[OBSCount].Address2 = h.ADDRESS_2;
                        OBS[OBSCount].Address_3 = h.ADDRESS_3;
                        OBS[OBSCount].City = h.CITY;
                        OBS[OBSCount].State = h.STATE;
                        OBS[OBSCount].Zip = h.ZIP;
                        OBS[OBSCount].Phone = h.PHONE;
                        OBS[OBSCount].PrepaidCollect = h.FREIGHT_BILLING_OPTION;
                        OBS[OBSCount].Purchase_order = h.PURCHASE_ORDER;
                        OBS[OBSCount].PurchasOrderDate = h.ORDER_DATE;
                        OBS[OBSCount].ScheduledShipDate = DateTime.Now;
                        OBS[OBSCount].OrigionalCarrier = h.CARRIER_ID;
                        OBS[OBSCount].Carrier_id = h.CARRIER_ID;
                        OBS[OBSCount].Original_ship_mode = h.ORIGINAL_SHIP_MODE;
                        OBS[OBSCount].Ship_mode = h.ORIGINAL_SHIP_MODE;
                        OBS[OBSCount].Payment_terms = h.TERMS.ToString();
                        OBS[OBSCount].Notes = h.NOTES;
                        OBS[OBSCount].Deposit_amount = h.DEPOSIT_AMOUNT;
                        OBS[OBSCount].Freight_bill_to_account = h.FREIGHT_BILL_TO_ACCOUNT;
                        OBS[OBSCount].Freight_bill_To_Name = h.FREIGHT_BILL_TO_NAME;
                        OBS[OBSCount].Freight_bill_to_address_1 = h.FREIGHT_BILL_TO_ADDRESS_1;
                        OBS[OBSCount].Freight_bill_to_address_2 = h.FREIGHT_BILL_TO_ADDRESS_2;
                        OBS[OBSCount].Freight_bill_to_address_3 = h.FREIGHT_BILL_TO_ADDRESS_3;
                        OBS[OBSCount].Freight_bill_to_city = h.FREIGHT_BILL_TO_CITY;
                        OBS[OBSCount].Freight_bill_to_state = h.FREIGHT_BILL_TO_STATE;
                        OBS[OBSCount].freight_bill_to_Zip = h.FREIGHT_BILL_TO_ZIP;
                        OBS[OBSCount].QC_Codes = h.QC_CODES;
                        OBS[OBSCount].Deliver_By_Date = h.DELIVER_BY_DATE;
                        OBS[OBSCount]._Last_User_ = Environment.MachineName;
                        OBS[OBSCount]._last_module_ = "CARTONIZE";


                        if (h.COD_INDICATOR == "")
                            OBS[OBSCount].COD_Indicator = "0";
                        else
                            OBS[OBSCount].COD_Indicator = h.COD_INDICATOR;

                        if (h.CARRIER_ID == "Active FM")
                            OBS[OBSCount].Address_3 = h.NAME;


                    }

                    int OrderLine = 0;
                    OBS[OBSCount].OrderNumber = OBS[OBSCount].getOrderNumber(i + 1);
                    OrderNumberToContinueAt = i + 1;
                    if (Pieces[i].CartonTypeIndex == -1)
                    {
                        Pieces[i].CartonTypeIndex = 0;
                    }

                    OBS[OBSCount].CartonType = boxes[Pieces[i].CartonTypeIndex].Name;
                    //OBS[OBSCount].AddToDatabase();
                    OBS[OBSCount].PrintOutboundShipmentData();
                    //add to details
                    if (TypeOfOrder == "WHT")
                    {
                        for (int k = 0; k < OrderLines[0].OrderLineCount; k++)
                        {

                            if (Pieces[i].ProductID == OrderLines[k].ProductID)
                            {

                                CreatePieceOutboundShipmentDetail(1, Pieces[i], OrderLines[k], OBS[OBSCount], OBSDCount);
                                OBSDCount++;
                            }
                        }
                    }

                    else
                    {
                        for (int j = 0; j < PieceCount; j++)
                        {
                            if (Pieces[j].CartonID == i)
                            {

                                OrderLine++;

                                for (int k = 0; k < OrderLines[0].OrderLineCount; k++)
                                {

                                    if (Pieces[j].ProductID == OrderLines[k].ProductID)
                                    {

                                        CreatePieceOutboundShipmentDetail(OrderLine, Pieces[j], OrderLines[k], OBS[OBSCount], OBSDCount);
                                        OBSDCount++;
                                    }
                                }
                            }
                        }
                    }
                    OBSCount++;
                }

            }
            return OrderNumberToContinueAt;

        }
        //populates the outbound shipment details object( this should be a constructor in OBSD)
        private void CreatePieceOutboundShipmentDetail(int OrderLine, Piece piece, OrderLine orderLine, OutboundShipments outboundShipments, int index)
        {
            OBSD[index] = new OutboundShipmentDetails();

            OBSD[index].ShipmentID = OBS[OBSCount].ShipmentID;
            OBSD[index].OrderNumber = OBS[OBSCount].OrderNumber;
            OBSD[index].OrderLine = Convert.ToInt16(OrderLine);
            OBSD[index].Type = OBS[OBSCount].Type;
            OBSD[index].ProductID = piece.ProductID;
            OBSD[index].QCCategory = "";
            OBSD[index].UnitOfMesure = "EA";
            OBSD[index].UOMWeight = orderLine.ProductInfo.PieceWeight;
            OBSD[index].OrderedUnits = piece.PiecesToCartonize;
            OBSD[index].StagedUnits = 0;
            OBSD[index].ShippedUnits = 0;
            OBSD[index].Status = "B";
            OBSD[index].UnitPriceEach = (decimal?)orderLine.ProductInfo.PricePerEach;
            OBSD[index].Notes = OBS[OBSCount].Notes;
            OBSD[index].LastUpdated = OBS[OBSCount]._Last_Updated_;
            OBSD[index].LastUser = OBS[OBSCount]._Last_User_;
            OBSD[index].LastModule = OBS[OBSCount]._last_module_;
            OBSD[index].AllocationStatus = "N";
            // OBSD[index].AddToDatabase();


        }
        //creates a new outboundshipmentdetail object and stores it in the array
        internal void InsertCasesIntoOutboundShipments(int OrderNumber)
        {
            int OrderLine = 1;
            int idx = OrderNumber;
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<HOST_ORDERS> Item = from HO in wms.HOST_ORDERS where HO.EIS_ORDER_ID == OrderID select HO;
                for (int i = 0; i < OrderLines[0].OrderLineCount; i++)
                {
                    foreach (HOST_ORDERS h in Item)
                    {
                        for (int j = 1; j <= OrderLines[i].ProductInfo.CaseQuantity; j++)
                        {
                            OBS[OBSCount] = new OutboundShipments(OrderNumber, OrderID, "CSE");
                            OrderNumber++;
                            CreateCaseOutboundShipmentDetail(OrderLine, OrderLines[i], OBSDCount);
                            OBSCount++;
                            OBSDCount++;
                        }

                    }

                }
            }
            Log.Write("number of items in OBS " + OBSCount);
        }
        //creates a outbound shipment detail object for a case
        private void CreateCaseOutboundShipmentDetail(int OrderLine, OrderLine orderLine, int index)
        {
            OBSD[index] = new OutboundShipmentDetails(OrderLine, orderLine, index, OBS[OBSCount]);
            /*OBSD[index].ShipmentID = OBS[OBSCount].ShipmentID;
            OBSD[index].OrderNumber = OBS[OBSCount].OrderNumber; 
            OBSD[index].OrderLine = (short)OrderLine;
            OBSD[index].Type = OBS[OBSCount].Type; 
            OBSD[index].UnitOfMesure = "CSE"; 
            OBSD[index].QCCategory = ""; 
            OBSD[index].ShippedUnits = 0;
            OBSD[index].LastUpdated = OBS[OBSCount]._Last_Updated_;
            OBSD[index].LastUser = OBS[OBSCount]._Last_User_;
            OBSD[index].LastModule = OBS[OBSCount]._last_module_;
            OBSD[index].ProductID = orderLine.ProductInfo.ProductID;
            OBSD[index].UOMWeight = orderLine.ProductInfo.CaseWeight;
            OBSD[index].OrderedUnits = orderLine.ProductInfo.CaseQuantity;
            OBSD[index].UnitPriceEach = (decimal?)orderLine.ProductInfo.PricePerEach;
            OBSD[index].Status = "B";
            OBSD[index].AllocationStatus = "N";
            OBSD[index].StagedUnits = 0;
            OBSD[index].AddToDatabase();*/

        }
        //returns the number of indexes that are not null in the outbound shipment details array
        private int GetTotalNumberOfOBSDLines()
        {
            int total = 0;
            for (int i = 0; i < 9999; i++)
            {
                if (OBSD[i] != null)
                    total++;
            }
            return total;
        }
        //returns the number of indexes that are not null in the outbound shipment array
        private int GetTotalNumberofOBSLines()
        {
            int total = 0;
            for (int i = 0; i < 9999; i++)
            {
                if (OBS[i] != null)
                    total++;
            }
            return total;
        }
        //creats picks for orders
        internal string PiecePickCreation()
        {
            int InventoryAvliable;
            int InventoryCount = 0;
            int PiecesToAllocate = 0;
            string CaseOrderStatus = "A";
            string PickOrderStatus = "A";

            //index counter for inventory allocation table object array
            int inventoryAllocatedCount = 0;

            for (int i = 0; i < GetTotalNumberOfOBSDLines(); i++)
            {
                string LineStatus = "";
                if (OBSD[i].UnitOfMesure == "EA")
                {
                    if (OBSD[i].AllocationStatus == "N")
                    {
                        if (OBSD[i] == null)
                        {
                            //this is really bad and should never happen
                            Log.Write("well Shit");
                        }
                        else
                        {
                            LineStatus = "A";
                            ProductMaster ProductMasterInfo = new ProductMaster(OBSD[i].ProductID);
                            ProductMasterInfo.CheckIfProductIDisNull();
                            ProductMasterInfo.CheckifPiecePickLocationIsNull();
                            Locations location = new Locations(ProductMasterInfo.PiecePickLocation);
                            InventoryTable[] InvTable = new InventoryTable[100];
                            InventoryAllocationTabel[] InvAllTable = new InventoryAllocationTabel[100];
                            InventoryAvliable = GetAvliableInventory(location.LocationID, ProductMasterInfo.ProductID, InvTable, InventoryCount, InvAllTable, inventoryAllocatedCount);
                            InventoryAvliable += GetReplenQuantity(location.LocationID, InvTable, InventoryCount);
                            InvAllocat[InvAllCount] = new InventoryAllocation(ProductMasterInfo, location, OBSD[i], "CASEPICK");

                            Log.Write("Inventory Avliable to be Picked for " + OBSD[i].ProductID + ": " + InventoryAvliable);
                            PiecesToAllocate = Convert.ToInt16(OBSD[i].OrderedUnits);
                            if (InventoryAvliable < PiecesToAllocate)
                            {
                                Log.Write("Not enough Inventory avliable in Piece Pick Location:");
                                Log.Write("Total avliable Quantity: " + InventoryAvliable);
                                Log.Write("Total Pieces needed: " + OBSD[i].OrderedUnits);
                                //allocate the inventory avliable before generating the replen
                                if (InventoryAvliable > 0)
                                {
                                    PiecesToAllocate = CreatePiecePick(ProductMasterInfo.PiecePickLocation, OBSD[i].ProductID, location.PickingZone, OBSD[i].ShipmentID,
                                        OBSD[i].OrderNumber, OBSD[i].OrderLine, Convert.ToInt16(OBSD[i].OrderedUnits), CarrierID);
                                }

                                // PiecesToAllocate= GenerateReplen(OBSD[i].ShipmentID, OBSD[i].OrderLine, OBSD[i].OrderNumber, ProductMasterInfo.PiecePickLocation,
                                //    location.PickingZone, OBSD[i].ProductID, PiecesToAllocate);

                                if (PiecesToAllocate == -1)
                                {
                                    //handel error
                                    Log.Write("Database Locked or something");
                                    //exit
                                }

                                if (PiecesToAllocate == 0)
                                {
                                    Log.Write("Full Replenishment Sucessfully ordered");
                                }
                                else if (PiecesToAllocate > 0)
                                {
                                    Log.LogError(OBSD[i].ShipmentID, OBSD[i].OrderNumber, Convert.ToInt16(OBSD[i].OrderLine), OBSD[i].ProductID, "Not Enough Inventory In Piece Pick Location");
                                    LineStatus = "S";
                                }
                                else
                                {
                                    Log.Write("No Replenishment Created for Order " + OrderID + " Product ID: " + OBSD[i].ProductID);
                                    LineStatus = "S";
                                }
                            }
                            else
                            {
                                if (InventoryAvliable == 0)
                                {
                                    Log.Write("No Inventroy Avliable Anywhere for Order: " + OrderID + " Order Line: " + OBSD[i].OrderLine);
                                    Log.Write("Product ID: " + OBSD[i].ProductID + " Quantity Required: " + PiecesToAllocate);
                                    Log.LogError(OBSD[i].ShipmentID, OBSD[i].OrderNumber, Convert.ToInt16(OBSD[i].OrderNumber), OBSD[i].ProductID, "No Inventory Avliable Anywhere");
                                    Host = new HostOrder(OBSD[i].ShipmentID);
                                    Host.UpdateStatus("E");
                                    LineStatus = "S";

                                }
                                else
                                {
                                    PiecesToAllocate = CreatePiecePick(ProductMasterInfo.PiecePickLocation, OBSD[i].ProductID, location.PickingZone, OBSD[i].ShipmentID,
                                         OBSD[i].OrderNumber, OBSD[i].OrderLine, Convert.ToInt16(OBSD[i].OrderedUnits), CarrierID);
                                }
                                if (PiecesToAllocate > 0)
                                {
                                    PiecesToAllocate = GenerateReplen(OBSD[i].ShipmentID, OBSD[i].OrderLine, OBSD[i].OrderNumber, ProductMasterInfo.PiecePickLocation,
                                     location.PickingZone, OBSD[i].ProductID, PiecesToAllocate);

                                    if (PiecesToAllocate > 0)
                                    {
                                        Log.Write("Line short");
                                        LineStatus = "S";
                                    }
                                }
                            }
                            if (LineStatus == "S")
                            {
                                PickOrderStatus = LineStatus;
                            }
                            OBSD[i].updateAllocationStatus(LineStatus);
                            OBSD[i].UpdateLastUpdated();
                            OBSD[i].UpdateLastModule("CARTONIZE");
                            OBSD[i].UpdateLastUser(Environment.MachineName);
                        }
                    }
                }//end Piece Pick Allocation
                //case Allocation
                else
                {
                    if (OBSD[i].AllocationStatus == "N")
                    {
                        Log.Write("Allocation a case for product line" + OBSD[i].OrderNumber);


                        ProductMaster ProductInfo = new ProductMaster(OBSD[i].ProductID);

                        //get the number of cases for this product and store the index
                        int[] CaseIndexCount = new int[9999];
                        int CaseCounter = 1;
                        /*
                        for (int j = 0; j < OBSDCount; j++)
                        {
                            if(OBSD[j].ProductID==ProductInfo.ProductID && OBSD[j].AllocationStatus=="N" && OBSD[j].UnitOfMesure=="CSE")
                            {
                               // CaseIndexCount[CaseCounter] = j;
                               // Log.Write("Index: " + CaseIndexCount[CaseCounter] + " is product: " + ProductInfo.ProductID);
                                //CaseCounter++;
                                OBSD[j].AllocationStatus = "I";
                            }
                            
                        }
                         * */
                        // Log.Write("we need " + CaseCounter + " cases for product " + ProductInfo.ProductID);

                        string ReturnedStatus = CreateCasePick(OBSD[i], ProductInfo.ProductID,
                            ProductInfo.PiecePickLocation, CaseCounter, CaseIndexCount, CarrierID, OBSD[i].OrderedUnits);
                        if (ReturnedStatus == "S")
                        {
                            CaseOrderStatus = "S";
                        }

                        OBSD[i].updateAllocationStatus(ReturnedStatus);
                        OBSD[i].UpdateLastUpdated();
                        OBSD[i].UpdateLastUser(Environment.MachineName);
                        OBSD[i].UpdateLastModule("Warehouse Transer");

                    }
                }//end case Picks

            }//end For Loop
            /* inv last resort not yet implemented need to find out what it does
            if (CaseOrderStatus == "S"&&PickOrderStatus=="A")
            {
                Log.Write("Calling INVLASTRESORT to look for non-std case Quantites.....");
                CaseOrderStatus = InvLastResort(OBSD, CarrierID);
            }
            */

            if (CaseOrderStatus == "S" || PickOrderStatus == "S")
            {
                Log.Write("One or more lines was short");
                return "S";
            }
            else
                return "C";
        }

        //try and fill a case from piece pick
        private string InvLastResort(OutboundShipmentDetails[] OBSD, string CarrierID)
        {
            int count = 0;
            int TotalAllocatedQuantity;
            int NonStandardCaseQuantity;
            string NewCaseStatus = "S";


            for (int i = 0; i < GetTotalNumberOfOBSDLines(); i++)
            {
                string LineStatus = "";
                if (OBSD[i].AllocationStatus == "S")
                {
                    if (OBSD[i].UnitOfMesure == "CSE")
                    {

                        string ProdID = OBSD[i].ProductID;




                    }
                }
            }


            Log.Write("Number of short cases: " + count);
            return NewCaseStatus;
        }

        private void ReviseOBS(OutboundShipmentDetails OBSD, OutboundShipments OBS, string ProdID, int CaseQuantity, int p1, string p2, string CarrierID, string p3)
        {
            OBSD.UpdateOrderQuantity(CaseQuantity);
            OBSD.updateAllocationStatus("A");
            OBS.updateAllocationStatus("A");
        }


        private string CreateCasePick(OutboundShipmentDetails OBSD, string ProductID, string PiecePickLocation, int NumCasesRequired, int[] CaseIndexCount, string CarrierID, int? QuantityToAllocate)
        {
            InvAvail[] AvailableInventory = new InvAvail[9999];
            DatabaseConnection DbCon = new DatabaseConnection();
            string conString;
            DataSet ds;

            int InvAvailcount = 0;
            int BulkPickCount = 10;
            bool BulkPick = false;
            //get the minimum number of cases to qualify bulk picking



            //if the number of cases >=10 then we bulk pick
            Log.Write("Number of cases Required: " + NumCasesRequired);
            if (NumCasesRequired >= BulkPickCount)
            {
                BulkPick = true;
                BulkPickCount = 1999;
            }
            else
            {
                BulkPickCount = NumCasesRequired;
            }


            Log.Write("Bulk Picking = " + BulkPick);

            //read in avliable inventory in the primary locations
            var SQL1 = @"Select I._RID_ INVRID, I.LOCATION_ID LOCID, 
              0 MQRID, 'N' MQSTATUS, (select count(*) from inventory where product_id='"+ProductID+ @"' and location_ID=I.Location_ID )as Caseqty From
           Inventory I with(NOLOCK), LOCATIONS L with(NOLOCK),LOOKUP_VALUES LV with(NOLOCK) 
             Where I.LOCATION_ID  = L.LOCATION_ID              
             And L.VLEVEL = LV.VALUE      
				And LV.TABLE_NAME = 'COMMON' 
             And LV.FIELD_NAME='CASEPKLEVELSEQ'                         
            And PRODUCT_ID = '" + ProductID + @"'                           
             And L.TYPE != 'X'                                 
             And (RESERVATION_TYPE = '' OR RESERVATION_TYPE IS NULL) 
             And HOLD = 0 And RESERVED = 0                   
             And QUANTITY =" + QuantityToAllocate + @"                       
            And Not Exists (Select 1 From Move_Queue M  Where 
            M.INV_RID = I._RID_ ) ";

            var OrderPrimary = "Order By Caseqty desc, I.LOCATION_ID, LV.DESCRIPTION";
            var WherePrimary = " And L.VLEVEL in ('C','D','E') and allowpiecepicking <> 1";
            var whereSecondary = "And L.VLEVEL Not in ('C','D','E') and allowpiecepicking <> 1 And L.Location_ID != '" + PiecePickLocation + "' ";
            var wherePiecepick = " And L.Location_ID ='" + PiecePickLocation + "' ";



            //look for any replens going to the primary locationsy
            var SQL2 = @"Select I._RID_ INVRID, I.LOCATION_ID LOCID, 
             M._RID_  MQRID, M.STATUS MQSTATUS,(select count(*) from inventory where product_id='" + ProductID + @"'and location_ID=I.Location_ID )as Caseqty From 
            Inventory  I with(NOLOCK),
            MOVE_QUEUE  M with(NOLOCK), 
            LOOKUP_VALUES  LV with(NOLOCK),       
            LOCATIONS  L with(NOLOCK)           
             Where  I._RID_ = M.INV_RID                         
             AND M.FROM_LOCATION = L.LOCATION_ID                
             AND L.VLEVEL   = LV.VALUE                          
             AND PRODUCT_ID =" + ProductID + @"             
             AND M.TYPE  = 'REPLEN'                            
             AND (M.SHIPMENT_ID = '' OR M.SHIPMENT_ID IS NULL)  
             AND LV.TABLE_NAME = 'COMMON'                       
             AND LV.FIELD_NAME='CASEPKLEVELSEQ'                 
             And RESERVATION_TYPE = 'REPLEN'                   
            And HOLD = 0 And RESERVED = 0                     
             And QUANTITY =" + QuantityToAllocate;



            var SQL3 = @"Select I._RID_ INVRID, M.TO_LOCATION LOCID, 
             M._RID_ MQRID, M.STATUS MQSTATUS From 
            Inventory   I with(NOLOCK), 
            MOVE_QUEUE  M with(NOLOCK)         
             Where I._RID_        = M.INV_RID                   
            AND M.TO_LOCATION  =" + PiecePickLocation + @"   
            AND I.PRODUCT_ID =   " + ProductID + @"    
             AND M.SHIPMENT_ID  IS NOT NULL 
             And I.RESERVATION_TYPE = 'REPLEN'   
             And I.HOLD = 0 And I.RESERVED = 1 
             And I.QUANTITY = " + QuantityToAllocate;

            string sql = SQL1 + WherePrimary + OrderPrimary;


            DbCon = new DatabaseConnection();
            SqlConnection Con = new SqlConnection(DbCon.GetPhillyConString());
            SqlDataReader MyReader = null;
            SqlCommand SqlCommand = new SqlCommand(sql, Con);
            Con.Open();
            MyReader = SqlCommand.ExecuteReader();

            while (MyReader.Read())
            {

                AvailableInventory[InvAvailcount] = new InvAvail();
                AvailableInventory[InvAvailcount].InventoryRID = Convert.ToInt32(MyReader["InvRID"]); ;
                AvailableInventory[InvAvailcount].MoveQRID = Convert.ToInt32(MyReader["MQRID"]);
                AvailableInventory[InvAvailcount].FromLocation = MyReader["LocID"].ToString();
                AvailableInventory[InvAvailcount].MoveQStatus = MyReader["MQStatus"].ToString();
                BreakApartToLocation(ref AvailableInventory[InvAvailcount]);


                InvAvailcount++;
            }

            Con.Close();

            if (InvAvailcount == 0)
            {
                Log.Write("No Cases in Primary Locations found. looking for replens going to primary locations");
            }
            else
            {
                Log.Write("Number of cases for product: " + ProductID + " in Primary Locations: " + InvAvailcount);
            }
            //if we didnt find enough cases we will look for replens going to Primary Locations 
            if (NumCasesRequired > InvAvailcount)
            {
                int ReplenCount = 0;
                sql = SQL2 + WherePrimary + OrderPrimary;
                DbCon = new DatabaseConnection();
                Con = new SqlConnection(DbCon.GetPhillyConString());
                MyReader = null;
                SqlCommand = new SqlCommand(sql, Con);
                Con.Open();
                MyReader = SqlCommand.ExecuteReader();
                while (MyReader.Read())
                {

                    AvailableInventory[InvAvailcount] = new InvAvail();
                    AvailableInventory[InvAvailcount].InventoryRID = Convert.ToInt32(MyReader["InvRID"]); ;
                    AvailableInventory[InvAvailcount].MoveQRID = Convert.ToInt32(MyReader["MQRID"]);
                    AvailableInventory[InvAvailcount].FromLocation = MyReader["LocID"].ToString();
                    AvailableInventory[InvAvailcount].MoveQStatus = MyReader["MQStatus"].ToString();
                    BreakApartToLocation(ref AvailableInventory[InvAvailcount]);

                    InvAvailcount++;
                    ReplenCount++;
                }
                Con.Close();

            }
            if (InvAvailcount == 0)
            {
                Log.Write("No Cases heading to primary locations found. looking for cases in secondary locations");
            }
            else
            {
                Log.Write("Number of cases for product: " + ProductID + " replens going to primary Locations: ");
            }



            //still not enough found. looking in secondary locations
            if (NumCasesRequired > InvAvailcount)
            {
                int secondaryCount = 0;
                sql = SQL1 + whereSecondary + OrderPrimary;
                DbCon = new DatabaseConnection();
                Con = new SqlConnection(DbCon.GetPhillyConString());
                MyReader = null;
                SqlCommand = new SqlCommand(sql, Con);
                Con.Open();
                MyReader = SqlCommand.ExecuteReader();
                while (MyReader.Read())
                {

                    AvailableInventory[InvAvailcount] = new InvAvail();
                    AvailableInventory[InvAvailcount].InventoryRID = Convert.ToInt32(MyReader["InvRID"]); ;
                    AvailableInventory[InvAvailcount].MoveQRID = Convert.ToInt32(MyReader["MQRID"]);
                    AvailableInventory[InvAvailcount].FromLocation = MyReader["LocID"].ToString();
                    AvailableInventory[InvAvailcount].MoveQStatus = MyReader["MQStatus"].ToString();
                    BreakApartToLocation(ref AvailableInventory[InvAvailcount]);
                    InvAvailcount++;
                    secondaryCount++;

                }
                Con.Close();
                if (InvAvailcount == 0)
                {
                    Log.Write("No Cases in Secondary locations found. looking for replens in secondary locations");
                }
                else
                {
                    Log.Write("Number of cases for product: " + ProductID + " Found In Secondary Locations: " + secondaryCount);
                }


            }
            /****************************************
            4.Still not enough Product found. looking for any replens going to secondary locations
             * **************************************/
            if (NumCasesRequired > InvAvailcount)
            {
                int secondaryReplenCount = 0;
                sql = SQL2 + whereSecondary + OrderPrimary;
                DbCon = new DatabaseConnection();
                Con = new SqlConnection(DbCon.GetPhillyConString());
                MyReader = null;
                SqlCommand = new SqlCommand(sql, Con);
                Con.Open();
                MyReader = SqlCommand.ExecuteReader();
                while (MyReader.Read())
                {

                    AvailableInventory[InvAvailcount] = new InvAvail();
                    AvailableInventory[InvAvailcount].InventoryRID = Convert.ToInt32(MyReader["InvRID"]); ;
                    AvailableInventory[InvAvailcount].MoveQRID = Convert.ToInt32(MyReader["MQRID"]);
                    AvailableInventory[InvAvailcount].FromLocation = MyReader["LocID"].ToString();
                    AvailableInventory[InvAvailcount].MoveQStatus = MyReader["MQStatus"].ToString();
                    BreakApartToLocation(ref AvailableInventory[InvAvailcount]);
                    InvAvailcount++;
                    secondaryReplenCount++;

                }
                Con.Close();
                if (InvAvailcount == 0)
                {
                    Log.Write("No Cases heading to Secondary locations found. Trying Piece Pick Location");
                }
                else
                {
                    Log.Write("Number of cases for product: " + ProductID + " replens going to Secondary Locations: " + secondaryReplenCount);
                }

            }
            /****************************************
            5.Still not enough Product found. Looking in Piece Pick Locations
             * **************************************/
            if (NumCasesRequired > InvAvailcount)
            {


                int PiecePCount = 0;
                sql = SQL1 + wherePiecepick;
                Con = new SqlConnection(DbCon.GetPhillyConString());
                MyReader = null;
                SqlCommand = new SqlCommand(sql, Con);
                Con.Open();
                MyReader = SqlCommand.ExecuteReader();

             
                while (MyReader.Read())
                {

                    AvailableInventory[InvAvailcount] = new InvAvail();
                    AvailableInventory[InvAvailcount].InventoryRID = Convert.ToInt32(MyReader["InvRID"]); ;
                    AvailableInventory[InvAvailcount].MoveQRID = Convert.ToInt32(MyReader["MQRID"]);
                    AvailableInventory[InvAvailcount].FromLocation = MyReader["LocID"].ToString();
                    AvailableInventory[InvAvailcount].MoveQStatus = MyReader["MQStatus"].ToString();
                    BreakApartToLocation(ref AvailableInventory[InvAvailcount]);
                    InvAvailcount++;
                    PiecePCount++;

                }
                Con.Close();
                if (InvAvailcount == 0)
                {
                    Log.Write("No Cases Found In Piece Pick Location. Trying Replens already picked");
                    //Log.Write(sql);
                    //Console.ReadLine();
                }
                else
                {
                    Log.Write("Number of cases for product: " + ProductID + " Cases In Piece Pick Location: " + PiecePCount);
                }
            }

            if (NumCasesRequired > InvAvailcount)
            {
               int PiecePCount = 0;
               sql = SQL1;
                Con = new SqlConnection(DbCon.GetPhillyConString());
                MyReader = null;
                SqlCommand = new SqlCommand(sql, Con);
                Con.Open();
                MyReader = SqlCommand.ExecuteReader();


                while (MyReader.Read())
                {

                    AvailableInventory[InvAvailcount] = new InvAvail();
                    AvailableInventory[InvAvailcount].InventoryRID = Convert.ToInt32(MyReader["InvRID"]); ;
                    AvailableInventory[InvAvailcount].MoveQRID = Convert.ToInt32(MyReader["MQRID"]);
                    AvailableInventory[InvAvailcount].FromLocation = MyReader["LocID"].ToString();
                    AvailableInventory[InvAvailcount].MoveQStatus = MyReader["MQStatus"].ToString();
                    BreakApartToLocation(ref AvailableInventory[InvAvailcount]);
                    InvAvailcount++;
                    PiecePCount++;

                }
                Con.Close();
                if (InvAvailcount == 0)
                {
                    Log.Write("No Cases Found Anywhere");
                    Log.Write(sql);
                    //Console.ReadLine();
                }
                else
                {
                    Log.Write("Number of cases for product: " + ProductID + " Cases In Piece Pick Location: " + PiecePCount);
                }
            }
            /****************************************
           6.Still not enough Product found. Looking For replens already picked going to Piece Pick Locations
            * **************************************/
            if (NumCasesRequired > InvAvailcount)
            {

                if (InvAvailcount == 0)
                {
                    Log.Write("No Inventoy Found Anywhere");
                    Log.LogError(OBSD.ShipmentID, OBSD.OrderNumber, Convert.ToInt16(OBSD.OrderLine), OBSD.ProductID, "No Inventory Found Anywhere to case Pick");
                    Host = new HostOrder(OBSD.ShipmentID);
                    Host.UpdateStatus("E");
                }

            }
            //no cases found going to piece pick locations. looking anywhere now that isnt allocated


            /****************************************
           * Print Available Inventory Struct
           * **************************************/
            // PrintAvailableInventory(AvailableInventory, InvAvailcount);



            /*********************************
             * Sort AvailableInventory so the drivers are not driving all over the place
             * *******************************/
            SortAvailableInventory(ref AvailableInventory, InvAvailcount);

            /*********************************
            * Print After the sort
            * *******************************/
            //PrintAvailableInventory(AvailableInventory, InvAvailcount);

            if (BulkPick == true)
            {
                SortForBulk(ref AvailableInventory, InvAvailcount);
                //PrintAvailableInventory(AvailableInventory, InvAvailcount);
            }

            if (InvAvailcount == 0)
            {
                Log.Write("No Inventory Found exiting fuction");

                return "S";
            }
            else if (NumCasesRequired > InvAvailcount)
            {
                Log.Write("Not enough Inventory Avliable, exiting function");

                return "S";


            }

            for (int i = 0; i < NumCasesRequired; i++)
            {
                if (AvailableInventory[i].FromLocation == PiecePickLocation)
                {
                    InventoryAllocation inv = new InventoryAllocation(OBSD, PiecePickLocation, ProductID, QuantityToAllocate, "CASEPICK");
                }
                if (AvailableInventory[i].MoveQRID > 0 && AvailableInventory[i].MoveQStatus == "N")
                {
                    Log.Write("deleteing Move Queue Entry");
                    var temp = AvailableInventory[i].MoveQRID;
                    using (WMSEntities wms = new WMSEntities())
                    {
                        var EntreyToDelete = wms.MOVE_QUEUE.SingleOrDefault(x => x.C_RID_ == temp);
                        if (EntreyToDelete != null)
                        {
                            wms.MOVE_QUEUE.Remove(EntreyToDelete);
                            // wms.SaveChanges();
                        }
                        temp = AvailableInventory[i].InventoryRID;
                        var EntreyToUpdate = wms.Inventories.SingleOrDefault(x => x.C_RID_ == temp);
                        if (EntreyToUpdate != null)
                        {
                            EntreyToUpdate.RESERVATION_TYPE = null;
                            // wms.SaveChanges();
                        }
                    }
                }

                MoveQueue MoveQ = new MoveQueue();
                MoveQ.MQType = "PICK";
                MoveQ.Priority = 3;
                MoveQ.LabelLevel = "CSE";
                MoveQ.ToZone = "";
                MoveQ.ToLocation = "";
                MoveQ.FromZone = AvailableInventory[i].zone.ToString();
                MoveQ.FromLocation = AvailableInventory[i].FromLocation;
                MoveQ.OrigionalQuantityRequired = QuantityToAllocate;
                MoveQ.shipmentID = OBSD.ShipmentID;
                MoveQ.OrderNumber = OBSD.OrderNumber;
                MoveQ.OrderLine = OBSD.OrderLine;
                MoveQ.InvRID = AvailableInventory[i].InventoryRID;
                MoveQ.CreateMoveQueue(CarrierID);
            }

            return "A";
        }

        private void SortForBulk(ref InvAvail[] AvailableInventory, int InvAvailcount)
        {
            int startIndex = 0;
            string baseVale = "";
            int zoneIndexes = 0;
            InvAvail temp = new InvAvail();

            using (WMSEntities wms = new WMSEntities())
            {
                var zoneData = from SYS in wms.SYSTEMs where SYS.NAME == "BULK_PICK_ZONES" select SYS.VALUE;
                foreach (var item in zoneData)
                {
                    baseVale = item;
                }
            }

            string[] BulkZones = baseVale.Split(',');


            for (int i = 0; i < InvAvailcount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    foreach (string zones in BulkZones)
                    {

                        if ((char.ToUpper(AvailableInventory[j].zone) == Convert.ToChar(zones)))
                        {
                            temp = AvailableInventory[i];
                            AvailableInventory[i] = AvailableInventory[j];
                            AvailableInventory[j] = temp;
                        }
                        else if ((char.ToUpper(AvailableInventory[j].zone) - 64) == (char.ToUpper(AvailableInventory[i].zone) - 64))
                        {
                            if (Convert.ToInt16(AvailableInventory[j].Aisle) > Convert.ToInt16(AvailableInventory[i].Aisle))
                            {
                                temp = AvailableInventory[i];
                                AvailableInventory[i] = AvailableInventory[j];
                                AvailableInventory[j] = temp;
                            }
                            else if (Convert.ToInt16(AvailableInventory[j].Aisle) == Convert.ToInt16(AvailableInventory[i].Aisle))
                            {
                                if (Convert.ToInt16(AvailableInventory[j].Bin) > Convert.ToInt16(AvailableInventory[i].Bin))
                                {
                                    temp = AvailableInventory[i];
                                    AvailableInventory[i] = AvailableInventory[j];
                                    AvailableInventory[j] = temp;
                                }
                                else if (Convert.ToInt16(AvailableInventory[j].Bin) == Convert.ToInt16(AvailableInventory[i].Bin))
                                {
                                    if ((char.ToUpper(AvailableInventory[j].Level) - 64) > (char.ToUpper(AvailableInventory[i].Level) - 64))
                                    {
                                        temp = AvailableInventory[i];
                                        AvailableInventory[i] = AvailableInventory[j];
                                        AvailableInventory[j] = temp;
                                    }

                                }

                            }
                        }
                    }
                }


            }
        }


        private void BreakApartToLocation(ref InvAvail invAvail)
        {
            if (invAvail.FromLocation.Length == 7)
            {
                invAvail.zone = Convert.ToChar(invAvail.FromLocation.Substring(0, 1));
                invAvail.Aisle = invAvail.FromLocation.Substring(1, 2);
                invAvail.Bin = invAvail.FromLocation.Substring(4, 3);
                invAvail.Level = Convert.ToChar(invAvail.FromLocation.Substring(3, 1));
            }
            else
            {
                invAvail.zone = Convert.ToChar(invAvail.FromLocation.Substring(0, 1));
                invAvail.Aisle = invAvail.FromLocation.Substring(0, 1);
                invAvail.Bin = invAvail.FromLocation.Substring(0, 1);
                invAvail.Level = Convert.ToChar(invAvail.FromLocation.Substring(0, 1));
            }
        }

        private void PrintAvailableInventory(InvAvail[] AvailableInventory, int InvAvailcount)
        {
            Log.Write("******************************************");
            for (int i = 0; i < InvAvailcount; i++)
            {
                Log.newLine();
                // Log.Write("Inventory RID: " + AvailableInventory[i].InventoryRID);
                // Log.Write("Move QUeue RID: " + AvailableInventory[i].MoveQRID);
                Log.Write("To Location: " + AvailableInventory[i].FromLocation);
                //  Log.Write("Move Queue Status: " + AvailableInventory[i].MoveQStatus);
                //  Log.Write("Zone: " + AvailableInventory[i].zone);
                // Log.Write("Aisle: " + AvailableInventory[i].Aisle);
                //  Log.Write("Level: " + AvailableInventory[i].Level);
                //  Log.Write("Bin: " + AvailableInventory[i].Bin);

            }
            Log.Write("******************************************");
        }

        private void SortAvailableInventory(ref InvAvail[] AvailableInventory, int InvAvailcount)
        {
            InvAvail temp = new InvAvail();



            for (int i = 0; i < InvAvailcount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if ((char.ToUpper(AvailableInventory[j].zone) - 64) > (char.ToUpper(AvailableInventory[i].zone) - 64))
                    {
                        temp = AvailableInventory[i];
                        AvailableInventory[i] = AvailableInventory[j];
                        AvailableInventory[j] = temp;
                    }
                    else if ((char.ToUpper(AvailableInventory[j].zone) - 64) == (char.ToUpper(AvailableInventory[i].zone) - 64))
                    {
                        if (Convert.ToInt16(AvailableInventory[j].Aisle) > Convert.ToInt16(AvailableInventory[i].Aisle))
                        {
                            temp = AvailableInventory[i];
                            AvailableInventory[i] = AvailableInventory[j];
                            AvailableInventory[j] = temp;
                        }
                        else if (Convert.ToInt16(AvailableInventory[j].Aisle) == Convert.ToInt16(AvailableInventory[i].Aisle))
                        {
                            if (Convert.ToInt16(AvailableInventory[j].Bin) > Convert.ToInt16(AvailableInventory[i].Bin))
                            {
                                temp = AvailableInventory[i];
                                AvailableInventory[i] = AvailableInventory[j];
                                AvailableInventory[j] = temp;
                            }
                            else if (Convert.ToInt16(AvailableInventory[j].Bin) == Convert.ToInt16(AvailableInventory[i].Bin))
                            {
                                if ((char.ToUpper(AvailableInventory[j].Level) - 64) > (char.ToUpper(AvailableInventory[i].Level) - 64))
                                {
                                    temp = AvailableInventory[i];
                                    AvailableInventory[i] = AvailableInventory[j];
                                    AvailableInventory[j] = temp;
                                }

                            }

                        }

                    }
                }
            }
        }

        //generates a replen for an order
        private int GenerateReplen(string ShipmentID, short OrderLine, string OrderNumber, string ToLocation, string ToZone,
            string ProductID, int ReplenRequiredQuantity)
        {
            int CasesToReplen = 1;
            int CasesToReplenNew;
            int OrigionalRequiredQuantity = ReplenRequiredQuantity;
            using (WMSEntities wms = new WMSEntities())
            {

                IQueryable<LOCATION> locData = from Lo in wms.LOCATIONS where Lo.LOCATION_ID != ToLocation && Lo.TYPE != "X" select Lo;
                foreach (LOCATION Locate in locData)
                {
                    IQueryable<Inventory> Item = from In in wms.Inventories
                                                 where In.PRODUCT_ID == ProductID && In.LOCATION_ID == Locate.LOCATION_ID && In.HOLD == false && In.RESERVED == false
                                                     && (In.RESERVATION_TYPE == "" || In.RESERVATION_TYPE == null)
                                                 orderby In.DATA1STR, In.QUANTITY descending
                                                 select In;
                    foreach (Inventory inv in Item)
                    {
                        IQueryable<float?> CapData = (from LC in wms.LOCATION_CAPACITY
                                                      where LC.LOCATION_ID == ToLocation
                                                          && (LC.PRODUCT_ID == ProductID || LC.PRODUCT_ID == null)
                                                      select LC.CAPACITY).Take(1);

                        foreach (float? Cap in CapData)
                        {
                            CasesToReplen = (int)Cap;
                        }

                        if (CasesToReplen == 0)
                        {
                            Log.Write("No More Cases can be Moved here");
                            CasesToReplen = -1;
                        }

                        int count = (from MQ in wms.MOVE_QUEUE where MQ.TYPE == "REPLEN" && MQ.PRIORITY == 1 && MQ.TO_ZONE == ToZone && MQ.TO_LOCATION == ToLocation select MQ).Count();
                        CasesToReplenNew = CasesToReplen;
                        CasesToReplenNew -= count;
                        if (CasesToReplenNew < 0)
                            CasesToReplenNew = 0;
                        Log.Write("After Factoring in Existing Priority 1 Replens, CasesToReplenNew is now " + CasesToReplenNew);
                        bool satisfiedpick = false;

                        while (CasesToReplenNew > 0)
                        {
                            Locations Loc = new Locations(inv.LOCATION_ID);
                            if (satisfiedpick == true)
                            {
                                CasesToReplen--;
                                CasesToReplenNew--;
                            }
                            else
                            {
                                ReplenRequiredQuantity -= (int)inv.QUANTITY;
                                if (ReplenRequiredQuantity < 0)
                                {
                                    CasesToReplen--;
                                    CasesToReplenNew--;
                                }
                            }
                            if (CasesToReplenNew >= 0)
                            {
                                Log.Write("inventory used for this replen :" + inv.C_RID_);
                                Log.Write("Quantity going: " + inv.QUANTITY);
                                Log.Write("replen required quantity: " + ReplenRequiredQuantity);

                                MoveQueue MoveQ = new MoveQueue();
                                MoveQ.MQType = "REPLEN";
                                MoveQ.LabelLevel = null;
                                MoveQ.ToZone = ToZone;
                                MoveQ.ToLocation = ToLocation;
                                MoveQ.FromLocation = inv.LOCATION_ID;
                                MoveQ.FromZone = Loc.PickingZone;
                                MoveQ.OrigionalQuantityRequired = inv.QUANTITY;
                                MoveQ.InvRID = inv.C_RID_;
                                MoveQ.Priority = 1;

                                if (satisfiedpick == false)
                                {
                                    MoveQ.shipmentID = ShipmentID;
                                    MoveQ.OrderNumber = OrderNumber;
                                    MoveQ.OrderLine = OrderLine;
                                }
                                else
                                {
                                    MoveQ.shipmentID = null;
                                    MoveQ.OrderNumber = null;
                                    MoveQ.OrderLine = null;
                                }

                                MoveQ.CreateMoveQueue(CarrierID);
                                InventoryTable InvTable = new InventoryTable(inv.C_RID_);
                                InvTable.updateReservationType("REPLEN");
                                InvTable.UpdateLastUpdated();
                                InvTable.UpdateLastUser(Environment.MachineName);
                                InvTable.UpdateLastModule("CARTONIZE");
                            }
                            if (satisfiedpick == false)
                            {
                                satisfiedpick = true;
                                MoveQueue MoveQ = new MoveQueue();
                                MoveQ.MQType = "PICK";
                                MoveQ.Priority = 3;
                                MoveQ.LabelLevel = "CSE";
                                MoveQ.ToZone = "";
                                MoveQ.ToLocation = "";
                                MoveQ.FromLocation = ToLocation;
                                MoveQ.FromZone = ToZone;
                                if (ReplenRequiredQuantity > 0)
                                    OrigionalRequiredQuantity = (int)inv.QUANTITY;

                                MoveQ.OrigionalQuantityRequired = OrigionalRequiredQuantity;
                                MoveQ.shipmentID = ShipmentID;
                                MoveQ.OrderLine = OrderLine;
                                MoveQ.OrderNumber = OrderNumber;
                                MoveQ.InvRID = inv.C_RID_;
                                MoveQ.CreateMoveQueue(CarrierID);

                            }
                        }


                    }

                }
                if (ReplenRequiredQuantity <= 0)
                    return 0;
                else
                    return ReplenRequiredQuantity;

            }



        }
        //checks to see if inventory is avliable in a location to be picked, if so it allocates it
        private int CreatePiecePick(string PiecePickLocation, string ProductID, string PickingZone, string ShipmentID, string OrderNumber, short OrderLine,
            int QuantityToAllocate, string carrierID)
        {
            int AvliableQuantity = 0;
            int ReplenQuantity = 0;
            int PickQuantity = 0;
            int RequiredQuantity = QuantityToAllocate;
            InventoryTable[] InvTable = new InventoryTable[200];
            int InvTableCount = 0;
            int MoveQCount = 0;
            MoveQueue[] MoveQ = new MoveQueue[200];
            MoveQueue MoveQBase = new MoveQueue();

            MoveQBase.MQType = "PICK";
            MoveQBase.Priority = 3;
            MoveQBase.LabelLevel = "CSE";
            MoveQBase.ToZone = "";
            MoveQBase.ToLocation = "";
            MoveQBase.FromZone = PickingZone;
            MoveQBase.FromLocation = PiecePickLocation;
            MoveQBase.shipmentID = ShipmentID;
            MoveQBase.OrderNumber = OrderNumber;
            MoveQBase.OrderLine = OrderLine;

            //getting theInventory in the piece pick location
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<Inventory> Item = from In in wms.Inventories
                                             where In.LOCATION_ID == PiecePickLocation && In.PRODUCT_ID == ProductID && In.HOLD == false && In.RESERVED == false
                                                 && (In.RESERVATION_TYPE == "" || In.RESERVATION_TYPE == null)
                                             orderby In.QUANTITY ascending
                                             select In;
                foreach (Inventory inv in Item)
                {
                    InvTable[InvTableCount] = new InventoryTable(inv.C_RID_);
                    AvliableQuantity += Convert.ToInt16(InvTable[InvTableCount].Quantity);
                    InvTableCount++;
                }
                int i = 0;
                while (i < InvTableCount && RequiredQuantity > 0)
                {
                    var temp = InvTable[i].RID;
                    IQueryable<MOVE_QUEUE> Data = from MQ in wms.MOVE_QUEUE where MQ.INV_RID == temp select MQ;
                    foreach (MOVE_QUEUE Move in Data)
                    {
                        MoveQ[MoveQCount] = new MoveQueue(Move.C_RID_);
                        AvliableQuantity -= Convert.ToInt16(MoveQ[MoveQCount].OrigionalQuantityRequired);
                        MoveQCount++;
                    }
                    if (AvliableQuantity > 0)
                    {
                        MoveQBase.InvRID = InvTable[i].RID;
                        if (RequiredQuantity <= AvliableQuantity)
                        {
                            MoveQBase.OrigionalQuantityRequired = RequiredQuantity;
                            RequiredQuantity = 0;
                        }
                        else
                        {
                            MoveQBase.OrigionalQuantityRequired = AvliableQuantity;
                            RequiredQuantity -= AvliableQuantity;
                        }

                        MoveQBase.CreateMoveQueue(CarrierID);
                        Log.Write("creating move queue");

                    }
                    else
                    {
                        Log.Write("Not Enough Inventory To Create Move Queue");
                        Log.LogError(MoveQBase.shipmentID, MoveQBase.OrderNumber, MoveQBase.OrderLine, ProductID, "Not Enough Product In Location");
                        Host = new HostOrder(ShipmentID);
                        Host.UpdateStatus("E");
                    }
                    i++;
                }

                IQueryable<Inventory> data = from Inv in wms.Inventories where Inv.PRODUCT_ID == ProductID && Inv.LOCATION_ID != "QCEMPTY" select Inv;
                foreach (Inventory Inven in data)
                {
                    IQueryable<MOVE_QUEUE> MQData = from MQ in wms.MOVE_QUEUE
                                                    where MQ.INV_RID == Inven.C_RID_ && MQ.TO_LOCATION == PiecePickLocation &&
                                                        (MQ.TYPE == "REPLEN" || MQ.TYPE == "MANMOVE")
                                                    orderby MQ.PRIORITY, MQ.INV_RID
                                                    select MQ;
                    foreach (MOVE_QUEUE move in MQData)
                    {
                        ReplenQuantity += Convert.ToInt16(move.ORIG_QUANTITY_REQUIRED);
                    }
                    IQueryable<MOVE_QUEUE> MQData2 = from MQ in wms.MOVE_QUEUE
                                                     where MQ.INV_RID == Inven.C_RID_ && MQ.TO_LOCATION == PiecePickLocation &&
                                                         (MQ.TYPE == "PICK" || MQ.TYPE == "MANMOVE")
                                                     orderby MQ.PRIORITY, MQ.INV_RID
                                                     select MQ;
                    foreach (MOVE_QUEUE move in MQData)
                    {
                        PickQuantity += Convert.ToInt16(move.ORIG_QUANTITY_REQUIRED);
                    }
                    AvliableQuantity = ReplenQuantity - PickQuantity;

                    if (AvliableQuantity > 0)
                    {
                        MoveQBase.InvRID = Inven.C_RID_;
                        if (RequiredQuantity <= AvliableQuantity)
                        {
                            MoveQBase.OrigionalQuantityRequired = RequiredQuantity;
                            RequiredQuantity = 0;
                        }
                        else
                        {
                            MoveQBase.OrigionalQuantityRequired = AvliableQuantity;
                            RequiredQuantity -= AvliableQuantity;
                        }
                        MoveQBase.CreateMoveQueue(carrierID);
                    }
                }



            }

            return RequiredQuantity;
        }

        private int GetReplenQuantity(string LocID, InventoryTable[] InvTable, int inventoryCount)
        {
            int replenQuantity = 0;
            int replenCount = 0;
            MoveQueue[] MoveQ = new MoveQueue[200];

            using (WMSEntities wms = new WMSEntities())
            {
                for (int i = 0; i < inventoryCount; i++)
                {

                    IQueryable<MOVE_QUEUE> Item = from MQ in wms.MOVE_QUEUE where MQ.INV_RID == InvTable[i].RID && MQ.TO_LOCATION == LocID && MQ.TYPE == "REPLEN" select MQ;
                    foreach (MOVE_QUEUE Move in Item)
                    {
                        MoveQ[replenCount] = new MoveQueue(Move.C_RID_);
                        replenQuantity += Convert.ToInt16(MoveQ[replenCount].OrigionalQuantityRequired);
                        replenCount++;

                    }
                    IQueryable<int> data1 = from MQ in wms.MOVE_QUEUE where MQ.TYPE == "PICK" && MQ.LABELLEVEL == "CSE" select MQ.INV_RID;
                    foreach (int Move in data1)
                    {
                        IQueryable<MOVE_QUEUE> data = from MQ in wms.MOVE_QUEUE
                                                      where MQ.INV_RID == InvTable[i].RID && MQ.TO_LOCATION == LocID && MQ.TYPE == "REPLEN"
                                                          && MQ.INV_RID == Move
                                                      select MQ;
                        foreach (MOVE_QUEUE MovQ in data)
                        {
                            MoveQ[replenCount] = new MoveQueue(MovQ.C_RID_);
                            replenQuantity -= Convert.ToInt16(MoveQ[replenCount].OrigionalQuantityRequired);
                            replenCount++;
                        }
                    }
                }
            }


            Log.Write("Replen amount going to Location " + LocID + ": " + replenQuantity);
            return replenQuantity;
        }


        private int GetAvliableInventory(string LocID, string ProdID, InventoryTable[] InvTable, int InventoryCount, InventoryAllocationTabel[] InvAllTable, int InventoryAllCount)
        {
            int InventoryAvliable = 0;
            int inventoryAllocated = 0;
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<Inventory> Item = from In in wms.Inventories
                                             where In.LOCATION_ID == LocID && In.PRODUCT_ID == ProdID && In.HOLD == false && In.RESERVED == false
                                                 && (In.RESERVATION_TYPE == "" || In.RESERVATION_TYPE == null)
                                             select In;
                foreach (Inventory inv in Item)
                {
                    InvTable[InventoryCount] = new InventoryTable(inv.C_RID_);
                    InvTable[InventoryCount].PrintInventoryTable();
                    InventoryAvliable += Convert.ToInt16(InvTable[InventoryCount].Quantity);
                    InventoryCount++;
                }

                IQueryable<INVENTORY_ALLOCATION> InAData = from IA in wms.INVENTORY_ALLOCATION where IA.LOCATION_ID == LocID && IA.PRODUCT_ID == ProdID select IA;
                foreach (INVENTORY_ALLOCATION InvA in InAData)
                {
                    InvAllTable[InventoryAllCount] = new InventoryAllocationTabel(InvA.C_RID_);
                    inventoryAllocated += Convert.ToInt16(InvAllTable[InventoryAllCount].Quantity);
                    InventoryAllCount++;
                }
            }
            return InventoryAvliable - inventoryAllocated;
        }


        private bool CheckIfProductExists(string ProductID)
        {
            int count = 0;
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<PRODUCT_MASTER> data = from PM in wms.PRODUCT_MASTER where PM.PRODUCT_ID == ProductID select PM;

                foreach (PRODUCT_MASTER P in data)
                {
                    count++;
                }

            }
            if (count == 0)
            {
                Log.Write("No Items Returned From Product Master");
                return false;
            }
            else if (count > 1)
            {
                Log.Write("Product Master Returned More than One Entry");
                return false;
            }
            else
                return true;
        }


        internal void RemoveAllocation(string allocationstatus, string OrdID)
        {
            try
            {

                using (WMSEntities wms = new WMSEntities())
                {
                    var Data = from MQ in wms.MOVE_QUEUE where MQ.SHIPMENT_ID == OrdID && MQ.TYPE == "REPLEN" select MQ.C_RID_;

                    foreach (var Move in Data)
                    {
                        MoveQueue MoveQ = new MoveQueue(Move);
                        IQueryable<MOVE_QUEUE> Data2 = from MQ in wms.MOVE_QUEUE where MQ.INV_RID == MoveQ.InvRID && MQ.SHIPMENT_ID != MoveQ.shipmentID select MQ;
                        //no other order is using this inventory for a replen so we can free it up
                        if (Data2 == null)
                        {
                            InventoryTable InvTable = new InventoryTable(MoveQ.InvRID);
                            InvTable.updateReservationType(null);
                        }

                    }

                    Log.Write("Deleting from Move Queue");
                    var MQData = from MQ in wms.MOVE_QUEUE where MQ.SHIPMENT_ID == OrdID select MQ.C_RID_;
                    foreach (var Move in MQData)
                    {
                        MoveQueue MoveQ = new MoveQueue();
                        MoveQ.DeleteMoveQueueEntry(Move);

                    }
                    Log.Write("Deleting from Inventory Allocation");

                    DatabaseConnection DbCon = new DatabaseConnection();
                    string conString = DbCon.GetPhillyConString();
                    DbCon.connection_String = conString;
                    DataSet ds;
                    DbCon.Sql = "delete from inventory_allocation where shipment_id='" + OrdID + "'";
                    ds = DbCon.GetConnection;

                    Log.Write("Deleting Outbound Shipment Details");
                    int OBSDcount = GetTotalNumberOfOBSDLines();
                    for (int i = 0; i < OBSDcount; i++)
                    {
                        OBSD[i].DeleteOBSDEntry();
                        OBSD[i] = null;
                    }
                    Log.Write("Deleting OutBound Shipments");
                    int OBScount = GetTotalNumberofOBSLines();
                    for (int i = 0; i < OBScount; i++)
                    {
                        OBS[i].DeleteOBSEntry();
                        OBS[i] = null;
                    }

                }
            }
            catch
            {

            }
        }


        internal void UpdateStatuses(string NewStatus)
        {
            double Totalvolume = 0;
            double TotalWeight = 0;
            string Shipmode = OBS[0].Ship_mode;

            for (int i = 0; i < GetTotalNumberOfOBSDLines(); i++)
            {
                ProductMaster ProdInfo = new ProductMaster(OBSD[i].ProductID);
                Totalvolume += ((double)ProdInfo.CubePerUom * (double)OBSD[i].OrderedUnits);
                TotalWeight += OBSD[i].GetTotalWeightofLine();
            }

            Log.Write("Total Volume=" + Totalvolume);
            Log.Write("Total Weight=" + TotalWeight);

            CustomerRules Rules = new CustomerRules();

            if (TotalWeight != 0 && Totalvolume != 0)
            {
                if (Rules.POARule(Shipmode, TotalWeight, Totalvolume) == true)
                {
                    for (int i = 0; i < OBSCount; i++)
                    {
                        OBS[i].UpdateCatrtonType("POA");
                    }
                }
                else if (Rules.POBRule(Shipmode, TotalWeight, Totalvolume) == true)
                {
                    for (int i = 0; i < OBSCount; i++)
                    {
                        OBS[i].UpdateCatrtonType("POB");
                    }
                }
                if (Rules.BagRule(Shipmode, TotalWeight, Totalvolume) == true)
                {
                    for (int i = 0; i < OBSCount; i++)
                    {
                        OBS[i].UpdateCatrtonType("5 BAG");
                    }
                }
            }

            for (int i = 0; i < GetTotalNumberofOBSLines(); i++)
            {
                OBS[i].UpdateLastUpdated();
                OBS[i].UpdateStatus(NewStatus);
                if ((OBS[i].CartonType == "CASE") && (QCCodes == "VHC" || QCCodes == "EAG"))
                {
                    OBS[i].UpdateQCRequired("N");
                    OBS[i].UpdateQCCodes("");

                }
                if (TypeOfOrder == "WHT" && OBS[i].CartonType != "CASE")
                {
                    OBS[i].UpdateCatrtonType("CASE");
                }
            }

            for (int i = 0; i < GetTotalNumberOfOBSDLines(); i++)
            {
                OBSD[i].UpdateLastUpdated();
                OBSD[i].UpdateStatus(NewStatus);
                if (TypeOfOrder == "WHT" && OBSD[i].UnitOfMesure != "CSE")
                {
                    OBSD[i].updateUnitOfMesure("CSE");
                }
            }
            AuditOutbound outbound = new AuditOutbound("ORDER", "STATUS");
            outbound.UpdateTransactionTime(DateTime.Now);
            outbound.UpdateShipmentID(OrderID);
            outbound.UpdateStatus("N");
            string ProgramName = Assembly.GetExecutingAssembly().CodeBase;
            outbound.UpdateLastModule(Path.GetFileNameWithoutExtension(ProgramName));









        }

        private bool IsQCEnabled()
        {
            using (WMSEntities wms = new WMSEntities())
            {
                var enabled = (from sys in wms.SYSTEMs where sys.NAME == "QC_ENABLED" select sys).SingleOrDefault();

                if (enabled.VALUE == "1")
                    return true;
                else
                    return false;
            }
        }



        internal void UpdateHostOrderStatus(string allocationstatus, bool OrderCancelRequest, string orderType)
        {

            Log.Write("Allocation Status once order is done;" + allocationstatus);
            string ProgramName = Assembly.GetExecutingAssembly().CodeBase;

            if (OrderCancelRequest == true)
            {
                Host.UpdateStatus("N");
            }

            if (allocationstatus != "S")
            {
                Host.UpdateStatus("C");
            }
            else if (allocationstatus == "S")
            {
                Host.UpdateStatus("E");
            }
            else
            {
                Host.UpdateStatus(allocationstatus);
            }

            if (Status == "E")
            {
                Host.UpdateStatus("E");

            }

            Host.UpdateLastUpdates();
            Host.UpdateLastModule(Path.GetFileNameWithoutExtension(ProgramName));
            Host.UpdateLastUser(Environment.MachineName);
        }



        internal void ResortOBS()
        {
            OutboundShipmentDetails temp = new OutboundShipmentDetails();
            //sorts by zone, then by pick sequence;



            //for (int i = 0; i < GetTotalNumberofOBSLines(); i++)
            //{
            //    for (int j = 0; j < i; j++)
            //    {
            //        if (Convert.ToInt32(OBSD[j].ProductID) > Convert.ToInt32(OBSD[i].ProductID))
            //        {
            //            temp = OBSD[i];
            //            OBSD[i] = OBSD[j];
            //            OBSD[j] = temp;


            //        }
            //    }
            //}
            for (int i = 0; i < GetTotalNumberofOBSLines(); i++)
            {
                OBS[i].UpdateQCCodes("WHT");
                //OBS[i].UpdatePurchaseOrder("0");
                OBS[i].AddToDatabase();

                OBSD[i].OrderNumber = OBSD[i].getOrderNumber(i + 1);
                OBSD[i].AddToDatabase();

            }

        }
    }
}




