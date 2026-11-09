# ParkSmart TDD log

Student: Ibrahem Refai  
Group / ID: _add before submission_

## Feature cycles to reproduce in Git

| Stage | Test behavior | Minimal production change | Refactor idea |
| --- | --- | --- | --- |
| S1 | 0, 15, and 16 minute sessions | Return zero at or under 15 minutes. | Keep session construction in one test helper. |
| S2 | 60 and 61 minutes | Charge one started hour, then two. | Extract normal-fee calculation. |
| S3 | Exit before entry | Throw `ArgumentException`. | Validate at the calculator entry point. |
| S4 | 10h, 30h, 48h, and 48h10 | Cap each 24-hour block. | Process one daily block in a loop. |
| S5 | Motorcycle and truck prices | Map rate/cap by vehicle type. | Keep type mappings next to each other. |
| S6 | Night time boundaries | Compare normal price against night flat rate. | Extract `IsNightSession`. |
| S7 | Lost tickets | Apply vehicle minimum after normal calculation. | Extract minimum mapping. |
| S8 | Friday 2h boundary | Change the free threshold based on Friday entry. | Keep it next to the standard free threshold. |
| S9 | Subscriber free/discount | Check the weekday time window, otherwise half the fee. | Extract `IsSubscriberFreeSession`. |
| S10 | Charging and negative kWh | Add charging after parking and reject negative data. | Keep validation at the top of `Calculate`. |

The code and tests are present, but the required red/green/refactor commit sequence, screenshots, and commit hashes must be created through your own Git workflow. Do not invent a history after the fact. Run the tests after each red and green commit and add the real output images under `screenshots/`.

## Design reflection

The daily cap was the stage that changed the simple hourly calculation the most. A single `ceil(totalHours) * rate` expression stops working once every 24-hour block has its own cap. Separating normal calculation from discounts and extras made later changes easier because the order of rules is visible.
