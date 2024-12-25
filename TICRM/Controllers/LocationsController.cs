using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Locations Controller************
    Class [LocationsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with Locations like, 
    ||             navigating to the pages, getting associated modules for specific Location]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||  Changes Made:   [20/08/2020     Added ServerSide Processing for Jquery DataTables     Sikandar Mustafa]
    ||                  
     ********************************************/

    
    public class LocationsController : BaseController
    {
        LocationManager lm = new LocationManager();
        /// <summary>
        /// get the locations and return on Index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                ViewBag.AccountId = lm.GetAccounts();
                return View(lm.GetLocations());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the locations list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetLocationsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            List<LocationDto> obj = lm.GetLocationsList(sEcho, iDisplayStart, iDisplayLength, sSearch);

            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Name).ToList();
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
                        obj = obj.OrderBy(x => x.Latitude).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Latitude).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Longitude).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Longitude).ToList();
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
                        obj = obj.OrderBy(x => x.Address.Street1).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Address.Street1).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.LocationType.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.LocationType.Name).ToList();
                    }
                    break;
                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                    }
                    break;
                case 8:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
                    }
                    break;
                case 9:
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
            int totalRecord = lm.GetTotalCount();

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
            sb.Append(JsonConvert.SerializeObject(obj));
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Detials View .
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
                var location = lm.GetLocation(id);
                if (location == null)
                {
                    return HttpNotFound();
                }
                return View(location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial detail view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var location = lm.GetLocation(id);
                ViewBag.AccountId = lm.GetAccounts().FirstOrDefault(x => x.AccountId == location.AccountId).Name;
                return PartialView("_PartialLocationDetails", location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
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
                var location = lm.GetLocation(id);
                ViewBag.AccountId = lm.GetAccounts().FirstOrDefault(x => x.AccountId == location.AccountId).Name;
                return PartialView("_PartialLocationDelete", location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // GET: Locations/Create
        /// <summary>
        /// Create view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                LocationDto location = new LocationDto();
                location.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1");
                location.LocationTypeDropdown = new SelectList(lm.LocationTypes, "LocationTypeId", "Name");
                location.StatusDropdown = new SelectList(lm.Status, "StatusId", "Name");
                location.AssignedTeamDropdown = new SelectList(lm.Teams, "TeamId", "Name");
                location.AssignedUserDropdown = new SelectList(lm.Users, "UserId", "Name");
                location.AccountsDropdown = new SelectList(lm.GetAccounts(), "AccountId", "Name");
                return View(location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        
        /// <summary>
        /// Creates the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="loc">The loc.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationDto location, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    location.LocationTypeDropdown = new SelectList(lm.LocationTypes, "LocationTypeId", "Name", location.LocationTypeId);
                    bool condition = lm.SaveLocation(location);
                    if (condition == true)
                    {

                        TempData["FormSubmissionMessage"] = "Location Created successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        if (loc != "False")
                        {
                            Guid temp = new Guid(loc);
                            return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                        }
                        else
                            return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "Location is not Created";
                TempData["FormSubmissionStatus"] = "success";
                location.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1", location.AddressId);
                location.LocationTypeDropdown = new SelectList(lm.LocationTypes, "LocationTypeId", "Name", location.LocationTypeId);
                location.StatusDropdown = new SelectList(lm.Status, "StatusId", "Name", location.StatusId);
                location.AssignedTeamDropdown = new SelectList(lm.Teams, "TeamId", "Name", location.AssignedTeam);
                location.AssignedUserDropdown = new SelectList(lm.Users, "UserId", "Name", location.AssignedUser);
                location.AccountsDropdown = new SelectList(lm.GetAccounts(), "AccountId", "Name");
                if (loc != "False")
                {
                    Guid temp = new Guid(loc);
                    return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                }
                else
                    return View(location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // GET: Locations/Edit/5
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
                var location = lm.GetLocation(id);
                if (location == null)
                {
                    return HttpNotFound();
                }
                location.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1", location.AddressId);
                location.LocationTypeDropdown = new SelectList(lm.LocationTypes, "LocationTypeId", "Name", location.LocationTypeId);
                location.StatusDropdown = new SelectList(lm.Status, "StatusId", "Name", location.StatusId);
                location.AssignedTeamDropdown = new SelectList(lm.Teams, "TeamId", "Name", location.AssignedTeam);
                location.AssignedUserDropdown = new SelectList(lm.Users, "UserId", "Name", location.AssignedUser);
                location.AccountsDropdown = new SelectList(lm.GetAccounts(), "AccountId", "Name");
                return View(location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        
        /// <summary>
        /// Edits the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LocationDto location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = lm.SaveLocation(location, true);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = "Location Updated successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "Location is not Updated.";
                TempData["FormSubmissionStatus"] = "error";
                location.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1", location.AddressId);
                location.LocationTypeDropdown = new SelectList(lm.LocationTypes, "LocationTypeId", "Name", location.LocationTypeId);
                location.StatusDropdown = new SelectList(lm.Status, "StatusId", "Name", location.StatusId);
                location.AssignedTeamDropdown = new SelectList(lm.Teams, "TeamId", "Name", location.AssignedTeam);
                location.AssignedUserDropdown = new SelectList(lm.Users, "UserId", "Name", location.AssignedUser);
                location.AccountsDropdown = new SelectList(lm.GetAccounts(), "AccountId", "Name");
                return View(location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // GET: Locations/Delete/5
        /// <summary>
        /// Delete view .
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
                var location = lm.GetLocation(id);
                if (location == null)
                {
                    return HttpNotFound();
                }
                return View(location);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // POST: Locations/Delete/5
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
                var location = lm.GetLocation(id);
                lm.SaveLocation(location, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the track.
        /// </summary>
        /// <param name="accTrack">The acc track.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult getTrack(string accTrack)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("accTrack");
                System.Diagnostics.Debug.WriteLine(accTrack);
                return Content(accTrack);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the location from account.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Desc">The desc.</param>
        /// <param name="Lat">The lat.</param>
        /// <param name="Long">The long.</param>
        /// <param name="User">The user.</param>
        /// <param name="Team">The team.</param>
        /// <param name="Loc">The loc.</param>
        /// <param name="Add">The add.</param>
        /// <param name="Status">The status.</param>
        /// <param name="AccID">The acc identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult CreateLfromAccount(String Name, String Desc, String Lat, String Long, String User, String Team, String Loc, String Add, String Status, String AccID)
        {
            try
            {
                LocationDto ldt = new LocationDto();
                ldt.AccountId = Guid.Parse(AccID);
                ldt.Name = Name;
                ldt.Description = Desc;
                ldt.Latitude = Convert.ToDecimal(Lat);
                ldt.Longitude = Convert.ToDecimal(Long);
                ldt.AssignedUser = Guid.Parse(User);
                ldt.AssignedTeam = Guid.Parse(Team);
                ldt.LocationTypeId = Guid.Parse(Loc);
                ldt.AddressId = Guid.Parse(Add);
                ldt.StatusId = Guid.Parse(Status);
                bool condition = lm.SaveLocation(ldt);
                if (condition == true)
                {
                    TempData["FormSubmissionMessage"] = "Location Created successfully.";
                    TempData["FormSubmissionStatus"] = "success";
                }
                return Json("success",JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message + ex.InnerException);
                return Json("errors");
            }
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
