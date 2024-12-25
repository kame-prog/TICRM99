using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TICRM.Controllers
{
    /************Readings Controller************
   Class [ReadingsController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Readings like, 
   ||             navigating to the pages, getting associated modules for specific Reading]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
   ||                  
    ********************************************/

    public class ReadingsController : BaseController
    {
        
        ReadingManager rm = new ReadingManager();


        /// <summary>
        /// Provide all Readings on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(rm.GetReadings());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details of a Reading with 
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
                var reading = rm.GetReading(id);
                if (reading == null)
                {
                    return HttpNotFound();
                }
                return View(reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details on a partial view of a Reading with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                var reading = rm.GetReading(id);
                return PartialView("_PartialReadingsDetails", reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details to delete on partail view 
        /// of a Reading with respect to id passed 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                var reading = rm.GetReading(id);
                return PartialView("_PartialReadingsDelete", reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// GET request for Create page to create new Reading.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                ReadingDto reading = new ReadingDto();
                reading.ReadingTypeDropdown = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name");
                reading.ReadingUnitDropdown = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name");
                reading.StatusDropdown = new SelectList(rm.Status, "StatusId", "Name");
                reading.AssignedTeamDropdown = new SelectList(rm.Teams, "TeamId", "Name");
                reading.AssignedUserDropdown = new SelectList(rm.Users, "UserId", "Name");
                return View(reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request to create Resources, Receive object of,
        /// new Reading validate it and creates a new Reading.
        /// </summary>
        /// <param name="reading">The reading.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReadingDto reading)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = rm.SaveReading(reading);
                    if (condition == true)
                    {
                        return RedirectToAction("Index");
                    }
                }

                reading.ReadingTypeDropdown = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name");
                reading.ReadingUnitDropdown = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name");
                reading.StatusDropdown = new SelectList(rm.Status, "StatusId", "Name");
                reading.AssignedTeamDropdown = new SelectList(rm.Teams, "TeamId", "Name");
                reading.AssignedUserDropdown = new SelectList(rm.Users, "UserId", "Name");
                return View(reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request to edit a Reading, 
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
                var reading = rm.GetReading(id);
                if (reading == null)
                {
                    return HttpNotFound();
                }
                reading.ReadingTypeDropdown = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name");
                reading.ReadingUnitDropdown = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name");
                reading.StatusDropdown = new SelectList(rm.Status, "StatusId", "Name");
                reading.AssignedTeamDropdown = new SelectList(rm.Teams, "TeamId", "Name");
                reading.AssignedUserDropdown = new SelectList(rm.Users, "UserId", "Name");
                return View(reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for Reading and update specified Reading
        /// </summary>
        /// <param name="reading">The reading.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReadingDto reading)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = rm.SaveReading(reading, true);
                    if (condition == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.ReadingTypeId = new SelectList(rm.ReadingTypes, "ReadingTypeId", "Name", reading.ReadingTypeId);
                ViewBag.ReadingUnitId = new SelectList(rm.ReadingUnits, "ReadingUnitId", "Name", reading.ReadingUnitId);
                ViewBag.StatusId = new SelectList(rm.Status, "StatusId", "Name", reading.StatusId);
                ViewBag.AssignedTeam = new SelectList(rm.Teams, "TeamId", "Name", reading.AssignedTeam);
                ViewBag.AssignedUser = new SelectList(rm.Users, "UserId", "Name", reading.AssignedUser);
                return View(reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Delete form with Reading details,
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
                var reading = rm.GetReading(id);
                if (reading == null)
                {
                    return HttpNotFound();
                }
                return View(reading);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Receive confirmation for Reading Deletion and Delete.
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
                var reading = rm.GetReading(id);
                rm.SaveReading(reading, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Get Reading list data for pagination.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetReadingsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            var obj = rm.GetReadingsList(sEcho, iDisplayStart, iDisplayLength, sSearch);


            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Value).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.MarginOfErrorInPercent).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.MarginOfErrorInPercent).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Description).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Description).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.ReadingType.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.ReadingType.Name).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Team.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Team.Name).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.User.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.User.Name).ToList();
                    }
                    break;


                default:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Status.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Status.Name).ToList();
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
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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
