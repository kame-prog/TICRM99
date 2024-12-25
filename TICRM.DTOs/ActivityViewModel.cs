using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TICRM.DTOs
{
    public class ActivityViewModel
    {
        public virtual List<EmailDTO> Emails { get; set; }
        public virtual List<MeetingDTO> Meetings { get; set; }
        public virtual List<PhoneCallDTO> PhoneCalls { get; set; }
        public virtual List<TaskDTO> Tasks { get; set; }
    }
}
