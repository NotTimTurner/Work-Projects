using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    class CustomerRules
    {

        public bool POARule(string ShipMode, double TotalWeight, double TotalVolume)
        {
            if (ShipMode == "19" || ShipMode == "25" || ShipMode == "26")
            {
                if (TotalWeight >= 0.887)
                {
                    if (TotalVolume <= 0.192)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool POBRule(string ShipMode, double TotalWeight, double TotalVolume)
        {
            if (ShipMode == "19" || ShipMode == "25" || ShipMode == "26")
            {
                if (TotalWeight >= 0.887)
                {
                    if (TotalVolume <= 0.204)
                    {
                        return true;
                    }
                }
            }

            return false;
            
        }

         public bool BagRule(string ShipMode, double TotalWeight, double TotalVolume)
         {
             if (ShipMode == "19" || ShipMode == "25" || ShipMode == "40"||ShipMode == "1" ||ShipMode == "2" ||ShipMode == "3" 
                 ||ShipMode == "17" ||ShipMode == "10" ||ShipMode == "18" ||ShipMode == "26")
             {
                 if (TotalWeight < 0.887)
                 {
                     return true;
                 }
             }

             return false;
         }


    }
}
