using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Sessions.Commands.CreateSession;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, ErrorOr<Session>>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly ITrainersRepository _trainersRepository;

    public CreateSessionCommandHandler(IRoomsRepository roomsRepository, ITrainersRepository trainersRepository)
    {
        _roomsRepository = roomsRepository;
        _trainersRepository = trainersRepository;
    }

    
    public async Task<ErrorOr<Session>> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomsRepository.GetByIdAsync(command.RoomId);

        if (room is null)
        {
            return Error.NotFound(description:"Room not found.");
        }

        var trainer = await _trainersRepository.GetByIdAsync(command.TrainerId);

        if (trainer is null)
        {
            return Error.NotFound(description: "Trainer not found.");
        }

        var createTimeRangeResult = TimeRange.FromDateTimes(command.StartDateTime, command.EndDateTime);

        if (createTimeRangeResult.IsError && createTimeRangeResult.FirstError.Type == ErrorType.Validation)
        {
            return Error.Validation(description: "Invalid date and time");
        }

        if (!trainer.IsTimeSlotFree(DateOnly.FromDateTime(command.StartDateTime), createTimeRangeResult.Value))
        {
            return Error.Validation(description: "Trainer's calendar is not free for the entire session duration");
        }

        var session = new Session(
                            name: command.Name,
                            description: command.Description,
                            roomId: command.RoomId,
                            maxParticipants: command.MaxParticipants,
                            trainerId: command.TrainerId,
                            date: DateOnly.FromDateTime(command.StartDateTime),
                            time: createTimeRangeResult.Value);

        var scheduleSessionResult = room.ScheduleSession(session);

        if (scheduleSessionResult.IsError)
        {
            return scheduleSessionResult.Errors;
        }

        await _roomsRepository.UpdateAsync(room);

        return session;
    }
}