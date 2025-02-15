# Structure of project
1. Mosquitto container to be used as MQTT Broker
2. Postgres container to be used as persistent storage
3. Python script to feed data into MQTT Broker
4. Python script to read data from MQTT Broker and save it into persistent storage(Postgres)
5. C# REST API to access data easily from different places
6. Some kind of FrontEnd to display data in graphical sense




# Prerequisites

1. Python 3.12
2. Docker installation
3. Install python packages from python_scripts/requirements.txt
4. Install Docker container for MQTT Broker
> docker run -d --name mosquitto -p 1883:1883 eclipse-mosquitto
  - Add to mosquitto.conf these two lines, they are pretty unsafe, but are perfect for mocking/testing
>  allow_anonymous true
> 
>  listener 1883 0.0.0.0
5. Install Docker container for Postgres
> docker run --name postgres-mqtt -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -e POSTGRES_DB=mqtt -p 54321:5432 -d postgres
- Create this table:
>     CREATE TABLE sensor_data (
>     id SERIAL PRIMARY KEY,
>     topic VARCHAR(50),
>     value REAL,
>     timestamp TIMESTAMP
> );

