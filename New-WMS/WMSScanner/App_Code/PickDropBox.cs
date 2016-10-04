using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PickDropBox
/// </summary>
public class PickDropBox
{
    private string Rid;
    private string UserId;
    private string LocationId;
    private string ShipId;
    private string CaseNumber;
    private string BoxSize;
    private string CustomerId;
    private string Quantity;
    private bool Flag;
    private string Status;

    public PickDropBox()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public PickDropBox(string rid, string userId, string locationId, string shipId, string caseNumber, string boxSize, string customerId, string quantity, bool flag, string status)
    {
        Rid = rid;
        UserId = userId;
        LocationId = locationId;
        ShipId = shipId;
        CaseNumber = caseNumber;
        BoxSize = boxSize;
        CustomerId = customerId;
        Quantity = quantity;
        Flag = flag;
        Status = status;
        // dkl;fvjhdslfkvh klsdliskdfvx
    }
}