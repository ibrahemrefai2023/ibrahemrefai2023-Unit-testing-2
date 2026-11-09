using DriveNow.Application.Models;
using DriveNow.Application.Services;
using DriveNow.Domain.Entities;
using DriveNow.Domain.Enums;
using FluentAssertions;
using FluentAssertions.Execution;

namespace DriveNow.UnitTests;

public class PricingServiceTests
{
    private readonly PricingService _pricing = new();

    [Fact]
    [Trait("Rule", "BR-30")]
    [Trait("Rule", "BR-31")]
    [Trait("Rule", "BR-34")]
    [Trait("Rule", "BR-38")]
    [Trait("Rule", "BR-40")]
    public void Calculate_WorkedExampleA_ReturnsExpectedBreakdown()
    {
        var pickup = new DateTime(2026, 11, 1, 10, 0, 0);
        var customer = Customer(pickup.AddYears(-34), LoyaltyTier.Gold);
        var vehicle = Vehicle(600m, VehicleCategory.Economy);
        var request = Request(pickup, pickup.AddDays(3), InsurancePlan.Standard);

        var result = _pricing.Calculate(customer, vehicle, request);

        using (new AssertionScope())
        {
            result.RentalDays.Should().Be(3);
            result.BaseAmount.Should().Be(1800m);
            result.WeekendSurcharge.Should().Be(0m);
            result.YoungDriverSurcharge.Should().Be(0m);
            result.LoyaltyDiscount.Should().Be(180m);
            result.InsuranceCost.Should().Be(450m);
            result.Total.Should().Be(2070m);
        }
    }

    [Fact]
    [Trait("Rule", "BR-33")]
    [Trait("Rule", "BR-34")]
    [Trait("Rule", "BR-35")]
    [Trait("Rule", "BR-37")]
    [Trait("Rule", "BR-38")]
    [Trait("Rule", "BR-39")]
    [Trait("Rule", "BR-40")]
    public void Calculate_WorkedExampleB_CapsPromoAndReturnsExpectedBreakdown()
    {
        var pickup = NextThursday(DateTime.Today).AddHours(9);
        var customer = Customer(pickup.AddYears(-23), LoyaltyTier.Silver);
        var vehicle = Vehicle(1500m, VehicleCategory.SUV);
        var request = Request(
            pickup,
            pickup.AddDays(7).AddMinutes(30),
            InsurancePlan.Premium,
            additionalDrivers: 1,
            promoCode: "SAVE15");

        var result = _pricing.Calculate(customer, vehicle, request);

        using (new AssertionScope())
        {
            result.RentalDays.Should().Be(7);
            result.BaseAmount.Should().Be(10500m);
            result.WeekendSurcharge.Should().Be(600m);
            result.YoungDriverSurcharge.Should().Be(1575m);
            result.DurationDiscount.Should().Be(1050m);
            result.LoyaltyDiscount.Should().Be(525m);
            result.PromoDiscount.Should().Be(1050m);
            result.InsuranceCost.Should().Be(2100m);
            result.AdditionalDriversCost.Should().Be(700m);
            result.Total.Should().Be(12850m);
        }
    }

    private static Customer Customer(DateTime dateOfBirth, LoyaltyTier tier) => new()
    {
        DateOfBirth = dateOfBirth,
        LicenseIssueDate = dateOfBirth.AddYears(18),
        LoyaltyTier = tier
    };

    private static Vehicle Vehicle(decimal dailyRate, VehicleCategory category) => new()
    {
        DailyRate = dailyRate,
        Category = category
    };

    private static BookingRequest Request(
        DateTime pickup,
        DateTime returnAt,
        InsurancePlan insurance,
        int additionalDrivers = 0,
        string? promoCode = null) => new()
    {
        PickupAt = pickup,
        ReturnAt = returnAt,
        InsurancePlan = insurance,
        AdditionalDrivers = additionalDrivers,
        PromoCode = promoCode
    };

    private static DateTime NextThursday(DateTime date)
    {
        var days = ((int)DayOfWeek.Thursday - (int)date.DayOfWeek + 7) % 7;
        return date.AddDays(days == 0 ? 7 : days).Date;
    }
}
