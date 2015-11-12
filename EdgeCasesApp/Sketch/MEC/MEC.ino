#include <LiquidCrystal.h>

// initialize the library with the numbers of the interface pins
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

const int MSG_MAX = 24;
const int SCREEN_WIDTH = 16;
const int SCREEN_HEIGHT = 2;

void setup() {
  // initialise Serial comms
  Serial.begin(9600);

  // initialise LCD screen
  lcd.begin(SCREEN_WIDTH, SCREEN_HEIGHT);

  // print welcome message
  lcd.print("Hello,");
  lcd.setCursor(4, 1);
  lcd.print("#stacked2015");
}

void loop() {
  if(Serial.available()) {
     String message = Serial.readString().substring(0, MSG_MAX);
     
     lcd.clear();
     lcd.setCursor(SCREEN_WIDTH - 1,0);
     lcd.print(message);
     
     for(int i = 0; i < message.length() + SCREEN_WIDTH - 1; i++) {
        // shift the display by 1
        lcd.scrollDisplayLeft();
        // control the speed of the scrolling
        delay(400);
     }
  }
  delay(200);
}
