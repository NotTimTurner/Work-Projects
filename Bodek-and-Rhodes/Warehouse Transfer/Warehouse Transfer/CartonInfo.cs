using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    public class CartonInfo{
        DebugLog Log = new DebugLog();
    


    public CartonInfo()
    {
        Name = "";
        Description = "";
        Length = 0;
        Width =0;
        Height = 0;
        UCBPercent = 0;
        LCBPercent = 0;
        MaxWeight = 0;
        DunnageWeight = 0;
        Convayable = true;
    }
        
    
        public CartonInfo(string InName,string InDesc,float InLength, float InWidth, float InHeight, float InUBCPercent, float InLCBPercent, 
            float InMaxWeight, double InDunnageWeight,bool InConayable, int InBoxCnt)
        {
             Name = InName;
             Description = InDesc;
             Length = InLength;
             Width = InWidth;
             Height = InHeight;
             UCBPercent = InUBCPercent;
             LCBPercent = InLCBPercent;
             MaxWeight = InMaxWeight;
             DunnageWeight = InDunnageWeight;
             Convayable= InConayable;
             BoxCnt = InBoxCnt;
        }


        public string Name { get; set; }

        public string Description { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float  Height { get; set; }
        public float UCBPercent { get; set; }
        public float LCBPercent { get; set; }
        public float MaxWeight { get; set; }
        public double DunnageWeight { get; set; }
        public bool Convayable { get; set; }
        public int BoxCnt { get; set; }

        public float GetMaxVolume()
        {
            float total=((Length * Width * Height)/1728)*(UCBPercent/100);
            return(total);
        }

        public float GetMinVolume()
        {
             return(((Length * Width * Height)/1728)*(LCBPercent/100));
        }
        


        public void PrintBoxInfo()
        {
            Log.Write("Box Name: " + Name);
            Log.Write("Box Description: " + Description);
            Log.Write("Box Length: " + Length);
            Log.Write("Box Width: " + Width);
            Log.Write("Box Height: " + Height);
            Log.Write("Box UCBPercent: " + UCBPercent);
            Log.Write("Box LCBPercent: " + LCBPercent);
            Log.Write("Box Max Weight: " + MaxWeight);
            Log.Write("Box DunnageWeight: " + DunnageWeight);
            Log.Write("Box Convayable: " + Convayable);
            Log.Write("Box min Volume: " + GetMinVolume());
            Log.Write("Box Max Volume: " + GetMaxVolume());
            Log.Write("********************************");
            Log.newLine();
        }
    }
}
