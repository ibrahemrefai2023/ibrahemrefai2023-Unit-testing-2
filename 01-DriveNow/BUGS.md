# Bugs found

Student: Ibrahem Salah Refai  
Group / ID: 3

Rule Problem Expected Actual starter behavior Fix.

BR 03 60 minute grace boundary Exactly 60 minutes does not add a day Greater than or equal to 60 added a day Changed to greater than 60.

BR 16 Luxury driver age Age 25 is valid with 3 license years The condition required an age greater than 25 Changed greater than 25 to greater than or equal to 25.

BR 21 Back to back bookings Pickup equal to an existing return is allowed Inclusive overlap check rejected it Use strict overlap bounds.

BR 33 Seven day discount 7 to 13 days receive 10 percent Seven days received no duration discount Changed the boundary to greater than or equal to 7.

BR 37 Promo cap Worked example B caps promo at 1050 Promo was not reduced after other discounts Limit promo after duration and loyalty discounts.

BR 39 Free additional driver Only Gold and Platinum receive the first free driver Silver was also free Restrict free driver rule to Gold Platinum.

BR 41 240 late minutes 240 minutes uses the hourly path It used a full daily rate path Changed daily rate boundary to greater than 240.

BR 51 48 hour cancellation 48 hours or more has no fee Exactly 48 hours charged 50 percent Changed boundary to greater than or equal to 48.

Create one failing test commit before each production fix commit to satisfy the assignment's Git-history requirement.
