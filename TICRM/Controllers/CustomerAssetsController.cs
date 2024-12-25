using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************CustomerAssets Controller************
   Class [CustomerAssetsController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with CustomerAssets like, 
   ||             navigating to the pages, getting associated modules for specific CustomerAsset]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
   ||                  
    ********************************************/
    
    public class CustomerAssetsController : BaseController
    {
        CustomerAssetManager cam = new CustomerAssetManager();
        LocationManager locationManager = new LocationManager();

        /// <summary>
        /// get assets and return on Index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                ViewBag.AccountId = cam.Accounts;
                ViewBag.LocationId = locationManager.GetLocations();
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the customer asset list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetCustomerAssetList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            List<CustomerAssetDto> obj = cam.GetCustomerAssetList(sEcho, iDisplayStart, iDisplayLength, sSearch);

            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Title).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Model).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Model).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Account.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Account.Name).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Location.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Location.Name).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                    }
                    break;

                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
                    }
                    break;
                default:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.User.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.User.Name).ToList();
                    }
                    break;
            }

            int totalRecord = cam.GetTotalCount();

            var aaData = JsonConvert.SerializeObject(obj);

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("{");
            sb.Append("\"sEcho\": ");
            sb.Append(sEcho);
            sb.Append(",");
            sb.Append("\"iTotalRecords\": ");
            sb.Append(totalRecord);
            sb.Append(",");
            sb.Append("\"iTotalDisplayRecords\": ");
            sb.Append(totalRecord);
            sb.Append(",");
            sb.Append("\"aaData\": ");
            sb.Append(aaData);
            sb.Append("}");
            return sb.ToString();
        }


        /// <summary>
        /// Details view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var customerAsset = cam.GetCustomerAsset(id);
                if (customerAsset == null)
                {
                    return HttpNotFound();
                }
                return View(customerAsset);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial details view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var customerAsset = cam.GetCustomerAsset(id);

                //ViewBag.AccountId = (string)cam.Accounts.Where(x => x.AccountId == customerAsset.AccountId).FirstOrDefault().Name;
                //ViewBag.LocationId = (string)locationManager.GetLocation(customerAsset.LocationId).Name;

                return PartialView("_PartialCustomerAssetsDetails", customerAsset);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message +": "+ ex.InnerException);
            }
        }

        /// <summary>
        /// Partial delete view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                var customerAsset = cam.GetCustomerAsset(id); // get customer assets on id
                //ViewBag.AccountId = (string)cam.Accounts.Where(x => x.AccountId == customerAsset.AccountId).FirstOrDefault().Name; // get name of account
                //ViewBag.LocationId = (string)locationManager.GetLocation(customerAsset.LocationId).Name; // get name of location which is associate with account
                return PartialView("_PartialCustomerAssetsDelete", customerAsset);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the location of account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetLocationOfAccount(Guid accountId)
        {
            try
            {
                SelectList data = new SelectList(locationManager.GetLocations(accountId), "LocationId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Create View.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                CustomerAssetDto customerAsset = new CustomerAssetDto();
                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CustomerAssetTypes, "CustomerAssetTypeId", "Name");
                customerAsset.StatusDropdown= new SelectList(cam.Status, "StatusId", "Name");
                customerAsset.AssignedTeamDropdown = new SelectList(cam.Teams, "TeamId", "Name");
                customerAsset.AssignedUserDropdown = new SelectList(cam.Users, "UserId", "Name");

                customerAsset.AccountsDropdown = new SelectList(cam.Accounts, "AccountId", "Name");
                customerAsset.LocationDropdown = new SelectList(new List<LocationDto>(), "LocationId", "Name");
                return View(customerAsset);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified customer asset.
        /// </summary>
        /// <param name="customerAsset">The customer asset.</param>
        /// <param name="loc">The loc.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerAssetDto customerAsset, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.LocationId = new SelectList(locationManager.GetLocations((Guid)customerAsset.AccountId), "LocationId", "Name", customerAsset.LocationId);

                    customerAsset.CustomerAssetId = Guid.NewGuid();

                    string CurrentUserId = User.Identity.GetUserId();
                    bool condition = cam.SaveCustomerAsset(customerAsset, CurrentUserId);
                    if (condition == true)
                    {
                        if (loc != "False")
                        {
                            Guid temp = new Guid(loc);
                            return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                        }
                        else
                            return RedirectToAction("Index");
                    }
                }

                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CustomerAssetTypes, "CustomerAssetTypeId", "Name", customerAsset.CustomerAssetTypeId);
                customerAsset.StatusDropdown = new SelectList(cam.Status, "StatusId", "Name", customerAsset.StatusId);
                customerAsset.AssignedTeamDropdown = new SelectList(cam.Teams, "TeamId", "Name", customerAsset.AssignedTeam);
                customerAsset.AssignedUserDropdown = new SelectList(cam.Users, "UserId", "Name", customerAsset.AssignedUser);
                customerAsset.AccountsDropdown = new SelectList(cam.Accounts, "AccountId", "Name", customerAsset.AccountId);
                customerAsset.LocationDropdown = new SelectList(locationManager.GetLocations((Guid)customerAsset.AccountId), "LocationId", "Name", customerAsset.LocationId);
               
                if (loc != "False")
                {
                    Guid temp = new Guid(loc);
                    return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                }
                else
                    return View(customerAsset);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edit view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var customerAsset = cam.GetCustomerAsset(id);
                if (customerAsset == null)
                {
                    return HttpNotFound();
                }

                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CustomerAssetTypes, "CustomerAssetTypeId", "Name", customerAsset.CustomerAssetTypeId);
                customerAsset.StatusDropdown = new SelectList(cam.Status, "StatusId", "Name", customerAsset.StatusId);
                customerAsset.AssignedTeamDropdown = new SelectList(cam.Teams, "TeamId", "Name", customerAsset.AssignedTeam);
                customerAsset.AssignedUserDropdown = new SelectList(cam.Users, "UserId", "Name", customerAsset.AssignedUser);
                customerAsset.AccountsDropdown = new SelectList(cam.Accounts, "AccountId", "Name", customerAsset.AccountId);
                customerAsset.LocationDropdown = new SelectList(locationManager.GetLocations((Guid)customerAsset.AccountId), "LocationId", "Name", customerAsset.LocationId);

                return View(customerAsset);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified customer asset.
        /// </summary>
        /// <param name="customerAsset">The customer asset.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerAssetDto customerAsset)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string CurrentUserId = User.Identity.GetUserId();
                    bool condition = cam.SaveCustomerAsset(customerAsset, CurrentUserId, true);
                    if (condition == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
               
                customerAsset.CustomerAssetTypeDropdown = new SelectList(cam.CustomerAssetTypes, "CustomerAssetTypeId", "Name", customerAsset.CustomerAssetTypeId);
                customerAsset.StatusDropdown = new SelectList(cam.Status, "StatusId", "Name", customerAsset.StatusId);
                customerAsset.AssignedTeamDropdown = new SelectList(cam.Teams, "TeamId", "Name", customerAsset.AssignedTeam);
                customerAsset.AssignedUserDropdown = new SelectList(cam.Users, "UserId", "Name", customerAsset.AssignedUser);
                customerAsset.AccountsDropdown = new SelectList(cam.Accounts, "AccountId", "Name", customerAsset.AccountId);
                customerAsset.LocationDropdown = new SelectList(locationManager.GetLocations((Guid)customerAsset.AccountId), "LocationId", "Name", customerAsset.LocationId);
               
                return View(customerAsset);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var customerAsset = cam.GetCustomerAsset(id);
                if (customerAsset == null)
                {
                    return HttpNotFound();
                }
                return View(customerAsset);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var customerAsset = cam.GetCustomerAsset(id);

                string CurrentUserId = User.Identity.GetUserId();
                cam.SaveCustomerAsset(customerAsset, CurrentUserId, true, true);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Creating Customer Asset from Account Right Pannel
        /// </summary>
        /// 
        /// <returns></returns>
        //
        public ActionResult CreateAfromAccount(String Title, String Desc, String User, String Team, String Loc, String Man, String Status, String Model, String Year, String Val, String Dep, String SKU, String Type, String AccID)
        {
            try
            {
                CustomerAssetDto customer = new CustomerAssetDto();
                customer.Title = Title;
                customer.Description = Desc;
                customer.AssignedUser = Guid.Parse(User);
                customer.AssignedTeam = Guid.Parse(Team);
                customer.LocationId = Guid.Parse(Loc);
                customer.Manufacture = Man;
                customer.YearOfManufacture = Int32.Parse(Year);
                customer.StatusId = Guid.Parse(Status);
                customer.Model = Model;
                customer.Value = Int32.Parse(Val);
                customer.DepriciatedValue = Convert.ToDecimal(Dep);
                customer.SKU = SKU;
                customer.CustomerAssetTypeId = Guid.Parse(Type);
                customer.AccountId = Guid.Parse(AccID);
                customer.CustomerAssetId = Guid.NewGuid();
                bool condition = cam.SaveCustomerAsset(customer, null);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the asset types.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAssetTypes()
        {
            try
            {
                return Json(cam.GetAssetTypes(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Gets the asset types based on account ID
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public JsonResult GetAccountAssetTypes(Guid AccountId)
        {
            try
            {
                return Json(cam.GetAccountAssetTypes(AccountId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
