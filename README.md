# Real-Time Embodied Agent with LLM and TurtleBot3

Creating an embodied agent by integrating the Llama model with a Turtlebot3 and ROS to enable a robot to navigate a grid autonomously from a given start point to an endpoint. 

Using the Llama-3.1-70B model and Hugging Face space with Gradio to communicate and rely instructions to the robot. The model was not fine-tuned, but  we utilized a system prompt to ensure the LLM understood the task.

Example Input/Output of the System

User prompt:

"The robot is at 'start_position': {'x': 25, 'y': 24}. What is the shortest list of actions to take in sequence to get to 'target_position': {'x': 27, 'y': 25}? "

Model response:

"Model Response: [["The robot is at 'start_position': {'x': 25, 'y': 24}. What is the shortest list of actions to take in sequence to get to 'target_position': {'x': 27, 'y': 25}?". 'The full list is: ["right", "right", "up"].']]"

