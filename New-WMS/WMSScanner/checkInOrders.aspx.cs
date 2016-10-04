using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class checkInOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //GetData();
        }
    }

    protected void txtScanPickLoc_TextChanged(object sender, EventArgs e)
    {
        string boxId = "";
        if (String.IsNullOrEmpty(txtScanPickLoc.Text))
        {
            boxId = txtScanPickLoc.Text;
            PickBox(boxId);
        }
    }

    private void PickBox(string BoxID)
    {

    }

}