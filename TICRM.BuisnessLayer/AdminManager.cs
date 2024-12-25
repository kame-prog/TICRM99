using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;
using TICRM.Mapper;

namespace TICRM.BuisnessLayer
{
    public class AdminManager : BaseManager
    {

        //Retrieves  all companies accounts List and return them as a list of AccountDto object
        public List<AccountDto> AllCompaniesAccounts()
        {
            try
            {
                // Log the entry into AllCompaniesAccounts method
                InsertEventLog("AllCompaniesAccounts", EventType.Log, EventColor.yellow, "Successfully Enter in AllCompaniesAccounts", "TICRM.BusinessLayer.AdminManager", "");

                List<AccountDto> AllAccounts = new List<AccountDto>();              //Create the AccountDto object
                List<Account> accounts = dbEnt.sp_AllCompAccounts_Get().ToList();   //Create the Account object

                // Iterate through each Account and convert it to AccountDto using objMapper
                foreach (Account item in accounts.CollectionNotNull())
                {
                    AllAccounts.Add(objMapper.GetAccountDTO(item));
                }
                // Return the list of All companies Accounts
                return AllAccounts;
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        //Retrieves  all companies Devices List and return them as a list of DeviceDto object
        public List<DeviceDto> AllCompanyDevices()
        {
            try
            {
                // Log the entry into AllCompanyDevices method
                InsertEventLog("AllCompanyDevices", EventType.Log, EventColor.yellow, "Successfully Enter in AllCompanyDevices", "TICRM.BusinessLayer.AdminManager", "");
              
                List<DeviceDto> AllDevices = new List<DeviceDto>();                     //Create the DeviceDto object
                List<Device> devices = dbEnt.sp_AllCompDevices_Get().ToList();          //Create the Device object

                // Iterate through each devices and convert it to DeviceDto using objMapper
                foreach (Device item in devices.CollectionNotNull())
                {
                    AllDevices.Add(objMapper.GetDeviceDTO(item));
                }
                //Return the list of all compnies devices
                return AllDevices;
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred in AllCompanyDevices", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

       
        //Retrive the Payment link in the list object
        public List<PaymentDto> GetPayments()
        {
            try
            {
                // Log the entry into AllCompanyDevices method
                InsertEventLog("GetPayments", EventType.Log, EventColor.yellow, "Successfully Enter in GetPayments", "TICRM.BusinessLayer.AdminManager", "");
              
                List<PaymentDto> paymentDtos = new List<PaymentDto>();       //Create the PaymentDto object            
                List<Payment> payments= dbEnt.Payments.ToList();            //Create the Payment object

                // Iterate through each Payment and convert it to paymentDtos using objMapper
                foreach (Payment item in payments.CollectionNotNull())
                {
                    paymentDtos.Add(objMapper.GetPaymentDto(item));
                }
                //Return the Payment data
                return paymentDtos;
            }
            catch (Exception ex)
            { 
                // Log the exception using log4net
                log.Error("An error occurred in GetPayments", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        //Get Single record for edit/updating record
        public PaymentDto GetPayment(Guid? id)
        {
            try
            {
                // Log the entry into GetPayment  method
                InsertEventLog("GetPayment", EventType.Log, EventColor.yellow, "Successfully Enter in GetPayment", "TICRM.BusinessLayer.AdminManager", "");

                var payment = objMapper.GetPaymentDto(dbEnt.Payments.Find(id));  //Retrive payment data according to id from the DB.
                //Return the payment data.
                return payment;
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred in GetPayment", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        //Save and update Payment Link method::

        public bool SavePaymentLink(PaymentDto paymentDto, bool isEdit=false)
        {
            try
            {
                // Log the entry into SavePaymentLink  method
                InsertEventLog("SavePaymentLink", EventType.Log, EventColor.yellow, "Successfully Enter in SavePaymentLink", "TICRM.BusinessLayer.AdminManager", "");
               
                //Create the payment object
                Payment payment;
                if (isEdit)
                {
                    // Log the entry into edit  method in the SavePaymentLink method
                    InsertEventLog("EditPaymentLink", EventType.Log, EventColor.yellow, "Successfully Enter in Edit Payment Link method", "TICRM.BusinessLayer.AdminManager", "");

                    payment = objMapper.GetPayment(paymentDto);                                 
                    Payment ObjPayment=dbEnt.Payments.FirstOrDefault(x=>x.ID==paymentDto.ID);   //Retrive the Payment data according to the id that's we want to update
                    if (ObjPayment!=null)
                    {
                        ObjPayment.PaymentLink = payment.PaymentLink;
                        ObjPayment.Description = payment.Description;
                    }
                }
                else
                {
                    //Save payment link method
                    payment = objMapper.GetPayment(paymentDto);
                    payment.ID=Guid.NewGuid();                      //Give new Guid.
                    dbEnt.Payments.Add(payment);                    //Data add in the db Payment table.
                }
                if (dbEnt.SaveChanges()>0)                          //If data save then return true otherwise return false.
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred in SavePaymentLink", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        // Method to authenticate the super admin credentials
        public bool AuthenticateSuperAdmin(SuperAdminLoginDto adminLoginDto)
        {
            // here we fetch all the needed details from the DB agains this email
            var SuperAdmin = (from super in dbEnt.SuperAdminCreds
                             where super.Email == adminLoginDto.Email
                             select new
                             {
                                 super.Email,
                                 super.Password
                             }).FirstOrDefault();
            //Check against this email data present in DB or Not
            if (SuperAdmin != null)
            {
                //Here compare password manually.
                if (SuperAdmin.Password==adminLoginDto.Password)
                {
                    //If user is authenticate then return True
                    return true;
                }
            }
            //If user is not authenticate then return False
            return false;
        }

    }
}
