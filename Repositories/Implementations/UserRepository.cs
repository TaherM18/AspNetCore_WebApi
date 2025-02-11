using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations;

public class UserRepository : IUserInterface
{
    private readonly NpgsqlConnection _conn;

    public UserRepository(NpgsqlConnection connection)
    {
        _conn = connection;
    }

    public async Task<int> Register(t_User data)
    {
        string checkQuery = @"
        SELECT * 
        FROM api.t_user
        WHERE c_email = @c_email";

        string insertQuery = @"
        INSERT INTO 
            api.t_user (c_username, c_email, c_password,c_address, c_gender, c_mobile, c_image) 
        VALUES 
            (@c_username, @c_email, @c_password, @c_address, @c_gender, @c_mobile, @c_image)";

        int status = 0;

        try
        {
            await _conn.CloseAsync();

            NpgsqlCommand comcheck = new NpgsqlCommand(checkQuery, _conn);
            comcheck.Parameters.AddWithValue("@c_email", data.c_Email);
            await _conn.OpenAsync();
            using (NpgsqlDataReader datadr = await comcheck.ExecuteReaderAsync())
            {
                if (datadr.HasRows)
                {
                    await _conn.CloseAsync();
                    return 0;
                }
                else
                {
                    await _conn.CloseAsync();

                    NpgsqlCommand com = new NpgsqlCommand(insertQuery, _conn);
                    com.Parameters.AddWithValue("@c_username", data.c_UserName);
                    com.Parameters.AddWithValue("@c_email", data.c_Email);
                    com.Parameters.AddWithValue("@c_password", data.c_Password);
                    com.Parameters.AddWithValue("@c_address", data.c_Address);
                    com.Parameters.AddWithValue("@c_gender", data.c_Gender);
                    com.Parameters.AddWithValue("@c_mobile", data.c_Mobile);
                    com.Parameters.AddWithValue("@c_image", data.c_Image);
                    await _conn.OpenAsync();
                    await com.ExecuteNonQueryAsync();
                    await _conn.CloseAsync();
                    return 1; //returning 1 is user has Successfuly Registered
                }
            }
        }
        catch (Exception e)
        {
            await _conn.CloseAsync();
            Console.WriteLine("Register Failed , Error :- " + e.Message);
            return -1; //-1 if There is error during registration
        }
    }
    public async Task<t_User> Login(vm_Login user)
    {
        t_User UserData = new t_User();
        var query = @"
        SELECT * 
        FROM api.t_user 
        WHERE c_email=@c_email AND c_password = @c_password";
        try
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@c_email", user.c_Email);
                cmd.Parameters.AddWithValue("@c_password", user.c_Password);
                await _conn.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    UserData.c_UserId = (int)reader["c_userid"];
                    UserData.c_UserName = (string)reader["c_username"];
                    UserData.c_Email = (string)reader["c_email"];
                    UserData.c_Gender = (string)reader["c_gender"];
                    UserData.c_Mobile = (string)reader["c_mobile"];
                    UserData.c_Address = (string)reader["c_address"];
                    UserData.c_Image = (string)reader["c_image"];
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("----------->Login Error : " + e.Message);
        }
        finally
        {
            await _conn.CloseAsync();
        }

        return UserData;
    }
}