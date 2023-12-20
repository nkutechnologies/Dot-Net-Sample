using RWFOUNDATIONWebsite.DataModels.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.SaveAsDrafts
{
    public class BeneficiaryFamilyMemberSaveAsDraft
    {
        public int FamilyMemberId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int RelationId { get; set; }
        public Relation Relation { get; set; }
        public int GroceryKitId { get; set; }
        public BeneficiaryFormSaveAsDraft BeneficiarySaveAsDraft { get; set; }
        public int FamilyMemberStatusId { get; set; }
        public FamilyMemberStatus FamilyMemberStatus { get; set; }       
    }
}
