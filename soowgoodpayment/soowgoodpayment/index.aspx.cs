using soowgoodpayment.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace soowgoodpayment
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            if (!IsPostBack)
            {
                if (Request.QueryString["transactionid"] != null && Request.QueryString["transactionid"] != string.Empty)
                {
                    c_common objcom = new c_common();
                    ds = objcom.gettransactiondetails(Request.QueryString["transactionid"]);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        processpayment(ds);
                    }
                    else
                    {

                    }
                }

            }
        }


        public void processpayment(DataSet ds)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            NameValueCollection PostData = new NameValueCollection();
            PostData.Add("total_amount", ds.Tables[0].Rows[0]["PaidAmount"].ToString());
            PostData.Add("tran_id", ds.Tables[0].Rows[0]["refno"].ToString());
            PostData.Add("success_url", baseUrl + "transactioncomplete.aspx");
            PostData.Add("fail_url", baseUrl + "transactioncomplete.aspx"); // "Fail.aspx" page needs to be created
            PostData.Add("cancel_url", baseUrl + "transactioncomplete.aspx"); // "Cancel.aspx" page needs to be created
            PostData.Add("version", "3.00");
            PostData.Add("cus_name", ds.Tables[0].Rows[0]["ServiceReceiverName"].ToString());
            PostData.Add("cus_email", ds.Tables[0].Rows[0]["ServiceReceiverEmail"].ToString());
            PostData.Add("cus_add1", ds.Tables[0].Rows[0]["ServiceReceiverAddress"].ToString());            
            PostData.Add("cus_city", ds.Tables[0].Rows[0]["ServiceReceiverCity"].ToString());
            PostData.Add("cus_state", ds.Tables[0].Rows[0]["ServiceReceiverState"].ToString());
            PostData.Add("cus_postcode", ds.Tables[0].Rows[0]["ServiceReceiverPostalCode"].ToString());
            PostData.Add("cus_country", ds.Tables[0].Rows[0]["ServiceReceiverCountry"].ToString());
            PostData.Add("cus_phone", ds.Tables[0].Rows[0]["ServiceReceiverPhoneNo"].ToString());
           // PostData.Add("cus_fax", "0171111111");
            PostData.Add("ship_name", ds.Tables[0].Rows[0]["ServiceReceiverName"].ToString());
            PostData.Add("ship_add1", ds.Tables[0].Rows[0]["ServiceReceiverAddress"].ToString());
            //PostData.Add("ship_add2", "Address Line Tw");
            PostData.Add("ship_city", ds.Tables[0].Rows[0]["ServiceReceiverCity"].ToString());
            PostData.Add("ship_state", ds.Tables[0].Rows[0]["ServiceReceiverState"].ToString());
            PostData.Add("ship_postcode", ds.Tables[0].Rows[0]["ServiceReceiverPostalCode"].ToString());
            PostData.Add("ship_country", ds.Tables[0].Rows[0]["ServiceReceiverCountry"].ToString());
            //PostData.Add("value_a", "ref00");
            //PostData.Add("value_b", "ref00");
            //PostData.Add("value_c", "ref00");
            //PostData.Add("value_d", "ref00");
            PostData.Add("shipping_method", "NO");
            PostData.Add("num_of_item", "1");
            PostData.Add("product_name", "Soowgood");
            PostData.Add("product_profile", "general");
            PostData.Add("product_category", "Soowgood - Appointment");
            SSLCommerz sslcz = new SSLCommerz(c_common.key, c_common.secretkey, c_common.paymenttestmode);
            String response = sslcz.InitiateTransaction(PostData);
            Response.Redirect(response);
        }


    }
}