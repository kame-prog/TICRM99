using System.Web.Mvc;

namespace TICRM.Controllers
{
    /************ControlPanel Controller************
   Class [ControlPanelController] 
   ||  Author:  [Undefined]
   ||
   ||  Purpose:  [The class serves all the functionlities related with ControlPanel ]
   ||
   ||  Inherits From:  [Controller]
   ||
   ||  Changes Made:   [10/08/2020     Added Comment block to this Class     Sikandar Mustafa]
   ||                  
    ********************************************/
    
    public class ControlPanelController : BaseController
    {

        // GET: ControlPanel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult WorkFlowGojs()
        {
            return View();
        }

        public ActionResult demo()
        {
            return View();
        }

        public ActionResult testresult()
        {



            return View();
        }

    }

    public class student
    {

        // is a early binding example
        string _name;
        public string Name
        {
            set
            {
                _name = value;
            }
            get { return _name; }
        }
        // late binding 
        public object _obj = "name";

        public string GetObj
        {
            set { _obj = value; }
            get { return _obj.GetType().ToString(); }
        }
    }

}