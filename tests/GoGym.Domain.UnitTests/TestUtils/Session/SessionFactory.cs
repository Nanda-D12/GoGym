using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.SessionAggregate;
using GoGym.Domain.UnitTests.TestConstants;

namespace GoGym.Domain.UnitTests.TestUtils.Sessions;

public static class SessionFactory
{
    public static Session CreateSession(
                            int maxParticipants = Constants.Session.MaxParticipants,
                            Guid? roomId = null,
                            Guid? trainerId = null,
                            DateOnly? date = null,
                            TimeRange? time = null,
                            Guid? id = null)
    {
        var session = new Session(maxParticipants: maxParticipants,
                                    name: Constants.Session.Name,
                                    description: Constants.Session.Description,
                                    roomId: roomId ?? Constants.Room.Id,
                                    trainerId: trainerId ?? Constants.Trainer.Id,
                                    date: date ?? Constants.Session.Date,
                                    time: time ?? Constants.Session.Time,
                                    id: id ?? Constants.Session.Id);
        return session;
    }
}