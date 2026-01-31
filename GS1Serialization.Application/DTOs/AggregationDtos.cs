using GS1Serialization.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS1Serialization.Application.DTOs
{
    public class CreateAggregationRequest
    {
        public int WorkOrderId { get; set; }
        public PackageLevel TargetLevel { get; set; }
        public List<string> ChildSerialNumbers { get; set; } = new();
    }

    public class AggregationResponse
    {
        public string ParentSerialNumber { get; set; }
        public string SSCC { get; set; }
        public int ChildCount { get; set; }
    }
}
