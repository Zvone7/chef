CREATE TABLE UserTable
(
    UserId          UNIQUEIDENTIFIER    PRIMARY KEY     NOT NULL,
    UserName    NVARCHAR(100)       UNIQUE          NOT NULL,
    Birthday    DATETIME2           UNIQUE          NOT NULL
);