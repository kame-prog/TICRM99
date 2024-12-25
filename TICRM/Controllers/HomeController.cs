using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TICRM.Controllers
{
    /************Home= Controller************
    Class [HomeController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the basic pages it was auto generated controller]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
    ||                  
     ********************************************/
   
    public class HomeController : BaseController
    {
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

        public ActionResult About()
        {
            try
            {
                ViewBag.Message = "Your application description page.";

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult Contact()
        {
            try
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult Error()
        {
            try
            {

                return View("Error");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        public ActionResult Contactdemo()
        {
            try
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        public ActionResult Metronic()
        {
            try
            {
                ViewBag.Message = "Your Metronic page.";

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        public ActionResult MetronicMaster()
        {
            try
            {
                ViewBag.Message = "Your MetronicMaster page.";

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult MetronicDatatable()
        {
            try
            {
                ViewBag.Message = "Your MetronicDatatable page.";

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult MetronicMaxlength()
        {
            try
            {
                ViewBag.Message = "Your MetronicMaxlength page.";
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        

        // not reomve this this is in used

        public ActionResult SetLayout(string value)
        {
            try
            {
                Session["DynamicLayout"] = value;
                return Content("success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult demo()
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

        public ActionResult autocompleteDemo()
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

        public JsonResult autocompletedata()
        {
            try
            {
                string result = "[{ 'name': 'Afghanistan', 'code': 'AF'}, { 'name': 'Albania', 'code': 'AL'},{ 'name': 'Algeria', 'code': 'DZ'}]";
                return Json(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

    }
}