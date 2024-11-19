using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kayak.DataAccess.DeviceData.Entities
{
    [Table("Sys_ReportPropertyLog")]
    public class ReportPropertyLog:EntityFull
    {
        public int PropertyId { get; set;}

        public string ProductCode { get; set;}

        public string? DeviceCode { get; set; }

        public string ThresholdValue { get; set;}

        public string PropertyValue { get; set;}


        public string Level {  get; set;}

        public string? Content { get; set;}

        public string ThresholdType { get; set;}

        public int Status { get; set;}
    }
}
