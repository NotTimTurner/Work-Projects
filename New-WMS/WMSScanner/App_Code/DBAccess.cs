using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;


public class DBAccess
    {
    public string strErr;
    private string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["DBPhilly"].ConnectionString;

    private DataTable LoadTable(SqlCommand cmd)
        {
        DataTable retn = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        adapter.Fill(retn);
        return retn;
        }



    /* ///////////////////////////////////////////// 
      ///                                        ///
      /// PLEASE KEEP EXAMPLES FOR FEATURED USE  ///
      ///                                        ///
      //////////////////////////////////////////////
        */

    //-----select Locations for populate Location GridView-----
    public DataTable GetSBoxLocation()
    {
        DataTable retn = null;

        try
        {
            //Connecting SQL connections
            using (SqlConnection cn = new SqlConnection()) // Instantiation
            {
                cn.ConnectionString = Conn; // Connections
                cn.Open(); // Opening connections

                //Connect to Store Procedure "   "
                using (SqlCommand cmd = new SqlCommand("spGetPULocations"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    retn = LoadTable(cmd);
                }
                cn.Close();
            }
        }
        catch (Exception ex)
        {
            strErr = ex.Message;
            return null;
        }
        return retn;
    }


    // ---------select example with paramiters-------
    public int SaveFullInfo(string AppId, string scheduleNewInterviewDate, string comments, string suggestion)
    {
        int NewInID = 0;
        try
        {
            //Connecting SQL connections
            using (SqlConnection cn = new SqlConnection()) // Instantiation
            {
                cn.ConnectionString = Conn; // Connections
                cn.Open(); // Opening connections

                //Connect to Store Procedure "spGetAllProducts"
                using (SqlCommand cmd = new SqlCommand("spInterviewDetails"))
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@AppId", SqlDbType.Int).Value = Convert.ToInt32(AppId);
                    cmd.Parameters.Add("@ScheduleNewInterviewDate", SqlDbType.Date).Value = Convert.ToDateTime(scheduleNewInterviewDate).Date;
                    cmd.Parameters.Add("@Comments", SqlDbType.VarChar).Value = comments;
                    cmd.Parameters.Add("@Suggestion", SqlDbType.VarChar).Value = suggestion;

                    cmd.Parameters.Add("@InterviewDetalID", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    NewInID = (int)cmd.Parameters[4].Value;
                }
                cn.Close();
            }
        }
        catch (Exception ex)
        {
            strErr = ex.Message;
            return NewInID;
        }
        return NewInID;
    }


    

    //-----select example-----
    public DataTable GetStates()
        {
        DataTable retn = null;

        try
            {
            //Connecting SQL connections
            using (SqlConnection cn = new SqlConnection()) // Instantiation
                {
                cn.ConnectionString = Conn; // Connections
                cn.Open(); // Opening connections

                //Connect to Store Procedure "   "
                using (SqlCommand cmd = new SqlCommand("spGetPULocations"))
                    {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    retn = LoadTable(cmd);
                    }
                cn.Close();
                }
            }
        catch (Exception ex)
            {
            strErr = ex.Message;
            return null;
            }
        return retn;
        }


    //---------Insert Example--------

    public int InsertApplycant(string firstName, string lastName, string mi, string dob, string gender, string race, string addrs1, string addrs2, string city, string state, string zipCode, string phone, string eMail)
        {

        int appID = 0;
        try
            {
            //Connecting SQL connections
            using (SqlConnection cn = new SqlConnection()) // Instantiation
                {
                cn.ConnectionString = Conn; // Connections
                cn.Open(); // Opening connections

                //Connect to Store Procedure "spGetAllProducts"
                using (SqlCommand cmd = new SqlCommand("spInsertApplycant"))
                    {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = firstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = lastName;
                    cmd.Parameters.Add("@MI", SqlDbType.NVarChar).Value = mi;
                    cmd.Parameters.Add("@DOB", SqlDbType.NVarChar).Value = Convert.ToDateTime(dob);
                    cmd.Parameters.Add("@Gender", SqlDbType.Int).Value = gender;
                    cmd.Parameters.Add("@Race", SqlDbType.Int).Value = race;
                    cmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = addrs1;
                    cmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = addrs2;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = city;
                    cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = state;
                    cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar).Value = zipCode;
                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = phone;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = eMail;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    appID = (int)cmd.Parameters[13].Value;
                    }
                cn.Close();
                }
            }
        catch (Exception ex)
            {
            strErr = ex.Message;
            return appID;
            }
        return appID;
        }



    //---------Update example with paramiters--------

    public void UpdateApplicant(ref int UserID, string firstName, string lastName, string address1, string address2, string city, string state, string zipCode, string phone, string eMail, DateTime intervievDate)
        {
        try
            {

            using (SqlConnection cn = new SqlConnection())
                {
                cn.ConnectionString = Conn;
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spUpdateApplycantFromDv"))
                    {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = firstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = lastName;
                    cmd.Parameters.Add("@Address1", SqlDbType.VarChar).Value = address1;
                    cmd.Parameters.Add("@Address2", SqlDbType.VarChar).Value = address2;
                    cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = city;
                    cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = state;
                    cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = zipCode;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = eMail;
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = phone;
                    cmd.Parameters.Add("@InterviewDate", SqlDbType.DateTime).Value = intervievDate;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                    cmd.ExecuteNonQuery();
                    }
                cn.Close();
                }
            }
        catch (Exception ex)
            {
            strErr = ex.Message;
            }
        }
   

    //---Ubpdate Users by Id---

    public void UpdateUsers(ref int UserID, string firstName, string lastName, string UserName, string Password, string Phone, string Email)
    {
       string encriptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");
        try
            {

            using (SqlConnection cn = new SqlConnection())
                {
                cn.ConnectionString = Conn;
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("spUpdateUsersFromDv"))
                    {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = firstName;
                    cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = lastName;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = encriptedPassword;
                    cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = Phone;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                    cmd.ExecuteNonQuery();
                    }
                cn.Close();
                }
            }
        catch (Exception ex)
            {
            strErr = ex.Message;
            }
        }
    }

