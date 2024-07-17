#include <IRremote.h>


// Define Pins
#define PIN_SEND 3
#define PIN_RECV 11
int LED = 12;
unsigned int raw[20];
unsigned int rawTwo[20] = {5850,650,1600,600,650,1600,1550,550,700,1500,1650,550,1600,600,650,1550,650,1650,450,};

IRsend mySender;

IRrecv irrecv(PIN_RECV);
decode_results results;

void setup()
{
  mySender.begin(PIN_SEND);
  pinMode(LED, OUTPUT);
  Serial.begin(9600);
  irrecv.enableIRIn();
}


// main loop
void loop()
{
  if (Serial.available() > 0) {

    String data = Serial.readStringUntil('\n');  // Read the incoming string until newline character
    parseData(data);  // Parse the string into an array

    mySender.sendRaw(raw, 20, 38);

    // Turning on IR reciever:
    // if (irrecv.decode(&results)) {
    //   digitalWrite(LED, HIGH);  // Indicate IR signal not decoded
    //   delay(100);
    //   digitalWrite(LED, LOW);
    //   irrecv.resume();
    // } else {
    //   // digitalWrite(LED, HIGH);  // Indicate IR signal not decoded
    //   // delay(100);
    //   // digitalWrite(LED, LOW);
    //}
  }
  delay(1000);
}

void parseData(String data) {
  int index = 0;
  int start = 0;
  int end = data.indexOf(',');
  while (end != -1 && index < 20) {
    raw[index] = data.substring(start, end).toInt();
    start = end + 1;
    end = data.indexOf(',', start);
    index++;
  }
  if (index < 20)
  {
    raw[index] = data.substring(start).toInt();
  }
}

