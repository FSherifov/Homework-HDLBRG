# Structure of project
1. Mosquitto container to be used as MQTT Broker
2. Postgres container to be used as persistent storage
3. Python script to feed data into MQTT Broker
4. Python script to read data from MQTT Broker and save it into persistent storage(Postgres)
5. C# REST API to access data easily from different places

# Prerequisites

1. Python 3.12
2. Docker installation
3. .NET 8.0
4. Install python packages from python_scripts/requirements.txt
5. Install Docker container for MQTT Broker
> docker run -d --name mosquitto -p 1883:1883 eclipse-mosquitto
  - Add to mosquitto.conf these two lines, they are pretty unsafe, but are perfect for mocking/testing
>  allow_anonymous true
> 
>  listener 1883 0.0.0.0
6. Install Docker container for Postgres
> docker run --name postgres-mqtt -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -e POSTGRES_DB=mqtt -p 54321:5432 -d postgres
- Create this table:
>     CREATE TABLE sensor_data (
>     id SERIAL PRIMARY KEY,
>     topic VARCHAR(50),
>     value REAL,
>     timestamp TIMESTAMP
> );

# Order of running

1. Start docker container mosquitto
2. Start docker container postgres-mqtt
3. Start python script python_scripts/save_sensor_data_to_database.py
> python save_sensor_data_to_database.py
4. Start python script python_scripts/sensor_mock.py
> python sensor_mock.py
5. Build and run SensorREST project
> dotnet run
6. You can now send request to the REST API in this format:
> curl --location 'http://localhost:5173/api/sensor_data/candles?sensor=humidity&startTime=2024-01-01&endTime=2025-12-31'