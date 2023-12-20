using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Master
{
    public class FamilyMember:IBasic
    {
        public int FamilyMemberId { get; set; }
        public string Name { get; set; }      
        public int Age { get; set; }       
        public int RelationId { get; set; }
        public Relation Relation { get; set; }
        public int GroceryKitId { get; set; }
        public GroceryKit GroceryKit { get; set; }
        public int FamilyMemberStatusId { get; set; }
        public FamilyMemberStatus FamilyMemberStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
