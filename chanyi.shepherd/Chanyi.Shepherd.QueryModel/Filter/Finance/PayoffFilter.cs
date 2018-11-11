﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Finance
{
    public class PayoffFilter : ExpenditureFilter
    {
        public string EmployeeId { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                List<string> list = new List<string>();
                list.AddRange(base.GetBaseSqlWhere(out pms));

                if (!string.IsNullOrWhiteSpace(EmployeeId))
                {
                    list.Add("\"EmployeeId\"=@EmployeeId");
                    pms.AddWithValue("EmployeeId", EmployeeId);
                }

                return list;
            };

        }
    }
}