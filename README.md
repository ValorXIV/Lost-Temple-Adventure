# Lost Temple Adventure - 2D Platformer Game & ESP32S3 Controller Project #
A Unity 2D Game Project for Introduction to Computer Hardware Development Subject, featuring custom input action using customized ESP32 board as controller.

A project presentation video could be found at : https://www.youtube.com/watch?v=ysNn58gjF5k

======================================================================================================================================================

Project for 01204114 Introduction to Computer Hardware Development  

Developed by :

	1) 6510503298 : ชญานนท์ มานะกิจจานนท์ 
	2) 6510503310 : ชวัลวิทย์ 	เกียรติณัฐกร
	3) 6510503425 : ธนภูมิ แตงวงศ์
	4) 6510503816 : ศุภกิตต์ วงศ์โต
 	
Computer Engineering (CPE) Faculty, Kasetsart University, Bangkok

======================================================================================================================================================

Directories :

	1) Game Folder : contain a zip file for "Lost Temple Adventure" game

		1.1) Lost Temple Adventure.zip : contain a full playable game created by Unity Game Engine
	
	2) Source Code Folder : contain all source code used

		2.1) Firmware Folder :
 
			2.1.1) Controller 1 - Player Movement Controller Folder : contain a MicroPython code used in ESP32S3 board 1

					- accelerometer.py : code for recieving value from accelerometer and publishing movement direction to MQTT Broker
					- config.py : configuration code for device to connect to MQTT broker at iot.cpe.ku.ac.th

			2.1.1) Controller 2 - Player Jump and Attack Folder : contain a MicroPython code used in ESP32S3 board 1

					- jump_and_attack.py : code for recieving value from two switch and publishing attacking and jumping status to MQTT Broker
					- config.py : configuration code for device to connect to MQTT broker at iot.cpe.ku.ac.th

		2.2) Node-RED Folder : contain a Node-RED flow used in this project

			2.2.1) flows.json 
		
		2.3) Unity C# scripts : contain all C# scripts used in Unity Game Engine
			
			2.3.1) Chest Folder : contain all scripts attached to Chest object in game
				
			2.3.2) Monster Folder : contain all scripts attached to Monster object in game

			2.3.3) MQTT Folder : contain all scripts used to receive MQTT payload from MQTT broker to use in Unity Game Engine

				- MQTTAttack.cs : receive attacking status 
				- MQTTJump.cs : receive jumping status
				- MQTTWalk.cs : receive moving direction

			2.3.4) Player Folder : contain all scripts attached to Player object in game

			2.3.5) Trap Folder : contain all scripts attached to Trap object in game

			2.3.6) UI Folder : contain all scripts used for game UI

	3) LICENSE.txt: a MIT License text file
	
	4) README.txt: a text file describing the project and the files included in this directory
	
	5) Schematic.pdf: a schematic file containing details of the two controller board

======================================================================================================================================================

Libraries Used :

	1) MQTTnet : .NET library for MQTT based communication in C# language
		by chkr1011 (https://www.nuget.org/packages/MQTTnet)

	2) Unity : Unity Game Engine built-in library 

======================================================================================================================================================

Hardwares Used :
	
	1) 2 ESP32-S3 microcontrollers 
	2) 1 GY-61 3-axis Accelerometer Module (ADXL335)
	3) 2 Switches
