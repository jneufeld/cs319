using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public enum MsgType
    {
        Error,
        Warning,
        Success,
        Info,
    }

    public class MsgViewModel
    {
        public MsgType MsgType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public string AlertClass
        {
            get
            {
                string clss = GetAlertClass(MsgType);
                if (!String.IsNullOrEmpty(clss))
                    return "alert-" + clss;
                return "";
            }
        }

        public static string GetAlertClass(MsgType type)
        {
            switch (type)
            {
                case MsgType.Error:
                    return "error";
                case MsgType.Success:
                    return "success";
                case MsgType.Info:
                    return "info";
                default:
                    return "";
            }
        }
    }
}