import paho.mqtt.client as mqtt
import random
import json
import time
from datetime import datetime, timezone


def main():
    # MQTT Broker settings
    BROKER = "localhost"
    PORT = 1883
    TOPIC_TEMPERATURE = "sensor/temperature"
    TOPIC_HUMIDITY = "sensor/humidity"
    INTERVAL = 10 # Seconds between messages

    #Current sensor values
    global current_temperature
    global current_humidity

    current_temperature = 20
    current_humidity = 50


    # Create an MQTT client instance
    client = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2)

    # The callback for when the client receives a CONNACK response from the server.
    def on_connect(reason_code):
        print(f"Connected with result code {reason_code}")

    # Connect to the MQTT broker
    client.connect(BROKER, PORT, 60)
    client.on_connect = on_connect

    try:
        while True:
            # Generate sensor data
            generate_sensor_data()

            # Publish data to MQTT topics
            timestamp = datetime.now(timezone.utc).strftime("%Y-%m-%d %H:%M:%S")
            client.publish(TOPIC_TEMPERATURE, json.dumps({"value": current_temperature, "timestamp": timestamp}))
            print(f"Published to {TOPIC_TEMPERATURE}: {current_temperature}")
            client.publish(TOPIC_HUMIDITY, json.dumps({"value": current_humidity, "timestamp": timestamp}))
            print(f"Published to {TOPIC_HUMIDITY}: {current_humidity}")

            time.sleep(INTERVAL)

    except KeyboardInterrupt:
        print("\nSimulation stopped.")
        client.disconnect()

# Function to generate random sensor data
def generate_sensor_data():
        global current_humidity
        global current_temperature
        current_temperature = round(current_temperature + random.uniform(-2, 2), 2)
        current_humidity = round(current_humidity + random.uniform(-1, 1), 2)

main()