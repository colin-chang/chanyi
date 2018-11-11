using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using AutoMapper;
using System.Data;

namespace Chanyi.ERP.QueryService.Core.Helper
{
    public class SqlHelper
    {
        private MySqlConnection GetConnection()
        {
            string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            return new MySqlConnection(connstr);
        }

        public List<T> ExecuteReader<T>(string sql, params MySqlParameter[] parameters) where T : class
        {
            List<T> list = new List<T>();
            MySqlConnection conn = GetConnection();
            using (conn)
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Mapper.CreateMap<IDataReader, T>();
                        list.Add(Mapper.Map<T>(reader));
                    }
                }
            }
            return list;
        }

        public object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            MySqlConnection conn = GetConnection();
            using (conn)
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                        cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
