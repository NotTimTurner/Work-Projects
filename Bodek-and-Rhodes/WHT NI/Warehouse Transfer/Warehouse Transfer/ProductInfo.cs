using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    public class ProductInfo
    {
        DebugLog Log = new DebugLog();
        public string ProductID { get; set; }
        public string PiecePickLocation { get; set; }
        public float? OrderedUnits { get; set; }
        public int? CaseQuantity { get; set; }
        public float?  PieceQuantity{ get; set; }
        public int PiecesPerCase { get; set; }
        public float PieceVolume { get; set; }
        public decimal? TotalPieceVolume { get; set; }
        public float? PieceWeight { get; set; }
        public float? CaseWeight { get; set; }
        public float PricePerEach { get; set; }
        public float UnitsPerCase { get; set; }



        public ProductInfo()
        {

        }

        public void GetProductInfo()
        {
            
            float? CubePerUOM=0;
            using (WMSEntities wms = new WMSEntities())
            {
                var Item = (from h in wms.UOM_CONVERSION where h.PRODUCT_ID == ProductID &&h.FROM_UOM=="cse" select h.NUMERATOR).SingleOrDefault();
                     UnitsPerCase= Item;
                     if (UnitsPerCase != 0)
                         CaseQuantity = (Convert.ToInt16(OrderedUnits) / Convert.ToInt16(UnitsPerCase));
                     else
                         CaseQuantity = 0;
                     var Cube = from h in wms.PRODUCT_MASTER where h.PRODUCT_ID == ProductID select h;

                     foreach (var item in Cube)
                     {
                         CubePerUOM = item.CUBEPERUOM;
                         if(CubePerUOM==0)
                         {
                             //Tim look at this later, this might have to change
                             Log.Write("Cube Per UOM is 0. cartonize wont work right, exiting program");
                             System.Environment.Exit(1);
                         }
                         CaseWeight = item.CASELEVEL_WEIGHT;
                         PieceWeight =(float?)item.EIS_PIECE_WEIGHT;

                        
                     }
                     PieceQuantity = (OrderedUnits % UnitsPerCase);
                     if (PieceQuantity > 0)
                     {
                         TotalPieceVolume = Convert.ToDecimal(CubePerUOM) * Convert.ToDecimal(PieceQuantity);
                     }
                     else
                     {
                         TotalPieceVolume = 0;

                     }
                    // PrintProductInfo();
               
             
               
            }
        }

        public void PrintProductInfo()
        {
            Log.Write("Product ID: " + ProductID);
            Log.Write("Piece Pick Location: " + PiecePickLocation);
            Log.Write("Cube weight:" + CaseWeight);
            Log.Write("Piece Weight:" + PieceWeight);
            Log.Write("Case Weight: " + CaseWeight);
            Log.Write("Number of Pieces:" + PieceQuantity);
            Log.Write("Total Volume of piece picks:" + TotalPieceVolume);
            Log.Write("Units Per Case: " + UnitsPerCase);
            Log.Write("Number of Cases:" + CaseQuantity);
            Log.Write("*****************************");
            Log.newLine();
        }



    }
}
