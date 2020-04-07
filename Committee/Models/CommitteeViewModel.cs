using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Committee.Models
{
    public class CommitteeViewModel
    {
        public CommitteeViewModel()
        {
            CommitteeMembers = new List<CommitteeMembers>();
        }

        public string ErrorMessage { get; set; }
        public int Number { get; set; }
        public List<CommitteeMembers> CommitteeMembers { get; set; }
        public string DisplayNumber
        {
            get
            {
                switch (Number)
                {
                    case 1:
                        return Number + "st";
                    case 2:
                        return Number + "nd";
                    case 3:
                        return Number + "rd";
                    default:
                        return Number + "th";
                }
            }
        }
    }
}
