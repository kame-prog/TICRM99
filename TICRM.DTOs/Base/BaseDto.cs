using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TICRM.DTOs.Base
{
    public class BaseDto : BaseBasicDto
    {
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> AssignedUser { get; set; }
        public Nullable<System.Guid> AssignedTeam { get; set; }
        
        public virtual StatusDto Status { get; set; }
        public virtual TeamDto Team { get; set; }
        public virtual UserDto User { get; set; }
        
        public SelectList WorkFlowIdDropdown { get; set; }
        public SelectList AccountIdDropdown { get; set; }
        public SelectList DeviceIdDropdown { get; set; }

    }

    public class BaseDropDownDto : BaseBasicDto
    {
        public Nullable<bool> IsDeleted { get; set; }
        [Required(ErrorMessage = "Please select assigned user")]
        public Nullable<System.Guid> AssignedUser { get; set; }
        [Required(ErrorMessage = "Please select assigned team")]
        public Nullable<System.Guid> AssignedTeam { get; set; }

        public SelectList AccountsDropdown { get; set; }
        public SelectList CountryDropdown { get; set; }
        public SelectList DevicesDropDown { get; set; }
        public SelectList SensorsDropDown { get; set; }
        //public SelectList PriorityDropDown { get; set; }
        public SelectList StatusDropdown { get; set; }
        public SelectList AssignedTeamDropdown { get; set; }
        public SelectList AssignedUserDropdown { get; set; }
        public virtual StatusDto Status { get; set; }
        public virtual TeamDto Team { get; set; }
        public virtual UserDto User { get; set; }

    }



    public class BaseBasicDto
    {
        [Required(ErrorMessage = "Please select status type")]
        public Nullable<System.Guid> StatusId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }

    public class DropDownDto 
    {
        public Nullable<bool> IsDeleted { get; set; }
        [Required(ErrorMessage = "Please select assigned user")]
        public Nullable<System.Guid> AssignedUser { get; set; }
        [Required(ErrorMessage = "Please select assigned team")]
        public Nullable<System.Guid> AssignedTeam { get; set; }

        public SelectList AccountsDropdown { get; set; }
        public SelectList StatusDropdown { get; set; }
        public SelectList AssignedTeamDropdown { get; set; }
        public SelectList AssignedUserDropdown { get; set; }
        public virtual StatusDto Status { get; set; }
        public virtual TeamDto Team { get; set; }
        public virtual UserDto User { get; set; }

    }

}
