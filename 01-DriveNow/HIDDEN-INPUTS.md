# Hidden inputs

Student: Ibrahem Salah Refai  
Group / ID: 3

Class Hidden input Effect on a test Recommended refactor.

BookingService DateTime Now new repositories payment SMS email audit and random reference Time state I O and result are uncontrolled Constructor inject interfaces TimeProvider and a reference generator.

CancellationService DateTime Now concrete repositories and external services The 24 48 hour boundaries cannot be fixed Inject a clock and collaboration interfaces.

ReturnService DateTime Now concrete repositories payment and email Return time and side effects are tied to the machine Inject a clock and collaboration interfaces.

PricingService Static promo repository and DateTime Today Promo validity comes from global state Inject promo lookup and TimeProvider.

InMemoryDatabase Static lists and static NextBookingId Tests leak data into each other Make it an instance store or use a fake repository per test.

PaymentGateway Thread Sleep and new Random Tests are slow and flaky Hide it behind IPaymentGateway and fake it in unit tests.

EmailSender and AuditLog Filesystem and current time Tests write real files Inject notification audit interfaces.

This is an audit of the imported starter Complete the refactor before claiming that no hidden input remains.
