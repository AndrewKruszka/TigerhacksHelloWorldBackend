import json
import time
import os
import requests
from Database.models import Base
from Database.engine import CONNECTION_STRING, create_database_engine
from Database.stored_procedures import create_stored_procedures

backstone_host = os.getenv("BACKSTONE_HOST", "backstone")

if __name__ == "__main__":
    print("Connection string: ", CONNECTION_STRING)

    engine = create_database_engine(CONNECTION_STRING)

    print("Creating database tables...")
    Base.metadata.create_all(engine)
    print("Tables created!")
    time.sleep(3)
    create_stored_procedures(engine)
    print("Created stored procedures")
