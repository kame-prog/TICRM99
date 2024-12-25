using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    public class UserAccountManager:BaseManager
    {

        protected CRMEntities dbEnt = new CRMEntities();
        //This method is used for saveing the company name in the DB when any user signup in the SWUICH
        public bool SaveCompany(CompanyDto companyDto) 
        {
			try
			{
                Company company;
                company=objMapper.GetCompany(companyDto);
                dbEnt.Companies.Add(company);
                if (dbEnt.SaveChanges() > 0)
                {
                    return true;
                }
                return false;
            }
			catch (Exception ex)
			{
                // Log the exception using log4net
                log.Error("An error occurred in AllCompanyDevices", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            
		}

        //This method is used for Fetching the user detail
        public EditUserDto GetAccUser(string id)
        {
            try
            {
                InsertEventLog("GetAccUser", EventType.Log, EventColor.yellow, "Successfully Enter in GetAccUser", "TICRM.BusinessLayer.UserAccountManager", "");
                return objMapper.GetAccUserDto(dbEnt.AspNetUsers.Find(id));
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAccUser", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.UserAccountManager", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        //Edit Profile method
        public bool EditProfile(EditUserDto userRegister)
        {
            try
            {
                InsertEventLog("EditProfile", EventType.Log, EventColor.yellow, "Successfully Enter in EditProfile", "TICRM.BusinessLayer.UserAccountManager", "");

                AspNetUser aspNetUser;
                aspNetUser = objMapper.GetAccUser(userRegister);

                //Checking record is present in DB or not.
                AspNetUser ObjUser = dbEnt.AspNetUsers.FirstOrDefault(x => x.Id == aspNetUser.Id);
                if (ObjUser != null)
                {
                    ObjUser.FirstName= aspNetUser.FirstName;
                    ObjUser.LastName= aspNetUser.LastName;
                    //ObjUser.Email= aspNetUser.Email;
                    ObjUser.PhoneNumber= aspNetUser.PhoneNumber;
                    //ObjUser.CompanyId= aspNetUser.CompanyId;
                    ObjUser.Industryid= aspNetUser.Industryid;
                    ObjUser.Countryid= aspNetUser.Countryid;
                }
                if (dbEnt.SaveChanges()>0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                InsertEventMonitor("EditProfile", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.UserAccountManager", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
    }
}
