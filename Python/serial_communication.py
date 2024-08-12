import serial
import time
import ollama
import requests
import json
import re
from struct import *

arduino = serial.Serial(port='COM3', baudrate=9600, timeout=.1) 
api_url = "http://localhost:11434/api/chat"
model_name = "llama3"

def model(prompt):
    try:
        response = ollama.chat(model='llama3', messages=[
            {
                'role': 'system',
                'content': 'You are a robot in position (0,0) facing north. You can only move one coordinate at a time in the direction you are facing. The commands to move the robot are: step forward (move one unity forward in current direction), step back (move one unity backward), turn left (rotate 90 degrees left without moving), and turn right (rotate 90 degrees right without moving). When given a target coordinate, output the shortest possible sequence of commands in an array format. The array should contain the exact commands you would give step by step, with each command as a string, in the order you would execute them.'
            },
            {
                'role': 'user',
                'content': prompt,
            }
        ])

        return response['message']['content']

    except httpx.ConnectError as e:
        print(f"Connection error: {e}")

def write_read(x):
    array_data = ','.join(map(str, x))

    #print("array_data is" + array_data)

    arduino.write(bytes(array_data, 'utf-8'))

    #arduino.write(bytes(x, 'utf-8'))
    #data = arduino.readline()
    #return data

def main():
    print("check")

    user_input = input("Input text: ")

    model_response = model(user_input)

    print("Here")
    print(model_response)

    print("Break\n")
    #array_match = re.search(r'\[\s*\".*?\"\s*\]', model_response, re.DOTALL)

    #if array_match:
        #print("reason\n\n")
        #array_str = array_match.group(0)
        #print(array_str)
        #print("hum\n\n")
        #result_array = eval(array_str)  # Safely evaluate the list format string
        # write_read(result_array)
        #print(result_array)
    #else:
        #print("array of commands not found")

    # Debugging 
    #while True:
        #led_control = input("How do you want to manipulate the LED? (1 for on 2 for off) ")
        #value = write_read(num)
        #print(value)

if __name__ == "__main__":
    main()