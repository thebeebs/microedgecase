#include <LiquidCrystal.h>

// initialize the library with the numbers of the interface pins
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

void setup() {
  // initialise Serial comms
  Serial.begin(9600);

  // initialise LCD screen
  lcd.begin(16, 2);

  // print welcome message
  lcd.print("Welcome...");
}

void loop() {
  if(Serial.available()) {
     String message = Serial.readString();
     
     lcd.clear();
     lcd.setCursor(15,0);
     lcd.print(message);
     
     for(int i = 0; i < message.length() + 15; i++) {
        // shift the display by 1
        lcd.scrollDisplayLeft();
        // control the speed of the scrolling
        delay(200);
     }
  }
}
