using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    SerialPort serialPort;
    Dictionary<string, ushort[]> robotCommands = new Dictionary<string, ushort[]>()
    {
        {"slideF", new ushort[] {5850,650,1600,600,650,1600,1550,550,700,1500,1650,550,1600,600,650,1550,650,1650,450,}},
        {"slideB", new ushort[] {5900,600,1650,550,650,1600,1550,550,700,1600,1550,550,650,1600,650,1550,700,1650,400,}},
        {"stepF", new ushort[] {5900,550,1600,550,700,1500,1600,550,700,1550,650,1600,650,1550,650,1550,1550,700,450,}},
        {"stepB", new ushort[] {5950,650,1550,600,650,1600,1550,550,550,1700,650,1550,1550,600,700,1550,1550,700,550,}},
        {"turnL", new ushort[] {5900,600,1650,550,650,1600,1550,550,600,1600,1650,550,700,1500,1600,700,700,1650,400,}},
        {"turnR", new ushort[] {5900,600,1650,550,650,1600,1550,550,600,1600,1650,550,700,1500,1600,700,700,1650,400,}}
    };

    void Start()
    {
        serialPort = new SerialPort("COM3", 9600);
        serialPort.ReadTimeout = 100;
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
        }
    }

    public void ControlArdunio(string action)
    {
        SendArray(robotCommands[action]);
    }

    // Convert the unshort array to a comma-separated string
    void SendArray(ushort[] array)
    {
        if (serialPort.IsOpen)
        {
            string arrayString = string.Join(",", array);
            serialPort.WriteLine(arrayString);
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}