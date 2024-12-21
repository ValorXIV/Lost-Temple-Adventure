from machine import Pin, ADC
import time
import network
from config import (
    WIFI_SSID, WIFI_PASS,
    MQTT_BROKER, MQTT_USER, MQTT_PASS,
    TOPIC_PREFIX
)
from umqtt.simple import MQTTClient

# Set up the ADC pins for the accelerometer
x_pin = ADC(Pin(5))
y_pin = ADC(Pin(6))
z_pin = ADC(Pin(7))

x_pin.atten(ADC.ATTN_11DB)
y_pin.atten(ADC.ATTN_11DB)
z_pin.atten(ADC.ATTN_11DB)

# Set up the topic
TOPIC_MOVEMENT = f'{TOPIC_PREFIX}/movement'

# Set up the scaling factor for the accelerometer
SCALE_FACTOR = 2350

def connect_wifi():
    mac = ':'.join(f'{b:02X}' for b in wifi.config('mac'))
    print(f'WiFi MAC address is {mac}')
    wifi.active(True)
    print(f'Connecting to WiFi {WIFI_SSID}.')
    wifi.connect(WIFI_SSID, WIFI_PASS)
    while not wifi.isconnected():
        print('.', end='')
        time.sleep(0.5)
    print('\nWiFi connected.')

def connect_mqtt():
    print(f'Connecting to MQTT broker at {MQTT_BROKER}.')
    mqtt.connect()
    print('MQTT broker connected.')

# Set up connection
wifi = network.WLAN(network.STA_IF)
mqtt = MQTTClient(client_id='',
                  server=MQTT_BROKER,
                  user=MQTT_USER,
                  password=MQTT_PASS)
connect_wifi()
connect_mqtt()
last_publish = 0

# Set up the loop to read the accelerometer values
while True:
    
    current_time = time.time()
    
    # Read the raw ADC values for x, y, z axes
    x_raw = x_pin.read()
    y_raw = y_pin.read()
    z_raw = z_pin.read()

    # Convert the raw values to g-force
    x_g = x_raw / SCALE_FACTOR
    y_g = y_raw / SCALE_FACTOR
    z_g = z_raw / SCALE_FACTOR

    # Print the G-force values
    #print("X: {:.2f}g, Y: {:.2f}g, Z: {:.2f}g".format(x_g, y_g, z_g))
    #print("X: {:.2f}g, Y: {:.2f}g, Z: {:.2f}g".format(x_g, y_g, z_g))
    movement = ""

    if z_g>=0.93:
        movement = "Right"
        #print("Right")
    elif z_g<=0.73:
        movement = "Left"
        #print("Left")
    else:
        movement = "Center"
        #print('Center')
    

    # Wait for a short time before reading again
    print('Publishing movement: X: {:.2f}g, Y: {:.2f}g, Z: {:.2f}g as {}'.format(x_g, y_g, z_g, movement))
    mqtt.publish(TOPIC_MOVEMENT, movement)
