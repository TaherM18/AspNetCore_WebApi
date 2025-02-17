using System.Data;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class ContactRepository : IContactInterface
    {
        private readonly NpgsqlConnection _conn;
        public ContactRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }

        #region Add
        public async Task<int> Add(t_Contact data)
        {
            string query = @"
            INSERT INTO 
                api.t_contact 
                (c_userid,c_contactname,c_email,c_mobile, c_address,c_image,c_status,c_group) 
            VALUES 
                (@c_userid, @c_contactname, @c_email, @c_mobile, @c_address, @c_image, @c_status, @c_group)";
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(query, _conn);
                cm.Parameters.AddWithValue("@c_userid", data.c_UserId);
                cm.Parameters.AddWithValue("@c_contactname", data.c_ContactName);
                cm.Parameters.AddWithValue("@c_email", data.c_Email);
                cm.Parameters.AddWithValue("@c_mobile", data.c_Mobile);
                cm.Parameters.AddWithValue("@c_address", data.c_Address == null ? DBNull.Value : data.c_Address);
                cm.Parameters.AddWithValue("@c_image", data.c_Image == null ? DBNull.Value : data.c_Image);
                cm.Parameters.AddWithValue("@c_status", data.c_Status == null ? DBNull.Value : data.c_Status);
                cm.Parameters.AddWithValue("@c_group", data.c_Group == null ? DBNull.Value : data.c_Group);
                await _conn.CloseAsync();
                await _conn.OpenAsync();
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ContactRepo - Add() - " + ex.Message);
                return 0;
            }
            finally
            {
                _conn.Close();
            }
            return 1;
        }
        #endregion Add


        #region Delete
        public async Task<int> Delete(string contactid)
        {
            string query = @"
            DELETE FROM 
                api.t_contact 
            WHERE 
                c_contactid = @c_contactid";
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(query, _conn);
                cm.Parameters.AddWithValue("@c_contactid", int.Parse(contactid));
                await _conn.CloseAsync();
                await _conn.OpenAsync();
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ContactRepo - Delete() - " + ex.Message);
                return 0;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return 1;
        }
        #endregion Delete

        #region GetAll
        public async Task<List<t_Contact>> GetAll()
        {
            DataTable dt = new DataTable();
            List<t_Contact> contactList = new List<t_Contact>();
            try {
                NpgsqlCommand cm = new NpgsqlCommand("select * from api.t_contact", _conn);
                await _conn.CloseAsync();
                await _conn.OpenAsync();
                NpgsqlDataReader datar = cm.ExecuteReader();
                if (datar.HasRows)
                {
                    dt.Load(datar);
                    contactList = (from DataRow dr in dt.Rows
                                select new t_Contact()
                                {
                                    c_ContactId = Convert.ToInt32(dr["c_contactid"]),
                                    c_UserId = int.Parse(dr["c_userid"].ToString() ?? "0"),
                                    c_ContactName = dr["c_contactname"].ToString() ?? "N/A",
                                    c_Email = dr["c_email"].ToString() ?? "N/A",
                                    c_Mobile = dr["c_mobile"].ToString() ?? "N/A",
                                    c_Address = dr["c_address"].ToString(),
                                    c_Image = dr["c_image"].ToString(),
                                    c_Group = dr["c_group"].ToString(),
                                    c_Status = dr["c_status"].ToString()
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ContactRepo - GetAll() - " + ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return contactList;
        }
        #endregion GetAll


        #region GetAllByUser
        public async Task<List<t_Contact>> GetAllByUser(string userid)
        {
            string query = @"
            SELECT * 
            FROM api.t_contact
            WHERE c_userid = @c_userid";
            DataTable dt = new DataTable();
            List<t_Contact> contactList = new List<t_Contact>();

            try
            {
                using (NpgsqlCommand cm = new NpgsqlCommand(query, _conn))
                {
                    cm.Parameters.AddWithValue("@c_userid", int.Parse(userid));
                    await _conn.CloseAsync();
                    await _conn.OpenAsync();
                    NpgsqlDataReader datar = cm.ExecuteReader();
                    if (datar.HasRows)
                    {
                        dt.Load(datar);
                    }

                    contactList = (from DataRow dr in dt.Rows
                                   select new t_Contact()
                                   {
                                       c_ContactId = Convert.ToInt32(dr["c_contactid"]),
                                       c_UserId = int.Parse(dr["c_userid"].ToString() ?? "0"),
                                       c_ContactName = dr["c_contactname"].ToString() ?? "",
                                       c_Email = dr["c_email"].ToString() ?? "",
                                       c_Mobile = dr["c_mobile"].ToString() ?? "",
                                       c_Address = dr["c_address"].ToString(),
                                       c_Image = dr["c_image"].ToString(),
                                       c_Group = dr["c_group"].ToString(),
                                       c_Status = dr["c_status"].ToString()
                                   }).ToList();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ContactRepository - GetAllByUser() - "+ex.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return contactList;
        }
        #endregion GetAllByUser


        #region GetOne
        public async Task<t_Contact> GetOne(string contactid)
        {
            string query = @"
            SELECT * 
            FROM api.t_contact 
            WHERE c_contactid=@c_contactid";
            t_Contact contact = new t_Contact();

            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(query, _conn);
                cm.Parameters.AddWithValue("@c_contactid", int.Parse(contactid));
                _conn.Close();
                _conn.Open();
                NpgsqlDataReader datar = cm.ExecuteReader();

                if (datar.Read())
                {
                    contact = new t_Contact()
                    {
                        c_ContactId = Convert.ToInt32(datar["c_contactid"]),
                        c_UserId = int.Parse(datar["c_userid"].ToString() ?? "0"),
                        c_ContactName = datar["c_contactname"].ToString() ?? "",
                        c_Email = datar["c_email"].ToString() ?? "",
                        c_Mobile = datar["c_mobile"].ToString() ?? "",
                        c_Address = datar["c_address"].ToString(),
                        c_Image = datar["c_image"].ToString(),
                        c_Group = datar["c_group"].ToString(),
                        c_Status = datar["c_status"].ToString()
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ContactSesApi - GetOne() - " + ex.Message);
            }
            finally
            {
                _conn.Close();
            }

            return contact;
        }
        #endregion GetOne

        #region Update
        public async Task<int> Update(t_Contact data)
        {
            Console.WriteLine(data);
            string query = @"
            UPDATE 
                api.t_contact 
            SET
                c_userid=@c_userid,
                c_contactname=@c_contactname,
                c_email=@c_email,
                c_mobile=@c_mobile,
                c_address=@c_address,
                c_Image=@c_image,
                c_Status=@c_status,
                c_group=@c_group
            WHERE 
                c_contactid=@c_contactid";
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(query, _conn);
                cm.Parameters.AddWithValue("@c_userid", data.c_UserId);
                cm.Parameters.AddWithValue("@c_contactname", data.c_ContactName);
                cm.Parameters.AddWithValue("@c_email", data.c_Email);
                cm.Parameters.AddWithValue("@c_mobile", data.c_Mobile);
                cm.Parameters.AddWithValue("@c_address", data.c_Address == null ? DBNull.Value : data.c_Address);
                cm.Parameters.AddWithValue("@c_image", data.c_Image == null ? DBNull.Value : data.c_Image);
                cm.Parameters.AddWithValue("@c_status", data.c_Status == null ? DBNull.Value : data.c_Status);
                cm.Parameters.AddWithValue("@c_group", data.c_Group == null ? DBNull.Value : data.c_Group);
                cm.Parameters.AddWithValue("@c_contactid", data.c_ContactId == null ? DBNull.Value : data.c_ContactId);
                _conn.Close();
                _conn.Open();
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ContactRepository - Update() - " + ex.Message);
                return 0;
            }
            finally
            {
                _conn.Close();
            }

            return 1;
        }
        #endregion Update
    }
}