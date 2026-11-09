# Test doubles plan

Student: Ibrahem Salah Refai  
Group / ID: 3

Double Kind Intended use Why this kind.

DummyBookingRequest Dummy A parameter that a test does not read It only lets the test reach the behavior under test.

StubPromoRepository Stub Return a known promo code for pricing scenarios The return value controls the price path.

SpySmsSender Spy Capture the booking cancellation text The test needs to inspect a side effect after the call.

MockPaymentGateway Mock Verify Platinum booking does not charge a deposit The interaction itself is the rule.

FakeBookingRepository Fake Store bookings in a list for one test It behaves like the real repository without static data or I O.

Implement these only after the application service accepts interfaces through its constructor. The imported starter still creates concrete collaborators internally.
