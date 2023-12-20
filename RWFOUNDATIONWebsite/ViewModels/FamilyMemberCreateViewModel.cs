using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.ViewModels
{
    public class FamilyMemberCreateViewModel
    {
        public int FamilyMemberId { get; set; }
        public string Name { get; set; }
        public int RelationId { get; set; }
        public int Age { get; set; }
        public int FamilyMemberStatusId { get; set; }
    }
}
