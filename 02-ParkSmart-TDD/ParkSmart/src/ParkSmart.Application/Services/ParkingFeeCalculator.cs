using ParkSmart.Domain;

namespace ParkSmart.Application.Services;

public class ParkingFeeCalculator
{
    public decimal Calculate(ParkingSession session)
    {
        if (session.ExitTime < session.EntryTime)
            throw new ArgumentException("Exit time must be after entry time.");

        if (session.ChargedKwh < 0)
            throw new ArgumentException("Charged kWh cannot be negative.");

        var parkingFee = CalculateParkingFee(session);
        var chargingFee = session.ChargedKwh * 3.50m;

        return Round(parkingFee + chargingFee);
    }

    private static decimal CalculateParkingFee(ParkingSession session)
    {
        var duration = session.ExitTime - session.EntryTime;
        var freeMinutes = session.EntryTime.DayOfWeek == DayOfWeek.Friday ? 120 : 15;

        decimal fee;
        if (duration.TotalMinutes <= freeMinutes)
        {
            fee = 0m;
        }
        else
        {
            fee = CalculateNormalFee(duration, session.Vehicle);

            if (IsNightSession(session))
                fee = Math.Min(fee, NightFlatRate(session.Vehicle));

            if (session.IsSubscriber)
                fee = IsSubscriberFreeSession(session) ? 0m : Round(fee * 0.50m);
        }

        if (session.TicketLost)
            fee = Math.Max(fee, LostTicketMinimum(session.Vehicle));

        return Round(fee);
    }

    private static decimal CalculateNormalFee(TimeSpan duration, VehicleType vehicle)
    {
        var durationLeft = duration;
        var total = 0m;

        while (durationLeft > TimeSpan.Zero)
        {
            var block = durationLeft > TimeSpan.FromHours(24)
                ? TimeSpan.FromHours(24)
                : durationLeft;

            var startedHours = (int)Math.Ceiling(block.TotalHours);
            total += Math.Min(startedHours * HourlyRate(vehicle), DailyCap(vehicle));
            durationLeft -= block;
        }

        return Round(total);
    }

    private static bool IsNightSession(ParkingSession session)
    {
        var entryIsAtNight = session.EntryTime.TimeOfDay >= TimeSpan.FromHours(22);
        var nextMorning = session.EntryTime.Date.AddDays(1).AddHours(8);

        return entryIsAtNight && session.ExitTime <= nextMorning;
    }

    private static bool IsSubscriberFreeSession(ParkingSession session)
    {
        var sameDay = session.EntryTime.Date == session.ExitTime.Date;
        var weekday = session.EntryTime.DayOfWeek is >= DayOfWeek.Sunday and <= DayOfWeek.Thursday;
        var startsInWindow = session.EntryTime.TimeOfDay >= TimeSpan.FromHours(7);
        var endsInWindow = session.ExitTime.TimeOfDay <= TimeSpan.FromHours(19);

        return sameDay && weekday && startsInWindow && endsInWindow;
    }

    private static decimal HourlyRate(VehicleType vehicle) => vehicle switch
    {
        VehicleType.Motorcycle => 5m,
        VehicleType.Truck => 25m,
        _ => 10m
    };

    private static decimal DailyCap(VehicleType vehicle) => vehicle switch
    {
        VehicleType.Motorcycle => 40m,
        VehicleType.Truck => 200m,
        _ => 80m
    };

    private static decimal NightFlatRate(VehicleType vehicle) => vehicle switch
    {
        VehicleType.Motorcycle => 15m,
        VehicleType.Truck => 75m,
        _ => 30m
    };

    private static decimal LostTicketMinimum(VehicleType vehicle) => vehicle switch
    {
        VehicleType.Motorcycle => 75m,
        VehicleType.Truck => 300m,
        _ => 150m
    };

    private static decimal Round(decimal amount) =>
        Math.Round(amount, 2, MidpointRounding.AwayFromZero);
}
