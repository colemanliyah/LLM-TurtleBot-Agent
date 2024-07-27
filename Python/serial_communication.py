import serial
import time
import ollama
import requests
import json

arduino = serial.Serial(port='COM3', baudrate=9600, timeout=.1) 
api_url = "http://localhost:11434/api/chat"
model_name = "llama3"

def model(prompt):
    try:
        response = ollama.chat(model='llama3', messages=[
            {
                'role': 'user',
                'content': prompt,
            },
        ])

        return response['message']['content']

    except httpx.ConnectError as e:
        print(f"Connection error: {e}")

def write_read(x: int):
    arduino.write(bytes(x, 'utf-8'))
    data = arduino.readline()
    return data

def main():
    print("check")

    user_input = input("Input text: ")
    result = model(user_input)
    print(result)

    #while True:
        #led_control = input("How do you want to manipulate the LED? ")
        #result = generate_text(api_url, model_name, result)

        #num = input("Enter 1 to turn on LED, enter 0 to turn off: ")
        #value = write_read(num)
        #print(value)

if __name__ == "__main__":
    main()