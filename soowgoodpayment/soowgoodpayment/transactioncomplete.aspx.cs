using soowgoodpayment.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace soowgoodpayment
{
    public partial class transactioncomplete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tranpkid = "";
            try
            {
                string TrxID = Request.Form["tran_id"];
                c_common objcommon = new c_common();
                DataSet ds = objcommon.gettransactiondetailsforrefno(TrxID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tranpkid = ds.Tables[0].Rows[0]["Id"].ToString();
                    string amount = ds.Tables[0].Rows[0]["PaidAmount"].ToString();
                    string currency = ds.Tables[0].Rows[0]["trancurrency"].ToString();
                    string bank_tran_id = "";
                    string statuscode = "";
                    string errormessage = "";
                    SSLCommerz sslcz = new SSLCommerz(c_common.key, c_common.secretkey, c_common.paymenttestmode);

                    if (!String.IsNullOrEmpty(Request.Form["status"]) && Request.Form["status"] == "VALID")
                    {
                        if (sslcz.OrderValidate(TrxID, amount, currency, Request))
                        {
                            if (!String.IsNullOrEmpty(Request.Form["bank_tran_id"]))
                            {
                                bank_tran_id = Request.Form["bank_tran_id"].ToString();
                            }
                        }
                        statuscode = "E000";
                        errormessage = "";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Request.Form["bank_tran_id"]))
                        {
                            bank_tran_id = Request.Form["bank_tran_id"].ToString();
                        }
                        statuscode = "E008";
                        if (!String.IsNullOrEmpty(Request.Form["error"]))
                        {
                            errormessage = Request.Form["error"].ToString(); ;
                        }
                        else
                        {
                            errormessage = "";
                        }
                    }
                    DataSet dspayment = objcommon.updatepaymentstatusforappointment(tranpkid, bank_tran_id, statuscode, errormessage);
                    Response.Redirect(c_common.mainapplicationurl + "payment-response/" + tranpkid);
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}