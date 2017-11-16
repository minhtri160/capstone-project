#include <VirtualWire.h>
const int led_pin = 13;
// Dat ten hoac so hieu cho arduino.Vidu:Canh bao den, canh bao quat,...
//hoac dat theo so
String deviceID = "2ddd";

//Dat ten hoac so cho cac cam bien gan vao arduino
String sensorId = "1";

//Thong bao cac cong cho sensor tren arduino
#define sensorDongDien 7
#define sensorDienAp 8

//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;



long randomNumber;

void setup()
{
  pinMode(led_pin, OUTPUT);
  Serial.begin(9600);	  // Debugging only
  Serial.println("Setup");

  // Initialise the IO and ISR
  vw_set_tx_pin(10);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);	 // Bits per sec
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  digitalWrite(led_pin, LOW);
  //Thiet lap xuat nhap cho arduino
  pinMode(sensorDongDien, INPUT);
  pinMode(sensorDienAp, INPUT);
  vw_rx_start();
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
  String sensorDong = "1";
  String sensorAp = "2";
  String finalValue = sensorDong + ";" + sensorAp;
  return finalValue;
}
*/

void loop()
{
  /*
    randomNumber=random(0,1000);
    String b =String(randomNumber);
    char copy[50];
    b.toCharArray(copy, 50);
    char *a ="123456789";
    //Chep gia tri do duoc cua sensor vao trong cac bien String de chuan bi gui di
    String giatrido1 = xulydoctinhieusensor1();
    String giatrido2 = xulydoctinhieusensor2();
    String finalValue = giatrido1+";"+giatrido2;

    //chuyen doi String sang char*
    char sendValue[50];
    finalValue.toCharArray(sendValue, 50);

    //Gui chuoi gia tri di
    const char *msg = sendValue ;
    vw_send((uint8_t *)msg, strlen(msg));
    vw_wait_tx(); // Wait until the whole message is gone
    delay(200);
    byte buf[VW_MAX_MESSAGE_LEN];
    byte buflen = VW_MAX_MESSAGE_LEN;

    if (vw_get_message(buf, &buflen)) // Non-blocking
    {
    int i;

    digitalWrite(led_pin, HIGH); // Flash a light to show received good message
    // Message with a good checksum received, print it.
    Serial.print("Got: ");

    for (i = 0; i < buflen; i++)
    {
      char c = buf[i];
      Serial.print(c);
      //Serial.print(' ');
    }

    Serial.println();
    digitalWrite(led_pin, LOW);
    }
  */

  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  
  String finalValue = deviceID;
  //Serial.println(finalValue);
  char sendValue[20];
  finalValue.toCharArray(sendValue, 20);
  const char *msg = sendValue ;
  vw_send((uint8_t *)msg, strlen(msg));
  vw_wait_tx(); // Wait until the whole message is gone
  delay(200);
  
  

  if (vw_get_message(buf, &buflen)) // Non-blocking
  {
    int i;
    Serial.print("nhan: ");

    for (i = 0; i < buflen; i++)
    {
      char b = buf[i];
      Serial.print(b);
    }
    Serial.println();
  }
  delay(200);
}
