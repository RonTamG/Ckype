﻿using PacketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

    class Program
    {

        private static ClientSocket clientSocket;

        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("Client started!");
            Console.WriteLine("Enter nickname"); 
            string nickname = Console.ReadLine();
            Console.Title = nickname;
            clientSocket = new ClientSocket();
            clientSocket.nickname = nickname;

            clientSocket.Connect("192.168.1.20", 6556);

            while (true)
            {
                Console.WriteLine("Please enter the ip of the person you would like to send a message to: ");
                string ip = Console.ReadLine();
                Console.WriteLine("Please enter the port of the person you would like to send a message to: ");
                int port = int.Parse(Console.ReadLine());
                Person destFriend = clientSocket.FindFriendByIPandPort(ip, port);
                Console.WriteLine("You have chosen to send the message to: " + destFriend);
                string msg = Console.ReadLine();
                if (msg.ToLower() == "exit")
                {
                    clientSocket.Disconnect();
                    Exit();
                }

                if (msg.ToLower() == "send file")
                {
                    Console.WriteLine("Enter file path and name please");
                    string filename = Console.ReadLine();
                    clientSocket.SendFile(filename, destFriend);
                }
                else if (msg.ToLower() == "call")
                {
                    Console.WriteLine("You have chosen to call {0}", destFriend);
                    CallPacket callP = new CallPacket(destFriend);
                    clientSocket.Send(callP.Data);
                }
                else
                {
                    Console.WriteLine("Sending: " + msg);
                    MessagePacket packet = new MessagePacket(msg, destFriend);
                    Console.WriteLine("Packet is: " + packet.Text + " sending to: " + packet.destClient);
                    clientSocket.Send(packet.Data);
                }
            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private static void Exit()
        {
            Environment.Exit(0);
        }
    }
}
