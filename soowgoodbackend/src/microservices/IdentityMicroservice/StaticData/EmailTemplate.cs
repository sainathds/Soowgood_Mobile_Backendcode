using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public static class EmailTemplate
    {
        public static string UserSignupSubject = "SoowGood - E-Mail Verification for SignUp";
        public static string UserForgotPassword = "SoowGood - Password Reset";
        public static string UserConfirmation = "SoowGood - Confirmation";

        public static string AppointmentCancelByPatient = "SoowGood - Confirming Cancelled Appointments";
        public static string AppointmentCancelToProviderByPatient = "SoowGood - Appointments Cancelled By Patient";

        public static string AppointmentCancelByDoctor = "SoowGood - Confirming Cancelled Appointments";
        public static string AppointmentCancelToPatientByProvider = "SoowGood - Confirming Cancelled Appointments";

        public static string AppointmentConfirmedByPatient = "SoowGood - Confirming Appointments";


        public static string OnlineAppointment = "SoowGood - Scheduled Appointments";



        public static string RequestforPayment = "SoowGood - Request for Payment";

        public static string CompleteRequestforPayment = "SoowGood - Payment Request Completed";


        public static string NewEnquiry = "SoowGood - New Equiry from website";

        public static string EnquiryConfirmation = "SoowGood - Thank you for contacting us.";


        public static string GetUserSignupTemplate(string FullName, string OTP)
        {


            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Verify your email address</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +
                         "<p>" +
                            " Your OTP to authenticate your email. Do not share this OTP with anyone.</p>" +
             "<p style='text-align:center;font-size:24px;'>" +
                 "<b>" + OTP + "</b>" +
             "</p>" +
             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }

        public static string GetUserForgotPasswordTemplate(string FullName, string OTP)
        {


            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Reset Your Password</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +
                         "<p>" +
                            " Your OTP to authenticate your email for reset password. Do not share this OTP with anyone. " +
                "</p>" +
            "<p style='text-align:center;font-size:24px;'>" +
                 "<b>" + OTP + "</b>" +
             "</p>" +
             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }



        public static string GetUserConfirmationTemplate(string FullName, string URL)
        {


            URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Confirmation</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +
                         "<p>" +
                            " Your profile has been confirmed by Team SoowGood." +
                "</p>" +
             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }




        public static string AppointmentCancelToPatientByPatient(string FullName, string AppointmentDate, string AppointmentTime)
        {


            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Confirming Cancelled Appointments</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "This email serves as a notification that you have cancelled your appointment on <b>" + AppointmentDate + "</b> at <b>" + AppointmentTime + "</b>." +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }


        public static string AppointmentCancelToDoctorByDoctor(string FullName, string AppointmentDate, string AppointmentTime)
        {


            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Confirming Cancelled Appointments</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "This email serves as a notification that you have cancelled your appointment on <b>" + AppointmentDate + "</b> at <b>" + AppointmentTime + "</b>." +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }


        public static string AppointmentCancelToDoctorByPatient(string FullName, string PatientFullName, string patientComment, string AppointmentDate, string AppointmentTime)
        {


            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Confirming Cancelled Appointments</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "This email serves as a notification that, <b>" + PatientFullName + "</b> have cancelled  appointment on <b>" + AppointmentDate + "</b> at <b>" + AppointmentTime + "</b>." +
             "</p>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  " Reason to cancel appointment is , <b>" + patientComment + "</b>." +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }


        public static string AppointmentCancelToPatientByDoctor(string FullName, string PatientFullName, string doctorComment, string AppointmentDate, string AppointmentTime)
        {


            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Confirming Cancelled Appointments</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + PatientFullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "This email serves as a notification that <b>" + FullName + "</b> have cancelled appointment on <b>" + AppointmentDate + "</b> at <b>" + AppointmentTime + "</b>." +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  " Reason to cancel appointment is , <b>" + doctorComment + "</b>." +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }




        public static string AppointmentConfirmedToPatient(string FullName, string DoctorName, string CliencName, string appointmenttype, string patientAddress, string ratingnurl, string AppointmentDate, string AppointmentTime)
        {
            string visitingaddress = "";

            if(appointmenttype== "Clinic")
            {
                visitingaddress = CliencName;
            }else if (appointmenttype == "Online")
            {
                visitingaddress = appointmenttype;
            }
            else
            {
                visitingaddress = patientAddress;
            }

            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Confirming Appointments</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "This email is to let you know that your appointment on [" + AppointmentDate + " - " + AppointmentTime + "] with <b>" + DoctorName + "</b> at <b>" + visitingaddress + "</b> has been confirmed." +
             "</p>";
            if (appointmenttype != "Online")
            {

                URL= URL+ "<p style='text-align:left;font-size:20px;padding:15px 15px 0px 15px;'>" +
                  "After appointment complete please rate your doctor using following link <br />" +
                  "<a href='" + ratingnurl + "' target='_blank'>" + ratingnurl + "</a>" +
             "</p>";
             }


            URL = URL + "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +
             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }


        public static string AppointmentConfirmedToDoctor(string FullName, string PatientName, string patientAddress,string ClinicName, string AppointmentType, string AppointmentDate, string AppointmentTime)
        {


            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Confirming Appointments</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "You have new appointment for <b>" + AppointmentType + "</b>" +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  " Following are the appointment details" +
             "</p>" +
               
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "<b>Patient Name : </b>" + PatientName +
             "</p>";
            if (AppointmentType == "Physical Visit")
            {
                URL = URL + "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                 "<b>Patient Address : </b>" + patientAddress +
                "</p>";
            }else if (AppointmentType == "Clinic")
            {
                URL = URL + "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                 "<b>Clinic Name : </b>" + ClinicName +
                "</p>";
            }
            URL = URL + "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                "<b>Appointment Date : </b>" + AppointmentDate +
               "</p>"+
               "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                "<b>Appointment Time : </b>" + AppointmentTime +
               "</p>" +
           "<p> Have a Good Day!<br>Team" +
                   "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
           "</div>" +
           "<div style = 'margin-bottom:20px;'>" +
                    "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                         "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                "<br>Copyright &copy; 2020.All rights reserved!" +
                              "</p>" +
                          "</div>" +
                      "</div>" +
                    "</div>" +
              "</body>" +
              "</html>";

            return URL;
        }



        public static string AppointmentSchedulePatient(string FullName,string DoctorName,string scheduleDate,string scheduleTime,string scheduleDay,string ScheduleLink)
        {
            
            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Online Appointment</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "When - "+ scheduleDay+" "+ scheduleDate + " - " + scheduleTime +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "With - " + DoctorName +
             "</p>" +
              "<p style='text-align:left;font-size:20px;padding:15px 15px 0px 15px;text-align:center;'>" +
                   
                  "<a href='" + ScheduleLink + "'  style='padding:10px 15px;background-color:#312e81;color:#FFFFFF;' target='_blank'>Join</a>" +
             "</p>" +


              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }


        public static string AppointmentScheduleDoctor(string FullName, string PatientName, string scheduleDate, string scheduleTime, string scheduleDay, string ScheduleLink)
        {

            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Online Appointments</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +

             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "When - "+ scheduleDay + " " + scheduleDate + " - " + scheduleTime+
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "With - " + PatientName +
             "</p>" +
              "<p style='text-align:left;font-size:20px;padding:15px 15px 0px 15px;text-align:center'>" +
                  
                  "<a href='" + ScheduleLink + "'  style='padding:10px 15px;background-color:#312e81;color:#FFFFFF;' target='_blank'>Join</a>" +
             "</p>" +


              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }

        public static string RequestforPaymentToAdmin(string FullName, string scheduleDate, string scheduleTime,double doctorcharges)
        {

            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Payment Request</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear Admin,</h2>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "New payment request has been received. Following are the details "+
             "</p>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "Provider Name - <b>" + FullName+"</b>"+
             "</p>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "Appointment - <b>" +  scheduleDate + " - " + scheduleTime + "</b>" +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "Amount - <b>" + doctorcharges + "</b>" +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }



        public static string CompleteRequestforPaymentToAdmin(string FullName, string scheduleDate, string scheduleTime, double doctorcharges)
        {

            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Payment Request</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear "+ FullName + ",</h2>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  " This email serves as a notification that your request for payment of <b>"+ doctorcharges + "</b> against appointment on Dated "+ scheduleDate + " has been processed successfully by SoowGood Team." +
             "</p>" + 
             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }



        public static string SendNewEnquiryEmailToAdmin(string FullName, string email, string mobileno, string message,string adress)
        {

            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>New Equiry from website</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear Admin,</h2>" +
             "<p style='text-align:left;font-size:13px;padding:15px 15px 0px 15px;'>" +
                  "My name is  <b>" + FullName + "</b> and I'm reaching out because "+
             "</p>" +
             "<p style='text-align:left;font-size:16px;padding:15px 15px 0px 15px;'>" +
                  "<b>"+message+"</b>"+
             "</p>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "More about me"+
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "Email - " + email +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "Mobile No. - " + mobileno +
             "</p>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "Address - " + adress +
             "</p>" +
              "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  "We look forward to serving you in the future." +
             "</p>" +


             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }



        public static string SendNewEnquiryConfirmation(string FullName)
        {

            string URL = $"<html xmlns = 'http://www.w3.org/1999/xhtml'>" +
             "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<meta name='viewport' content='width=device-width'/>" +
            "</head>" +
            "<body style='font-family:Lato;width:100%;height:100%;background:#f8f8f8;text-align:center;'>" +
            "<div style='padding:10px 0 0 0;font-size: 100%;line-height:1.65;'>" +
                "<div style = 'display: block ; clear: both ; margin: auto; max-width: 580px ; width: 100%;" +
            "border-style:solid;border-width:1px;border-color:#2ed3ae;background:white;'>" +
            "<div style='padding:80px 0;background:#2ed3ae; color:white;'>" +
              "<h1 style ='margin:auto;font-size:32px;line-height:1.5;max-width:60%;text-transform:uppercase;'>Thank you for contacting us.</h1>" +
               "</div>" +
               "<div style='font-size:16px;font-weight:normal;margin-bottom:20px;'>" +
                    "<h2 style='font-size:28px;line-height:1.25;'>Dear " + FullName + ",</h2>" +
             "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  " This is to let you know that we have received your email and Team SoowGood will contact you soon." +
             "</p>" +
               "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  " This is an auto generated response to your email sent to us." +
             "</p>" +
               "<p style='text-align:left;font-size:15px;padding:15px 15px 0px 15px;'>" +
                  " Please do not reply to this email as it will not be received." +
             "</p>" +
             "<p> Have a Good Day!<br>Team" +
                    "<a style='color:#2ed3ae;text-decoration:none;' href='https://SoowGood.com'> SoowGood </a>.</p>" +
            "</div>" +
            "<div style = 'margin-bottom:20px;'>" +
                     "<p style='margin-bottom: 0;color:#888;font-size:14px;'>" +
                          "<a style='color: #888;text-decoration:none;font-weight: bold;'href='mailto:'>info@SoowGood.com </a>" +
                                 "<br>Copyright &copy; 2020.All rights reserved!" +
                               "</p>" +
                           "</div>" +
                       "</div>" +
                     "</div>" +
               "</body>" +
               "</html>";

            return URL;
        }




    }
}
