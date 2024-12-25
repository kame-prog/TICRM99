using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TICRM.BuisnessLayer.Base;
using TICRM.DAL;
using TICRM.DTOs;

namespace TICRM.BuisnessLayer
{
    public class CategoryManager : BaseManager
    {
        public CategoryManager() { }

        /// <summary>
        /// Gets the category dtos.
        /// </summary>
        /// <returns>List&lt;CategoryDto&gt;.</returns>
        public List<CategoryDto> GetCategoryDtos()
        {
            try
            {
                InsertEventLog("GetCategoryDtos", EventType.Log, EventColor.yellow, "to get list of Category ", "TICRM.BuisnessLayer.CategoryManager.GetCategoryDtos", "");

                List<CategoryDto> categoryDtos = new List<CategoryDto>(); // initilize the list object


                List<Category> categories = dbEnt.Categories.Include(c => c.Status).Include(c => c.Team).Include(c => c.User).Where(x=>x.IsDeleted == false).ToList(); // Get List Of Category from DB
                                                                     // apply iteration on workFlowMappings
                foreach (Category item in categories.CollectionNotNull())
                {
                    categoryDtos.Add(objMapper.GetCategoryDTO(item)); // add in a list object
                }
                return categoryDtos; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCategoryDtos", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CategoryManager.GetCategoryDtos", "");
                throw ex;
            }
        }

        /// <summary>
        /// Saving Category.
        /// </summary>
        /// <param name="categoryDto">The category dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SubmitCategory(CategoryDto categoryDto, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter ", "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");

                Category category; // create a new object
                category = objMapper.GetDtoToCategory(categoryDto); // pass parameter object to categoryDto object
                if (isEditMode) // check if is is edit mode is true
                {
                    Category dbData = dbEnt.Categories.FirstOrDefault(x => x.CategoryId == category.CategoryId); // get data from database and pass in new Category class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter in Delete mode to delete event log ", "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");
                            dbEnt.Categories.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter in edit mode to update Data event log ", "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");
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
                        InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "enter in edit mode data is null ", "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "for edit and delete data is saved in DB ", "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "Enter In Create new record ", "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");
                    category.CategoryId = Guid.NewGuid();
                    category.CreatedBy = CurrentUserId;
                    category.CreatedDate = DateTime.Now;
                    category.IsDeleted = false;
                    dbEnt.Categories.Add(category); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("SubmitCategory", EventType.Log, EventColor.yellow, "New Record is saved ", "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SubmitCategory", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CategoryManager.SubmitCategory", "");
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
                InsertEventLog("GetCategoryOnId", EventType.Log, EventColor.yellow, "get event log on id ", "TICRM.BuisnessLayer.CategoryManager.GetCategoryOnId", "");
                return objMapper.GetCategoryDTO(dbEnt.Categories.FirstOrDefault(x => x.CategoryId == guid)); // Get Category On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCategoryOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.CategoryManager.GetCategoryOnId", "");
                throw ex;
            }
        }

    }
}
