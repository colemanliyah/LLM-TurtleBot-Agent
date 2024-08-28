#!/usr/bin/env python3
import rclpy
from rclpy.node import Node
from geometry_msgs.msg import Twist
import time
import sys
import json

class CommandSender(Node):
    def __init__(self, command_list):
        super().__init__('command_sender')
        self.publisher = self.create_publisher(Twist, '/cmd_vel', 10)

        # Variables for speed and angular velocity
        self.linear_speed = 0.15 # 0.15 m/s
        self.angular_velocity = 0.81  # 45 degrees per second
        self.command_delay = 2.0  # 2 seconds

        # Apply the self.command_delay to the commands from the list
        self.command_list = [(command, self.command_delay) for command in command_list]
        # add (stop, 1.0) at the start of the seld.command_list
        self.command_list.insert(0, ('stop', 1.0))
        self.execute_commands()

    def execute_commands(self):
        for command, duration in self.command_list:
            if command == 'step forward':
                self.move_forward(duration)
            elif command == 'step back':
                self.move_backward(duration)
            elif command == 'turn right':
                self.turn_right(duration)
            elif command == 'turn left':
                self.turn_left(duration)
            elif command == 'stop':
                self.stop_moving(duration)
            else:
                self.get_logger().warn(f"Unknown command: {command}")
            time.sleep(1)  # Delay between commands
        self.get_logger().info("Finished executing all commands.")
        self.destroy_node()
        rclpy.shutdown()

    def move_forward(self, duration):
        twist = Twist()
        twist.linear.x = self.linear_speed
        twist.angular.z = 0.0  # Ensure straight movement
        self.publisher.publish(twist)
        self.get_logger().info(f"Moving Forward for {duration} seconds.")
        time.sleep(duration)
        self.stop_moving()

    def move_backward(self, duration):
        twist = Twist()
        twist.linear.x = -self.linear_speed  # Negative speed for moving backward
        twist.angular.z = 0.0
        self.publisher.publish(twist)
        self.get_logger().info(f"Moving Backward for {duration} seconds.")
        time.sleep(duration)
        self.stop_moving()

    def turn_right(self, duration):
        twist = Twist()
        twist.linear.x = 0.0
        twist.angular.z = -self.angular_velocity  # Adjust angular speed for turning
        self.publisher.publish(twist)
        self.get_logger().info(f"Turning Right for {duration} seconds.")
        time.sleep(duration)
        self.stop_moving()

    def turn_left(self, duration):
        twist = Twist()
        twist.linear.x = 0.0
        twist.angular.z = self.angular_velocity  # Adjust angular speed for turning
        self.publisher.publish(twist)
        self.get_logger().info(f"Turning Left for {duration} seconds.")
        time.sleep(duration)
        self.stop_moving()

    def stop_moving(self, duration=0.0):
        twist = Twist()
        twist.linear.x = 0.0
        twist.angular.z = 0.0
        self.publisher.publish(twist)
        self.get_logger().info("Stopping.")
        if duration > 0.0:
            time.sleep(duration)

def main(args=None):
    rclpy.init(args=args)
    command_list = json.loads(sys.argv[1])  # Receive the command list from ollama_test.py
    node = CommandSender(command_list)
    # No need to spin; just initialize, execute, and shut down

if __name__ == '__main__':
    main()
