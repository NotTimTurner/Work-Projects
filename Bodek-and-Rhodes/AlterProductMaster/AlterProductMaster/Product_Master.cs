using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace AlterProductMaster
{
    class Product_Master
    {
        public string Product_ID { get; set; }
        public string Short_Descritpion  { get; set; }
        public string Caselevel_weight{ get; set; }
        public string  Cube_Per_UOM { get; set; }
        public string EIS_Piece_weight  { get; set; }
        public string Vendor_id { get; set; }
        public string EIS_Mill { get; set; }
        public string Base_UOM_PER_caselevel { get; set; }
        public string  Description { get; set; }
        DatabaseConnection DbCon = new DatabaseConnection();
        string conString;
        DataSet ds;


        public Product_Master(string InProdID, string InShortDesc, string InCaselevel, string InCUbe, string InWeight, string InVendor, string InMill, string InUomPerCaseLevel, string InDescription)
        {
            this.Product_ID = InProdID;
            this.Short_Descritpion = InShortDesc;
            this.Caselevel_weight = InCaselevel;
            this.Cube_Per_UOM = InCUbe;
            this.EIS_Piece_weight = InWeight;
            this.Vendor_id = InVendor;
            this.EIS_Mill = InMill;
            this.Base_UOM_PER_caselevel = InUomPerCaseLevel;
            this.Description = InDescription;
        }

        internal void UpdateDatabase()
        {
          //  this.UpdateShort_Description(this.Product_ID, this.Short_Descritpion);
          //  this.UpdateCaselevel_weight(this.Product_ID, this.Caselevel_weight);
         //   this.UpdateCaseUOM(this.Product_ID, this.Cube_Per_UOM);
            this.UpdateDescription(this.Product_ID, this.Description);
        }

        private void UpdateDescription(string Product_id, string Description)
        {
            if ((Product_id != "") && (Description != ""))
            {
                DbCon = new DatabaseConnection();
                conString = DbCon.GetConString();
                DbCon.connection_String = conString;
                DbCon.Sql = "update product_master set Description='" + Description + "' where product_id='" + Product_id + "'";
                ds = DbCon.GetConnection;
            }
        }

        private void UpdateCaseUOM(string Product_id, string Cube_per_uom)
        {
            if (Product_id != "" && Cube_per_uom != "")
            {
                DbCon = new DatabaseConnection();
                conString = DbCon.GetConString();
                DbCon.connection_String = conString;
                DbCon.Sql = "update product_master set Cubeperuom='" + Cube_per_uom + "' where product_id='" + Product_id + "'";
                ds = DbCon.GetConnection;
            }
        }

        private void UpdateCaselevel_weight(string Product_id, string Case_level_weight)
        {
            if ((Product_id != "") && (Case_level_weight != ""))
            {
                DbCon = new DatabaseConnection();
                conString = DbCon.GetConString();
                DbCon.connection_String = conString;
                DbCon.Sql = "update product_master set Caselevel_weight='" + Case_level_weight + "' where product_id='" + Product_id + "'";
                ds = DbCon.GetConnection;
            }
        }

        private void UpdateShort_Description(string Product_id, string Short_description)
        {
            if (( Product_id != "")&& (Short_description!=""))
            {
                DbCon = new DatabaseConnection();
                conString = DbCon.GetConString();
                DbCon.connection_String = conString;
                DbCon.Sql = "update product_master set short_description='"+Short_description+"' where product_id='"+Product_id+"'";
                ds = DbCon.GetConnection;
            }
        }
    }
}
