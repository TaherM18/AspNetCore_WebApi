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

    #region Register
    public async Task<int> Register(t_User data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        const string checkQuery = "SELECT COUNT(*) FROM api.t_user WHERE c_email = @c_email";
        const string insertQuery = @"
        INSERT INTO api.t_user 
            (c_username, c_email, c_password, c_address, c_gender, c_mobile, c_image) 
        VALUES 
            (@c_username, @c_email, @c_password, @c_address, @c_gender, @c_mobile, @c_image)";

        try
        {
            await using var checkCmd = new NpgsqlCommand(checkQuery, _conn);
            checkCmd.Parameters.AddWithValue("@c_email", data.c_Email);
            await _conn.CloseAsync();
            await _conn.OpenAsync();
            var emailExists = Convert.ToInt64(await checkCmd.ExecuteScalarAsync()) > 0; // COUNT(*) returns long

            if (emailExists)
            {
                return -1; // Email already exists
            }

            await using var insertCmd = new NpgsqlCommand(insertQuery, _conn);
            insertCmd.Parameters.AddWithValue("@c_username", data.c_UserName);
            insertCmd.Parameters.AddWithValue("@c_email", data.c_Email);
            insertCmd.Parameters.AddWithValue("@c_password", data.c_Password);
            insertCmd.Parameters.AddWithValue("@c_address", data.c_Address ?? (object)DBNull.Value);
            insertCmd.Parameters.AddWithValue("@c_gender", data.c_Gender ?? (object)DBNull.Value);
            insertCmd.Parameters.AddWithValue("@c_mobile", data.c_Mobile ?? (object)DBNull.Value);
            insertCmd.Parameters.AddWithValue("@c_image", data.c_Image ?? (object)DBNull.Value);

            int rowsAffected = await insertCmd.ExecuteNonQueryAsync();
            return (rowsAffected > 0) ? 1 : 0; // Return 1 if success, 0 if insert failed
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Register Exception: {ex.Message}");
            return -2; // Return -2 for unexpected errors
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }
    #endregion Register


    #region Login
    public async Task<t_User?> Login(vm_Login user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        const string query = @"
        SELECT c_userid, c_username, c_email, c_gender, c_mobile, c_address, c_image 
        FROM api.t_user 
        WHERE c_email = @c_email AND c_password = @c_password";

        try
        {
            await using var cmd = new NpgsqlCommand(query, _conn);
            cmd.Parameters.AddWithValue("@c_email", user.c_Email);
            cmd.Parameters.AddWithValue("@c_password", user.c_Password);
            await _conn.CloseAsync();
            await _conn.OpenAsync();
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync()) // Check if user exists
            {
                return new t_User
                {
                    c_UserId = reader.GetInt32(reader.GetOrdinal("c_userid")),
                    c_UserName = reader["c_username"] as string ?? "Unknown",
                    c_Email = reader["c_email"] as string ?? "Unknown",
                    c_Gender = reader["c_gender"] as string ?? "Unknown",
                    c_Mobile = reader["c_mobile"] as string ?? "Unknown",
                    c_Address = reader["c_address"] as string ?? "Unknown",
                    c_Image = reader["c_image"] as string ?? "placeholder.jpg"
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Login Exception: {ex.Message}");
            
        }
        finally
        {
            await _conn.CloseAsync();
        }
        return null; // Return null instead of an empty object
    }
    #endregion Login
}