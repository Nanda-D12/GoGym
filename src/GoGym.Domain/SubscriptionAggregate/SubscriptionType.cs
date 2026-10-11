using Ardalis.SmartEnum;

namespace GoGym.Domain.SubscriptionAggregate;

public class SubscriptionType : SmartEnum<SubscriptionType>
{
    public int MaxGyms { get; }
    public int MaxRooms { get; }
    public int MaxDailySessions { get; }

    public static readonly SubscriptionType Free = new(nameof(Free), 0, 
                                                        maxGyms: 1, 
                                                        maxRooms: 1,
                                                        maxDailySessions: 4);
    public static readonly SubscriptionType Starter = new(nameof(Starter), 1,
                                                            maxGyms: 1,
                                                            maxRooms: 3,
                                                            maxDailySessions: int.MaxValue);
    public static readonly SubscriptionType Pro = new(nameof(Pro), 2, 
                                                        maxGyms: 3,
                                                        maxRooms: int.MaxValue,
                                                        maxDailySessions: int.MaxValue);

    private SubscriptionType(string name, 
                                int value, 
                                int maxGyms, 
                                int maxRooms, 
                                int maxDailySessions) 
                            : base(name, value)
    {
        MaxGyms = maxGyms;
        MaxRooms = maxRooms;
        MaxDailySessions = maxDailySessions;
    }
}