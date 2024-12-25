using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [EmailTemplateManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting email templates in general and specifically on Id and saving it. 
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class EmailTemplateManager : BaseManager
    {
        /// <summary>
        /// Gets the email template dtos.
        /// </summary>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <returns>List&lt;EmailTemplateDTO&gt;.</returns>
        public List<EmailTemplateDTO> GetEmailTemplateDTOs(string CurrentUserId)
        {
            try
            {
                InsertEventLog("GetEmailTemplateDTOs", EventType.Log, EventColor.yellow, "going to get list of Email Template DTO", "TICRM.BuisnessLayer.EmailTemplateManager.GetEmailTemplateDTOs", CurrentUserId);
                List<EmailTemplateDTO> emailTemplateDTOs = new List<EmailTemplateDTO>();// create strongly type list Object of EmailConfiguration DTO

                List<EmailTemplate> emailTemplates = dbEnt.EmailTemplates.Include(e => e.EmailConfiguration).Include(e => e.WorkFlow).ToList(); // Get List Of EmailTemplate from DB
                // apply iteration on email template list
                foreach (EmailTemplate item in emailTemplates.CollectionNotNull())
                {
                    emailTemplateDTOs.Add(objMapper.GetEmailTemplateDTO(item)); // convert object in DTO object
                }
                return emailTemplateDTOs; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEmailTemplateDTOs", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EmailTemplateManager.GetEmailTemplateDTOs", CurrentUserId);
                throw;
            }
        }

        /// <summary>
        /// Saves the email template.
        /// </summary>
        /// <param name="emailTemplateDTO">The email template dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveEmailTemplate(EmailTemplateDTO emailTemplateDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveEmailTemplate", EventType.Log, EventColor.yellow, "Successfully Enter in function", "TICRM.BuisnessLayer.EmailTemplateManager.SaveEmailTemplate", CurrentUserId);

                EmailTemplate emailTemplate; // create a new object
                emailTemplate = objMapper.GetEmailTemplate(emailTemplateDTO); // pass parameter object and convert in EmailTemplateDTO to EmailTemplate object
                if (isEditMode) // check if is is edit mode is true
                {
                    EmailTemplate dbData = dbEnt.EmailTemplates.FirstOrDefault(x => x.EmailTemplateId == emailTemplate.EmailTemplateId); // get data from database and pass in new EmailTemplate class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveEmailTemplate", EventType.Log, EventColor.yellow, "to delete Email Template on id=" + emailTemplate.EmailConfigurationId + " ", "TICRM.BuisnessLayer.EmailTemplateManager.SaveEmailTemplate", CurrentUserId);
                            dbEnt.EmailTemplates.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveEmailTemplate", EventType.Log, EventColor.yellow, "to update Email Template on id=" + emailTemplate.EmailConfigurationId + " ", "TICRM.BuisnessLayer.EmailTemplateManager.SaveEmailTemplate", CurrentUserId);
                            dbData.EmailTemplateId = emailTemplate.EmailTemplateId;
                            dbData.EmailConfigurationId = emailTemplate.EmailConfigurationId;
                            dbData.WorkFlowId = emailTemplate.WorkFlowId;
                            dbData.Subject = emailTemplate.Subject;
                            dbData.Body = emailTemplate.Body;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveEmailTemplate", EventType.Log, EventColor.yellow, "to Email Template on id=" + emailTemplate.EmailConfigurationId + " return null data ", "TICRM.BuisnessLayer.EmailTemplateManager.SaveEmailTemplate", CurrentUserId);
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveEmailTemplate", EventType.Log, EventColor.yellow, "create new record of Email Template", "TICRM.BuisnessLayer.EmailTemplateManager.SaveEmailTemplate", CurrentUserId);
                    emailTemplate.EmailTemplateId = Guid.NewGuid();
                    emailTemplate.CreatedBy = CurrentUserId;
                    emailTemplate.CreatedDate = DateTime.Now;
                    dbEnt.EmailTemplates.Add(emailTemplate); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveEmailTemplate", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EmailTemplateManager.SaveEmailTemplate", CurrentUserId);
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Gets the email template dto on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <returns>EmailTemplateDTO.</returns>
        public EmailTemplateDTO GetEmailTemplateDtoOnId(Guid? guid, string CurrentUserId)
        {
            try
            {
                InsertEventLog("GetEmailTemplateDtoOnId", EventType.Log, EventColor.yellow, "to get event monitor on id=" + guid + " ", "TICRM.BuisnessLayer.EmailTemplateManager.GetEmailTemplateDtoOnId", "");
                return objMapper.GetEmailTemplateDTO(dbEnt.EmailTemplates.FirstOrDefault(x => x.EmailTemplateId == guid)); // Get EmailTemplates On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetEmailTemplateDtoOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.EmailTemplateManager.GetEmailTemplateDtoOnId", "");
                throw ex;
            }
        }

    }
}
