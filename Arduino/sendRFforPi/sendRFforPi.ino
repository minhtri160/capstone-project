#include <VirtualWire.h>


//Thong bao cac cong nhan cua cac thiet bi tren arduino
const int led_pin = 13;
const int transmit_pin = 11;
const int receive_pin = 2;
const int transmit_en_pin = 3;

const String pzemId = "pmez5";
String pzem = "";



void setup() {
  pinMode(led_pin, OUTPUT);
  // put your setup code here, to run once:
  Serial.begin(9600);    // Debugging only
  //Serial1.begin(9600);

  // Initialise the IO and ISR
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2000);  // Bits per sec
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  // vw_set_ptt_pin(transmit_en_pin);
  digitalWrite(led_pin, LOW);
  vw_rx_start();
}

void TransmitUART(String value) {
  Serial.println(value);
  Serial.flush();
}

void TransmitRF(String value) {
  char sendValue[60];
  value.toCharArray(sendValue, 60);
  const char *msg = sendValue ;
  vw_send((uint8_t *)msg, strlen(msg));
  vw_wait_tx(); // Wait until the whole message is gone
  delay(100);
}

void loop() {
  // put your main code here, to run repeatedly:

  if (Serial.available()) {
    String receiveValue = Serial.readStringUntil('.');
    if (receiveValue == pzemId) {
      if (pzem != "")
        TransmitUART(pzem);
    }
    else
    {
      TransmitRF(receiveValue);
    }
  }

//  if (Serial1.available()) {
//    pzem = Serial1.readString();
//  }
  byte buf[VW_MAX_MESSAGE_LEN];
  byte buflen = VW_MAX_MESSAGE_LEN;
  if (vw_get_message(buf, &buflen)) // Non-blocking
  {
    int i;

    digitalWrite(led_pin, HIGH); // Flash a light to show received good message
    // Message with a good checksum received, print it.
    //Serial.print("Got: ");
    String receiveValue = "";
    for (i = 0; i < buflen; i++)
    {
      char c = buf[i];
      receiveValue += c;

      //Serial.print(' ');
    }
    //Serial.println(receiveValue);
    TransmitUART(receiveValue);
    //Serial.println("Got");
    digitalWrite(led_pin, LOW);
  }
}
