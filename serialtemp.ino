//    TmonN 
//    Arduino Thermistor reader
//    Copyright (C) <2017>  <Xander Jansen>
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Affero General Public License as published
//    by the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Affero General Public License for more details.
//
//    You should have received a copy of the GNU Affero General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

#define THERMISTORNOMINAL 10000      
// temp. for nominal resistance (almost always 25 C)
#define TEMPERATURENOMINAL 25   
// how many samples to take and average, more takes longer but is more 'smooth'
#define NUMSAMPLES 41
#define MEDIAN 21
// The beta coefficient of the thermistor (usually 3000-4000)
#define BCOEFFICIENT 3950
// the value of the 'other' resistor
#define SERIESRESISTOR 10000    
 
int samples[NUMSAMPLES];
int led2 = 13; 
boolean onoff=false;
byte inputByte_0;
byte inputByte_1;

void setup(void) {
  Serial.begin(9600);
  analogReference(EXTERNAL);
  digitalWrite(led2, HIGH);
}
 void insertionSort(int list[])
{
    int temp;
    for(long i = 1; i < NUMSAMPLES; i++)
    {
        temp = list[i];
        long j;
        for(j = i-1; j >= 0 && list[j] > temp; j--)
        {
            list[j+1] = list[j];
        }
        list[j+1] = temp;
    }
}
float MeasureTemp(int pin) {
  uint8_t i;
  float average =0;
  // take N samples in a row, with a slight delay
  for (i=0; i< NUMSAMPLES; i++) {
   samples[i] = analogRead(pin);
   delay(5);
  }
  // average all the samples out
  //for (i=0; i< NUMSAMPLES; i++) {
  //   average += samples[i];
  //}
  //average /= NUMSAMPLES;
  insertionSort(samples);
  average = samples[MEDIAN];
  // convert the value to resistance
  average = 1023 / average - 1;
  average = SERIESRESISTOR / average;
  float steinhart;
  steinhart = average / THERMISTORNOMINAL;     // (R/Ro)
  steinhart = log(steinhart);                  // ln(R/Ro)
  steinhart /= BCOEFFICIENT;                   // 1/B * ln(R/Ro)
  steinhart += 1.0 / (TEMPERATURENOMINAL + 273.15); // + (1/To)
  steinhart = 1.0 / steinhart;                 // Invert
  steinhart -= 273.15;                         // convert to C
  return steinhart;
}
void loop(void) {
String Temperature="";
  
  if (Serial.available() == 2) 
  {
    //Read buffer
    inputByte_0 = Serial.read();
    delay(100);    
    inputByte_1 = Serial.read();
    delay(100);      
  }
  //Check for start of Message
  if(inputByte_0 == 16)
  {       
       //Detect Command type
       switch (inputByte_1) 
       {
          case 128:
            //Say hello
            Serial.print("HELLO FROM ARDUINO");
            break;
        } 
        //Clear Message bytes
        inputByte_0 = 0;
        inputByte_1 = 0;
  }

 Temperature += (String)MeasureTemp(A0)+",";
 Temperature += (String)MeasureTemp(A1)+",";
 Temperature += (String)MeasureTemp(A2); // you can change these to the amount of sensors you need or dynamically add them.
Serial.println(String(Temperature));
 
  if(onoff){
    digitalWrite(led2, LOW);
   }
   else{
    digitalWrite(led2, HIGH);
   }
  onoff = !onoff;
}
