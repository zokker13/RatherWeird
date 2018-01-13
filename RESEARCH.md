# Memory hacking for RA3 
## Camera Unlock
The camera can be unlocked and you can scroll out until the view distance is too great.
Memory:
RA3_1.12.game+8DB7B4 -> [+48] -> target value (byte)
The value is 1 by default. Changing that to 0 allows you to scroll out.

This operation has to be done everytime you start a match - it will be resetted.
A more bulletproof way would be to find the function to check the limit and allow it but no success in finding that.
