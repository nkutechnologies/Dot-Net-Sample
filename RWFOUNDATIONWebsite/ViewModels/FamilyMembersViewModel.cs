using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels
{
    public class FamilyMembersViewModel
    {
        public int FamilyMemberId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public int Age { get; set; }
        public string Status { get; set; }
    }
}
