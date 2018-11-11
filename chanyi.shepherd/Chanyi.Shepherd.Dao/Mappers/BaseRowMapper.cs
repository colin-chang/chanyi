using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Spring.Data.Generic;
using AutoMapper;

namespace Chanyi.Shepherd.Dao.Mappers
{
    public class BaseRowMapper<T> : IRowMapper<T> where T : class,new()
    {
        static BaseRowMapper()
        {
            Mapper.CreateMap<IDataReader, T>();
        }
        public T MapRow(IDataReader reader, int rowNum)
        {
            return Mapper.Map<IDataReader, T>(reader);
        }
    }
}
