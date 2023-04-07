using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace videocalling_blazor.Server.StaticData.Manipulator
{
    public class SqlParam
    {
        public string ParamName { get; set; }
        public SqlDbType SqlType { get; set; }
        public object Value { get; set; }
        public SqlParam(string paramName, SqlDbType oracleType, object value)
        {
            ParamName = paramName;
            SqlType = oracleType;
            Value = value;
        }
    }
}
