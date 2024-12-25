using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using TICRM.BuisnessLayer;
using TICRM.DTOs;
using Excel = Microsoft.Office.Interop.Excel;
namespace TICRM.Controllers
{
    /************BulkDevices Controller************
  Class [BulkDevicesController] 
  ||  Author:  [Undefined]
  ||
  ||  Purpose:  [The class serves all the functionlities related with BulkDevices]
  ||
  ||  Inherits From:  [Controller]
  ||
  ||  Changes Made:   [10/08/2020     Added Comment block to this Class    Sikandar Mustafa]
   ********************************************/
    public class BulkDevicesController : BaseController
    {
        private DeviceManager dm = new DeviceManager();

        private OpportunityManager om = new OpportunityManager();
        private OpportunityDto opportunity = new OpportunityDto();
        /// <summary>
        /// Index view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult Index()
        {
            try
            {
                BulkDevicDTO dto = new BulkDevicDTO();
                dto.AccountsDropdown = new SelectList(om.Accounts, "AccountId", "Name");

                return View(dto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }
        /// <summary>
        /// Uploads file.
        /// </summary>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                string targetPath = @"C:\inetpub\wwwroot";


                HttpPostedFileBase file = Request.Files[0]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string fileName = file.FileName;
                string mimeType = file.ContentType;

                System.IO.Stream fileContent = file.InputStream;
                
                string path = Path.Combine(targetPath,
                                           Path.GetFileName(file.FileName));
                file.SaveAs(path);

                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        ///  creating the devices globaly.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Account">The account.</param>
        /// <param name="Asset">The asset.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult BulkCreateGlobal(String Name, String Account, String Asset)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                if (excelApp != null)
                {
                    Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(@"D:\TI_Projects\Dev\TFS Project\TICRM New\swuichs\Project\TICRM\TICRM\Content\Datas.xlsx", 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets[1];

                    Excel.Range excelRange = excelWorksheet.UsedRange;
                    int rowCount = excelRange.Rows.Count;
                    int colCount = excelRange.Columns.Count;

                    for (int i = 2; i <= rowCount; i++)
                    {
                        Excel.Range macRange = (excelWorksheet.Cells[i, 2] as Excel.Range);
                        string macValue = macRange.Value.ToString();

                        Excel.Range emeiRange = (excelWorksheet.Cells[i, 3] as Excel.Range);
                        string emeiValue = emeiRange.Value.ToString();

                        DeviceDto dvdt = new DeviceDto();
                        dvdt.AccountId = Guid.Parse(Account);
                        dvdt.Name = Name + i;
                        dvdt.Mac = macValue;
                        dvdt.EMEINumber = emeiValue;
                        dvdt.CustomerAssetId = Guid.Parse(Asset);
                        dm.SaveDevice(dvdt,null);

                    }

                    excelWorkbook.Close();
                    excelApp.Quit();
                }


                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the customer assets for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>JsonResult.</returns>
        /// <exception cref="System.Exception"></exception>
        public JsonResult GetCustomerAssetsForDD(Guid accountId)
        {
            try
            {
                SelectList data = new SelectList(dm.CustomerAssetsOnAccountId(accountId), "CustomerAssetId", "Title");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException);
            }
        }

    }
}
