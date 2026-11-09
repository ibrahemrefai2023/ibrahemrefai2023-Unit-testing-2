# LeetCode binary-search solutions

Student: Ibrahem Salah Refai  
Group / ID: 3

## Profile and accepted submissions

Profile add your public LeetCode profile URL.

875 Koko Eating Bananas add the URL of your accepted submission.

33 Search in Rotated Sorted Array add the URL of your accepted submission.

The screenshots folder must contain each accepted page with the username problem title accepted status runtime memory and submission date visible I did not add substitute images because these have to come from the user's own LeetCode account.

## 875 Koko Eating Bananas

I binary search the possible eating speed not the pile indexes The lowest useful speed is 1 and the highest useful speed is the largest pile because that speed finishes every pile in one hour For a chosen speed I calculate how many whole hours Koko needs If the hours are within h the speed might work but there could be a smaller answer so I keep the left half Otherwise the speed is too slow. so I keep the right half

The loop finishes when low and high meet Every speed below low was proved too slow and low itself was proved fast enough The time complexity is O n log m where n is the number of piles and m is the largest pile The extra space is O 1.

### Trace piles 3 6 7 11 h 8

low high mid hours at mid decision.

1 11 6 6 enough hours available keep left half.

1 6 3 10 too slow keep right half.

4 6 5 8 works keep left half.

4 5 4 8 works answer is 4.

## 33 Search in Rotated Sorted Array

I binary search the indexes At every step one half of the current range is sorted If the left half is sorted I check whether the target is inside that sorted range If it is I keep the left half otherwise I keep the other half If the left half is not sorted the right half must be sorted and I make the matching decision there.

The loop stops when the target is found or the search range becomes empty Each decision discards half of the remaining indexes so the time complexity is O log n and the extra space is O 1.

### Trace nums 4 5 6 7 0 1 2 target 0

low mid high values decision.

0 3 6 4 7 2 left half is sorted target is not there keep right half.

4 5 6 0 1 2 left half is sorted and target is inside it keep left half.

4 4 4 0 0 0 target found at index 4.

