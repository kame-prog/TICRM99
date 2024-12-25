using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************ProductCatelogs Controller************
   Class [ProductCatelogsController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with ProductCatelogs like, 
   ||             navigating to the pages, getting associated modules for specific ProductCatelog]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ||                  [17/08/2020     Added Comment block to All Action Methods of this class     Sikandar Mustafa]
   ||                  
    ********************************************/

    public class ProductCatelogsController : BaseController
    {
       
        private ProductCatelogManager catelogManager = new ProductCatelogManager();
        
        private CategoryManager categoryManager = new CategoryManager();


        /// <summary>
        /// Provide all Product catelogs on index page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                var productCatelogs = catelogManager.GetProductCatelogDtos();
                ViewBag.CategoryId = categoryManager.GetCategoryDtos();
                return View(productCatelogs.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }



        /// <summary>
        /// Provides details on a partial view of a Product catelog with 
        /// respect to id passed to this action method
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                ProductCatelogDTO productCatelogDTO = catelogManager.GetProductCatelogOnId(id);
                return PartialView("_PartialProductDetails", productCatelogDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Provides details to delete on partail view 
        /// of a Product catelog with respect to id passed 
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                ProductCatelogDTO productCatelogDTO = catelogManager.GetProductCatelogOnId(id);
                return PartialView("_PartialProductDelete", productCatelogDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Create page to create new Product catelog.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                ProductCatelogDTO productCatelog = new ProductCatelogDTO();
                productCatelog.CategoryDropdown = new SelectList(categoryManager.GetCategoryDtos(), "CategoryId", "Name");
                productCatelog.StatusDropdown = new SelectList(catelogManager.Status, "StatusId", "Name");
                productCatelog.AssignedTeamDropdown = new SelectList(catelogManager.Teams, "TeamId", "Name");
                productCatelog.AssignedUserDropdown = new SelectList(catelogManager.Users, "UserId", "Name");
                return View(productCatelog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request to create Product catelog, Receive object of,
        /// new Product catelog validate it and creates a new Product catelog.
        /// </summary>
        /// <param name="productCatelog">The product catelog.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCatelogDTO productCatelog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    bool status = catelogManager.SubmitProductCatelog(productCatelog, CurrentUserId);
                    if (status == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                productCatelog.CategoryDropdown = new SelectList(categoryManager.GetCategoryDtos(), "CategoryId", "Name");
                productCatelog.StatusDropdown = new SelectList(catelogManager.Status, "StatusId", "Name");
                productCatelog.AssignedTeamDropdown = new SelectList(catelogManager.Teams, "TeamId", "Name");
                productCatelog.AssignedUserDropdown = new SelectList(catelogManager.Users, "UserId", "Name");
                return View(productCatelog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request to edit a Product catelog, 
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
                ProductCatelogDTO productCatelog = catelogManager.GetProductCatelogOnId(id); ;
                if (productCatelog == null)
                {
                    return HttpNotFound();
                }
                productCatelog.CategoryDropdown = new SelectList(categoryManager.GetCategoryDtos(), "CategoryId", "Name");
                productCatelog.StatusDropdown = new SelectList(catelogManager.Status, "StatusId", "Name");
                productCatelog.AssignedTeamDropdown = new SelectList(catelogManager.Teams, "TeamId", "Name");
                productCatelog.AssignedUserDropdown = new SelectList(catelogManager.Users, "UserId", "Name");
                return View(productCatelog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Edit Action, 
        /// Recieve upated data for Product catelog and update specified Reading
        /// </summary>
        /// <param name="productCatelog">The product catelog.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCatelogDTO productCatelog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    bool status = catelogManager.SubmitProductCatelog(productCatelog, CurrentUserId, true);
                    if (status == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.CategoryId = new SelectList(categoryManager.GetCategoryDtos(), "CategoryId", "Name", productCatelog.CategoryId);
                ViewBag.StatusId = new SelectList(catelogManager.Status, "StatusId", "Name", productCatelog.StatusId);
                ViewBag.AssignedTeam = new SelectList(catelogManager.Teams, "TeamId", "Name", productCatelog.AssignedTeam);
                ViewBag.AssignedUser = new SelectList(catelogManager.Users, "UserId", "Name", productCatelog.AssignedUser);
                return View(productCatelog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// GET request for Delete form with Product catelog details,
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
                ProductCatelogDTO productCatelog = catelogManager.GetProductCatelogOnId(id);
                if (productCatelog == null)
                {
                    return HttpNotFound();
                }
                return View(productCatelog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// POST request for Delete Action, 
        /// Receive confirmation for Product catelog Deletion and Delete.
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
                ProductCatelogDTO productCatelog = catelogManager.GetProductCatelogOnId(id);
                // pass current userid
                string CurrentUserId = User.Identity.GetUserId();
                bool status = catelogManager.SubmitProductCatelog(productCatelog, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

    }
}
