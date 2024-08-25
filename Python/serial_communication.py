import serial
import time
import ollama
import requests
import json
import re
from struct import *

#arduino = serial.Serial(port='COM3', baudrate=9600, timeout=.1) 
api_url = "http://localhost:11434/api/chat"
model_name = "llama3"

def model(prompt):
    try:
        response = ollama.chat(model='llama3', messages=[
            {
                'role': 'system',
                'content': 'You are a robot in position (0,0) facing north. You can only move one coordinate at a time in the direction you are facing. The commands to move the robot are: step forward (move one unity forward in current direction), turn left (rotate 90 degrees left without moving), and turn right (rotate 90 degrees right without moving). When given a target coordinate, output the shortest possible sequence of commands in an array format. The ouput should include the following phrase before the array: \'The full list is:\'. The array should contain the exact commands you would give step by step, with each command as a string, in the order you would execute them.'
            },
            #{
                #'role': 'system',
                #'content': 'You are a robot in position (0,0) facing north. You can only move one coordinate at a time in the direction you are facing. The commands to move the robot are: step forward (move one unity forward in current direction), step back (move one unity backward), turn left (rotate 90 degrees left without moving), and turn right (rotate 90 degrees right without moving). When given a target coordinate, output the shortest possible sequence of commands in an array format. The ouput should include the following phrase before the array: \'The full list is:\'. The array should contain the exact commands you would give step by step, with each command as a string, in the order you would execute them.'
            #},
            {
                'role': 'user',
                'content': prompt,
            }
        ])

        return response['message']['content']

    except httpx.ConnectError as e:
        print(f"Connection error: {e}")

def main():
    user_input = input("Input text: ")

    model_response = model(user_input)

    match = re.search(r'The full list is:\s*(\[[^\]]*\])', model_response, re.DOTALL)

    if match:
        array_str = match.group(1).strip()
        print("Extracted Array String:", array_str)
        try:
            result_array = eval(array_str)  # Safely evaluate the list format string
            print("Extracted Array:", result_array)
        except (SyntaxError, NameError) as e:
            print("Error evaluating the array string:", e)
    else:
        print("No array found after 'The full list is:'")

if __name__ == "__main__":
    main()