CREATE TABLE IF NOT EXISTS Army (
id integer PRIMARY KEY,
teamNumber INTEGER,
class TEXT,
armor text,
shield text,
weapon TEXT,
currentHealth INTEGER,
isLeader BOOLEAN
);
--DROP TABLE Army