using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frank.Expressage
{
    public class MQueryParameter
    {
        /// <summary>
        /// 快递公司
        /// </summary>
        public string TypeCom { get; set; }

        /// <summary>
        /// 快递订单号
        /// </summary>
        public string OrderId { get; set; }
    }

    public class MResultMsg
    {
        public bool Result { get; set; }

        public StateType State { get; set; }

        public string JsonMessage { get; set; }

        public List<ExpressageInfo> Data { get; set; }

        public ErrorMsg Error { get; set; }
    }

    public class ExpressageInfo
    {

        public DateTime Time { get; set; }

        public string Context { get; set; }
    }

    public enum StateType
    {
        在途,
        揽件,
        疑难,
        签收,
        退签,
        派件,
        退回
    }

    public class ErrorMsg
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
