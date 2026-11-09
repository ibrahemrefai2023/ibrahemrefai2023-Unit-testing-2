# Coverage log

Student: Ibrahem Salah Refai  
Group / ID: 3

Run dotnet test --collect:"XPlat Code Coverage" and put the measured figures here. Do not estimate them.

Checkpoint Domain line Domain branch Application line Application branch

Baseline legacy tests measure measure measure measure

After Task 1 measure measure measure measure.

After Task 5 measure measure measure measure.

After Task 6 measure measure measure measure.

After Task 7 measure measure measure measure.

After Task 8 measure measure measure measure.

The console project is the only permitted coverage exclusion. Keep a screenshot of the final HTML report in screenshots.

One example of why coverage is not correctness: the rental-day test can execute the grace-hour line without testing the difference between exactly 60 minutes and 61 minutes. The original code was covered but still implemented the 60-minute boundary incorrectly.
