def convert_commands(current_direction, commands):

    facing_direction = current_direction
    new_commands = []

    for command in commands:
        if command == "up":
            if facing_direction == "north":
                new_commands.append("step forward")
            elif facing_direction == "east":
                new_commands.append("turn left")
                new_commands.append("step forward")
                facing_direction = "north"
            elif facing_direction == "south":
                new_commands.append("turn right")
                new_commands.append("turn right")
                new_commands.append("step forward")
                facing_direction = "north"
            elif facing_direction == "west":
                new_commands.append("turn right")
                new_commands.append("step forward")
                facing_direction = "north"
        elif command == "down":
            if facing_direction == "north":
                new_commands.append("turn right")
                new_commands.append("turn right")
                new_commands.append("step forward")
                facing_direction = "south"
            elif facing_direction == "east":
                new_commands.append("turn right")
                new_commands.append("step forward")
                facing_direction = "south"
            elif facing_direction == "south":
                new_commands.append("step forward")
            elif facing_direction == "west":
                new_commands.append("turn left")
                new_commands.append("step forward")
                facing_direction = "south"
        elif command == "left":
            if facing_direction == "north":
                new_commands.append("turn left")
                new_commands.append("step forward")
                facing_direction = "west"
            elif facing_direction == "east":
                new_commands.append("turn left")
                new_commands.append("turn left")
                new_commands.append("step forward")
                facing_direction = "west"
            elif facing_direction == "south":
                new_commands.append("turn right")
                new_commands.append("step forward")
                facing_direction = "west"
            elif facing_direction == "west":
                new_commands.append("step forward")
        elif command == "right":
            if facing_direction == "north":
                new_commands.append("turn right")
                new_commands.append("step forward")
                facing_direction = "east"
            elif facing_direction == "east":
                new_commands.append("step forward")
            elif facing_direction == "south":
                new_commands.append("turn left")
                new_commands.append("step forward")
                facing_direction = "east"
            elif facing_direction == "west":
                new_commands.append("turn right")
                new_commands.append("turn right")
                new_commands.append("step forward")
                facing_direction = "east"

    return new_commands, facing_direction
