using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class StatusRepository : IStatusInterface
    {
        private readonly NpgsqlConnection _con;

        public StatusRepository(NpgsqlConnection connection)
        {
            _con = connection;
        }

        #region GetAll
        public async Task<List<t_Status>> GetAll()
        {
            string query = @"
            SELECT c_statusid, c_statusname
            FROM api.t_status";
            DataTable dt = new DataTable();
            List<t_Status> statusList = new List<t_Status>();
            try
            {
                await using (NpgsqlCommand command = new NpgsqlCommand(query, _con))
                {
                    await _con.CloseAsync();
                    await _con.OpenAsync();
                    NpgsqlDataReader dataReader = await command.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        dt.Load(dataReader);
                    }
                    statusList = (from DataRow row in dt.Rows
                                select new t_Status() {
                                    c_StatusId = Convert.ToInt32(row["c_statusid"]),
                                    c_StatusName = row["c_statusname"].ToString() ?? ""
                                }).ToList();
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("StatusRepository - GetAll() - " + ex.Message);
            }
            finally
            {
                await _con.CloseAsync();
            }
            return statusList;
        }
        #endregion GetAll


        #region GetOne
        public async Task<t_Status> GetOne(int id)
        {
            string query = @"
            SELECT c_statusid, c_statusname
            FROM api.t_status
            WHERE c_statusid = @c_statusid";
            t_Status status = new t_Status();
            try
            {
                await using (NpgsqlCommand command = new NpgsqlCommand(query, _con))
                {
                    await _con.CloseAsync();
                    await _con.OpenAsync();
                    command.Parameters.AddWithValue("@c_statusid", id);
                    NpgsqlDataReader dataReader = await command.ExecuteReaderAsync();
                    if (await dataReader.ReadAsync())
                    {                    
                        status = new t_Status() 
                        {
                            c_StatusId = Convert.ToInt32(dataReader["c_statusid"]),
                            c_StatusName = dataReader["c_statusname"].ToString() ?? ""
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("StatusRepository - GetOne() - " + ex.Message);
            }
            finally
            {
                await _con.CloseAsync();
            }
            return status;
        }
        #endregion GetOne
    }
}