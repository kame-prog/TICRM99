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
    ||  Class [ProductPriceListManager] 
    ||
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [This class serves as a bridge between the front end and the database. 
    ||             Getting, Updating and Saving poduct price list. Getting a specific poduct
    ||             price list on the basis of Id]
    ||
    ||  Inherits From:  [BaseManager]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class       Akhtar Zaman]
    ****************************************************************************************/
    public class ProductPriceListManager : BaseManager
    {
        public ProductPriceListManager() { }

        /// <summary>
        /// Gets the product price list dt os.
        /// </summary>
        /// <returns>List&lt;ProductPriceListDTO&gt;.</returns>
        public List<ProductPriceListDTO> GetProductPriceListDTOs()
        {
            try
            {
                InsertEventLog("GetProductPriceListDTOs", EventType.Log, EventColor.yellow, "to get list of Product price list ", "TICRM.BuisnessLayer.ProductPriceListManager.GetProductPriceListDTOs", "");

                List<ProductPriceListDTO> productPriceListDTOs = new List<ProductPriceListDTO>(); // initilize the list object

                List<ProductPriceList> productPriceList = dbEnt.ProductPriceLists.Include(c => c.Status).Include(x =>x.Currency).Include(c=>c.ProductCatelog).ToList(); // Get List Of Product Price List from DB
                foreach (ProductPriceList item in productPriceList)
                {
                    productPriceListDTOs.Add(objMapper.ProductPriceListDTO(item)); // add in a list object
                }
                return productPriceListDTOs; // return Collection Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetProductPriceListDTOs", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ProductPriceListManager.GetProductPriceListDTOs", "");
                throw;
            }
        }

        /// <summary>
        /// Submits the product price list.
        /// </summary>
        /// <param name="productPriceListDTO">The product price list dto.</param>
        /// <param name="CurrentUserId">The current user identifier.</param>
        /// <param name="isEditMode">if set to <c>true</c> [is edit mode].</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SubmitProductPriceList(ProductPriceListDTO productPriceListDTO, string CurrentUserId, bool isEditMode = false, bool isDeleteMode = false)
        {
            try
            {
                InsertEventLog("SubmitProductPriceList", EventType.Log, EventColor.yellow, "enter ", "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");

                ProductPriceList productPriceList; // create a new object
                productPriceList = objMapper.GetProductPriceList(productPriceListDTO); // pass parameter object to productPriceListDTO object
                if (isEditMode) // check if is is edit mode is true
                {
                    ProductPriceList dbData = dbEnt.ProductPriceLists.FirstOrDefault(x => x.ProductPriceId == productPriceList.ProductPriceId); // get data from database and pass in new productPriceList class object

                    if (dbData != null) // check if data is null
                    {
                        if (isDeleteMode) // if is delete mode is true 
                        {
                            InsertEventLog("SubmitProductPriceList", EventType.Log, EventColor.yellow, "enter in Delete mode to delete event log ", "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");
                            dbEnt.ProductPriceLists.Remove(dbData); // remove object in database
                        }
                        else
                        {
                            InsertEventLog("SubmitProductPriceList", EventType.Log, EventColor.yellow, "enter in edit mode to update Data event log ", "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");
                            dbData.CurrencyId = productPriceList.CurrencyId;
                            dbData.Amount = productPriceList.Amount;
                            dbData.ProductId = productPriceList.ProductId;
                            dbData.StatusId = productPriceList.StatusId;
                            dbData.Description = productPriceList.Description;
                            dbData.UpdatedDate = DateTime.Now;
                            dbData.UpdatedBy = CurrentUserId;
                            dbEnt.Entry(dbData).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        InsertEventLog("SubmitProductPriceList", EventType.Log, EventColor.yellow, "enter in edit mode data is null ", "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");
                        return false; // return false if no any condition found for edit and delete
                    }

                    if (dbEnt.SaveChanges() > 0) // check if database save changes is done return true
                    {
                        InsertEventLog("SubmitProductPriceList", EventType.Log, EventColor.yellow, "for edit and delete data is saved in DB ", "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");
                        return true;
                    }

                }
                else
                {
                    InsertEventLog("SubmitProductPriceList", EventType.Log, EventColor.yellow, "Enter In Create new record ", "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");
                    productPriceList.ProductPriceId = Guid.NewGuid();
                    productPriceList.CreatedBy = CurrentUserId;
                    productPriceList.CreatedDate = DateTime.Now;
                    dbEnt.ProductPriceLists.Add(productPriceList); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        InsertEventLog("SubmitProductPriceList", EventType.Log, EventColor.yellow, "New Record is saved ", "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                InsertEventMonitor("SubmitProductPriceList", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ProductPriceListManager.SubmitProductPriceList", "");
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Gets the product price list on identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>ProductPriceListDTO.</returns>
        public ProductPriceListDTO GetProductPriceListOnId(Guid? guid)
        {
            try
            {
                InsertEventLog("GetProductPriceListOnId", EventType.Log, EventColor.yellow, "get event log on id ", "TICRM.BuisnessLayer.ProductPriceListManager.GetProductPriceListOnId", "");
                return objMapper.ProductPriceListDTO(dbEnt.ProductPriceLists.FirstOrDefault(x => x.ProductPriceId == guid)); // Get Category On Id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetProductPriceListOnId", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BuisnessLayer.ProductPriceListManager.GetProductPriceListOnId", "");
                throw ex;
            }
        }

    }
}
