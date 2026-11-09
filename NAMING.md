# Naming convention

Student: Ibrahem Salah Refai  
Group / ID: 3

Test projects test classes and test files use ClassUnderTestTests.

Test methods use Method Scenario ExpectedResult This gives each failing test a useful name in the test runner.

Hand written test doubles begin with their role for example FakeBookingRepository or SpySmsSender.

Bogus customizations end with Customization and AutoFixture attributes end with DataAttribute.

Interfaces begin with I Repository classes end with Repository and live in a Repositories namespace Application services end with Service and live in Services Domain exceptions end with Exception and live in DriveNow Domain Exceptions.

ParkingFeeCalculator is kept as the exception required by the ParkSmart brief it lives in ParkSmart Application Services.
