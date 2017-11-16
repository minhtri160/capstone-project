#include <VirtualWire.h>

const int led_pin = 13;
const int transmit_pin = 10;
const int receive_pin = 3;
const int transmit_en_pin = 4;
const int abc = 15;
// Dat ten hoac so hieu cho arduino.Vidu:Canh bao den, canh bao quat,...
//hoac dat theo so
#define sensorDongDien 7
#define sensorDienAp 8

String deviceID = "333";

//Dat ten hoac so cho cac cam bien gan vao arduino
String sensorId = "2";
void setup()
{
  pinMode(led_pin, OUTPUT);
  delay(1000);
  Serial.begin(9600);  // Debugging only
  Serial.println("setup");

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  digitalWrite(led_pin, LOW);
 // pinMode(sensorDongDien, INPUT);
 // pinMode(sensorDienAp, INPUT);
  vw_rx_start();       // Start the receiver PLL running
}
/*String xulydoctinhieusensor1() {

  //Chuyen gia tri cua cam bien sang String
  //String giatri1 = String(digitalRead(5));
  //String giatri2 = String(analogRead(A1));
  //String finalValue = giatri1+";"+giatri2
  //Demo

  String sensorDong = "1";
  String sensorAp = "2";
  String finalValue = sensorDong + ";" + sensorAp;
  return finalValue;
  }

  String xulydoctinhieusensor2() {

  //Chuyen gia tri cua cam bien sang String
  //String giatri1 = String(digitalRead(5));
  //String giatri2 = String(analogRead(A1));
  //String finalValue = giatri1+";"+giatri2
  //Demo

  String sensorDong = "abc";
  String sensorAp = "cdm";
  String finalValue = sensorDong + ";" + sensorAp;
  return finalValue;
  }
*/
float dovong() {
  return 0;
}
float dodong() {
  return 0;
}
float donhiet() {
  return 0;
}
void loop()
{
  char copy[50];
  String status1 = "0";
  //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
  //String giatrido1 = xulydoctinhieusensor1();
  //String giatrido2 = xulydoctinhieusensor2();
  // String finalValue = deviceID + ";" + giatrido1 + ";" + giatrido2;
  String giatridovong = String(dovong());
  String giatridonhiet = String(donhiet());
  String giatridodong = String(dodong());
  // String finalValue = deviceID +  ";" + status1 +";" + "dovong:" + giatridovong
  //     + ";" + "donhiet:" + giatridonhiet ;+ ";" + "dodong:" + giatridodong;
  String finalValue = deviceID +  ";" + status1 + ";" + "dovong:" + giatridovong
                      + ";" + "donhiet:" + giatridonhiet + ";" + "donhiet:" + giatridonhiet;
  //Serial.println(finalValue);

  //chuyen doi String sang char*

  char sendValue[100];
  finalValue.toCharArray(sendValue, 100);

  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  //Gui chuoi gia tri di
  const char *msg = sendValue ;
  // vw_send((uint8_t *)msg, strlen(msg));
  //vw_wait_tx(); // Wait until the whole message is gone
  // delay(200);
  String b;
  if (vw_get_message(buf, &buflen)) // Non-blocking
  {
    int i;
    Serial.print("Got: ");
    for (i = 0; i < buflen; i++)
    {
      char c = buf[i];
      String a = String(c);
      b += a;
    }
    Serial.print(b);
    if (b == "2dd")
    {
      vw_send((uint8_t *)msg, strlen(msg));
      vw_wait_tx();
      delay(200);
      Serial.print(" da nhan");
    }
    Serial.println();
  }
}
