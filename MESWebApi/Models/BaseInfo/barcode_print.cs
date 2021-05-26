using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MESWebApi.Models.BaseInfo
{
    public class barcode_print
    {
        public string vin { get; set; }
        public DateTime? print_time { get; set; }
        public string zpjh { get; set; }
        public string order_no { get; set; }
        public string ztbm { get; set; }
        public string write_req { get; set; }
        public string seq_no { get; set; }
        public string printer { get; set; }
        public string status_flag { get; set; }
        public string in_flag { get; set; }
        public string frequency { get; set; }
        public string line { get; set; }
        public string ddh { get; set; }
        public string out_flag { get; set; }
        public string finish_flag { get; set; }
        public DateTime? in_flag_time { get; set; }
        public string old_vin { get; set; }
        public string printer_1 { get; set; }
        public string ewm { get; set; }
        public string mpdk_flag { get; set; }
        public string vin_jmh { get; set; }
        public DateTime? vin_jmh_lrsj { get; set; }
        public string vin_jmh_lrr { get; set; }
        public int kzcd { get; set; }
        public string co { get; set; }
    }
}