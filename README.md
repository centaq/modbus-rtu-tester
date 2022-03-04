# modbus-tester
Simple C# desktop app to check ModBus RTU protocol implementation on slave devices.

## Hardware requirements
Simply make a connection with your ModBus Slave Device from some USB-UART converter and you pretty much are ready to go.
Please don't forget to connect GNDs of your converter and modbus slave device together.

## Usage
After setting up your hardware make sure that your USB-UART converter has been recognized by your system correctly.
Then press `Get COMs` button in the `MainForm` of the application and select the proper one in `COMs` combo box.
After that select the rest of the connection properties (I was using default ones) and click `Open` button.

If there were't any exceptions during opening selected COM port you can start testing your device by entering its address, cmd type and data to sent.
All values should be written in HEX.
Now press sent an examinate your response and communication total start/finish time

## Limitations
1. As the name "simple" obligate to something, this application is not calculating CRC sum at all, and is not sending it. Probably something to be added in the future.
2. Application is written to support only `03`, `06` and `16` cmds. I wasn't testing the rest of them
