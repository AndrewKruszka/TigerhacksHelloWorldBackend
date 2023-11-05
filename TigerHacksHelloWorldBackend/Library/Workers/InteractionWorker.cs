using System;
using Library.DataAccess.Interfaces;
using Library.Models;
using Library.Workers.Interfaces;

namespace Library.Workers
{
	public class InteractionWorker : IInteractionWorker
	{
		private readonly IInteractionDataAccess dataAccess;
        private readonly IUserDataAccess userDataAccess;
        private readonly IGridWorker gridWorker;

		public InteractionWorker(IInteractionDataAccess interactionDataAccess,
							     IGridWorker gridWorker,
								 IUserDataAccess userDataAcces)
		{
			this.dataAccess = interactionDataAccess;
			this.gridWorker = gridWorker;
			this.userDataAccess = userDataAcces;
        }

		public Interaction GetInteraction(double latitude, double longitude, Int64 userId)
		{
			//hash the users coordinates
			var hash = gridWorker.GenerateHash(new Coordinate() { Latitude = latitude, Longitude = longitude });

			//update the location in db
			var updateLocationResult = userDataAccess.UpdateUserLocation(userId, hash);

			//try to find other users in same geohash
			var users = userDataAccess.GetUsersInHash(hash);
			if (!users.Any() || !updateLocationResult.IsSuccessful)
				return null;

			//Get all past interactions
            var interactions = dataAccess.GetInteractions(userId);

			//Go through all of the users in the hash until you find one that the user has not interacted with yet
            User interactingWithUser = null;
			foreach(var user in users)
			{
				if (!interactions.Any(x => x.InteractedWithUserId == user.UserId))
				{
					interactingWithUser = user;
                    break;
				}
            }

			//create the interaction
            var result = dataAccess.UpsertInteraction(userId, interactingWithUser.UserId, hash);
			if (result == null)
				return null;

			//we should probably return the created interaction id but we're not so do this
            interactions = dataAccess.GetInteractions(userId);

			return interactions.FirstOrDefault(x => x.InteractedWithUserId == interactingWithUser.UserId);
		}
	}
}

