using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Spring.Data.Common;

using Chanyi.Utility.Common;
namespace Chanyi.Shepherd.QueryModel.Filter.Breeding
{
    public class SecondAssessFilter : AssessFilter
    {
        /// <summary>
        /// 品种特征评分
        /// </summary>
        public float? MaxBreedFeatureScore { get; set; }
        public float? MinBreedFeatureScore { get; set; }

        /// <summary>
        /// 生殖器官评分
        /// </summary>
        public float? MaxGenitaliaScore { get; set; }
        public float? MinGenitaliaScore { get; set; }

        protected override GeneratorDelegate CreateGenerator()
        {
            return (out IDbParameters pms) =>
            {
                pms = Template.CreateDbParameters();
                List<string> list = base.GetSqlWhereAndPms(out pms);
                if (MaxBreedFeatureScore != null)
                {
                    list.Add("sa.\"BreedFeatureScore\"<=@MaxBreedFeatureScore");
                    pms.AddWithValue("MaxBreedFeatureScore", MaxBreedFeatureScore);
                }
                if (MinBreedFeatureScore != null)
                {
                    list.Add("sa.\"BreedFeatureScore\">=@MinBreedFeatureScore");
                    pms.AddWithValue("MinBreedFeatureScore", MinBreedFeatureScore);
                }


                if (MaxGenitaliaScore != null)
                {
                    list.Add("sa.\"GenitaliaScore\"<=@MaxGenitaliaScore");
                    pms.AddWithValue("MaxGenitaliaScore", MaxGenitaliaScore);
                }
                if (MinGenitaliaScore != null)
                {
                    list.Add("sa.\"GenitaliaScore\">=@MinGenitaliaScore");
                    pms.AddWithValue("MinGenitaliaScore", MinGenitaliaScore);
                }

                return list;
            };
        }
    }
}
