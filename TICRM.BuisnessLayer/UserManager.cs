using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DTOs;
using TICRM.DAL;
using System.Data.Entity;

namespace TICRM.BuisnessLayer
{
    /************************************************************************************
    ||  Class [UserManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving users . Getting a specific users
    ||             on the basis of Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ||  Changes Made:   [13/10/2020     User Escalation       Akhtar Zaman]
    ****************************************************************************************/
    public class UserManager : BaseManager
    {
        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets all the users and returns it in the list
        /// </summary>
        /// <returns></returns>

        public List<UserDto> GetAllUsers()
        {
            try
            {
                InsertEventLog("GetAllUsers", EventType.Log, EventColor.yellow, "Successfully Enter in GetAllUsers", "TICRM.BusinessLayer.UserManager", "");

                List<UserDto> userDto = new List<UserDto>(); // create list Object of user DTO

                List<User> user = dbEnt.Users.ToList(); // Get List Of User from DB
                // apply iteration on getting ReadingTypes
                foreach (User  item in user.CollectionNotNull())
                {
                    userDto.Add(objMapper.GetUserDTO(item)); // add in a list object
                    
                }
               
                return userDto; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetAllUsers", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.UserManager", "");
                throw;
            }
        }

        /// <summary>
        /// Gets the cases on user id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public List<CaseDto> GetUserCases(Guid userId)
        {
            try
            {
                InsertEventLog("GetUserCases", EventType.Log, EventColor.yellow, "Successfully Enter in GetUserCases", "TICRM.BusinessLayer.UserManager", "");

                List<CaseDto> caseDto = new List<CaseDto>(); // create list Object of user DTO

                List<Case> cases = dbEnt.Cases.Where(x => x.AssignedUser == userId && x.RelatedTo == RelatedToEnum.Device.ToString() && x.CaseResolution == null).ToList(); // Get List Of User from DB
                // apply iteration on getting ReadingTypes
                foreach (Case item in cases.CollectionNotNull())
                {
                    CaseDto c = new CaseDto();
                    c = objMapper.GetCaseDto(item);
                    c.dLat = objMapper.GetDeviceDTO(dbEnt.Devices.Where(x => x.DeviceId == item.RelatedToId).FirstOrDefault()).Latitude.ToString();
                    c.dLong = objMapper.GetDeviceDTO(dbEnt.Devices.Where(x => x.DeviceId == item.RelatedToId).FirstOrDefault()).Longitude.ToString();
                    caseDto.Add(c); // add in a list object

                }

                return caseDto; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetUserCases", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.UserManager", "");
                throw;
            }
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method gets a specific User on id and returns it as an object
        /// </summary>
        /// <returns></returns>

        public UserDto GetUser(Guid? guid)
        {
            try
            {
                InsertEventLog("GetUser", EventType.Log, EventColor.yellow, "Successfully Enter in ReadingTypeManager to Get Data on id", "TICRM.BusinessLayer.UserManager", "");
                return objMapper.GetUserDTO(dbEnt.Users.Find(guid)); // get User on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetUser", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.UserManager", "");
                throw ex;
            }
        }

        /// <summary>
        /// Escalates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cases">The cases.</param>
        /// <returns></returns>
        public UserDto EscalateUser(UserDto user, CaseDto cases)
        {
            User newUser = dbEnt.Users.Where(x => x.UserId != user.UserId && x.IsAssigned == false).FirstOrDefault();
            Case caseup = objMapper.GetCase(cases);
            user.AssignedItem = null;
            user.AssignedItemId = null;
            user.AssignedItemTime = null;
            user.IsAssigned = false;
            user.StatusId = Guid.Parse("fb6bab54-3e26-4270-a875-34bc7f72afd8");
            dbEnt.SaveChanges();
            caseup.AssignedUser = newUser.UserId;
            dbEnt.SaveChanges();
            newUser.AssignedItem = RelatedToEnum.Cases.ToString();
            newUser.AssignedItemId = cases.CaseId;
            newUser.IsAssigned = true;
            newUser.AssignedItemTime = DateTime.Now;
            newUser.StatusId = Guid.Parse("192f959f-2dfa-4d41-8464-dd482325dc6c");
            
            if (dbEnt.SaveChanges() > 0)
            {
                return objMapper.GetUserDTO(newUser);
            }
            else
                return null;
            
        }

        /// <summary>
        /// Gets the user team.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public TeamDto GetUserTeam (Guid userId)
        {
            Team team = dbEnt.TeamUsers.Where(x => x.UserId == userId).FirstOrDefault().Team;
            return objMapper.GetTeamDTO(team);
        }

        /// <summary>
        /// Code by AKhtar Zaman
        /// 17/7/2020
        /// the method save a specific user on edit, create and delete actions 
        /// </summary>
        /// <returns></returns>
        public bool SaveUser(UserDto userDto, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SaveUser", EventType.Log, EventColor.yellow, "Successfully Enter in SaveUser", "TICRM.BusinessLayer.UserManager", "");

                User user; // create a new object
                user = objMapper.GetUser(userDto); // pass parameter object to radingType object
                if (isEditMode) // check if is is edit mode is true
                {


                    User dbData = dbEnt.Users.FirstOrDefault(x => x.UserId == user.UserId); // get data from database and pass in new reading class object

                    if (dbData != null) // check if data is null
                    {

                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SaveUser", EventType.Log, EventColor.yellow, "For Delete Successfully Enter in SaveUser", "TICRM.BusinessLayer.UserManager", "");
                            dbEnt.Users.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SaveUser", EventType.Log, EventColor.yellow, "For Create Successfully Enter SaveUser", "TICRMTICRM.BuisnessLayer.UserManager", "");
                            dbData.UserId = user.UserId;
                            dbData.Name = user.Name;
                            dbData.Email = user.Email;
                            dbData.Phone = user.Phone;
                            dbData.StatusId = user.StatusId;
                            dbData.CreatedBy = user.CreatedBy;
                            dbData.CreatedDate = user.CreatedDate;
                            dbData.UpdatedBy = user.UpdatedBy;
                            dbData.UpdatedDate = user.UpdatedDate;
                            dbData.IsAssigned = user.IsAssigned;
                            dbData.AssignedItem = user.AssignedItem;
                            dbData.AssignedItemId = user.AssignedItemId;
                            dbData.AssignedItemTime = user.AssignedItemTime;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SaveUser", EventType.Log, EventColor.yellow, "For Edit and Delete: Data is null on id " + userDto.UserId, "TICRM.BuisnessLayer.UserManager", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {

                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SaveUser", EventType.Log, EventColor.yellow, "For Create Successfully Enter SaveUser", "TICRMTICRM.BuisnessLayer.UserManager", "");

                    user = objMapper.GetUser(userDto);  // pass parameter user object to user object
                    user.UserId= Guid.NewGuid();
                    dbEnt.Users.Add(user); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveUser", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRMTICRM.BuisnessLayer.UserManager", "");
                throw ex;
            }
            return false;

        }

    }
}
