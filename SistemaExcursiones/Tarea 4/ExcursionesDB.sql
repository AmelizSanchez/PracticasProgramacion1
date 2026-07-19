
CREATE DATABASE ExcursionesDB;
GO

USE ExcursionesDB;
GO

CREATE TABLE Excursiones
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL UNIQUE,
    Lugar VARCHAR(100) NOT NULL,
    Dia VARCHAR(50) NOT NULL,
    Cupos INT NOT NULL CHECK (Cupos >= 0)
);
GO

CREATE TABLE Participantes
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Documento VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(20) NOT NULL,
    IdExcursion INT NOT NULL,
    NumeroCupo INT NOT NULL,
    Pagado BIT NOT NULL DEFAULT 0,

    FOREIGN KEY (IdExcursion) REFERENCES Excursiones(Id)
);
GO

INSERT INTO Excursiones (Nombre, Lugar, Dia, Cupos)
VALUES ('Playa Bavaro', 'Punta Cana', 'Sabado', 20);

INSERT INTO Excursiones (Nombre, Lugar, Dia, Cupos)
VALUES ('Salto El Limon', 'Samana', 'Domingo', 15);

INSERT INTO Participantes (Nombre, Documento, Telefono, IdExcursion, NumeroCupo, Pagado)
VALUES ('Juan Perez', '00112223334', '8095551234', 1, 1, 1);

INSERT INTO Participantes (Nombre, Documento, Telefono, IdExcursion, NumeroCupo, Pagado)
VALUES ('Maria Gomez', '00199988877', '8295556789', 1, 2, 0);

SELECT * FROM Excursiones;
SELECT * FROM Participantes;