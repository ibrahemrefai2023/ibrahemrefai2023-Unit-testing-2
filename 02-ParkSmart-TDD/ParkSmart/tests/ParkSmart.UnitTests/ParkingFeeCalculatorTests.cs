using FluentAssertions;
using ParkSmart.Application.Services;
using ParkSmart.Domain;

namespace ParkSmart.UnitTests;

public class ParkingFeeCalculatorTests
{
    private readonly ParkingFeeCalculator _calculator = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(15, 0)]
    [InlineData(16, 10)]
    [InlineData(60, 10)]
    [InlineData(61, 20)]
    [InlineData(180, 30)]
    public void Calculate_CarSessionAroundFreeAndHourlyBoundaries_ReturnsExpectedFee(int minutes, decimal expected)
    {
        var session = Session(new DateTime(2026, 11, 1, 10, 0, 0), minutes);

        var fee = _calculator.Calculate(session);

        fee.Should().Be(expected);
    }

    [Fact]
    public void Calculate_ExitEqualsEntry_ReturnsZero()
    {
        var entry = new DateTime(2026, 11, 1, 10, 0, 0);

        _calculator.Calculate(Session(entry, 0)).Should().Be(0m);
    }

    [Fact]
    public void Calculate_ExitBeforeEntry_ThrowsArgumentException()
    {
        var session = new ParkingSession
        {
            EntryTime = new DateTime(2026, 11, 1, 10, 0, 0),
            ExitTime = new DateTime(2026, 11, 1, 9, 59, 0)
        };

        var act = () => _calculator.Calculate(session);

        act.Should().Throw<ArgumentException>().WithMessage("Exit time must be after entry time.");
    }

    [Theory]
    [InlineData(10, 80)]
    [InlineData(30, 140)]
    [InlineData(48, 160)]
    public void Calculate_CarSessionWithDailyCap_ReturnsExpectedFee(int hours, decimal expected)
    {
        var session = Session(new DateTime(2026, 11, 1, 10, 0, 0), hours * 60);

        _calculator.Calculate(session).Should().Be(expected);
    }

    [Fact]
    public void Calculate_CarSessionPastTwoDailyBlocks_ReturnsFeeForRemainingBlock()
    {
        var session = Session(new DateTime(2026, 11, 1, 10, 0, 0), (48 * 60) + 10);

        _calculator.Calculate(session).Should().Be(170m);
    }

    [Theory]
    [InlineData(VehicleType.Motorcycle, 180, 15)]
    [InlineData(VehicleType.Truck, 600, 200)]
    [InlineData(VehicleType.Truck, 1800, 350)]
    public void Calculate_VehicleSpecificRateAndCap_ReturnsExpectedFee(VehicleType vehicle, int minutes, decimal expected)
    {
        var session = Session(new DateTime(2026, 11, 1, 10, 0, 0), minutes);
        session.Vehicle = vehicle;

        _calculator.Calculate(session).Should().Be(expected);
    }

    [Theory]
    [InlineData(23, 0, 7, 0, 30)]
    [InlineData(22, 0, 23, 0, 10)]
    [InlineData(21, 59, 7, 0, 80)]
    [InlineData(23, 0, 8, 1, 80)]
    public void Calculate_NightRateBoundary_ReturnsLowerApplicableFee(int entryHour, int entryMinute, int exitHour, int exitMinute, decimal expected)
    {
        var entry = new DateTime(2026, 11, 1, entryHour, entryMinute, 0);
        var exitDay = exitHour < entryHour ? entry.AddDays(1) : entry;
        var session = new ParkingSession
        {
            EntryTime = entry,
            ExitTime = new DateTime(exitDay.Year, exitDay.Month, exitDay.Day, exitHour, exitMinute, 0)
        };

        _calculator.Calculate(session).Should().Be(expected);
    }

    [Theory]
    [InlineData(VehicleType.Car, 5, 150)]
    [InlineData(VehicleType.Car, 1800, 150)]
    [InlineData(VehicleType.Truck, 1800, 350)]
    public void Calculate_LostTicket_ReturnsAtLeastMinimumFee(VehicleType vehicle, int minutes, decimal expected)
    {
        var session = Session(new DateTime(2026, 11, 1, 10, 0, 0), minutes);
        session.Vehicle = vehicle;
        session.TicketLost = true;

        _calculator.Calculate(session).Should().Be(expected);
    }

    [Theory]
    [InlineData(120, 0)]
    [InlineData(121, 30)]
    public void Calculate_FridaySessionAroundExtendedFreePeriod_ReturnsExpectedFee(int minutes, decimal expected)
    {
        var session = Session(new DateTime(2026, 11, 6, 10, 0, 0), minutes);

        _calculator.Calculate(session).Should().Be(expected);
    }

    [Theory]
    [InlineData(9, 0, 17, 0, 0)]
    [InlineData(18, 0, 20, 0, 10)]
    public void Calculate_WeekdaySubscriberSession_ReturnsFreeOrDiscountedFee(int entryHour, int entryMinute, int exitHour, int exitMinute, decimal expected)
    {
        var entry = new DateTime(2026, 11, 2, entryHour, entryMinute, 0);
        var session = new ParkingSession
        {
            EntryTime = entry,
            ExitTime = new DateTime(2026, 11, 2, exitHour, exitMinute, 0),
            IsSubscriber = true
        };

        _calculator.Calculate(session).Should().Be(expected);
    }

    [Fact]
    public void Calculate_SaturdaySubscriberSession_AppliesHalfPrice()
    {
        var session = Session(new DateTime(2026, 11, 7, 10, 0, 0), 180);
        session.IsSubscriber = true;

        _calculator.Calculate(session).Should().Be(15m);
    }

    [Fact]
    public void Calculate_SubscriberLostTicket_AppliesMinimumAfterDiscount()
    {
        var session = Session(new DateTime(2026, 11, 2, 9, 0, 0), 60);
        session.IsSubscriber = true;
        session.TicketLost = true;

        _calculator.Calculate(session).Should().Be(150m);
    }

    [Fact]
    public void Calculate_ElectricVehicleCharging_AddsUncappedChargeAfterParkingFee()
    {
        var session = Session(new DateTime(2026, 11, 2, 9, 0, 0), 120);
        session.IsSubscriber = true;
        session.ChargedKwh = 4m;

        _calculator.Calculate(session).Should().Be(14m);
    }

    [Fact]
    public void Calculate_NegativeCharging_ThrowsArgumentException()
    {
        var session = Session(new DateTime(2026, 11, 1, 10, 0, 0), 120);
        session.ChargedKwh = -1m;

        var act = () => _calculator.Calculate(session);

        act.Should().Throw<ArgumentException>().WithMessage("Charged kWh cannot be negative.");
    }

    private static ParkingSession Session(DateTime entry, int minutes) => new()
    {
        EntryTime = entry,
        ExitTime = entry.AddMinutes(minutes)
    };
}
