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
    /************WorkFlowMappings Controller************
    Class [WorkFlowMappingsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with WorkFlowMappings like, 
    ||             navigating to the pages, getting associated modules for specific WorkFlowMapping]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
    ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
    ||                  [21/08/2020     Added Sorting to pagination method     Akhtar Zaman]
     ********************************************/

    public class WorkFlowMappingsController : BaseController
    {
        private WorkFlowMappingManager workFlowMappingManager = new WorkFlowMappingManager();
        private WorkFlowManager workFlowManager = new WorkFlowManager();


        /// <summary>
        /// Provide all Workflow Mappings on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(workFlowMappingManager.GetWorkFlowMappingList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details of a workFlow Mapping with 
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
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                if (workFlowMappingDTO == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details on a partial view of a workorder mapping with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                return PartialView("_PartialWorkFlowMappingsDetails", workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details to delete on partail view 
        /// of a workorder mapping with respect to id passed 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                return PartialView("_PartialWorkFlowMappingsDelete", workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// GET request for Create page to create new WorkFlow Reports.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                WorkFlowMappingDTO workflowMapping = new WorkFlowMappingDTO();
                workflowMapping.WorkflowDropdown = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name");
                workflowMapping.SourceTypeDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                workflowMapping.DestinationTypeDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                workflowMapping.SourceColumnDropdown = new SelectList("");
                workflowMapping.DestinationColumnDropdown = new SelectList("");
                workflowMapping.ActionDropdown = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text");
                return View(workflowMapping);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request to create Work Flow Mapping, Receive object of,
        /// new workflow Mapping validate it and creates a new workflow mapping
        /// </summary>
        /// <param name="workFlowMappingDTO">The work flow mapping dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkFlowMappingDTO workFlowMappingDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = workFlowMappingManager.SaveWorkFlowMapping(workFlowMappingDTO, CurrentUserId, false, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Workflow is not created.";
                        TempData["FormSubmissionStatus"] = "error";
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Workflow is created Successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }

                ViewBag.WorkFlowId = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name", workFlowMappingDTO.WorkFlowId);
                ViewBag.SourceColumn = new SelectList("");

                ViewBag.Action = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text", workFlowMappingDTO.Action);

                return View(workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request to edit a workflow mapping, 
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
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                if (workFlowMappingDTO == null)
                {
                    return HttpNotFound();
                }
                workFlowMappingDTO.WorkflowDropdown = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name", workFlowMappingDTO.WorkFlowId);
                workFlowMappingDTO.SourceTypeDropdown = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", workFlowMappingDTO.SourceType);
                workFlowMappingDTO.ActionDropdown = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text", workFlowMappingDTO.Action);


                return View(workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for WorkFlow Mapping and update specified WorkFlow Mapping
        /// </summary>
        /// <param name="workFlowMappingDTO">The work flow mapping dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkFlowMappingDTO workFlowMappingDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId(); // get current userid
                    bool condition = workFlowMappingManager.SaveWorkFlowMapping(workFlowMappingDTO, CurrentUserId, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Workflow is not updated.";
                        TempData["FormSubmissionStatus"] = "error";
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Workflow is Updated Successfully.";
                        TempData["FormSubmissionStatus"] = "success";
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.WorkFlowId = new SelectList(workFlowManager.GetWorkFlows(), "WorkFlowId", "Name", workFlowMappingDTO.WorkFlowId);
                ViewBag.SourceType = new SelectList(from EntityTypes e in Enum.GetValues(typeof(EntityTypes)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", workFlowMappingDTO.SourceType);
                ViewBag.Action = new SelectList(new List<SelectListItem>    {
                 new SelectListItem { Text = "Create", Value = "Create"},
                 new SelectListItem { Text = "Update", Value = "Update"} }, "Value", "Text", workFlowMappingDTO.Action);

                return View(workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Delete form with workorder mapping details,
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
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                if (workFlowMappingDTO == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowMappingDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Recieve confirmation for workflow mapping Deletion and Delete.
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
                WorkFlowMappingDTO workFlowMappingDTO = workFlowMappingManager.GetWorkFlowMappingOnId(id);
                string CurrentUserId = User.Identity.GetUserId(); // get current userid
                bool condition = workFlowMappingManager.SaveWorkFlowMapping(workFlowMappingDTO, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the work type value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetWorkTypeValue(string type)
        {
            try
            {
                WorkFlowTypeViewModel data = new WorkFlowTypeViewModel();

                data.DataTypes = workFlowMappingManager.GetWorkFlowTypeList(type);
                data.Columns = new SelectList(data.DataTypes, "ColumnName", "ColumnName");

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the drop down of source value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="column">The column.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetDropDownOfSourceValue(string type, string column)
        {
            try
            {
                SelectList data = new SelectList(workFlowMappingManager.GetWorkFlowTypeDDList(type, column), "Id", "Name");

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the object on identifier.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="Selected">The selected.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetObjectOnId(string type,string Selected)
        {
            try
            {
                if (type == EntityTypes.Account.ToString())
                {
                    AccountManager am = new AccountManager();

                    Guid id = new Guid(Selected);

                    var data = am.GetAccount(id);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else if (type == EntityTypes.Lead.ToString())
                {
                    LeadManager lm = new LeadManager();

                    Guid id = new Guid(Selected);
                    var data = lm.GetLead(id);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Gets the workflow mapping list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetWorkflowMappingList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();
            List<WorkFlowMappingDTO> obj = workFlowMappingManager.GetWorkflowMappingList(sEcho, iDisplayStart, iDisplayLength, sSearch);
            int totalRecord = workFlowMappingManager.GetTotalCount();

            switch (sortColumnIndex)
            {

                case 0:
                    if (sortColumnDir == "asc")
                    {
                        //obj = obj.OrderBy(x => x.PropertyName).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.WorkFlow.Name).ToList();
                    }
                    break;
                case 1:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.Action).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Action).ToList();
                    }
                    break;
                case 2:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.SourceType).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.SourceType).ToList();
                    }
                    break;
                case 3:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.IsDone).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.IsDone).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.CreatedBy).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.CreatedBy).ToList();
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
                        obj = obj.OrderBy(x => x.UpdatedBy).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.UpdatedBy).ToList();
                    }
                    break;

                case 7:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.UpdatedDate).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.UpdatedDate).ToList();
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

    }

    /// <summary>
    /// Class WorkFlowTypeViewModel./
    /// </summary>
    public class WorkFlowTypeViewModel
    {
        /// <summary>
        /// Gets or sets the data types.
        /// </summary>
        /// <value>The data types.</value>
        public List<workflowDataTypeDTO> DataTypes { get; set; }
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public SelectList Columns { get; set; }
    }



}
