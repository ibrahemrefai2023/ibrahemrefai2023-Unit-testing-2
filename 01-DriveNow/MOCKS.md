# Legacy receipt tests

Student: Ibrahem Salah Refai  
Group / ID: 3

The five legacy tests protect calls to collaborators call counts ordering and the exact renderer return value They do not prove that VAT receipt labels line item amounts alignment or the total are correct A wrong VAT rate can still pass because the mock returns a value chosen by the test.

ILineItemFactory ITaxCalculator ICurrencyFormatter and IReceiptFormatter exist mainly for the interaction based tests The replacement test should build a receipt with the real factory tax calculator currency formatter and renderer then snapshot the result for a completed rental with late fee and damage.

Before doing that refactor print the old and new receipt text from the same booking and compare them Keep the behavior not the collaborator call count.
