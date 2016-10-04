using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
   public class OrderLine
    {
       DebugLog Log = new DebugLog();
 

        public string EISOrderID { get; set; }
        public int EISOrderLine { get; set; }
        public string Type { get; set; }
        public string ProductID { get; set; }
        public float? OrderedUnits { get; set; }
        public string UOM { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Notes { get; set; }
        public decimal? OrderedLinePrice { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime LastUpdated { get { return DateTime.Now; } }
        public string LastUser { get{return Environment.MachineName;} }
        public int OrderLineCount { get; set; }
        public string  PiecePickLocation { get; set; }
        public decimal? TotalUnitPrice { get; set; }
        public ProductInfo ProductInfo= new ProductInfo();





       public OrderLine(string InOrderID,int InOrderLine, string InType, string InProductID, float? InOrderedUnits, string InUOM, DateTime? InScheduledDate,
           string InNotes, decimal? InOrderLinePrice, decimal? InUnitPrice, int InLineCnt)
        {
            EISOrderID = InOrderID;
            EISOrderLine = InOrderLine;
            Type = InType;
            ProductID = InProductID;
            OrderedUnits = InOrderedUnits;
            UOM = InUOM;
            ScheduledDate = InScheduledDate;
            Notes = InNotes;
            OrderedLinePrice = InOrderLinePrice;
            UnitPrice = InUnitPrice;
            OrderLineCount = InLineCnt;
            PiecePickLocation = getPiecePickLocation(ProductID);
            ProductInfo.ProductID = InProductID;
            ProductInfo.OrderedUnits = InOrderedUnits;
            ProductInfo.PiecePickLocation = PiecePickLocation;
            GetLinePrice();
            ProductInfo.GetProductInfo();

        }


       public void printOrderLineInfo()
       {
             Log.Write("Order ID: " + EISOrderID);
             Log.Write("Order Line: " + EISOrderLine);
             Log.Write("Type: " + Type);
             Log.Write("Product ID: " + ProductID);
             Log.Write("Ordered Units: " + OrderedUnits);
             Log.Write("Scheduled Date: " + ScheduledDate);
             Log.Write("Notes: " + Notes);
             Log.Write("Ordered Line Price: " + OrderedLinePrice);
             Log.Write("Unit Price: " + UnitPrice);
             Log.Write("Total Unit Price: " + TotalUnitPrice);
             Log.Write("Last Update: " + LastUpdated);
             Log.Write("Last User: " + LastUser);
             Log.Write("Order Line COunt : " + OrderLineCount );
             Log.Write("Piece Pick Location: " + PiecePickLocation);
             Log.Write("*******************************");
             Log.Write("\n");

           //  ProductInfo.PrintProductInfo();
       }

       public string getPiecePickLocation(string ProductID)
       {
           string location = "";
           
         

           using (WMSEntities wms = new WMSEntities())
           {
               
             var item = (from h in wms.PRODUCT_MASTER where h.PRODUCT_ID == ProductID  select  h.PIECE_PICK_LOCATION).SingleOrDefault();

             if (item != null)
             {
                 location = item;
             }

           }
          
           return location;
       }

       public double GetLinePrice()
       {
           TotalUnitPrice = Convert.ToDecimal(OrderedUnits) * UnitPrice;
           return Convert.ToDouble(TotalUnitPrice);
       }

       public bool LineHasPieces()
       {

           if (ProductInfo.PieceQuantity > 0)
               return true;
           else
               return false;
       }

       public double GetTotalVolumeOfPieces()
       {
           return Convert.ToDouble(ProductInfo.TotalPieceVolume);
       }

       public int GetPiecesToCartonize()
       {
           return Convert.ToInt16(ProductInfo.PieceQuantity);
       }
   

    }
}
