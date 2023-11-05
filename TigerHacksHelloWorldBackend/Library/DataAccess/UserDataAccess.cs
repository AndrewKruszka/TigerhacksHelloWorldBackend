using System;
using System.Xml.Linq;
using Library.DataAccess.Interfaces;
using Library.Models;
using Library.Utilities.Interfaces;
using Npgsql;

namespace Library.DataAccess
{
	public class UserDataAccess : IUserDataAccess
	{
		private readonly ISettings settings;

		public UserDataAccess(ISettings settings)
		{
			this.settings = settings;
		}

        public IEnumerable<FriendEntry> GetFriends(Int64 userId)
        {
            try
            {
                var friends = new List<FriendEntry>();
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand($"SELECT * from gt_friends({userId})"))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var friend = new FriendEntry()
                        {
                            PrimaryUserId = int.Parse(reader["primary_user_id"].ToString()),
                            FriendUserId = int.Parse(reader["friend_user_id"].ToString()),
                            InteractionId = int.Parse(reader["interaction_id"].ToString()),
                        };

                        friends.Add(friend);
                    }
                }

                return friends;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get friends");
                return null;
            }
        }

        public Result AddFriend(Int64 userId, Int64 friendUserId, Int64 interactionId)
        {
            try
            {
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand("mw_create_friend"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Bigint, userId);
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Bigint, friendUserId);
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Bigint, interactionId);

                    command.ExecuteNonQuery();
                }

                return new Result { IsSuccessful = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add friend");
                return new Result() { IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }

        public User GetUser(string username)
        {
            try
            {
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand($"SELECT * from gt_user_by_name('{username}')"))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new User()
                        {
                            UserId = int.Parse(reader["userid"].ToString()),
                            Name = reader["name"].ToString(),
                            PassHash = reader["password"].ToString()
                        };
                        return user;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get user");
                return null;
            }
        }

        public IEnumerable<User> GetUsersInHash(string gridhash)
        {
            try
            {
                var users = new List<User>();
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand($"SELECT * from gt_users_in_geohash('{gridhash}')"))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User()
                        {
                            UserId = int.Parse(reader["user_id"].ToString()),
                            Name = reader["name"].ToString()
                        };
                        users.Add(user);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get user");
                return null;
            }
        }

        public Result UpsertUser(string username, string passhash)
        {
            try
            {
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand("mw_upsert_user"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, username);
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, passhash);

                    command.ExecuteNonQuery();
                }

                return new Result { IsSuccessful = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to upsert user");
                return new Result() { IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }

        public Result UpdateUserLocation(Int64 userId, string geohash)
        {
            try
            {
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand("mw_update_user_location"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Bigint, userId);
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, geohash);

                    command.ExecuteNonQuery();
                }

                return new Result { IsSuccessful = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to update user location");
                return new Result() { IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }
    }
}

