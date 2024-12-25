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
    ||  Class [ProductCatelogManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving poduct catalogs. Getting a specific poduct
    ||             catalogs on the basis of Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ProductCatelogManager : BaseManager
    {
        public ProductCatelogManager() { }
        /// <summary>
        /// Gets the product catelog dtos.
        /// </summary>
        /// <returns>List&lt;ProductCatelogDTO&gt;.</returns>
        public List<ProductCatelogDTO> GetProductCatelogDtos()
        {
            try
            {
                InsertEventLog("GetProductCatelogDtos", EventType.Log, EventColor.yellow, "to get list of productcatelog ", "TICRM.BuisnessLayer.ProductCatelogManager.GetProductCatelogDtos", "");

                List<ProductCatelogDTO> productCatelogDTOs = new List<ProductCatelogDTO>(); // initilize the list object


                List<ProductCatelog> products = dbEnt.ProductCatelogs.Include(c => c.Status).Include(c => c.Team).Include(c => c.User).Where(x => x.IsDeleted == false).ToList(); // Get List Of product from DB
                                                                                                                                                                         // apply iteration on list
                foreach (ProductCatelog item in products.CollectionNotNull())
                {
                    productCatelogDTOs.Add(objMapper.GetProductCatelogDTO(item)); // add in a list object
                }
                return productCatelogDTOs; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetProductCatelogDtos", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ProductCatelogManager.GetProductCatelogDtos", "");
                throw;
            }
        }

        /// <summary>
        /// Submits the product catelog.
        /// </summary>
        /// <param name="productCatelogDTO">The product catelog dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SubmitProductCatelog(ProductCatelogDTO productCatelogDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SubmitProductCatelog", EventType.Log, EventColor.yellow, "enter ", "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");

                ProductCatelog _productCatelog; // create a new object
                _productCatelog = objMapper.GetProductCatelog(productCatelogDTO); // pass parameter object to get productcatelog db object
                if (isEditMode) // check if is is edit mode is true
                {
                    ProductCatelog dbData = dbEnt.ProductCatelogs.FirstOrDefault(x => x.ProductId == _productCatelog.ProductId); // get data from database and pass in new Productcatelog class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SubmitProductCatelog", EventType.Log, EventColor.yellow, "enter in Delete mode to delete event log ", "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");
                            dbEnt.ProductCatelogs.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SubmitProductCatelog", EventType.Log, EventColor.yellow, "enter in edit mode to update Data event log ", "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");
                            dbData.ProductId = _productCatelog.ProductId;
                            dbData.ProductName = _productCatelog.ProductName;
                            dbData.SerialNumber = _productCatelog.SerialNumber;
                            dbData.CategoryId = _productCatelog.CategoryId;
                            dbData.ValidFrom = _productCatelog.ValidFrom;
                            dbData.ValidTo = _productCatelog.ValidTo;
                            dbData.Description = _productCatelog.Description;
                            dbData.ProductNote = _productCatelog.ProductNote;
                            dbData.AssignedUser = _productCatelog.AssignedUser;
                            dbData.AssignedTeam = _productCatelog.AssignedTeam;
                            dbData.StatusId = _productCatelog.StatusId;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SubmitProductCatelog", EventType.Log, EventColor.yellow, "enter in edit mode data is null ", "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SubmitProductCatelog", EventType.Log, EventColor.yellow, "for edit and delete data is saved in DB ", "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SubmitProductCatelog", EventType.Log, EventColor.yellow, "Enter In Create new record ", "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");
                    _productCatelog.ProductId = Guid.NewGuid();
                    _productCatelog.CreatedBy = CurrentUserId;
                    _productCatelog.CreatedDate = DateTime.Now;
                    _productCatelog.IsDeleted = false;
                    dbEnt.ProductCatelogs.Add(_productCatelog); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("SubmitProductCatelog", EventType.Log, EventColor.yellow, "New Record is saved ", "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SubmitProductCatelog", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ProductCatelogManager.SubmitProductCatelog", "");
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Gets the product catelog on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ProductCatelogDTO.</returns>
        public ProductCatelogDTO GetProductCatelogOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetProductCatelogOnId", EventType.Log, EventColor.yellow, "get event log on id ", "TICRM.BuisnessLayer.ProductCatelogManager.GetProductCatelogOnId", "");
                return objMapper.GetProductCatelogDTO(dbEnt.ProductCatelogs.FirstOrDefault(x => x.ProductId == guid)); // Get product catelog On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetProductCatelogOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ProductCatelogManager.GetProductCatelogOnId", "");
                throw ex;
            }
        }
    }
}
