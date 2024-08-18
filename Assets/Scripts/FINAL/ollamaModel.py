import serial
import time
import ollama
import requests
import json
import re
from struct import *
import socket

api_url = "http://localhost:11434/api/chat"

# running ollama chat
def model(prompt):
    try:
        response = ollama.chat(model='llama3', messages=[
            {
                'role': 'system',
                'content': 'You are a robot in position (0,0) facing north. You can only move one coordinate at a time in the direction you are facing. The commands to move the robot are: step forward (move one unity forward in current direction), step back (move one unity backward), turn left (rotate 90 degrees left without moving), and turn right (rotate 90 degrees right without moving). When given a target coordinate, output the shortest possible sequence of commands in an array format. The ouput should include the following phrase before the array: \'The full list is:\'. The array should contain the exact commands you would give step by step, with each command as a string, in the order you would execute them.'
            },
            {
                'role': 'user',
                'content': prompt,
            }
        ])
        print("finished running ollama")
        return response['message']['content']

    except httpx.ConnectError as e:
        print(f"Connection error: {e}")

# Fomat response from ollama to make list of commands in one string seperated by commmas
def formatResponse(model_response):
    match = re.search(r'The full list is:\s*\[(.*?)\]', model_response, re.DOTALL)

    if match:
        array_str = match.group(1).strip()
        array_str = array_str.replace("'", '"')
        try:
            result_array = json.loads(f'[{array_str}]')
            formatted_string = ', '.join(result_array)
            return formatted_string
        except (SyntaxError, NameError) as e:
            return f'Error evaluating the array string: {e}'
    else:
        return "No array found after 'The full list is:'"

# Setup the server to get data for ollama model
def startServer():
    host = '127.0.0.1'
    port = 65432  # Port to listen on

    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((host, port))
    server_socket.listen(1)

    print('Server listening on port', port)

    try:
        while True:
            conn, addr = server_socket.accept()
            print('Connected by', addr)
            with conn:
                data = conn.recv(1024).decode()
                if data:
                    print(f'Received prompt: {data}')
                    response = model(data)
                    formatted_response = formatResponse(response)
                    conn.sendall(formatted_response.encode())
    except KeyboardInterrupt:
         print("Server interrupted by user.")
    finally:
        print("Closing server socket.")
        server_socket.close()

if __name__ == "__main__":
    startServer()
