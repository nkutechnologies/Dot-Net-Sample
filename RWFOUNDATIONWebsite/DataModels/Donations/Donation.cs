using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.DataModels.Donations
{
    public class Donation
    {
        public int DonationId { get; set; }
        public string DonationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal PledgedAmount { get; set; }
        public DateTime PledgedDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
