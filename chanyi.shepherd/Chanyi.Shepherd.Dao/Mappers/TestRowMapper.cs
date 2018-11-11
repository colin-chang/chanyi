using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Spring.Data.Generic;
using Chanyi.Shepherd.QueryModel;


namespace Chanyi.Shepherd.Dao.Mappers
{
    public class TestRowMapper : IRowMapper<Test>
    {
        static TestRowMapper()
        {
            Mapper.CreateMap<IDataReader, Test>();//初始化映射器
        }

        /// <summary>
        /// 将IDataReader读取的数据格式化为Role的Model对象
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        public Test MapRow(IDataReader reader, int rowNum)
        {
            return Mapper.Map<IDataReader, Test>(reader);
        }

    }
}
