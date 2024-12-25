using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Categories Controller************
   Class [CategoriesController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with Categories like, 
   ||             navigating to the pages, getting associated modules for specific Categorie]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
    ********************************************/
    public class CategoriesController : BaseController
    {
        private CategoryManager categoryManager = new CategoryManager();

        /// <summary>
        /// Get Categories and return view
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                var categories = categoryManager.GetCategoryDtos();
                return View(categories);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partials details view on identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                CategoryDto category = categoryManager.GetCategoryOnId(id);

                return PartialView("_PartialCategoryDetail", category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partials delete view on identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                CategoryDto category = categoryManager.GetCategoryOnId(id);
                return PartialView("_PartialCategoryDelete", category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates View.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                CategoryDto categoryDto = new CategoryDto();
                categoryDto.StatusDropdown = new SelectList(categoryManager.Status, "StatusId", "Name");
                categoryDto.AssignedTeamDropdown = new SelectList(categoryManager.Teams, "TeamId", "Name");
                categoryDto.AssignedUserDropdown = new SelectList(categoryManager.Users, "UserId", "Name");
                return View(categoryDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the specified category dto.
        /// </summary>
        /// <param name="categoryDto">The category dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    bool status = categoryManager.SubmitCategory(categoryDto, CurrentUserId);
                    if (status == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                categoryDto.StatusDropdown = new SelectList(categoryManager.Status, "StatusId", "Name", categoryDto.StatusId);
                categoryDto.AssignedTeamDropdown = new SelectList(categoryManager.Teams, "TeamId", "Name", categoryDto.AssignedTeam);
                categoryDto.AssignedUserDropdown = new SelectList(categoryManager.Users, "UserId", "Name", categoryDto.AssignedUser);
                return View(categoryDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits page for specified category.
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
                CategoryDto category = categoryManager.GetCategoryOnId(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                category.StatusDropdown = new SelectList(categoryManager.Status, "StatusId", "Name", category.StatusId);
                category.AssignedTeamDropdown = new SelectList(categoryManager.Teams, "TeamId", "Name", category.AssignedTeam);
                category.AssignedUserDropdown = new SelectList(categoryManager.Users, "UserId", "Name", category.AssignedUser);
                return View(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified category dto.
        /// </summary>
        /// <param name="categoryDto">The category dto.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    bool status = categoryManager.SubmitCategory(categoryDto, CurrentUserId, true);
                    if (status == true)
                    {
                        return RedirectToAction("Index");
                    }
                }
                categoryDto.StatusDropdown = new SelectList(categoryManager.Status, "StatusId", "Name", categoryDto.StatusId);
                categoryDto.AssignedTeamDropdown = new SelectList(categoryManager.Teams, "TeamId", "Name", categoryDto.AssignedTeam);
                categoryDto.AssignedUserDropdown = new SelectList(categoryManager.Users, "UserId", "Name", categoryDto.AssignedUser);
                return View(categoryDto);
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
                CategoryDto categoryDto = categoryManager.GetCategoryOnId(id);
                // pass current userid
                string CurrentUserId = User.Identity.GetUserId();
                bool status = categoryManager.SubmitCategory(categoryDto, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

      
    }
}
