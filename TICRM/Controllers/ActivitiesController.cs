using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Activities Controller************
   Class [ActivitiesController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Activities like, 
   ||             navigating to the pages, getting associated modules for specific Activity,
   ||              Provding actions to return dropdown value]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]

    ********************************************/
    public class ActivitiesController : BaseController
    {
        private DeviceManager dm = new DeviceManager();
        private ActivityManager am = new ActivityManager();
        private AccountManager accountmangr = new AccountManager();
        private OpportunityManager om = new OpportunityManager();
        private LeadManager lm = new LeadManager();

        /// <summary>
        /// Get activities and return on Index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Return a string with all activities for DataTable to render
        /// </summary>
        /// <param name="sEcho"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sSearch"></param>
        /// <returns></returns>
        public string GetActivitesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            var obj =  am.GetActivitesList(sEcho, iDisplayStart, iDisplayLength, sSearch);


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
                        obj = obj.OrderBy(x => x.RelatedTo).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.RelatedTo).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.RelatedToName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.RelatedToName).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.AssignedTeam).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AssignedTeam).ToList();
                    }
                    break;

                default:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.AssignedUser).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AssignedUser).ToList();
                    }
                    break;
            }

            int totalRecord = am.GetTotalCount();

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
        /// Detail veiw for the specified activity.
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
                ActivityDTO activity = am.GetActivity(id);
                if (activity == null)
                {
                    return HttpNotFound();
                }
                return View(activity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Partials Detail veiw for the specified activity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            ActivityDTO activity = am.GetActivity(id);
            return PartialView("_PartialActivitiesDetails", activity);
        }


        /// <summary>
        /// Partials Detail veiw for the specified activity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                ActivityDTO activity = am.GetActivity(id);
                return PartialView("_PartialActivityDelete", activity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Create view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                ActivityDTO activity = new ActivityDTO();
                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToIDDropdown = new SelectList("");

                // Use collection initializer.
                activity.StatusDropdown = new SelectList(dm.Status, "StatusId", "Name");
                activity.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name");
                activity.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name");
                return View(activity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified activity.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="loc">The loc.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActivityDTO activity, string loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = am.SaveActivity(activity, CurrentUserId, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
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

                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");

                activity.RelatedToIDDropdown = new SelectList("");

                activity.StatusDropdown = new SelectList(dm.Status, "StatusId", "Name", activity.StatusId);
                activity.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name", activity.AssignedTeam);
                activity.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name", activity.AssignedUser);

                if (loc != "False")
                {
                    Guid temp = new Guid(loc);
                    return RedirectToAction("AccountsDetail", "Accounts", new { id = temp });
                }
                else
                {
                    return View(activity);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits view for the specified activity.
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
                ActivityDTO activity = am.GetActivity(id);
                if (activity == null)
                {
                    return HttpNotFound();
                }

                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");

                activity.RelatedToIDDropdown = new SelectList("");

                activity.StatusDropdown = new SelectList(dm.Status, "StatusId", "Name", activity.StatusId);
                activity.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name", activity.AssignedTeam);
                activity.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name", activity.AssignedUser);

                return View(activity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified activity.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActivityDTO activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid

                    bool condition = am.SaveActivity(activity, CurrentUserId, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                activity.TypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                activity.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");

                activity.RelatedToIDDropdown = new SelectList("");

                activity.StatusDropdown = new SelectList(dm.Status, "StatusId", "Name", activity.StatusId);
                activity.AssignedTeamDropdown = new SelectList(dm.Teams, "TeamId", "Name", activity.AssignedTeam);
                activity.AssignedUserDropdown = new SelectList(dm.Users, "UserId", "Name", activity.AssignedUser);

                return View(activity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Delete view for the specified activity.
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
                ActivityDTO activity = am.GetActivity(id);
                if (activity == null)
                {
                    return HttpNotFound();
                }
                return View(activity);
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
                ActivityDTO activity = am.GetActivity(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid

                bool condition = am.SaveActivity(activity, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the related to data.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetRelatedToData(string value)
        {
            try
            {

                if (RelatedToEnum.Account.ToString() == value)
                {

                    var data = new SelectList(accountmangr.GetAccounts(), "AccountId", "Name");

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else if (RelatedToEnum.Oppertunities.ToString() == value)
                {

                    var data = new SelectList(om.GetOpportunities(), "OpportunityId", "Title");

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else if (RelatedToEnum.Leads.ToString() == value)
                {

                    var data = new SelectList(lm.GetLeads(), "LeadId", "Name");

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the related to value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="selectedvalue">The selectedvalue.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetRelatedToValue(string value, Guid? selectedvalue)
        {
            try
            {
                if (RelatedToEnum.Account.ToString() == value)
                {

                    var data = new SelectList(accountmangr.GetAccounts(), "AccountId", "Name", selectedvalue);

                    return Json(data, JsonRequestBehavior.AllowGet);
                    //return Json(accountmangr.GetAccounts(), JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the activity.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult CreateActivity(ActivityDTO activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = am.SaveActivity(activity, CurrentUserId, false, false);
                    if (!condition)
                    {
                        return Json("error", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the status dropdown list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetStatusDropdownList()
        {
            try
            {
                var data = new SelectList(dm.Status, "StatusId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the assign user dropdown list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAssignUserDropdownList()
        {
            try
            {
                var data = new SelectList(dm.Users, "UserId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the assign team dropdown list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAssignTeamDropdownList()
        {
            try
            {
                var data = new SelectList(dm.Teams, "TeamId", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the activity type dropdown list.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetActivityTypeDropdownList()
        {
            try
            {
                var data = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the activity from account page.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Desc">The desc.</param>
        /// <param name="Party">The party.</param>
        /// <param name="Pointer">The pointer.</param>
        /// <param name="Create">The create.</param>
        /// <param name="CDate">The c date.</param>
        /// <param name="Update">The update.</param>
        /// <param name="UDate">The u date.</param>
        /// <param name="AUser">a user.</param>
        /// <param name="Team">The team.</param>
        /// <param name="Status">The status.</param>
        /// <param name="Type">The type.</param>
        /// <param name="AccID">The acc identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult CreateAfromAccount(String Name, String Desc, String Party, String Pointer, String Create, String CDate, String Update, String UDate, String AUser, String Team, String Status, String Type, String AccID)
        {
            try
            {
                ActivityDTO ac = new ActivityDTO();
                ac.Name = Name;
                ac.Description = Desc;
                ac.AssignedTeam = Guid.Parse(Team);
                ac.AssignedUser = Guid.Parse(AUser);
                ac.StatusId = Guid.Parse(Status);
                ac.Type = Type;
                ac.CreatedBy = User.Identity.GetUserId();
                ac.UpdatedBy = Update;
                if (!string.IsNullOrEmpty(UDate))
                    ac.UpdatedDate = Convert.ToDateTime(UDate);

                if (!string.IsNullOrEmpty(CDate))
                    ac.CreatedDate = Convert.ToDateTime(CDate);

                ac.RelatedTo = "Account";
                ac.RelatedToID = Guid.Parse(AccID);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = am.SaveActivity(ac, CurrentUserId, false, false);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult CreateAfromCases(String Name, String Desc, String AUser, String Team, String Status, String Type, String Relatedto, String RelatedToId)
        {
            try
            {
                ActivityDTO ac = new ActivityDTO();
                ac.Name = Name;
                ac.Description = Desc;
                ac.AssignedTeam = Guid.Parse(Team);
                ac.AssignedUser = Guid.Parse(AUser);
                ac.StatusId = Guid.Parse(Status);
                ac.Type = Type;
                ac.RelatedTo = Relatedto;
                ac.RelatedToID = Guid.Parse(RelatedToId);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = am.SaveActivity(ac, CurrentUserId, false, false);

                return null;
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
            }
            base.Dispose(disposing);
        }
    }
}
