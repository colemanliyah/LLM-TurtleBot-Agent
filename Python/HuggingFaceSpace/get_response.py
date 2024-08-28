from gradio_client import Client
import os
import re
import subprocess
import json

# user defined imports
from system_prompt import system_messages
from utils import convert_commands




send_to_robot = True

# Create a client instance for the Gradio Space
# HF_TOKEN = "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"
# client = Client("Slimy619/LlamaVerse", hf_token=HF_TOKEN)
client = Client("Slimy619/LlamaVerse")

system_message = system_messages[0]


def AddUserMessage(user_message):
    result = client.predict(
        message=user_message,
        api_name="/user"
    )
    if result:
        print("Failed to add user message")
        return False
    else:
        print("User message added successfully")
        return True
    
def GetResponse(system_message):
    print("Getting response...")
    result = client.predict(
        system_message=system_message,
        max_tokens=400,
        temperature=0.8,
        top_p=0.95,
        top_k=40,
        repeat_penalty=1.1,
        api_name="/response"
    )
    if result:
        print("Response received successfully")
        print(result)
        return result
    else:
        print("Failed to get response")
        return False
    
def main():
    # user_input = input("Input text: ")

    # Read the JSON file data.json to get current position and direction
    with open('data.json') as f:
        data = json.load(f)

    current_position = data["current_position"]
    x = current_position["x"]
    y = current_position["y"]
    current_position = data["current_position"]
    current_direction = data["direction"]

    target_position = [25, 25]
    # user_input = f"You are at (25, 25) and facing {current_direction}. What is the shortest list of actions to take in sequence to get to {target_position}?"
    user_input = f"The robot is at 'start_position': {{'x': {x}, 'y': {y}}}. What is the shortest list of actions to take in sequence to get to 'target_position': {{'x': {target_position[0]}, 'y': {target_position[1]}}}?"

    client.predict(
		chat_message="",
		api_name="/clear_chat"
)
    # Add the user message to the Gradio Space
    AddUserMessage(user_input)

    # Get the response from the Gradio Space
    model_response = GetResponse(system_message)

    print("Model Response:", model_response)

    match = re.search(r'The full list is:\s*(\[[^\]]*\])', model_response[0][1], re.DOTALL)

    if match:
        array_str = match.group(1).strip()
        try:
            result_array = eval(array_str)  # Safely evaluate the list format string

            robot_commands, final_direction = convert_commands(current_direction, result_array)
            
            # Pass the result_array to the new_send.py
            if send_to_robot:
                print("Sending to robot...")
                print("Array:", robot_commands)
                subprocess.run(['python3', 'send_to_robot.py', json.dumps(robot_commands)])
            else:
                print("Extracted Array:", result_array)

            robot_data = {
                            "current_position": {"x": target_position[0], "y": target_position[1]},
                            "direction": final_direction,
                         }
            
            # Save this dictionary to a new JSON file
            with open('data.json', 'w') as outfile:
                json.dump(robot_data, outfile, indent=4)

        except (SyntaxError, NameError) as e:
            print("Error evaluating the array string:", e)
    else:
        print("No array found after 'The full list is:'")

if __name__ == "__main__":
    main()
