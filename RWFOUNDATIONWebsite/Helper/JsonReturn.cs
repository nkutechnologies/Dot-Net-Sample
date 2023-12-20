using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWFOUNDATIONWebsite.Helper
{
    public class JsonReturn
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public string RoleName { get; set; }
        public decimal EstimatedExpense { get; set; }
        public decimal Packagevalue { get; set; }
        public int TotalMeals { get; set; }
        public string NullMessage { get; set; }
        public int RequestCount { get; set; }
        public bool IsEmailSent { get; set; }
        public int formno { get; set; }
        public Object ObjectData { get; set; }
        public Object ObjectData2 { get; set; }
        public IEnumerable<Object> ListObjectData { get; set; }
        public IEnumerable<Object> SecondListObjectData { get; set; }
    }
}
