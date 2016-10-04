using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatePickSequence
{
   public class Locations
    {
        public string LocationID { get; set; }
        public string LocationGroup { get; set; }
        public string EisLocation { get; set; }
        public int PickSequence { get; set; }
        public string Aisle { get; set; }
        public string Slot { get; set; }
        public string VLevel { get; set; }
        public string Bin { get; set; }
        public string Status { get; set; }
        public string SiteID { get; set; }
        public string PickingZone { get; set; }
        public bool AllowPiecePick { get; set; }
        public string Type { get; set; }
        public string Usage { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }
        public int? PutawaySequence { get; set; }
        public string ReverseSlot { get; set; }

        public Locations()
        {

        }

        public Locations(string PiecePickLocation)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<LOCATION> Data = from LO in wms.LOCATIONS where LO.LOCATION_ID == PiecePickLocation select LO;
                foreach (LOCATION Dloc in Data)
                {
                    LocationID = Dloc.LOCATION_ID;
                    LocationGroup = Dloc.LOCATION_GROUP;
                    EisLocation = Dloc.EIS_LOCATION;
                    PickSequence = Dloc.PICK_SEQUENCE;
                    Aisle = Dloc.AISLE;
                    Slot = Dloc.SLOT;
                    VLevel = Dloc.VLEVEL;
                    Bin = Dloc.BIN;
                    Status = Dloc.STATUS;
                    SiteID = Dloc.SITE_ID;
                    PickingZone = Dloc.PICKING_ZONE;
                    AllowPiecePick = Dloc.ALLOWPIECEPICKING;
                    Type = Dloc.TYPE;
                    Usage = Dloc.USAGE;
                    LastUpdated = Dloc.C_LAST_UPDATED_;
                    LastUser = Dloc.C_LAST_USER_;
                    LastModule = Dloc.C_LAST_MODULE_;
                    PutawaySequence = Dloc.PUTAWAY_SEQUENCE;
                    ReverseSlot = Dloc.REVERSE_SLOT;
                }
            }

        }

        public int UpdatePickSequence()
        {
            try
            {
                if (LocationID.Length == 7)
                {
                    char zone = Convert.ToChar(LocationID.Substring(0, 1));
                    string NewAisle = LocationID.Substring(1, 2);
                    string NewBin = LocationID.Substring(4, 3);
                    char Level = Convert.ToChar(LocationID.Substring(3, 1));

                    string newPickSequence = ((int)zone).ToString() + NewAisle + NewBin + ((int)Level).ToString();
                    // Console.WriteLine("PickSequence: " + PickSequence + " LocationID: " + LocationID + " New Pick Sequence: " + newPickSequence);


                    return Convert.ToInt32(newPickSequence);
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


       

       
    }
}
