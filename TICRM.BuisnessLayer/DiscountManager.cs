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
    ||  Class [DiscountManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Crud operations on discounts module. Getting discounts and Categories.
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class DiscountManager : BaseManager
    {
        public DiscountManager() { }

        /// <summary>
        /// Gets the discount dt os.
        /// </summary>
        /// <returns>List&lt;DiscountDTO&gt;.</returns>
        public List<DiscountDTO> GetDiscountDTOs()
        {
            try
            {
                InsertEventLog("GetDiscountDTOs", EventType.Log, EventColor.yellow, "to get list of Category ", "TICRM.BuisnessLayer.DiscountManager.GetDiscountDTOs", "");

                List<DiscountDTO> discountDTOs = new List<DiscountDTO>(); // initilize the list object

                List<Discount> discounts = dbEnt.Discounts.ToList(); // Get List Of Discounts from DB
                                                                     // apply iteration on Discounts
                foreach (Discount item in discounts.CollectionNotNull())
                {
                    discountDTOs.Add(objMapper.GetDiscountDTO(item)); // add in a list object
                }

                return discountDTOs; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetDiscountDTOs", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DiscountManager.GetDiscountDTOs", "");
                throw;
            }
        }

        /// <summary>
        /// Submits the discount.
        /// </summary>
        /// <param name="categoryDto">The category dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SubmitDiscount(CategoryDto categoryDto, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter ", "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");

                Category category; // create a new object
                category = objMapper.GetDtoToCategory(categoryDto); // pass parameter object to categoryDto object
                if (isEditMode) // check if is is edit mode is true
                {
                    Category dbData = dbEnt.Categories.FirstOrDefault(x => x.CategoryId == category.CategoryId); // get data from database and pass in new Category class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter in Delete mode to delete event log ", "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");
                            dbEnt.Categories.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter in edit mode to update Data event log ", "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");
                            dbData.Name = category.Name;
                            dbData.Description = category.Description;
                            dbData.AssignedUser = category.AssignedUser;
                            dbData.AssignedTeam = category.AssignedTeam;
                            dbData.StatusId = category.StatusId;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter in edit mode data is null ", "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "for edit and delete data is saved in DB ", "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "Enter In Create new record ", "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");
                    category.CategoryId = Guid.NewGuid();
                    category.CreatedBy = CurrentUserId;
                    category.CreatedDate = DateTime.Now;
                    category.IsDeleted = false;
                    dbEnt.Categories.Add(category); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "New Record is saved ", "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SubmitCategory", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DiscountManager.SubmitCategory", "");
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Gets the category on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>CategoryDto.</returns>
        public CategoryDto GetCategoryOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetCategoryOnId", EventType.Log, EventColor.yellow, "get event log on id ", "TICRM.BuisnessLayer.DiscountManager.GetCategoryOnId", "");
                return objMapper.GetCategoryDTO(dbEnt.Categories.FirstOrDefault(x => x.CategoryId == guid)); // Get Category On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCategoryOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.DiscountManager.GetCategoryOnId", "");
                throw ex;
            }
        }


    }
}
