import paho.mqtt.client as mqtt
import psycopg2
import json

# PostgreSQL Database Configuration
DB_CONFIG = {
    "dbname": "mqtt",
    "user": "admin",
    "password": "admin",
    "host": "localhost",
    "port": 54321
}

# MQTT Broker Configuration
BROKER = "localhost"
PORT = 1883
TOPIC = "sensor/#"

# Connect to PostgreSQL
def connect_db():
    try:
        conn = psycopg2.connect(**DB_CONFIG)
        print("Connected to PostgreSQL")
        return conn
    except Exception as e:
        print("Database connection failed:", e)
        return None

# Insert sensor data into the database
def insert_data(topic: str, data):
    conn = connect_db()
    if conn:
        try:
            with conn.cursor() as cur:
                cur.execute(
                    "INSERT INTO sensor_data (topic, value, timestamp) VALUES (%s, %s, %s)",
                    (topic[7:], data["value"], data["timestamp"])
                )
            conn.commit()
            print(f"Saved to DB: {topic} -> {data}")
        except Exception as e:
            print("Database insert error:", e)
        finally:
            conn.close()

# MQTT Callback - When a message is received
def on_message(client, userdata, msg):
    try:
        data = json.loads(msg.payload.decode())
        insert_data(msg.topic, data)
    except json.JSONDecodeError as e:
        print("Error decoding JSON:", e)

# Set up MQTT client
client = mqtt.Client()
client.on_message = on_message

# Connect to MQTT Broker and subscribe
client.connect(BROKER, PORT, 60)
client.subscribe(TOPIC)

print(f"Listening to topics: {TOPIC}")
client.loop_forever()