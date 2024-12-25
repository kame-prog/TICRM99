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
    /************Resources Controller************
   Class [ResourcesController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Resources like, 
   ||             navigating to the pages, getting associated modules for specific Resource]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class                           Sikandar Mustafa]
   ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
   ||                  [17/08/2020     Added Server Side Processing For Jquery DataTables     Sikandar Mustafa]
               
    ********************************************/

    public class ResourcesController : BaseController
    {
      
        ResourceManager rm = new ResourceManager();


        /// <summary>
        /// Provide all Resources on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
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

        public string GetResourcesList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            List<ResourceDto> obj = rm.GetResourcesList(sEcho, iDisplayStart, iDisplayLength, sSearch);

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
                        obj = obj.OrderBy(x => x.PhoneHome).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.PhoneHome).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Email).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Email).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Website).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Website).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.PhoneOffice).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.PhoneOffice).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
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
                case 8:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.User.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.User.Name).ToList();
                    }
                    break;
                case 9:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Address1.Street1).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Address1.Street1).ToList();
                    }
                    break;
                case 10:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Address2.Street1).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Address2.Street1).ToList();
                    }
                    break;
            }

            int totalRecord = rm.GetTotalCount();

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
        /// Provides details of a Rources with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var resource = rm.GetResource(id);
                if (resource == null)
                {
                    return HttpNotFound();
                }
                return View(resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details on a partial view of a Resources with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var resource = rm.GetResource(id);
                return PartialView("_PartialResourcesDetails", resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details to delete on partail view 
        /// of a Resources with respect to id passed 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                var resource = rm.GetResource(id);
                return PartialView("_PartialResourcesDelete", resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Create page to create new Resources.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                ResourceDto resource = new ResourceDto();
                resource.StatusDropdown = new SelectList(rm.Status, "StatusId", "Name");
                resource.AssignedTeamDropdown = new SelectList(rm.Teams, "TeamId", "Name");
                resource.AssignedUserDropdown = new SelectList(rm.Users, "UserId", "Name");
                resource.AddressDorpdown = new SelectList(rm.Addresses, "AddressId", "Street1");
                resource.CurrentAddressDorpdown = new SelectList(rm.Addresses, "AddressId", "Street1");
                return View(resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request to create Resources, Receive object of,
        /// new Resources validate it and creates a new Resources
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ResourceDto resource)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    bool condition = rm.SaveResource(resource);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = "Resource is created successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "Resource is not created.";
                TempData["FormSubmissionStatus"] = "error";
                resource.StatusDropdown = new SelectList(rm.Status, "StatusId", "Name");
                resource.AssignedTeamDropdown = new SelectList(rm.Teams, "TeamId", "Name");
                resource.AssignedUserDropdown = new SelectList(rm.Users, "UserId", "Name");
                resource.AddressDorpdown = new SelectList(rm.Addresses, "AddressId", "Street1");
                resource.CurrentAddressDorpdown = new SelectList(rm.Addresses, "AddressId", "Street1");
                return View(resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request to edit a Resources, 
        /// with request to Id passed to this action method.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var resource = rm.GetResource(id);
                if (resource == null)
                {
                    return HttpNotFound();
                }
                resource.StatusDropdown = new SelectList(rm.Status, "StatusId", "Name");
                resource.AssignedTeamDropdown = new SelectList(rm.Teams, "TeamId", "Name");
                resource.AssignedUserDropdown = new SelectList(rm.Users, "UserId", "Name");
                resource.AddressDorpdown = new SelectList(rm.Addresses, "AddressId", "Street1");
                resource.CurrentAddressDorpdown = new SelectList(rm.Addresses, "AddressId", "Street1");
                return View(resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for Resources and update specified Resources
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ResourceDto resource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = rm.SaveResource(resource, true);
                    if (condition == true)
                    {
                        TempData["FormSubmissionMessage"] = "Resource is Updated successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                TempData["FormSubmissionMessage"] = "Resource is not Updated.";
                TempData["FormSubmissionStatus"] = "error";
                ViewBag.StatusId = new SelectList(rm.Status, "StatusId", "Name", resource.StatusId);
                ViewBag.AssignedTeam = new SelectList(rm.Teams, "TeamId", "Name", resource.AssignedTeam);
                ViewBag.AssignedUser = new SelectList(rm.Users, "UserId", "Name", resource.AssignedUser);
                ViewBag.Address = new SelectList(rm.Addresses, "AddressId", "Street1", resource.Address);
                ViewBag.CurrentAddress = new SelectList(rm.Addresses, "AddressId", "Street1", resource.CurrentAddress);
                return View(resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Delete form with Resources details,
        /// delete with respect to id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var resource = rm.GetResource(id);
                if (resource == null)
                {
                    return HttpNotFound();
                }
                return View(resource);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        ///  POST request for Delete Action, 
        /// Recieve confirmation for Resources Deletion and Delete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var resource = rm.GetResource(id);
                rm.SaveResource(resource, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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
