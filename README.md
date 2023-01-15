# Digital-Signal-Processing

"Practical Task2" 
This function takes a path for input signal, its sampling frequency ‘Fs’, the minimum ‘miniF’& maximum frequency ‘maxF’ of the signal and new sampling frequency ‘newFs’ and (L & M for sampling) as an input and then do the following:
1)	Display the given signal.
2)	Filter the signal using FIR filter with band [miniF, maxF]. (save filtered signal in file)
3)	Resample the signal to newFs only if newFs doesn’t destroy the signal, else show a message to the user “newFs is not valid” and continue executing the next instructions,
“sample using L & M” as explained in Practical task1. (if sampling is done, save resulted signal in file)
4)	Remove the DC component. (save resulted signal in file)
5)	Display the resulted signal from 4.
6)	Normalize the signal to be from -1 to 1. (save resulted signal in file)
7)	Display the resulted signal from 6.
8)	Compute DFT. (save resulted signal in file)
9)	Display the resulted components from 8.

