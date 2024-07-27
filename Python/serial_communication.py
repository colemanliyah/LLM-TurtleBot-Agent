import serial
import time 

arduino = serial.Serial(port='COM3', baudrate=9600, timeout=.1) 

def write_read(x: int):
    arduino.write(bytes(x, 'utf-8'))
    data = arduino.readline()
    return data


while True:
    num = input("Enter 1 to turn on LED, enter 0 to turn off: ")
    value = write_read(num)
    print(value)