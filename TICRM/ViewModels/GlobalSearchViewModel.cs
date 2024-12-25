using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TICRM.DTOs;
using Microsoft.AspNet.Identity;


namespace TICRM.ViewModels
{
    public class GlobalSearchViewModel
    {
        public string URL { get; set; }
        public List<SearchDataViewModel> FirstInSearch { get; set; }
        public List<SearchDataViewModel> SearchDataList { get; set; }
    }

    


}