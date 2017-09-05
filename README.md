# Arduino Thermistor Monitor
Simple way to read out 10K, 25C temperature sensors/thermistors via a arduino(nano)

Arduino setup:
0 ohm bridge between 3V3 & REF
10K resistors between 3V3 & A0 A1 A2 etc 
thermistors between GND and A0 A1 A2 etc
Upload serialtemp.ino and leave connected to usb for further communication


(10K resistor bands: brown(1) black(0) orange(*1K) + tolerance level)
