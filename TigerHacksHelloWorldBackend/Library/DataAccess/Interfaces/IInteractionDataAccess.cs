using System;
using Library.Models;

namespace Library.DataAccess.Interfaces
{
	public interface IInteractionDataAccess
	{
        IEnumerable<Interaction> GetInteractions(Int64 userId);
        Result UpsertInteraction(Int64 primaryUserId, Int64 interactedWithId, string geohash);
    }
}

