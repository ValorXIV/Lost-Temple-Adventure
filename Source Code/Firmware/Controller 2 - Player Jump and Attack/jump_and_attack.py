from machine import Pin, ADC
import time
import network
from config import (
    WIFI_SSID, WIFI_PASS,
    MQTT_BROKER, MQTT_USER, MQTT_PASS,
    TOPIC_PREFIX
)
from umqtt.simple import MQTTClient

SW1_GPIO = 2
SW2_GPIO = 21
TOPIC_JUMP = f'{TOPIC_PREFIX}/jump'
TOPIC_ATTACK = f'{TOPIC_PREFIX}/attack'

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

############
# setup
############
sw1 = Pin(SW1_GPIO, Pin.IN, Pin.PULL_UP)
sw2 = Pin(SW2_GPIO, Pin.IN, Pin.PULL_UP)
wifi = network.WLAN(network.STA_IF)
mqtt = MQTTClient(client_id='',
                  server=MQTT_BROKER,
                  user=MQTT_USER,
                  password=MQTT_PASS)
connect_wifi()
connect_mqtt()
last_publish = 0

# initate value
# initate value
is_jump = False
is_attack = False
jump_is_pressed = False
attack_is_pressed = False
############
# loop
############
while True:

    # publish light value periodically

    if sw1.value() == 0 and jump_is_pressed == False:
        is_jump, jump_is_pressed = True, True
    elif sw1.value() == 0 and jump_is_pressed == True:
        is_jump = False
    else:
        is_jump, jump_is_pressed = False, False
    if sw2.value() == 0 and attack_is_pressed == False:
        is_attack, attack_is_pressed = True, True
    elif sw2.value() == 0 and attack_is_pressed == True:
        is_attack = False
    else:
        is_attack, attack_is_pressed = False, False
            
    print(f'Publishing jumping state: {is_jump}')
    print(f'Publishing attacking state: {is_attack}')
    
    mqtt.publish(TOPIC_JUMP, str(is_jump))
    mqtt.publish(TOPIC_ATTACK, str(is_attack))
    
    time.sleep(0.1)