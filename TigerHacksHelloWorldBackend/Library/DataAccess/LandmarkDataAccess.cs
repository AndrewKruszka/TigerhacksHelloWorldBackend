using System;
using System.Xml.Linq;
using Library.DataAccess.Interfaces;
using Library.Models;
using Library.Utilities.Interfaces;
using Npgsql;

namespace Library.DataAccess
{
	public class LandmarkDataAccess :ILandmarkDataAccess
	{
        private readonly ISettings settings;

        public LandmarkDataAccess(ISettings settings)
		{
            this.settings = settings;
        }

        public IEnumerable<Landmark> GetLandmarks()
        {
            try
            {
                var landmarks = new List<Landmark>();
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand($"SELECT * from gt_landmarks()"))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var landmark = new Landmark()
                        {
                            LandmarkId = int.Parse(reader["landmark_id"].ToString()),
                            GeoHash = reader["geo_hash"].ToString(),
                            DisplayName = reader["display_name"].ToString()
                        };

                        landmarks.Add(landmark);
                    }
                }

                return landmarks;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get landmarks");
                return null;
            }
        }

        public Result UpsertLandmark(string geohash, string name)
        {
            try
            {
                using (var connection = NpgsqlDataSource.Create(settings.DB))
                using (var command = connection.CreateCommand("mw_create_landmark"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, geohash);
                    command.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, name);

                    command.ExecuteNonQuery();
                }

                return new Result { IsSuccessful = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to upsert landmark");
                return new Result() { IsSuccessful = false, ErrorMessage = ex.Message };
            }
        }
    }
}

