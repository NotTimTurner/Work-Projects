using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTransfer
{
    public class ProductMaster
    {
        DebugLog log=new DebugLog();
        public string ProductID { get; set; }
        public string EisProductID { get; set; }
        public string UpcCode { get; set; }
        public string VendorID { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string BaseUom { get; set; }
        public string ProductionUom { get; set; }
        public string StockUom { get; set; }
        public string RecivingUom { get; set; }
        public float? BaseUomPerCaseLevel { get; set; }
        public int? CaseLevelPerPallettote { get; set; }
        public int? CaseLevelPerContainer { get; set; }
        public string Location_group { get; set; }
        public string PutAwayZone1 { get; set; }
        public string PutAwayZone2 { get; set; }
        public string PutAwayZone3 { get; set; }
        public string PickZone1 { get; set; }
        public string PickZone2 { get; set; }
        public string PickZone3 { get; set; }
        public string PiecePickLocation { get; set; }
        public string CaseLevelPickLocation { get; set; }
        public string PallotePickLocation { get; set; }
        public string ContainerPickLocation { get; set; }
        public string BolCLass{ get; set; }
        public string ProductType { get; set; }
        public string InitQcCategory { get; set; }
        public short? ShelfLife { get; set; }
        public short? ShipByWindow { get; set; }
        public short? ConsolidationWindow{ get; set; }
        public float?  CaseLevelWeight { get; set; }
        public float? CubePerUom { get; set; }
        public float? CubeUom { get; set; }
        public bool AutoRelease { get; set; }
        public float? EisAvgUnitCost { get; set; }
        public string EisSize { get; set; }
        public string EisMillStyleNumber { get; set; }
        public float? EisPackageQty { get; set; }
        public float? EisQtyOnOrder { get; set; }
        public float? EisQtyOnHand { get; set; }
        public float? EisMillPurchasePrice { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUser { get; set; }
        public string LastModule { get; set; }
        public float? EisPieceWeight { get; set; }
        public string EisColor { get; set; }




        public ProductMaster(string InProductID)
        {
            using (WMSEntities wms = new WMSEntities())
            {
                IQueryable<PRODUCT_MASTER> Item = from PM in wms.PRODUCT_MASTER where PM.PRODUCT_ID == InProductID select PM;

                foreach (PRODUCT_MASTER PM in Item)
                {
                    ProductID = PM.PRODUCT_ID;
                    EisProductID = PM.EIS_PRODUCT_ID;
                    UpcCode = PM.UPC_CODE;
                    VendorID = PM.VENDOR_ID;
                    Description = PM.DESCRIPTION;
                    ShortDescription = PM.SHORT_DESCRIPTION;
                    BaseUom = PM.BASE_UOM;
                    ProductionUom = PM.PRODUCTION_UOM;
                    StockUom = PM.STOCK_UOM;
                    RecivingUom = PM.RECEIVING_UOM;
                    BaseUomPerCaseLevel = PM.BASE_UOM_PER_CASELEVEL;
                    CaseLevelPerPallettote = PM.CASELEVEL_PER_PALLETTOTE;
                    CaseLevelPerContainer = PM.CASELEVEL_PER_CONTAINER;
                    Location_group = PM.LOCATION_GROUP;
                    PutAwayZone1 = PM.PUTAWAY_ZONE_1;
                    PutAwayZone2 = PM.PUTAWAY_ZONE_2;
                    PutAwayZone3 = PM.PUTAWAY_ZONE_3;
                    PickZone1 = PM.PICK_ZONE_1;
                    PickZone2 = PM.PICK_ZONE_2;
                    PickZone3 = PM.PICK_ZONE_3;
                    PiecePickLocation = PM.PIECE_PICK_LOCATION;
                    CaseLevelPickLocation = PM.CASELEVEL_PICK_LOCATION;
                    PallotePickLocation = PM.PALLETTOTE_PICK_LOCATION;
                    ContainerPickLocation = PM.CONTAINER_PICK_LOCATION;
                    BolCLass = PM.BOL_CLASS;
                    ProductType = PM.PRODUCT_TYPE;
                    InitQcCategory = PM.INIT_QC_CATEGORY;
                    ShelfLife = PM.SHELF_LIFE;
                    ShipByWindow = PM.SHIP_BY_WINDOW;
                    ConsolidationWindow = PM.CONSOLIDATION_WINDOW;
                    CaseLevelWeight = PM.CASELEVEL_WEIGHT;
                    CubePerUom = PM.CUBEPERUOM;
                    AutoRelease = PM.AUTO_RELEASE;
                    EisAvgUnitCost = PM.EIS_AVG_UNIT_COST;
                    EisSize = PM.EIS_SIZE;
                    EisMillStyleNumber = PM.EIS_MILL_STYLE_NUMBER;
                    EisPackageQty = PM.EIS_PACKAGE_QTY;
                    EisQtyOnOrder = PM.EIS_QTY_ONORDER;
                    EisQtyOnHand = PM.EIS_QTY_ONHAND;
                    EisMillPurchasePrice = PM.EIS_MILL_PURCHASE_PRICE;
                    LastUpdated = PM.C_LAST_UPDATED_;
                    LastModule = PM.C_LAST_MODULE_;
                    EisPieceWeight = PM.EIS_PIECE_WEIGHT;
                    EisColor = PM.EIS_COLOR;
                }
            }
                    

        }

        public void CheckifPiecePickLocationIsNull()
        {
            if (PiecePickLocation==null)
            {
                log.Write("Piece Pick For Product " + ProductID + "in productMaster Table Is Null Please Fix this ASAP");
                Environment.Exit(1);
            }
        }


        internal void CheckIfProductIDisNull()
        {
            if (ProductID == null)
            {
                log.Write("ProductID: "+ProductID+" returned null Please Fix");
                Environment.Exit(1);
            }
        }
    }
}
