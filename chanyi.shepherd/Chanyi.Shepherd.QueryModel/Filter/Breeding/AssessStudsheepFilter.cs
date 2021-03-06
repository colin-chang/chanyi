﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Breeding
{
    public class AssessStudsheepFilter : AssessFilter
    {
        public float? MaxMatingAbility { get; set; }
        public float? MinMatingAbility { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                pms = Template.CreateDbParameters();
                List<string> list = base.GetSqlWhereAndPms(out pms);

                if (MaxMatingAbility != null)
                {
                    list.Add("ass.\"MatingAbility\"<=@MaxMatingAbility");
                    pms.AddWithValue("MaxMatingAbility", MaxMatingAbility);
                }
                if (MinMatingAbility != null)
                {
                    list.Add("ass.\"MatingAbility\">=@MinMatingAbility");
                    pms.AddWithValue("MinMatingAbility", MinMatingAbility);
                }

                return list;
            };
        }
    }
}
