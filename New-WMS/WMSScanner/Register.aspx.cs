using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.UI;
using WMSScanner;
using System.Web.UI.WebControls;

public partial class Account_Register : Page
{
    Validation valid;
    protected void Page_Load(object sender, EventArgs e)
    {
        PopulateMonthDropDown();
        PopulateYearDropDown();
        PopulateDayDropDown();
    }

    private void PopulateDayDropDown()
    {
        int month=0;
        int year=0;
        if (MonthDropDown.SelectedIndex != 0)
            month = MonthDropDown.SelectedIndex;
        if (YearDropDown.SelectedValue != "Year")
            year =Convert.ToInt16(YearDropDown.SelectedValue);
    }

    private void PopulateYearDropDown()
    {
        int CurrentYear = DateTime.Now.Year;
        int StartYear = 1920;

        for( int i=CurrentYear;i>=StartYear;i--)
        {
            YearDropDown.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }



    protected void CreateUser_Click(object sender, EventArgs e)
    {
        var manager = new UserManager();
        var user = new ApplicationUser() { UserName = UserName.Text };
        IdentityResult result = manager.Create(user, Password.Text);
        //if (result.Succeeded)
        //{
        valid = new Validation();
        string incriptedText=StringCipher.Encrypt(Password.Text);
        valid.CreateUser(UserName.Text,, FirstName.Text, LastName.Text);
        //    IdentityHelper.SignIn(manager, user, isPersistent: false);
        //    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
        //}
        //else
        //{
        //    ErrorMessage.Text = result.Errors.FirstOrDefault();
        //}

       
    }

    private void PopulateMonthDropDown()
    {
        var months = System.Globalization.DateTimeFormatInfo.InvariantInfo.MonthNames;
       

        for (int i = 0; i < months.Length; i++)
        {
            if (months[i].ToString() != "")
            {
                MonthDropDown.Items.Add(new ListItem(months[i], i.ToString()));
            }
        }

        
    }
    protected void FirstName_TextChanged(object sender, EventArgs e)
    {
        string Name;
        char FirstLetter;
        
        Name = FirstName.Text;

        FirstLetter = Name[0];
       
        UserName.Text = char.ToUpper(FirstLetter).ToString();
        LastName.Focus();
    }
    protected void LastName_TextChanged(object sender, EventArgs e)
    {
        string name = LastName.Text;
        string user = name.Substring(0, 6);

        
            UserName.Text = UserName.Text + user;
        
        
    }
}