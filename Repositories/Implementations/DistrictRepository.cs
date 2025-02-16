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
    public class DistrictRepository : IDistrictInterface
    {
        private readonly NpgsqlConnection _con;

        public DistrictRepository(NpgsqlConnection connection)
        {
            _con = connection;
        }
        
        public async Task<List<t_District>> GetAll()
        {
            string query = @"
            SELECT c_districtid, c_districtname
            FROM api.t_district";
            DataTable dt = new DataTable();
            List<t_District> districtList = new List<t_District>();
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
                    districtList = (from DataRow row in dt.Rows
                                select new t_District() {
                                    c_DistrictId = Convert.ToInt32(row["c_districtid"]),
                                    c_DistrictName = row["c_districtname"].ToString() ?? ""
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
            return districtList;
        }

        public async Task<t_District> GetOne(int id)
        {
            string query = @"
            SELECT c_districtid, c_districtname
            FROM api.t_district
            WHERE c_districtid = @c_districtid";
            t_District district = new t_District();
            try
            {
                await using (NpgsqlCommand command = new NpgsqlCommand(query, _con))
                {
                    await _con.OpenAsync();
                    command.Parameters.AddWithValue("@c_districtid", id);
                    NpgsqlDataReader dataReader = await command.ExecuteReaderAsync();
                    if (await dataReader.ReadAsync())
                    {                    
                        district = new t_District() 
                        {
                            c_DistrictId = Convert.ToInt32(dataReader["c_districtid"]),
                            c_DistrictName = dataReader["c_districtname"].ToString() ?? ""
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
            return district;
        }
    }
}