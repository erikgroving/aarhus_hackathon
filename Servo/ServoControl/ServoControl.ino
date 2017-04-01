
#include <Servo.h> 
Servo myservo;  // create servo object to control a servo 
                // a maximum of eight servo objects can be created 
 
char var;
char buffer[4];
int pos = 0;

void setup() 
{ 
  Serial.begin(9600);
  myservo.attach(9);  // attaches the servo on pin 9 to the servo object 
  
  pinMode(8, OUTPUT);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }
  
} 

void loop() {
  digitalWrite(8, HIGH);
  if (Serial.available() > 0){
    
    for (int i=0; i <3; i++){
       buffer[i] = 0;
       var = Serial.read();
      if (isdigit(var)){
        buffer[i] = var;
      }
      delay(5);
    }
    
    pos = atoi(buffer); 
   // pos = -90;
    myservo.write(pos);
    
    Serial.println(buffer);
    
    delay(5);
   
  }
}
