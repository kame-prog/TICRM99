using System;
using System.Net;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;

namespace TICRM.Controllers
{
    /************Addresses Controller************
  Class [AddressesController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with Addresses like, 
  ||             navigating to the pages, getting associated modules for specific Addresse]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
  ||                  [24/07/2020     Added Try catch blocks               Sikandar Mustafa]
  ||                  [17/07/2020     the methods now use business layer to get and set the entities Akhtar Zaman]
   ********************************************/
    
    public class AddressesController : BaseController
    {
        private AddressManager addressManager = new AddressManager();

        /// <summary>
        /// Get the addresses and return in view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                return View(addressManager.GetAllAddresses());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Detail view for the specified identifier.
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
                AddressDto address = addressManager.GetAddress(id);
                if (address == null)
                {
                    return HttpNotFound();
                }
                return View(address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }


        /// <summary>
        /// Partials details viwe for an address.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDetailsOnId(Guid? id)
        {
            try
            {
                AddressDto address = addressManager.GetAddress(id);
                return PartialView("_PartialAddressesDetails", address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        // GET: Readings/PartialDeleteOnId/5
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                AddressDto address = addressManager.GetAddress(id);
                return PartialView("_PartialAddressesDelete", address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        /// Create view
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
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

        /// <summary>
        /// Creates the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddressDto address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    address.AddressId = Guid.NewGuid();
                    bool condition = addressManager.SaveAddress(address, false, false);
                    if (!condition)
                    {
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                        TempData["FormSubmissionMessage"] = "Address is not created.";
                        TempData["FormSubmissionStatus"] = "error";
                        return View(address);

                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Address is created successfully.";
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

        /// <summary>
        /// Edits veiw for the specified addresss.
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
                AddressDto address = addressManager.GetAddress(id);
                if (address == null)
                {
                    return HttpNotFound();
                }
                return View(address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Edits the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AddressDto address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool condition = addressManager.SaveAddress(address, true, false);
                    if (!condition)
                    {
                        TempData["FormSubmissionMessage"] = "Address is not Updated.";
                        TempData["FormSubmissionStatus"] = "error";
                        ModelState.AddModelError("", "Data Is Not Saved Please Refresh the page.");
                        return View(address);

                    }
                    else
                    {
                        TempData["FormSubmissionMessage"] = "Address is Updated successfully.";
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

        /// <summary>
        /// Delete view for the specified address.
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
                AddressDto address = addressManager.GetAddress(id);
                if (address == null)
                {
                    return HttpNotFound();
                }
                return View(address);
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
                AddressDto address = addressManager.GetAddress(id);
                addressManager.SaveAddress(address, true, true);

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
