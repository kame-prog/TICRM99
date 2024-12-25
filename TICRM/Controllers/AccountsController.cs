using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.BuisnessLayer.Base;
using TICRM.CRMFilters;
using TICRM.DTOs;

namespace TICRM.Controllers
{

    /************Accounts Controller************
    Class [AccountsController] 
    ||  Author:  [Undefined]
    ||
    ||  Purpose:  [The class serves all the functionlities related with Accounts like, 
    ||             navigating to the pages, getting associated modules for specific Account]
    ||
    ||  Inherits From:  [Controller]
    ||
    ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
    ||                  [21/08/2020     Added ServerSide Processing for Jquery DataTables    Sikandar Mustafa]
     ********************************************/

    [Authorize]
    public class AccountsController : BaseController
    {
        
        private BaseManager bm = new BaseManager();
        private AccountManager am = new AccountManager();
        private DeviceManager deviceManager = new DeviceManager();
        private OpportunityManager om = new OpportunityManager();
        private OpportunityDto opportunity = new OpportunityDto();
        private DeviceDto deviceDto = new DeviceDto();
        private LocationManager lm = new LocationManager();
        private CustomerAssetManager cam = new CustomerAssetManager();
        private LocationManager locationManager = new LocationManager();
        private WorkOrderManager wom = new WorkOrderManager();
        private WorkOrderDto workOrder = new WorkOrderDto();
        private ActivityDTO activity = new ActivityDTO();
        private CostsManager cm = new CostsManager();
        private CaseManager cases = new CaseManager();

        // GET: Accounts        

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                List<AccountDto> accounts = am.GetAccounts();
                return View(accounts);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the accounts list.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>System.String.</returns>
        public string GetAccountsList(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch)
        {
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortColumnDir = Request["sSortDir_0"];
            sortColumnDir.ToLower();

            List<AccountDto> obj = am.GetAccountsList(sEcho, iDisplayStart, iDisplayLength, sSearch);


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
                        obj = obj.OrderBy(x => x.Email).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.Email).ToList();
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
                        obj = obj.OrderBy(x => x.AccountType.Name).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AccountType.Name).ToList();
                    }
                    break;
                case 4:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.OppCount).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.OppCount).ToList();
                    }
                    break;
                case 5:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.LocationCount).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.LocationCount).ToList();
                    }
                    break;
                case 6:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.AssetCount).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.AssetCount).ToList();
                    }
                    break;

                default:
                    if (sortColumnDir == "asc")
                    {
                        obj = obj.OrderBy(x => x.DeviceCount).ToList();
                    }
                    else
                    {
                        obj = obj.OrderByDescending(x => x.DeviceCount).ToList();
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
        /// Details for the specified identifier.
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
                AccountDto account = am.GetAccount(id);
                AccountViewModel accWithDetail = am.GetAccountAndDetails(id.Value);
                if (account == null)
                {
                    return HttpNotFound();
                }
                return View(accWithDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the tree of devices.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetTreeOfDevices(Guid assetId)
        {
            try
            {
                List<DeviceDto> deviceList = deviceManager.GetDevicesOnAssetsId(assetId);
                if (deviceList.Count == 0)
                {

                    return Json("NoData", JsonRequestBehavior.AllowGet);
                }
                return Json(deviceList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Get all the accounts associates
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public JsonResult GetAllAccountAssociate(Guid accountId)
        {
            try {
                //var data = am.GetAccount();
                AccountViewModel accWithDetail = am.GetAccountAndDetails(accountId);

                return Json(accWithDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Accounts detail pege.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult AccountsDetail(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AccountDto account = am.GetAccount(id);
                ContactManager contact = new ContactManager();
                
                AccountViewModel accWithDetail = am.GetAccountAndDetails(id.Value);

                accWithDetail.account.AccountsDropdown = new SelectList(om.Accounts, "AccountId", "Name");
                accWithDetail.account.AssignedTeamDropdown = new SelectList(om.Teams, "TeamId", "Name");
                accWithDetail.account.AssignedUserDropdown = new SelectList(om.Users, "UserId", "Name");
                accWithDetail.account.StatusDropdown = new SelectList(om.Status, "StatusId", "Name");
                accWithDetail.account.AddressDropdown = new SelectList(lm.Addresses, "AddressId", "Street1");

                accWithDetail.CurrencyDropdown = new SelectList(om.Currencies, "CurrencyId", "Name");
                accWithDetail.OpportunityStageDropdown = new SelectList(om.OpportunityStages, "OpportunityStageId", "Name");
                accWithDetail.ProbabilityDropdown = new SelectList(om.Probabilities, "ProbabilityId", "Name");

                accWithDetail.CustomerAssetDropdown = new SelectList(deviceManager.CustomerAssetsOnAccountId((Guid)id), "CustomerAssetId", "Title", deviceDto.CustomerAssetId);

                accWithDetail.MaintenanceDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = DeviceMaintenance.None, Value = DeviceMaintenance.None},
        new SelectListItem { Text = DeviceMaintenance.IsRepaired, Value = DeviceMaintenance.IsRepaired},
        new SelectListItem { Text = DeviceMaintenance.IsServiced, Value = DeviceMaintenance.IsServiced,}}, "Value", "Text", deviceDto.Maintenance);
                accWithDetail.CloudServicesDropdown = new SelectList(new List<SelectListItem>    {
        new SelectListItem { Text = CloudServiceForDD.INCA, Value = CloudServiceForDD.INCA},
        new SelectListItem { Text = CloudServiceForDD.IBM, Value = CloudServiceForDD.IBM},
        new SelectListItem { Text = CloudServiceForDD.Google, Value = CloudServiceForDD.Google},
        new SelectListItem { Text = CloudServiceForDD.Amazon, Value = CloudServiceForDD.Amazon},
        new SelectListItem { Text = CloudServiceForDD.Microsoft, Value = CloudServiceForDD.Microsoft,}}, "Value", "Text", deviceDto.CloudServices);

                accWithDetail.LocationTypeDropdown = new SelectList(lm.LocationTypes, "LocationTypeId", "Name");
                accWithDetail.CustomerAssetTypeDropdown = new SelectList(cam.CustomerAssetTypes, "CustomerAssetTypeId", "Name");
                accWithDetail.LocationDropdown = new SelectList(new List<LocationDto>(), "LocationId", "Name");
                accWithDetail.LocationDropdown = new SelectList(locationManager.GetLocations((Guid)id), "LocationId", "Name");
                accWithDetail.WorkOrderStageDropdown = new SelectList(wom.WorkStages, "WorkStageId", "Name", workOrder.WorkOrderStageId);
                accWithDetail.ActivityTypeDropdown = new SelectList(from ActivityType e in Enum.GetValues(typeof(ActivityType)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name", activity.Type);

                accWithDetail.account.AssignedTeamDropdown = new SelectList(cases.Teams, "TeamId", "Name");
                accWithDetail.account.AssignedUserDropdown = new SelectList(cases.Users, "UserId", "Name");
                accWithDetail.CaseResolutionDropdown = new SelectList(cases.GetCaseResolutions(), "CaseResolutionType", "Name");
                accWithDetail.CaseStatusDropdown = new SelectList(cases.GetCaseStatusDtos(), "CaseStatusId", "Name");
                accWithDetail.CaseTypeDropdown = new SelectList(cases.GetCaseTypeDtos(), "CaseTypeId", "Name");
                accWithDetail.ContactsDropdown = new SelectList(contact.GetAccountContacts((Guid)id), "ContactId", "Name");
                accWithDetail.RelatedToDropdown = new SelectList(from RelatedToEnum e in Enum.GetValues(typeof(RelatedToEnum)) select new { ID = e.ToString(), Name = e.ToString() }, "Name", "Name");
                accWithDetail.RelatedToIdDropdown = new SelectList("");



                if (account == null)
                {
                    return HttpNotFound();
                }
                return View(accWithDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Accounts detail right side panel.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult AccountsDetailRight(Guid accountId)
        {
            try
            {
                AccountDto account = am.GetAccount(accountId);
                return Json(account, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the assets of location.
        /// </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAssetsOfLocation(Guid locationId)
        {
            try
            {
                CustomerAssetManager customerAsset = new CustomerAssetManager();
                return Json(customerAsset.GetLocationAssets(locationId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Device sensor graph on device id
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public JsonResult DSGonDeviceId(Guid deviceId)
        {
            try
            {
                DeviceSensorGraphManager deviceSensor = new DeviceSensorGraphManager();

                return Json(deviceSensor.GetDeviceSensorGraphList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the account cost.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAccountCost(Guid accountId)
        {
            try
            {
                return Json(cm.GetAccountCostById(accountId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message +": "+ ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the account details.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAccountDetails(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                AccountDto account = am.GetAccount(id);
                AccountViewModel accWithDetail = am.GetAccountAndDetails(id.Value);
                if (account == null)
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                return Json(accWithDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Accounts details for partial views.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult AccountDetailsPartial(Guid? id)
        {
            try
            {
                //AccountDto account = am.GetAccount(id);
                AccountViewModel accWithDetail = am.GetAccountAndDetails(id.Value);
                return PartialView("_PartialAccountDetailOnId", accWithDetail);
            }catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the oppertunity detail on identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult GetOppertunityDetailOnId(Guid? id, Guid accountId)
        {
            try
            {
                CaseManager cm = new CaseManager();
                OpportunityDto opportunity = om.GetOpportunity(id);
                ContactManager contactManager = new ContactManager();
                opportunity.OpportunityCasesList = cm.GetOpportunityCases(id);

                opportunity.OpportunityCasesList = cm.GetOpportunityCases(id);
                ViewBag.AssignedTeamDropdown = new SelectList(cm.Teams, "TeamId", "Name");
                ViewBag.AssignedUserDropdown = new SelectList(cm.Users, "UserId", "Name");
                ViewBag.CaseResolutionDropdown = new SelectList(cm.GetCaseResolutions(), "CaseResolutionType", "Name");
                ViewBag.CaseStatusDropdown = new SelectList(cm.GetCaseStatusDtos(), "CaseStatusId", "Name");
                ViewBag.CaseTypeDropdown = new SelectList(cm.GetCaseTypeDtos(), "CaseTypeId", "Name");
                ViewBag.ContactsDropdown = new SelectList(contactManager.GetAccountContacts(accountId), "ContactId", "Name");

                return PartialView("~/Views/Opportunities/_PartialRightSideDetail.cshtml", opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the workflow.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpGet]
        public ActionResult GetWorkflow()
        {
            try
            {
                return PartialView("~/Views/Accounts/_WorkflowAccountsPartial.cshtml");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets all repaired devices.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>

       public JsonResult GetAllRepairedDevices()
        {
            try
            {
                return Json(deviceManager.GetDevices(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets all services devices.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAllServicesDevices()
        {
            try
            {
                return Json(deviceManager.GetDevices(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Create()
        {
            try
            {
                AccountDto account = new AccountDto();
                account.AccountSizeDropdown = new SelectList(am.AccountSizes, "AccountSizeId", "Name");
                account.AccountTypeDropdown = new SelectList(am.AccountTypes, "AccountTypeId", "Name");
                account.AddressDropdown = new SelectList(am.Addresses, "AddressId", "Street1");


                account.IndustryDropdown = new SelectList(am.Industries, "IndustryId", "Name");
                account.StatusDropdown = new SelectList(am.Status, "StatusId", "Name");
                account.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name");
                account.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name");
                return View(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Post request to create new accounts
        /// </summary>
        /// <param name="account"></param>
        /// <param name="IsEventSchedule"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccountActionFilter]
        public ActionResult Create(AccountDto account, bool? IsEventSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    Guid gb = Guid.NewGuid();
                    account.AccountId = gb;
                    am.SaveAccount(account, CurrentUserId);
                    // AccountsDetail(id);

                    return RedirectToAction("AccountsDetail", new { id = gb });

                }

                account.AccountSizeDropdown = new SelectList(am.AccountSizes, "AccountSizeId", "Name", account.AccountSizeId);
                account.AccountTypeDropdown = new SelectList(am.AccountTypes, "AccountTypeId", "Name", account.AccountTypeId);
                account.AddressDropdown = new SelectList(am.Addresses, "AddressId", "Street1");
                //ViewBag.BillingAddressDropdown = new SelectList(am.Addresses, "AddressId", "Street1", account.BillingAddress);
                account.IndustryDropdown = new SelectList(am.Industries, "IndustryId", "Name", account.IndustryId);
                account.StatusDropdown = new SelectList(am.Status, "StatusId", "Name", account.StatusId);
                account.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name", account.AssignedTeam);
                account.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name", account.AssignedUser);
                return View(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// GET: Accounts/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AccountDto account = am.GetAccount(id);
                if (account == null)
                {
                    return HttpNotFound();
                }

                account.AccountSizeDropdown = new SelectList(am.AccountSizes, "AccountSizeId", "Name", account.AccountSizeId);
                account.AccountTypeDropdown = new SelectList(am.AccountTypes, "AccountTypeId", "Name", account.AccountTypeId);
                account.AddressDropdown = new SelectList(am.Addresses, "AddressId", "Street1");
                //ViewBag.BillingAddressDropdown = new SelectList(am.Addresses, "AddressId", "Street1", account.BillingAddress);
                account.IndustryDropdown = new SelectList(am.Industries, "IndustryId", "Name", account.IndustryId);
                account.StatusDropdown = new SelectList(am.Status, "StatusId", "Name", account.StatusId);
                account.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name", account.AssignedTeam);
                account.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name", account.AssignedUser);
                return View(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }


        /// <summary>
        ///Edit account post action
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        
        [HttpPost]
        [AccountActionFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountDto account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // pass current userid
                    string CurrentUserId = User.Identity.GetUserId();
                    am.SaveAccount(account, CurrentUserId, true);
                    return RedirectToAction("AccountsDetail", new { id = account.AccountId });
                }

                account.AccountSizeDropdown = new SelectList(am.AccountSizes, "AccountSizeId", "Name", account.AccountSizeId);
                account.AccountTypeDropdown = new SelectList(am.AccountTypes, "AccountTypeId", "Name", account.AccountTypeId);
                account.AddressDropdown = new SelectList(am.Addresses, "AddressId", "Street1");
                //ViewBag.BillingAddressDropdown = new SelectList(am.Addresses, "AddressId", "Street1", account.BillingAddress);
                account.IndustryDropdown = new SelectList(am.Industries, "IndustryId", "Name", account.IndustryId);
                account.StatusDropdown = new SelectList(am.Status, "StatusId", "Name", account.StatusId);
                account.AssignedTeamDropdown = new SelectList(am.Teams, "TeamId", "Name", account.AssignedTeam);
                account.AssignedUserDropdown = new SelectList(am.Users, "UserId", "Name", account.AssignedUser);
                return View(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Partial delete view
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult PartialDeleteOnId(Guid? id)
        {
            try
            {
                AccountDto account = am.GetAccount(id);
                return PartialView("_PartialAccountsDelete", account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult PartialContactView(Guid id)
        {
            try
            {
                ContactManager cm = new ContactManager();
                List<ContactDto> contact = cm.GetAccountContacts(id);
                return PartialView("_Contact", contact);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult PartialopportunityView(Guid id)
        {
            try
            {
                OpportunityManager cm = new OpportunityManager();
                List<OpportunityDto> opportunity = cm.GetOpportunities(id);
                return PartialView("_Opportunity", opportunity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult PartialWorkordersView(Guid id)
        {
            try
            {
                WorkOrderManager wm = new WorkOrderManager();
                List<WorkOrderDto> workorder = wm.GetAccountWorkorders(id);
                return PartialView("_Workorders", workorder);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult PartialActivityView(Guid id)
        {
            try
            {
                ActivityManager acm = new ActivityManager();
                List<ActivityDTO> activity = acm.GetAccountActivities(id);
                return PartialView("_Activity", activity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult PartialCasesView(Guid id)
        {
            try
            {
                CaseManager cm = new CaseManager();
                List<CaseDto> cases = cm.GetAccountCases(id);
                return PartialView("_Cases", cases);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public ActionResult PartialTreeView(Guid id)
        {
            try
            {
                TreeViewModel tree = new TreeViewModel();
                tree.Locations = locationManager.GetLocations(id);
                tree.CustomerAssets = cam.CustomerAssetsOnAccountId(id);
                tree.Devices = deviceManager.GetDevices(id);
                return PartialView("_TreeView", tree);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes the account in delete view.
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
                AccountDto account = am.GetAccount(id);
                if (account == null)
                {
                    return HttpNotFound();
                }
                return View(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Deletes on confirm action.
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
                AccountDto account = am.GetAccount(id);
                // pass current userid
                string CurrentUserId = User.Identity.GetUserId();
                //soft delete for account
                am.SaveAccount(account, CurrentUserId, true, true);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Creates the account From header.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Desc">The desc.</param>
        /// <param name="Phone">The phone.</param>
        /// <param name="Email">The email.</param>
        /// <param name="Size">The size.</param>
        /// <param name="AssignUser">The assign user.</param>
        /// <param name="Team">The team.</param>
        /// <param name="Type">The type.</param>
        /// <param name="Status">The status.</param>
        /// <param name="Ind">The ind.</param>
        /// <param name="Latitude">The latitude.</param>
        /// <param name="Longitude">The longitude.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult CreateAccountHeader(String Name, String Desc, String Phone, String Email, String Size, String AssignUser, String Team, String Type, String Status, String Ind, String Latitude, String Longitude )
        {
            try
            {
                AccountDto account = new AccountDto();
                string CurrentUserId = User.Identity.GetUserId();
                Guid gb = Guid.NewGuid();
                account.AccountId = gb;
                account.Name = Name;
                account.Description = Desc;
                account.PhoneOffice = Phone;
                account.Email = Email;
                account.AccountSizeId = Guid.Parse(Size);
                account.AssignedUser = Guid.Parse(AssignUser);
                account.AssignedTeam = Guid.Parse(Team);
                account.AccountTypeId = Guid.Parse(Type);
                account.StatusId = Guid.Parse(Status);
                account.IndustryId = Guid.Parse(Ind);
                account.Latitude = Convert.ToDecimal(Latitude);
                account.Longitude = Convert.ToDecimal(Longitude);

                am.SaveAccount(account, CurrentUserId);

                return RedirectToAction("AccountsDetail", new { id = gb });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Gets the device counts.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetDeviceCounts()
        {
            try
            {
                int count;
                List<Counts> list = new List<Counts>();
                List<AccountDto> accounts = am.GetAccounts();
                foreach (var item in accounts)
                {
                   count = am.GetDevicesCounts(item.AccountId);
                    list.Add(new Counts { AccountId = item.AccountId, Count = count.ToString() });
                }
                string json = JsonConvert.SerializeObject(list);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }


        }

        /// <summary>
        /// Gets the opportunities counts.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetOppCounts()
        {
            try
            {
                int count;
                List<Counts> list = new List<Counts>();
                List<AccountDto> accounts = am.GetAccounts();
                foreach (var item in accounts)
                {
                    count = am.GetOpportunityCount(item.AccountId);
                    list.Add(new Counts { AccountId = item.AccountId, Count = count.ToString() });
                }
                string json = JsonConvert.SerializeObject(list);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Gets the location counts.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetLocationCounts()
        {
            try
            {
                int count;
                List<Counts> list = new List<Counts>();
                List<AccountDto> accounts = am.GetAccounts();
                foreach (var item in accounts)
                {
                    count = am.GetLocationsCounts(item.AccountId);
                    list.Add(new Counts { AccountId = item.AccountId, Count = count.ToString() });
                }
                string json = JsonConvert.SerializeObject(list);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Gets the asset count.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAssetCount()
        {
            try
            {
                int count;
                List<Counts> list = new List<Counts>();
                List<AccountDto> accounts = am.GetAccounts();
                foreach (var item in accounts)
                {
                    count = am.GetAssetsCounts(item.AccountId);
                    list.Add(new Counts { AccountId = item.AccountId, Count = count.ToString() });
                }
                string json = JsonConvert.SerializeObject(list);

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }

        }

        /// <summary>
        /// Gets all accounts longitudes and latitudes.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetAllAccountsLongLat()
        {
            try
            {
                return Json(am.GetAccounts(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        public JsonResult GetAllAcocuntDevicesCases(Guid accountId)
        {
            try
            {
                return Json(cases.GetAccountCasesforDevices(accountId), JsonRequestBehavior.AllowGet);  // return json List
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        public JsonResult GetAccountOpportunitiesLongLat(string accountId)
        {
            try
            {
                return Json(om.GetOpportunities(new Guid(accountId)), JsonRequestBehavior.AllowGet);
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

    public class Counts
    {
        public Guid AccountId { get; set; }
        public string Count { get; set; }
    }
}