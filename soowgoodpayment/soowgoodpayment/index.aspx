<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="soowgoodpayment.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title>SooWGooD</title>
    <link rel="icon" type="image/x-icon" href="favicon.png">
    <link href="css/app.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.5.0/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" rel="stylesheet">
    <link href="alertifyjs/css/alertify.min.css" rel="stylesheet" />
    <link href="alertifyjs/css/themes/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="d-flex justify-content-center align-items-center w-100 p-4">
            <div class="container-fluid">
                <div class="row h-100">
                    <div class="col-12">
                        <div class="row banner">
                            <div class="col-12">
                                <div class="row">
                                    <div class="col-md-12 col-lg-12 col-sm-12">
                                        <img src="/img/SoowGood-Logo.png" class="logo_sooWGood" />
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <div class="loadingdiv" style="height:80% !important;">
                                    <h3 style="text-align:center;font-size:18px;">Please wait, We are processing your payment request. Don't refresh page page</h3>
                                    <div class="spinner"></div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
