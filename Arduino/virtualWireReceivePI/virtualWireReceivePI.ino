#include <VirtualWire.h>

//const int led_pin = 13;
const int transmit_pin = 10;
const int receive_pin = 2;
const int transmit_en_pin = 3;

String deviceID = "2dd";

void setup()
{
  //pinMode(led_pin, OUTPUT);
  //delay(1000);
  Serial.begin(9600);  // Debugging only
  Serial.println("setup");

  // Initialise the IO and ISR
  vw_set_tx_pin(transmit_pin);
  vw_set_rx_pin(receive_pin);
  vw_set_ptt_pin(transmit_en_pin);
  vw_set_ptt_inverted(true); // Required for DR3100
  vw_setup(2500);  // Bits per sec
  //digitalWrite(led_pin, LOW);
  vw_rx_start();       // Start the receiver PLL running
}

void loop()
{
  String listDevice[5] = {"abc123", "bcd456","def789","tri123","tri456"};
  for (int i = 0; i < 5; i++) {
    deviceID = listDevice[i];
    boolean flag = true;
    //deviceID = "bcd456";
    String finalValue = deviceID;
    //Serial.println(finalValue);
    char sendValue[20];
    finalValue.toCharArray(sendValue, 20);
    const char *msg = sendValue ;

    if (flag == true) {
      vw_send((uint8_t *)msg, strlen(msg));
      vw_wait_tx(); // Wait until the whole message is gone
      delay(100);
      Serial.print("da gui ");
      Serial.println(deviceID);
    }

    byte buf[VW_MAX_MESSAGE_LEN];
    byte buflen = VW_MAX_MESSAGE_LEN;

    long timer = millis();
    while (true) {
      if (vw_get_message(buf, &buflen)) // Non-blocking
      {
        int i;
        //digitalWrite(led_pin, HIGH); // Flash a light to show received good message
        // Message with a good checksum received, print it.
        Serial.print("Got: ");
        for (i = 0; i < buflen; i++)
        {
          char c = buf[i];
          Serial.print(c);
          //Serial.print(' ');
        }
        Serial.println();
        break;
        //digitalWrite(led_pin, LOW);
      }
      long timer2 = millis();
      if ((timer2 - timer) > 500)
        break;
    }
    delay(50);
  }
}
