#include <SoftwareSerial.h>

//SIM800 TX is connected to Arduino D8
#define SIM800_TX_PIN 8
 
//SIM800 RX is connected to Arduino D7
#define SIM800_RX_PIN 7

SoftwareSerial serialSIM800(SIM800_TX_PIN,SIM800_RX_PIN);

void setup() {
  //Begin serial comunication with Arduino and Arduino IDE (Serial Monitor)
  Serial.begin(9600);
  while(!Serial);
   
  //Being serial communication witj Arduino and SIM800
  Serial2.begin(115200);
  delay(1000);

  init_gsm();
   
  Serial.println("Setup Complete!");
  Serial.println("Sending SMS...");
   
  //Set SMS format to ASCII
  Serial2.write("AT+CMGF=1\r\n");
  delay(1000);
 
  //Send new SMS command and message number
  Serial2.write("AT+CMGS=\"01644211255\"\r\n");
  delay(1000);
   
  //Send SMS content
  Serial2.write("TEST");
  delay(1000);
   
  //Send Ctrl+Z / ESC to denote SMS message is complete
  Serial2.write((char)26);
  delay(1000);
     
  Serial.println("SMS Sent!");
}

void init_gsm() {
  
  Serial2.println("ATE0");                            // Tat che do phan hoi (Echo mode)
  delay(2000);
  Serial2.println("AT+IPR=9600");              // Dat toc do truyen nhan du lieu 9600 bps
  delay(2000);
  Serial2.println("AT+CMGF=1");                // Chon che do TEXT Mode
  delay(2000);


}
 
void loop() {
  if (Serial1.available())
  {
    Serial.write(Serial.read());
  }
  if (Serial2.available()>0)
   Serial.write(Serial2.read());
}
