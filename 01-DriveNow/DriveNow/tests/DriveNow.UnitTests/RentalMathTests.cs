using DriveNow.Domain.Services;
using FluentAssertions;

namespace DriveNow.UnitTests;

public class RentalMathTests
{
    [Theory]
    [Trait("Rule", "BR-01")]
    [InlineData("2000-11-07", "2026-11-06", 25)]
    [InlineData("2000-11-07", "2026-11-07", 26)]
    [InlineData("2000-02-29", "2025-02-27", 24)]
    [InlineData("2000-02-29", "2025-02-28", 25)]
    public void CalculateAge_BirthdayAndLeapYearBoundaries_ReturnsFullYears(string birth, string onDate, int expected)
    {
        var result = RentalMath.CalculateAge(DateTime.Parse(birth), DateTime.Parse(onDate));

        result.Should().Be(expected);
    }

    [Theory]
    [Trait("Rule", "BR-02")]
    [InlineData("2020-11-07", "2026-11-06", 5)]
    [InlineData("2020-11-07", "2026-11-07", 6)]
    [InlineData("2020-02-29", "2025-02-28", 5)]
    public void CalculateLicenseYears_AnniversaryBoundaries_ReturnsFullYears(string issued, string onDate, int expected)
    {
        var result = RentalMath.CalculateLicenseYears(DateTime.Parse(issued), DateTime.Parse(onDate));

        result.Should().Be(expected);
    }

    [Theory]
    [Trait("Rule", "BR-03")]
    [InlineData(3, 1)]
    [InlineData(24 * 60 + 59, 1)]
    [InlineData(25 * 60, 1)]
    [InlineData(25 * 60 + 1, 2)]
    [InlineData(72 * 60, 3)]
    public void CalculateRentalDays_GraceHourAndDayBoundaries_ReturnsExpectedDays(int minutes, int expected)
    {
        var pickup = new DateTime(2026, 11, 1, 10, 0, 0);

        var result = RentalMath.CalculateRentalDays(pickup, pickup.AddMinutes(minutes));

        result.Should().Be(expected);
    }

    [Theory]
    [Trait("Rule", "BR-03")]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateRentalDays_ReturnAtOrBeforePickup_ThrowsArgumentException(int minutes)
    {
        var pickup = new DateTime(2026, 11, 1, 10, 0, 0);
        var act = () => RentalMath.CalculateRentalDays(pickup, pickup.AddMinutes(minutes));

        act.Should().Throw<ArgumentException>().WithMessage("Return must be after pickup");
    }
}
