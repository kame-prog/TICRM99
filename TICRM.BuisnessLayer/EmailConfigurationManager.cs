using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [EmailConfigurationManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting email configration in general and specifically on Id and saving it. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class EmailConfigurationManager : BaseManager
    {
        /// <summary>
        /// Gets the email configuration dtos.
        /// </summary>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <returns>List&lt;EmailConfigurationDTO&gt;.</returns>
        public List<EmailConfigurationDTO> GetEmailConfigurationDTOs(string CurrentUserId)
        {
            try
            {
                InsertEventLog("GetEmailConfigurationDTOs", EventType.Log, EventColor.yellow, "going to get list of Email Configuration DTO", "TICRM.BuisnessLayer.EmailConfigurationManager.GetEmailConfigurationDTOs", CurrentUserId);
                List<EmailConfigurationDTO> emailConfigurationDTOs = new List<EmailConfigurationDTO>();// create strongly type list Object of EmailConfiguration DTO

                List<EmailConfiguration> emailConfigurations = dbEnt.EmailConfigurations.ToList(); // Get List Of EmailConfiguration from DB
                // apply iteration on email configration
                foreach (EmailConfiguration item in emailConfigurations.CollectionNotNull())
                {
                    emailConfigurationDTOs.Add(objMapper.GetEmailConfigurationDTO(item)); // add in a list DTO object
                }
                return emailConfigurationDTOs; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEmailConfigurationDTOs", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EmailConfigurationManager.GetEmailConfigurationDTOs", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        /// <summary>
        /// Saves the email configuration.
        /// </summary>
        /// <param name="emailConfigurationDTO">The email configuration dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveEmailConfiguration(EmailConfigurationDTO emailConfigurationDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveEmailConfiguration", EventType.Log, EventColor.yellow, "Successfully Enter in function", "TICRM.BuisnessLayer.EmailConfigurationManager.SaveEmailConfiguration", CurrentUserId);

                EmailConfiguration emailConfiguration; // create a new object
                emailConfiguration = objMapper.GetEmailConfiguration(emailConfigurationDTO); // pass parameter object to Email Configuration object
                if (isEditMode) // check if is is edit mode is true
                {
                    EmailConfiguration dbData = dbEnt.EmailConfigurations.FirstOrDefault(x => x.EmailConfigurationId == emailConfiguration.EmailConfigurationId); // get data from database and pass in new EmailConfiguration class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveEmailConfiguration", EventType.Log, EventColor.yellow, "to delete Email Configuration on id=" + emailConfiguration.EmailConfigurationId + " ", "TICRM.BuisnessLayer.EmailConfigurationManager.SaveEmailConfiguration", CurrentUserId);
                            dbEnt.EmailConfigurations.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveEmailConfiguration", EventType.Log, EventColor.yellow, "to update Email Configuration on id=" + emailConfiguration.EmailConfigurationId + " ", "TICRM.BuisnessLayer.EmailConfigurationManager.SaveEmailConfiguration", CurrentUserId);
                            dbData.EmailConfigurationId = emailConfiguration.EmailConfigurationId;
                            dbData.Email = emailConfiguration.Email;
                            dbData.UserName = emailConfiguration.UserName;
                            dbData.Password = emailConfiguration.Password;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveEmailConfiguration", EventType.Log, EventColor.yellow, "to Email Configuration on id=" + emailConfiguration.EmailConfigurationId + " return null data ", "TICRM.BuisnessLayer.EmailConfigurationManager.SaveEmailConfiguration", CurrentUserId);
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveEmailConfiguration", EventType.Log, EventColor.yellow, "create new record of Email Configuration", "TICRM.BuisnessLayer.EmailConfigurationManager.SaveEmailConfiguration", CurrentUserId);
                    emailConfiguration.EmailConfigurationId = Guid.NewGuid();
                    emailConfiguration.CreatedBy = CurrentUserId;
                    emailConfiguration.CreatedDate = DateTime.Now;
                    dbEnt.EmailConfigurations.Add(emailConfiguration); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveEmailConfiguration", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EmailConfigurationManager.SaveEmailConfiguration", CurrentUserId);
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
            return false;
        }

        /// <summary>
        /// Gets the email configuration dto on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <returns>EmailConfigurationDTO.</returns>
        public EmailConfigurationDTO GetEmailConfigurationDtoOnId(Guid? guid,string CurrentUserId)
        {
            try
            {
                InsertEventLog("GetEmailConfigurationDtoOnId", EventType.Log, EventColor.yellow, "to get Email Configuration on id=" + guid + " ", "TICRM.BuisnessLayer.EmailConfigurationManager.GetEmailConfigurationDtoOnId", "");
                return objMapper.GetEmailConfigurationDTO(dbEnt.EmailConfigurations.FirstOrDefault(x => x.EmailConfigurationId == guid)); // Get EmailConfiguration On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEmailConfigurationDtoOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EmailConfigurationManager.GetEmailConfigurationDtoOnId", "");
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        public bool EmailIntegrations(EmailIntegrationDto emailIntegrationDto, string CurrentUserId, string UserCompanyID, string UserRole, bool isEditMode = false)
        {
            try
            {
                EmailIntegration emailIntegration;
                if (isEditMode) 
                { 
                    //Update email integration
                    emailIntegration=objMapper.GetEmailIntegration(emailIntegrationDto);
                    EmailIntegration ObjEmail = dbEnt.EmailIntegrations.FirstOrDefault(x => x.id == emailIntegrationDto.id);
                    if (ObjEmail != null)
                    {
                        ObjEmail.SenderEmail= emailIntegrationDto.SenderEmail;
                        ObjEmail.Subject   = emailIntegrationDto.Subject;
                        ObjEmail.Host= emailIntegrationDto.Host;
                        ObjEmail.Password= emailIntegrationDto.Password;
                        ObjEmail.Port= emailIntegrationDto.Port;
                        ObjEmail.UpdateBy = CurrentUserId;
                        ObjEmail.UpdatedDate= DateTime.Now;
                    }
                }
                else
                {
                    //Create email itegration
                    emailIntegration = objMapper.GetEmailIntegration(emailIntegrationDto);
                    emailIntegration.id= Guid.NewGuid();
                    emailIntegration.Role = UserRole;
                    Guid companyID = UserCompanyID != null ? Guid.Parse(UserCompanyID) : Guid.Empty;
                    emailIntegration.Company = companyID;
                    //emailIntegration.Company = Guid.Parse(UserCompanyID);
                    emailIntegration.CreatedBy= CurrentUserId;
                    emailIntegration.CreatedDate= DateTime.Now;
                    dbEnt.EmailIntegrations.Add(emailIntegration);
                }
                if (dbEnt.SaveChanges()>0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }
        

        //Get record for update

        public EmailIntegrationDto GetEmail(Guid? id)
        {
            try
            {
                 return objMapper.GetEmailIntegrationDto(dbEnt.EmailIntegrations.Find(id));
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }


        //Show Email on listing page
        public List<EmailIntegrationDto> GetEmailIntegration(string UserCompanyID, string UserRole)
        {

            try
            {
                List<EmailIntegrationDto> emailIntegrationDtos = new List<EmailIntegrationDto>();
                List<EmailIntegration> email = dbEnt.sp_EmailIntegration_Get(UserCompanyID, UserRole).ToList();
                foreach (EmailIntegration item in email.CollectionNotNull())
                {
                    emailIntegrationDtos.Add(objMapper.GetEmailIntegrationDto(item));
                }
                return emailIntegrationDtos;
            }
            catch (Exception ex)
            {

                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }
        }

        //Send email method
        public bool CompanyEmailSend(string email, string body, string UserCompanyID)
        {

            try
            {
                Guid CompanyID = Guid.Parse(UserCompanyID);   //Company id
                var result = dbEnt.EmailIntegrations.FirstOrDefault(x => x.Company == CompanyID);  //Fetch all data from DB against the Company
                if (result!=null)
                {
                    var smtpClient = new SmtpClient(result.Host)   //Configure host
                    {
                        Port = Convert.ToInt32(result.Port),  //Configure host
                        Credentials = new NetworkCredential(result.SenderEmail, result.Password),  //Configure user name and password for email sending
                        EnableSsl = true,
                    };
                    var message = new MailMessage(result.SenderEmail, email, result.Subject, body);    //Send mail by passing all credentials
                    message.IsBodyHtml = true;        //Remain it true for HTML Template for email
                    smtpClient.Send(message);               //Send Email

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception using log4net
                log.Error("An error occurred", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }


    }
}
