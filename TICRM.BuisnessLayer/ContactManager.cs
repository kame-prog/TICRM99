using System;
using System.Collections.Generic;
using System.Linq;
using TICRM.DTOs;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using System.Web;
using System.Data.Entity;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [ContactManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [|This class serves as a bridge between the front end and the database. 
    ||             All the crud operations are being performed here. Details for a specific
    ||             Contacts are get from the database and mapped with the
    ||             corrosponding DTO object to send it back to the controller]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ContactManager : BaseManager
    {
        public ContactManager()
        {
        }
        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ContactDto.</returns>
        public ContactDto GetContact(long? guid)
        {
            try
            {
                InsertEventLog("GetContact", EventType.Log, EventColor.yellow, "Get ContactDto", "TICRM.BusinessLayer.ContactManager.GetContact", "");
                return objMapper.GetContactDto(dbEnt.Contacts.Find(guid));
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetContact", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ContactManager.GetContact", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }
        /// <summary>
        /// get list of contacts from database
        /// </summary>
        /// <returns></returns>
        public List<ContactDto> GetContacts(string CurrentUserId, string UserRole, string UserCompanyID)
        {
            try
            {
                InsertEventLog("GetContacts", EventType.Log, EventColor.yellow, "Get list of ContactDto", "TICRM.BusinessLayer.ContactManager.GetContacts", "");
                List<ContactDto> ContactDtos = new List<ContactDto>();
                List<Contact> contacts = dbEnt.sp_Contacts_Get(CurrentUserId, UserRole, UserCompanyID).ToList();

                foreach (Contact item in contacts.CollectionNotNull())
                {
                    ContactDtos.Add(objMapper.GetContactDto(item));
                }

                //List<Contact> contacts = dbEnt.Contacts.Include(x => x.Account).Include(x => x.Status).Include(x => x.Team).Include(x => x.User).Include(x => x.Cases).Where(x => x.IsDeleted != true && x.CreatedBy == CurrentUserId).ToList(); //Include(l => l.LeadSource).Include(l => l.LeadType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true).ToList();

                //foreach (Contact item in contacts.CollectionNotNull())
                //{
                //    ContactDtos.Add(objMapper.GetContactDto(item));
                //}
                return ContactDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetContacts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ContactManager.GetContacts", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }


        }

        /// <summary>
        /// Get contacts on account id
        /// </summary>
        /// <param name="AccountId">The account identifier.</param>
        /// <returns>System.Collections.Generic.List&lt;TICRM.DTOs.ContactDto&gt;.</returns>
        public List<ContactDto> GetAccountContacts(Guid AccountId)
        {
            try
            {
                InsertEventLog("GetContacts", EventType.Log, EventColor.yellow, "Get list of ContactDto", "TICRM.BusinessLayer.ContactManager.GetContacts", "");
                List<ContactDto> ContactDtos = new List<ContactDto>();
                List<Contact> contacts = dbEnt.Contacts.Where(x => x.AccountId == AccountId).Include("Account").ToList(); //Include(l => l.LeadSource).Include(l => l.LeadType).Include(l => l.Status).Include(l => l.Team).Include(l => l.User).Where(a => a.IsDeleted != true).ToList();
                foreach (Contact item in contacts.CollectionNotNull())
                {
                    ContactDtos.Add(objMapper.GetContactDto(item));

                }
                return ContactDtos;
            }
            catch (Exception ex)
            {

                InsertEventMonitor("GetContacts", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ContactManager.GetContacts", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

        }

        /// <summary>
        /// Gets the contacts list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>List&lt;ContactDto&gt;.</returns>
        public List<ContactDto> GetContactsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            try
            {
                InsertEventLog("GetContactsList", EventType.Log, EventColor.yellow, "Get List of Contacts Based on Datatable Query", "TICRM.BuisnessLayer.ContactManager.GetContactsList", "");

                var ContactDto = new List<ContactDto>();
                var contact = new List<Contact>();

                string test = string.Empty;
                sSearch = sSearch.ToLower();
                dbEnt.Database.CommandTimeout = 1800;
                

                if (!string.IsNullOrEmpty(sSearch))
                {
                    contact = dbEnt.Contacts.Where(a => a.Name.ToLower().Contains(sSearch)
                    || a.Phone.ToLower().Contains(sSearch)
                    || a.Email.ToLower().Contains(sSearch)
                    || a.Team.Name.ToLower().Contains(sSearch)
                    || a.User.Name.ToLower().Contains(sSearch)
                    || a.CreatedDate.ToString().ToLower().Contains(sSearch)
                    || a.Address.ToLower().Contains(sSearch)
                    || a.Status.Name.ToLower().Contains(sSearch)
                    ).OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                }
                else
                    contact = dbEnt.Contacts.OrderBy(x => x.CreatedDate).Skip(iDisplayStart).Take(iDisplayLength).ToList();

                foreach (Contact item in contact.CollectionNotNull())
                {
                    ContactDto.Add(objMapper.GetContactDto(item)); // add in a list object
                }

                return ContactDto;

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetContactsList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ContactManager.GetContactsList", "");

                throw ex;
            }
        }

        public ContactDto GetContact(Guid? id)
        {
            throw new NotImplementedException();
        }

        public ContactDto GetAccountContacts(Guid? id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetTotalCount()
        {
            try
            {
                InsertEventLog("GetTotalCount", EventType.Log, EventColor.yellow, "Get total Count of Contacts", "TICRM.BuisnessLayer.ContactManager.GetTotalCount", "");
                dbEnt.Database.CommandTimeout = 1800;
                return dbEnt.Contacts.AsQueryable().Count(); // Get EventLog total and then return in response

            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetTotalCount", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ContactManager.GetTotalCount", "");
                throw ex;
            }
        }

        /// <summary>
        /// Save and edit the contacts
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="isEditMode"></param>
        /// <param name="isDeleteMode"></param>
        /// <param name="CurrentUserId"></param>
        /// <returns></returns>
        public bool SaveContact(ContactDto contactDto, string CurrentUserId,string UserCompanyID, bool isEditMode, bool isDeleteMode)
        {
            try
            {
                InsertEventLog("SaveContact", EventType.Log, EventColor.yellow, "Enter", "TICRM.BusinessLayer.ContactManager.SaveContact", "");
                Contact contact;

                if (isEditMode)
                {
                    InsertEventLog("SaveContact", EventType.Log, EventColor.yellow, "going to edit Contact of id =" + contactDto.ContactId + "", "TICRM.BusinessLayer.ContactManager.SaveContact", "");

                    contact = objMapper.GetContact(contactDto);
                    if (isDeleteMode)
                    {
                        InsertEventLog("SaveContact", EventType.Log, EventColor.yellow, "going to delete Contact of id =" + contact.Id + "", "TICRM.BusinessLayer.ContactManager.SaveContact", "");

                        Contact contactDelete = dbEnt.Contacts.FirstOrDefault(x => x.Id == contact.Id);
                        contactDelete.IsDeleted = true;
                        //dbEnt.Entry(contactDelete).State = EntityState.Modified;
                    }
                    else
                    {
                        Contact ContactEdit = dbEnt.Contacts.FirstOrDefault(x => x.Id == contact.Id);
                        ContactEdit.Name = contact.Name;
                        ContactEdit.Email = contact.Email;
                        ContactEdit.Phone = contact.Phone;
                        ContactEdit.AccountId = contact.AccountId;
                        ContactEdit.AssignedUser = contact.AssignedUser;
                        ContactEdit.AssignedTeam = contact.AssignedTeam;
                        ContactEdit.StatusId = contact.StatusId;
                        ContactEdit.Address = contact.Address;
                        ContactEdit.StreetAddress_2 = contact.StreetAddress_2;
                        ContactEdit.City = contact.City;
                        ContactEdit.ZipCode = contact.ZipCode;
                        ContactEdit.Country = contact.Country;
                        ContactEdit.UpdatedDate = DateTime.Now;
                        ContactEdit.UpdatedBy = CurrentUserId;
                    }
                }
                else
                {
                    InsertEventLog("SaveContact", EventType.Log, EventColor.yellow, "going to create new Record", "TICRM.BusinessLayer.ContactManager.SaveContact", "");
                    contact = objMapper.GetContact(contactDto);
                    contact.CreatedDate = DateTime.Now;
                    contact.CreatedBy = CurrentUserId;
                    contact.Company = Guid.Parse(UserCompanyID );
                    dbEnt.Contacts.Add(contact);
                }
                HttpContext.Current.Session["ContactObject"] = contact;
                HttpContext.Current.Session["CurrentUserId"] = CurrentUserId;

                if (dbEnt.SaveChanges() > 0)
                {
                    InsertEventLog("SaveContact", EventType.Log, EventColor.yellow, "Contact saved Successfully of id =" + contact.Id+ "", "TICRM.BusinessLayer.ContactManager.SaveContact", "");
                    return true;
                }
            }
            catch (Exception ex)
            {

                InsertEventMonitor("SaveContact", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.ContactManager.SaveContact", "");
                // Log the exception using log4net
                log.Error("An error occurred in AllCompaniesAccounts", ex);

                // Rethrow the exception to preserve the original exception stack trace
                throw;
            }

            return false;
        }
    }
}
