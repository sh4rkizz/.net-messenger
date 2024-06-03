CREATE TABLE MessengerUser (
    Id SERIAL PRIMARY KEY,
    Login VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Username VARCHAR(255) NOT NULL
);

CREATE TABLE Message (
    Id SERIAL PRIMARY KEY,
    SenderId INTEGER,
    ReceiverId INTEGER,
    Title VARCHAR(255),
    Content TEXT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Read BOOLEAN,

    CONSTRAINT SenderId FOREIGN KEY (SenderId) REFERENCES MessengerUser (Id),
    CONSTRAINT ReceiverId FOREIGN KEY (ReceiverId) REFERENCES MessengerUser (Id)
);
