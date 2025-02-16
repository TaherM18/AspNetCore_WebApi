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
    public class StateRepository : IStateInterface
    {
        private readonly NpgsqlConnection _con;

        public StateRepository(NpgsqlConnection connection)
        {
            _con = connection;
        }
        
        public async Task<List<t_State>> GetAll()
        {
            string query = @"
            SELECT c_stateid, c_statename
            FROM api.t_state";
            DataTable dt = new DataTable();
            List<t_State> statusList = new List<t_State>();
            try
            {
                await using (NpgsqlCommand command = new NpgsqlCommand(query, _con))
                {
                    await _con.OpenAsync();
                    NpgsqlDataReader dataReader = await command.ExecuteReaderAsync();
                    if (dataReader.HasRows)
                    {
                        dt.Load(dataReader);
                    }
                    statusList = (from DataRow row in dt.Rows
                                select new t_State() {
                                    c_StateId = Convert.ToInt32(row["c_stateid"]),
                                    c_StateName = row["c_statename"].ToString() ?? ""
                                }).ToList();
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("StateRepository - GetAll() - " + ex.Message);
            }
            finally
            {
                await _con.CloseAsync();
            }
            return statusList;
        }


        public async Task<t_State> GetOne(int id)
        {
            string query = @"
            SELECT c_stateid, c_statename
            FROM api.t_state
            WHERE c_stateid = @c_stateid";
            t_State state = new t_State();
            try
            {
                await using (NpgsqlCommand command = new NpgsqlCommand(query, _con))
                {
                    await _con.OpenAsync();
                    command.Parameters.AddWithValue("@c_stateid", id);
                    NpgsqlDataReader dataReader = await command.ExecuteReaderAsync();
                    if (await dataReader.ReadAsync())
                    {                    
                        state = new t_State() 
                        {
                            c_StateId = Convert.ToInt32(dataReader["c_stateid"]),
                            c_StateName = dataReader["c_statename"].ToString() ?? ""
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
            return state;
        }
    }
}