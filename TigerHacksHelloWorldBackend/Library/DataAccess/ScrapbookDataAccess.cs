using System;
using Library.DataAccess.Interfaces;
using Library.Models;
using Library.Utilities.Interfaces;
using Npgsql;

namespace Library.DataAccess
{
	public class ScrapbookDataAccess : IScrapbookDataAccess
	{
        private readonly ISettings settings;

        public ScrapbookDataAccess(ISettings settings)
		{
            this.settings = settings;
        }

        public IEnumerable<Interaction> GetInteractions(int userId)
        {
            try
            {
                var interactions = new List<Interaction>();
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand($"SELECT * from gt_user_interactions({userId})"))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var interaction = new Interaction()
                        {
                            Id = int.Parse(reader["interaction_id"].ToString()),
                            InteractedOn = DateTime.Parse(reader["interaction_time"]?.ToString()),
                            PrimaryUserId = int.Parse(reader["primary_user_id"].ToString()),
                            InteractedWithUserId = int.Parse(reader["interacted_with_user_id"].ToString()),
                            InteractedAtHash = reader["interacted_at_hash"].ToString()
                        };

                        interactions.Add(interaction);
                    }
                }

                return interactions;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get interactions");
                return null;
            }
        }

        public Result UpsertInteraction(Int64 primaryUserId, Int64 interactedWithId, string geohash)
        {
            try
            {
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand("mw_update_address_hash"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Bigint, primaryUserId);
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Bigint, interactedWithId);
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, geohash);

                    command.ExecuteNonQuery();
                }

                return new Result { IsSuccessful = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to upsert interaction");
                return new Result() { IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }
    }
}

