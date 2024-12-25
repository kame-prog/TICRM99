using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************WorkFlowNodes Controller************
    Class [WorkFlowNodesController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with WorkFlowNodes like, 
    ||             navigating to the pages, getting associated modules for specific WorkFlowNode]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||                  [17/7/2020      the methods now use businnes layer to get and set the entities  Akhtar Zaman]
     ********************************************/
    public class WorkFlowNodesController : BaseController
    {
        private WorkflowNodeManager workflowNodeManager = new WorkflowNodeManager();

        // GET: WorkFlowNodes
        public ActionResult Index()
        {
            try
            {
                return View(workflowNodeManager.GetWorkflowNodes());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // GET: WorkFlowNodes/Details/5
        public ActionResult Details(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkFlowNodeDTO workFlowNode = workflowNodeManager.GetWorkflowNode(id);
                if (workFlowNode == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // GET: WorkFlowNodes/Create
        public ActionResult Create()
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

        // POST: WorkFlowNodes/Create
        //[Bind(Include = "NodeDataId,text,key,figure,fill,loc")] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkFlowNodeDTO workFlowNode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    workFlowNode.NodeDataId = Guid.NewGuid();
                    bool condition = workflowNodeManager.SaveWorkflowNode(workFlowNode, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                        TempData["FormSubmissionMessage"] = "Workflow Node is not created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(workFlowNode);

                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Workflow Node is created successfully.";
                        TempData["FormSubmissionStatus"] = "success";

                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // GET: WorkFlowNodes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkFlowNodeDTO workFlowNode = workflowNodeManager.GetWorkflowNode(id);
                if (workFlowNode == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // POST: WorkFlowNodes/Edit
        //[Bind(Include = "NodeDataId,text,key,figure,fill,loc")] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkFlowNodeDTO workFlowNode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = workflowNodeManager.SaveWorkflowNode(workFlowNode, true, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                        TempData["FormSubmissionMessage"] = "Workflow Node is not edited.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(workFlowNode);

                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Workflow Node is edited successfully.";
                        TempData["FormSubmissionStatus"] = "success";

                    }

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // GET: WorkFlowNodes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkFlowNodeDTO workFlowNode = workflowNodeManager.GetWorkflowNode(id);

                if (workFlowNode == null)
                {
                    return HttpNotFound();
                }
                return View(workFlowNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        // POST: WorkFlowNodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                WorkFlowNodeDTO workFlowNode = workflowNodeManager.GetWorkflowNode(id);

                workflowNodeManager.SaveWorkflowNode(workFlowNode, false, true);
                return RedirectToAction("Index");
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
