using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    public class Piece
    {
        DebugLog Log = new DebugLog();
        public string ProductID { get; set; }
        public double TotalVolume { get; set; }
        public int PiecesToCartonize { get; set; }
        public double VolumeOfAPiece { get; set; }
        public int CartonTypeIndex { get; set; }
        public double VolumeLeftInBox { get; set; }
        public int CartonID { get; set; }
        public string PiecePickLocation { get; set; }
        public char zone { get; set; }
        public string Aisle { get; set; }
        public string Bin { get; set; }
        public char Level { get; set; }
        public string ShortProductID { get; set; }
        CartonInfo box = new CartonInfo();

        public Piece()
        {
            ProductID = "0";
            TotalVolume = 0;
            PiecesToCartonize = 0;
            CartonTypeIndex = -1;
            VolumeLeftInBox = 0;
            CartonID = 0;
            PiecePickLocation = "";
        }
    


        public void PrintPieceInfo()
        {
            Log.Write("ProductID: " + ProductID);
            Log.Write("Shortened ProductID:  " + ShortProductID);
            Log.Write("Total Volume Of Pieces: " + TotalVolume);
            Log.Write("Pieces To Cartonize: " + PiecesToCartonize);
            Log.Write("Piece Pick Location: " + PiecePickLocation);
            Log.Write("Zone :" + zone);
            Log.Write("Aisle :" + Aisle);
            Log.Write("Level :" + Level);
            Log.Write("Bin :" + Bin);
           
            Log.Write("*********************************\n");

        }

        public void BreakApartInformation()
        {
            try
            {
                string check=(PiecePickLocation.Substring(0, 2));
                BreakApartProductID();
                    VolumeOfAPiece = TotalVolume / PiecesToCartonize;
                if(check != "BW" || check != "SW")
                {
                    
                    zone = Convert.ToChar(PiecePickLocation.Substring(0, 1));
                    Aisle = PiecePickLocation.Substring(1, 2);
                    Bin = PiecePickLocation.Substring(4, 3);
                    Level = Convert.ToChar(PiecePickLocation.Substring(3, 1));
                }
                else if(check=="BW")
                {
                    zone='B';
                    Aisle="01";
                    Bin="01";
                    Level='W';

                }
                else{
                     zone='S';
                    Aisle="01";
                    Bin="01";
                    Level='W';
                }
            }
            catch (Exception e)
            {
                Log.Write("Error in :" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                Log.Write("Error Message: " + e.Message);
                Log.Write("Stack Trace: " + e.StackTrace);
                Log.Write("Target Site: " + e.TargetSite);
                Log.Write("Exiting Program");
                
            }

        }

        public bool WillItFit(double Volume)
        {
            if (VolumeOfAPiece > Volume)
            {
                Log.Write(ProductID + " is larger thanthe largest carton- please Update");
                return false;
            }
            else
                return true;
        }

        public bool IsThisProductAChair()
        {
            if (ProductID == "67715300" || ProductID == "67715330" || ProductID == "67715320")
            {
                Log.Write("Chair Found");
                return true;
            }
                
           
            return false;
        }

        public bool IsThisProductAssignedToACarton()
        {

            if (CartonTypeIndex == -1)
                return false;
            else
                return true;
        }

        public void BreakApartProductID()
        {
            ShortProductID = ProductID.Substring(0, 5);
        }

        


        
    }
}
