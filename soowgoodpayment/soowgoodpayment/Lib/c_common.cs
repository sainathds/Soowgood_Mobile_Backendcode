using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace soowgoodpayment.Lib
{
    public class c_common
    {
        string connstr = "";
        static public string key = "";
        static public string secretkey = "";
        static public bool paymenttestmode =false;
        static public string mainapplicationurl = "";
        public c_common()
        {
            connstr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            string paymode = System.Web.Configuration.WebConfigurationManager.AppSettings["paymentmode"];             
            if (paymode == "0")
            {
                key = "soowg628cb3f66b878";
                secretkey = "soowg628cb3f66b878@ssl";
                paymenttestmode = true;
            }
            else
            {
                key = "soowg628cb3f66b878";
                secretkey = "soowg628cb3f66b878@ssl";
                paymenttestmode = false;
            }
            mainapplicationurl = System.Web.Configuration.WebConfigurationManager.AppSettings["mainapplicationurl"];
        }


        public DataSet gettransactiondetails(string transactionid)
        {
            
            DataSet ds = new DataSet();            
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand sqlComm = new SqlCommand("pr_booking_gettransactiondetails", conn);
                sqlComm.Parameters.AddWithValue("@transactionid", transactionid);
                sqlComm.CommandTimeout = 0;
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
                
            }
           
        }

        public DataSet gettransactiondetailsforrefno(string refno)
        {

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand sqlComm = new SqlCommand("pr_booking_gettransactiondetailsforrefno", conn);
                sqlComm.Parameters.AddWithValue("@refno", refno);
                sqlComm.CommandTimeout = 0;
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
            }
            return ds;
        }



        public DataSet updatepaymentstatusforappointment(string tranpkid, string bank_tran_id, string statuscode, string errormessage)
        {

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connstr);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand sqlComm = new SqlCommand("pr_booking_updatepaymentstatusforappointment", conn);
                sqlComm.Parameters.AddWithValue("@tranpkid", tranpkid);
                sqlComm.Parameters.AddWithValue("@bank_tran_id", bank_tran_id);
                sqlComm.Parameters.AddWithValue("@statuscode", statuscode);
                sqlComm.Parameters.AddWithValue("@errormessage", errormessage);
                sqlComm.CommandTimeout = 0;
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
            }
            return ds;
        }


    }
}